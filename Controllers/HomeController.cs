using ExemploExportacaoExcel.Data;
using ExemploExportacaoExcel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

using ClosedXML.Excel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;



namespace ExemploExportacaoExcel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> ExportarExcelComClosedXML()
        {
            var clientes = await _context.Clientes.ToListAsync();
            if (clientes == null)
            {
                return NotFound();
            }
            var nomeArquivo = "clientes.xlsx";

            return GeraExcelComClosedXML(nomeArquivo, clientes);
        }

        [HttpGet]
        public async Task<IActionResult> ExportarExcelNpoi(string extensao)
        {
            var clientes = await _context.Clientes.ToListAsync();
            if (clientes == null)
            {
                return NotFound();
            }
            var nomeArquivo = $"clientes{extensao}";

            var arquivo = GeraExcelComNpoi(nomeArquivo, extensao, clientes);

            return arquivo;
        }


        private FileResult GeraExcelComClosedXML(string nomeArquivo, IEnumerable<Cliente> clientes)
        {
            var dataTable = GetDataTable(clientes);

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        nomeArquivo);
                }
            }
        }

        private FileResult GeraExcelComNpoi(string nomeArquivo, string extensao, IEnumerable<Cliente> clientes)
        {
            var dataTable = GetDataTable(clientes);

            using (MemoryStream stream = new MemoryStream())
            {
                
                IWorkbook wb = extensao == ".xls" ? new HSSFWorkbook() : new XSSFWorkbook();
                ISheet ws = wb.CreateSheet("Clientes");

                IRow headerRow = ws.CreateRow(0);
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    ICell headerCell = headerRow.CreateCell(i);
                    headerCell.SetCellValue(dataTable.Columns[i].ColumnName);
                }

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    IRow row = ws.CreateRow(i + 1);
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {

                        ICell cell = row.CreateCell(j);
                        String columnName = dataTable.Columns[j].ToString();
                        cell.SetCellValue(dataTable.Rows[i][columnName].ToString());
                    }
                }

                wb.Write(stream);
                var contentType = extensao == ".xls" 
                    ? "application/vnd.ms-excel" 
                    : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                
                return File(stream.ToArray(), contentType, nomeArquivo);
            }
        }


        private DataTable GetDataTable(IEnumerable<Cliente> clientes)
        {
            DataTable dataTable = new("Clientes");
            dataTable.Columns.AddRange(
            [
                new DataColumn("Id"),
                new DataColumn("Nome"),
                new DataColumn("Idade"),
                new DataColumn("Endereco"),
            ]);

            foreach (var cliente in clientes)
            {
                dataTable.Rows.Add(cliente.Id, cliente.Nome, cliente.Idade, cliente.Endereco);
            }

            return dataTable;
        }



    }
}

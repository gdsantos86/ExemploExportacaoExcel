using ClosedXML.Excel;
using ExemploExportacaoExcel.Data;
using ExemploExportacaoExcel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Data;
using System.Diagnostics;

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
        public async Task<IActionResult> ExportarExcel()
        {
            var clientes = await _context.Clientes.ToListAsync();
            if (clientes == null)
            {
                return NotFound();
            }
            var nomeArquivo = "clientes.xlsx";

            var arquivo =  GeraExcel(nomeArquivo, clientes);
            
            if(arquivo == null) 
            { 
                return NotFound("O arquivo não foi gerado."); 
            }
            
            return GeraExcel(nomeArquivo, clientes);
        }


        private FileResult GeraExcel(string nomeArquivo, IEnumerable<Cliente> clientes)
        {
            DataTable dataTable = new DataTable("Clientes");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("Nome"),
                new DataColumn("Idade"),
                new DataColumn("Endereco"),
            });

            foreach (var cliente in clientes)
            {
                dataTable.Rows.Add(cliente.Id, cliente.Nome, cliente.Idade, cliente.Endereco);
            }

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
    }
}

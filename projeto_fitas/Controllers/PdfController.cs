using iText.Kernel.Geom;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;


namespace TCC_Projeto.Controllers
{
    public class PdfController : Controller
    {
        public IActionResult CriarPdf()
        {
            return new ViewAsPdf("PDF_TESTE");
            {

            }
        }
    }
}
                        
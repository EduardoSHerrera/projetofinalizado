using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TCC_Projeto.Models;
using TCC_Projeto.Models.Etiqueta;
using CsvHelper;
using System.Data.Entity;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using Microsoft.AspNetCore.Authorization;
using TCC_Projeto.Models.Usuario;


namespace TCC_Projeto.Controllers
{
    public class EtiquetaController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly Conexao _context;

        public EtiquetaController(IWebHostEnvironment hostingEnvironment, Conexao context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        [Authorize]
        public IActionResult Etiqueta()
        {
            return View();
        }

        //Método para lidar com o envio do formulário via AJAX
        [HttpPost]
        public async Task<IActionResult> EnviarEtiqueta(EtiquetaViewModel etiqueta)
        {
            if (ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await etiqueta.Imagem.CopyToAsync(memoryStream);
                    var etiquetaModel = new Etiquetas
                    {
                        Nome = etiqueta.Nome,
                        Descricao = etiqueta.Descricao,
                        Imagem = memoryStream.ToArray()
                    };

                    _context.Etiquetas.Add(etiquetaModel);
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true, message = "Dados enviados com sucesso!" });
            }

            return Json(new { success = false, message = "Erro ao enviar os dados." });
        }
       
        [HttpPost]
        public async Task<IActionResult> ExcluirEtiqueta(int id)
        {
            try
            {
                // Encontrar a etiqueta pelo ID
                var etiqueta = await _context.Etiquetas.FindAsync(id);

                // Verificar se a etiqueta foi encontrada
                if (etiqueta != null)
                {
                    // Remover a etiqueta do contexto
                    _context.Etiquetas.Remove(etiqueta);

                    // Salvar as alterações no banco de dados
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Etiqueta excluída com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Etiqueta não encontrada." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public byte[]? ObterImagemDaEtiqueta(int id)
        {
            var etiqueta = _context.Etiquetas.FirstOrDefault(e => e.Id == id);

            if (etiqueta != null)
            {
                return etiqueta.Imagem;
            }

            return null;
        }

        [HttpGet]
        public IActionResult ObterTodasEtiquetas()
        {
            // Consultar todas as etiquetas do banco de dados
            var etiquetas = _context.Etiquetas.ToList();

            // Mapear os dados das etiquetas para um novo objeto com a imagem convertida para base64
            var etiquetasComImagemBase64 = etiquetas.Select(e => new
            {
                Id = e.Id,
                Nome = e.Nome,
                Descricao = e.Descricao,
                Imagem = Convert.ToBase64String(e.Imagem) // Convertendo a imagem para uma string base64
            }).ToList();

            return Json(etiquetasComImagemBase64);
        }

        [HttpGet]
        public IActionResult ModalFitas()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarNovaFita(FitaViewModel _request)
        {
            return Redirect("/Home/Entrar");
        }
    }
}

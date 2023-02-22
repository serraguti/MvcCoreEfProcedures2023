using Microsoft.AspNetCore.Mvc;
using MvcCoreEfProcedures.Models;
using MvcCoreEfProcedures.Repositories;

namespace MvcCoreEfProcedures.Controllers
{
    public class TrabajadoresController : Controller
    {
        private RepositoryTrabajadores repo;

        public TrabajadoresController(RepositoryTrabajadores repo)
        {
            this.repo = repo;
        }

        public IActionResult TrabajadoresOficio()
        {
            List<Trabajador> trabajadores =
                this.repo.GetTrabajadores();
            List<string> oficios = this.repo.GetOficios();
            ViewData["OFICIOS"] = oficios;
            ViewData["TRABAJADORES"] = trabajadores;
            return View();
        }

        [HttpPost]
        public IActionResult TrabajadoresOficio(string oficio)
        {
            TrabajadoresModel model =
                this.repo.GetTrabajadoresOficio(oficio);
            List<string> oficios = this.repo.GetOficios();
            ViewData["OFICIOS"] = oficios;
            return View(model);
        }
    }
}

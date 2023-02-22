using Microsoft.AspNetCore.Mvc;
using MvcCoreEfProcedures.Models;
using MvcCoreEfProcedures.Repositories;

namespace MvcCoreEfProcedures.Controllers
{
    public class EmpleadosVistaController : Controller
    {
        private RepositoryVistaEmpleados repo;

        public EmpleadosVistaController(RepositoryVistaEmpleados repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<VistaEmpleado> empleados = this.repo.GetEmpleados();
            return View(empleados);
        }
    }
}

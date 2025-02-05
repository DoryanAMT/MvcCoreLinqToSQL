using Microsoft.AspNetCore.Mvc;
using MvcCoreLinqToSQL.Models;
using MvcCoreLinqToSQL.Repositories;

namespace MvcCoreLinqToSQL.Controllers
{
    public class EmpleadosController : Controller
    {
        RepositoryEmpleado repo;
        public EmpleadosController()
        {
            this.repo = new RepositoryEmpleado();
        }
        public IActionResult Index()
        {
            List<Empleado> empleados = this.repo.GetEmpleados();
            return View(empleados);
        }
        public IActionResult Details
            (int idEmpleado)
        {
            Empleado empleado = this.repo.FindeEmpleado(idEmpleado);
            return View(empleado);
        }
    }
}

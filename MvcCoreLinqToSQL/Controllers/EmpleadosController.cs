using Azure.Core;
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
        public IActionResult BuscadorEmpleados()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BuscadorEmpleados
            (string oficio, int salario)
        {
            List<Empleado> empleados = this.repo.GetEmpleadosOficiosSalario(oficio, salario);
            if (empleados == null){
                ViewData["MENSAJE"] = "No existe empleados con oficio "
                    + oficio + " y salario a " + salario;
            }
            return View(empleados);
        }
        //-------------------- ResumeEmpleados --------------------------
        [HttpGet]
        public IActionResult EmpleadosOficio()
        {
            ViewData["OFICIOS"] = this.repo.GetOficios();
            return View();
        }
        [HttpPost]
        public IActionResult EmpleadosOficio
            (string oficio)
        {
            ViewData["OFICIOS"] = this.repo.GetOficios();
            ResumenEmpleados resumen = this.repo.GetEmpleadosOficio(oficio);
            return View(resumen);
        }


    }
}

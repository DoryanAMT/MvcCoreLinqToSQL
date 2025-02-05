using Microsoft.Data.SqlClient;
using MvcCoreLinqToSQL.Models;
using System.Data;

namespace MvcCoreLinqToSQL.Repositories
{
    public class RepositoryEmpleado
    {
        private DataTable tablaEmpleados;
        public RepositoryEmpleado()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Encrypt=True;Trust Server Certificate=True";
            string sql = "select * from EMP";
            SqlDataAdapter adEmp = new SqlDataAdapter(sql, connectionString);
            this.tablaEmpleados = new DataTable();
            //  RECUPERAMOS LOS DATOS
            adEmp.Fill(this.tablaEmpleados);
        }

        //  METODO PARA RECUPERAR TODOS LOS EMPLEADOS

        public List<Empleado> GetEmpleados()
        {
            //  LAS CONSULTAS LINQ SE ALMACENAN EN GENERICOS de tipo (var)
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           select datos;
            //  AHORA MISMO TENEMOS DENTRO DE LA CONSULTA LA INFORMACION DE 
            //  LA TABLA EMPLEADOS
            //  EN ESTE EJEMPLO TENEMOS OBJETO DataRow QUE SON FILAS
            //  DENTRO DE LA TABLA
            //  DEBEMOS RECORRER DICHAS FILAS Y EXTRAER LA INFORMACIO´N
            //  EN OBJETOS DE TIPO empleado
            List<Empleado> empleados = new List<Empleado>();
            //  RECORREMOS CADA FILA DE LA CONSULTA
            foreach (var row in consulta)
            {
                Empleado emp = new Empleado();
                //  PARA EXTRAER DATOS DE UN DataRow
                //  DataRow.Field<Tipo>("COLUMNA")
                emp.IdEmpleado = row.Field<int>("EMP_NO");
                emp.Apellido = row.Field<string>("APELLIDO");
                emp.Oficio = row.Field<string>("OFICIO");
                emp.Salario = row.Field<int>("SALARIO");
                emp.IdDepartamento = row.Field<int>("DEPT_NO");
                empleados.Add(emp);
            }
            return empleados;
        }

        //  METODO PARA BUSCAR EMPLEADOS POR SU ID
        public Empleado FindeEmpleado
            (int idEmpleado)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<int>("DEPT_NO") == idEmpleado
                           select datos;
            //  NOSOTROS SABEMOS QUE ESTA CONSULTA DEVUELVE UNA FILA
            //  LINQ SIEMPRE DEVUELVE UNA COLECCION
            //  DENTRO DEL CONJUNTO TENEMOS METODOS LAMBDA QUE NOS
            //  PERMITEN REALIZAR COSITAS
            //  TENEMOS UN METODO QUE NOS DEVUELVE EL PRIMER VALOR DEL CONJUNTO
            //  first()
            var row = consulta.First();
            Empleado emp = new Empleado();
            emp.IdEmpleado = row.Field<int>("EMP_NO");
            emp.Apellido = row.Field<string>("APELLIDO");
            emp.Oficio= row.Field<string>("OFICIO");
            emp.Salario = row.Field<int>("SALARIO");
            emp.IdDepartamento = row.Field<int>("DEPT_NO");
            return emp;
        }
        public List<Empleado> GetEmpleadosOficiosSalario
            (string oficio, int salario)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           && datos.Field<int>("SALARIO") >= salario
                           select datos;
            //  DEBEMOS COMPROBAR SI TENEMOS DATOS O NO....
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Empleado> empleados = new List<Empleado>();
                foreach (var row in consulta)
                {
                    Empleado empleado = new Empleado
                    {
                        IdEmpleado = row.Field<int>("EMP_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Oficio = row.Field<string>("OFICIO"),
                        Salario = row.Field<int>("SALARIO"),
                        IdDepartamento = row.Field<int>("DEPT_NO")
                    };
                    empleados.Add(empleado);
                }
                return empleados;
            }
            
        }
        
    }
}

using Azure.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using MvcCoreLinqToSQL.Models;
using System.Data;
using System.Diagnostics.Metrics;

namespace MvcCoreLinqToSQL.Repositories
{
    #region
    //create or alter procedure SP_GET_ENFERMO_INS
    //(@inscripcion as nvarchar(50))
    //as
    //SELECT* FROM ENFERMO
    //WHERE INSCRIPCION=@inscripcion
    //go
    #endregion
    public class RepositoryEnfermo
    {
        private DataTable tablaEnfermo;
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;
        public RepositoryEnfermo()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Encrypt=True;Trust Server Certificate=True";
            string sql = "select * from ENFERMO";
            SqlDataAdapter adEnf = new SqlDataAdapter(sql, connectionString);
            this.tablaEnfermo = new DataTable();
            adEnf.Fill(this.tablaEnfermo);

            //ado
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Enfermo> GetEnfermos()
        {
            var consulta = from datos in this.tablaEnfermo.AsEnumerable()
                           select datos;
            List<Enfermo> enfermos = new List<Enfermo>();
            foreach (var row in consulta)
            {
                Enfermo enfermo = new Enfermo();
                enfermo.Inscripcion = row.Field<string>("INSCRIPCION");
                enfermo.Apellido = row.Field<string>("APELLIDO");
                enfermo.Direccion = row.Field<string>("DIRECCION");
                enfermo.FechaNac = row.Field<DateTime>("FECHA_NAC");
                enfermo.S = row.Field<string>("S");
                enfermo.NSS = row.Field<string>("NSS");
                enfermos.Add(enfermo);
            }
            return enfermos;
        }

        public Enfermo FindEnfermo
            (string inscripcion)
        {
            string sql = "select * from ENFERMO where INSCRIPCION=@inscripcion";
            this.com.Parameters.AddWithValue("@inscripcion", inscripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();
            Enfermo enfermo = new Enfermo();
            enfermo.Inscripcion = this.reader["INSCRIPCION"].ToString();
            enfermo.Apellido = this.reader["APELLIDO"].ToString();
            enfermo.Direccion = this.reader["DIRECCION"].ToString();
            enfermo.FechaNac = (DateTime)this.reader["FECHA_NAC"];
            enfermo.S = this.reader["S"].ToString();
            enfermo.NSS = this.reader["NSS"].ToString();
            this.com.Parameters.Clear();
            this.cn.Close();
            this.reader.Close();
            return enfermo;
        }
        public void DeleteEnfermo
            (string inscripcion)
        {
            string sql = "delete from ENFERMO where INSCRIPCION=@inscripcion";
            this.com.Parameters.AddWithValue("@inscripcion", inscripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}


#region PROCEDIMIENTOS ALMACENADOS
//CREATE PROCEDURE SP_TODOS_ENFERMOS
//AS
//	SELECT * FROM ENFERMO
//GO
//CREATE PROCEDURE SP_BUSCAR_ENFERMO
//(@INSCRIPCION INT)
//AS
//	SELECT * FROM ENFERMO
//	WHERE INSCRIPCION = @INSCRIPCION
//GO
//CREATE PROCEDURE SP_DELETE_ENFERMO
//(@INSCRIPCION INT)
//AS
//	DELETE FROM ENFERMO WHERE INSCRIPCION = @INSCRIPCION
//GO

#endregion

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCoreEfProcedures.Data;
using MvcCoreEfProcedures.Models;
using System.Data;
using System.Data.Common;

namespace MvcCoreEfProcedures.Repositories
{
    public class RepositoryEnfermos
    {
        private EnfermosContext context;

        public RepositoryEnfermos(EnfermosContext context)
        {
            this.context = context;
        }

        public List<Enfermo> GetEnfermos()
        {
            //PARA LLAMAR A PROCEDIMIENTOS ALMACENADOS
            //CON CONSULTAS SELECT DEBEMOS EXTRAER LOS 
            //DATOS DE LA CONEXION DE EF
            using (DbCommand com =
                this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_TODOS_ENFERMOS";
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = sql;
                com.Connection.Open();
                DbDataReader reader = com.ExecuteReader();
                List<Enfermo> enfermos = new List<Enfermo>();
                while (reader.Read())
                {
                    Enfermo enfermo = new Enfermo
                    {
                        Inscripcion = int.Parse(reader["INSCRIPCION"].ToString()),
                        Apellido = reader["APELLIDO"].ToString(),
                        Direccion = reader["DIRECCION"].ToString(),
                        FechaNacimiento =
                            DateTime.Parse(reader["FECHA_NAC"].ToString()),
                        Sexo = reader["S"].ToString()
                    };
                    enfermos.Add(enfermo);
                }
                reader.Close();
                com.Connection.Close();
                return enfermos;
            }
        }

        public Enfermo FindEnfermo(int inscripcion)
        {
            //PARA LLAMAR A LOS PROCEDIMIENTOS QUE CONTENGAN
            //PARAMETROS DEBEMOS REALIZAR LA CONSULTA INCLUYENDO
            //LOS NOMBRES DE PARAMETRO
            // SP_PROCEDURE @PARAM1, @PARAM2
            string sql = "SP_BUSCAR_ENFERMO @INSCRIPCION";
            //LOS PARAMETROS SON LOS MISMOS QUE EN 
            //ADO .NET SqlParameter
            //EL NAMESPACE Microsoft.Data
            SqlParameter paminscripcion =
                new SqlParameter("@INSCRIPCION", inscripcion);
            //AL SER UN PROCEDIMIENTO SELECT, PUEDEO UTILIZAR 
            //EL METODO FromSqlRaw PARA EXTRAER LOS DATOS
            //DICHO METODO SE APLICA SOBRE EL DbSet QUE DESEAMOS 
            //EXTRAER
            //CUANDO UTILIZAMOS PROCEDIMIENTOS NO PODEMOS EJECUTAR 
            //EL PROCEDIMIENTO Y EXTRAER LOS DATOS A LA VEZ
            var consulta = 
                this.context.Enfermos.FromSqlRaw(sql, paminscripcion);
            //EXTRAEMOS LAS ENTIDADES
            Enfermo enfermo = consulta.AsEnumerable().FirstOrDefault();
            return enfermo;
        }

        public void DeleteEnfermo(int inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO @INSCRIPCION";
            SqlParameter paminscripcion =
                new SqlParameter("@INSCRIPCION", inscripcion);
            //PARA EJECUTAR CONSULTAS DE ACCION EN UN PROCEDURE
            //SE UTILIZA EL METODO ExecuteSqlRaw() Y VIENE DESDE
            // Database
            this.context.Database.ExecuteSqlRaw(sql, paminscripcion);
        }
    }
}

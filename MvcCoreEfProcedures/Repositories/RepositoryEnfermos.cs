
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
    }
}


#region PROCEDIMIENTOS ALMACENADOS
//CREATE PROCEDURE SP_TODOS_DOCTORES
//AS
//	SELECT * FROM DOCTOR
//GO
//CREATE PROCEDURE SP_ESPECIALIDADES
//AS
//	SELECT DISTINCT ESPECIALIDAD FROM DOCTOR
//GO
//CREATE PROCEDURE SP_DOCTORES_ESPECIALIDAD
//(@ESPECIALIDAD NVARCHAR(50))
//AS
//	SELECT * FROM DOCTOR WHERE ESPECIALIDAD = @ESPECIALIDAD
//GO
//CREATE PROCEDURE SP_INCREMENTAR_DOCTORES
//(@ESPECIALIDAD NVARCHAR(50), @INCREMENTO INT)
//AS
//	UPDATE DOCTOR SET SALARIO = SALARIO + @INCREMENTO
//	WHERE ESPECIALIDAD=@ESPECIALIDAD
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
    public class RepositoryDoctores
    {
        private EnfermosContext context;

        public RepositoryDoctores(EnfermosContext context)
        {
            this.context = context;
        }

        public List<Doctor> GetDoctores()
        {
            string sql = "SP_TODOS_DOCTORES";
            var consulta = this.context.Doctores.FromSqlRaw(sql);
            List<Doctor> doctores = consulta.AsEnumerable().ToList();
            return doctores;
        }

        public List<string> GetEspecialidades()
        {
            string sql = "SP_ESPECIALIDADES";
            using (DbCommand com = this.context.Database.GetDbConnection()
                .CreateCommand())
            {
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = sql;
                com.Connection.Open();
                DbDataReader reader = com.ExecuteReader();
                List<string> especialidades = new List<string>();
                while (reader.Read())
                {
                    string espe = reader["ESPECIALIDAD"].ToString();
                    especialidades.Add(espe);
                }
                reader.Close();
                com.Connection.Close();
                return especialidades;
            }
        }

        public List<Doctor> GetDoctoresEspecialidad(string especialidad)
        {
            string sql = "SP_DOCTORES_ESPECIALIDAD @ESPECIALIDAD";
            SqlParameter pamespe = new SqlParameter("@ESPECIALIDAD", especialidad);
            var consulta = this.context.Doctores.FromSqlRaw(sql, pamespe);
            List<Doctor> doctores = consulta.AsEnumerable().ToList();
            return doctores;
        }


        public async Task IncrementarSalarioDoctoresAsync
            (string especialidad, int incremento)
        {
            string sql = "SP_INCREMENTAR_DOCTORES @ESPECIALIDAD, @INCREMENTO";
            SqlParameter pamespe = new SqlParameter("@ESPECIALIDAD", especialidad);
            SqlParameter pamincremento = new SqlParameter("@INCREMENTO", incremento);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamespe, pamincremento);
        }
    }
}

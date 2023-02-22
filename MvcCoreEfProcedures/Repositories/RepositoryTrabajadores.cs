using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCoreEfProcedures.Data;
using MvcCoreEfProcedures.Models;
using System.Data;

#region VISTAS Y PROCEDIMIENTOS ALMACENADOS
//CREATE VIEW V_TRABAJADORES
//AS
//	SELECT ISNULL(EMP_NO, 0) AS IDTRABAJADOR
//    , APELLIDO, OFICIO, SALARIO 
//	FROM EMP
//	UNION
//	SELECT DOCTOR_NO, APELLIDO, ESPECIALIDAD, SALARIO
//	FROM DOCTOR
//	UNION
//	SELECT EMPLEADO_NO, APELLIDO, FUNCION, SALARIO
//	FROM PLANTILLA
//GO
//CREATE PROCEDURE SP_TRABAJADORES_OFICIO
//(@OFICIO NVARCHAR(50), @PERSONAS INT OUT
//, @MEDIA INT OUT, @SUMA INT OUT)
//AS
//	SELECT * FROM V_TRABAJADORES
//	WHERE OFICIO = @OFICIO
//	SELECT @PERSONAS = COUNT(IDTRABAJADOR)
//	, @MEDIA = AVG(SALARIO), @SUMA = SUM(SALARIO)

//    FROM V_TRABAJADORES
//	WHERE OFICIO = @OFICIO
//GO
#endregion

namespace MvcCoreEfProcedures.Repositories
{
    public class RepositoryTrabajadores
    {
        private HospitalContext context;

        public RepositoryTrabajadores(HospitalContext context)
        {
            this.context = context;
        }

        public List<Trabajador> GetTrabajadores()
        {
            var consulta = from datos in this.context.Trabajadores
                           select datos;
            return consulta.ToList();
        }

        public List<string> GetOficios()
        {
            var consulta = (from datos in this.context.Trabajadores
                            select datos.Oficio).Distinct();
            return consulta.ToList();
        }

        public TrabajadoresModel GetTrabajadoresOficio(string oficio)
        {
            //LOS PARAMETROS DE SALIDA DEBEN LLEVAR LA PALABRA 
            //OUT EN LA EJECUCION DE LA CONSULTA
            string sql = "SP_TRABAJADORES_OFICIO @OFICIO, @PERSONAS OUT "
                + ", @MEDIA OUT, @SUMA OUT";
            SqlParameter pamoficio = new SqlParameter("@OFICIO", oficio);
            //LOS PARAMETROS DE SALIDA DEBEN LLEVAR UN VALOR POR DEFECTO
            SqlParameter pampersonas = new SqlParameter("@PERSONAS", -1);
            SqlParameter pammedia = new SqlParameter("@MEDIA", -1);
            SqlParameter pamsuma = new SqlParameter("@SUMA", -1);
            //DEBEMOS INDICAR LA DIRECCION DEL PARAMETRO
            pampersonas.Direction = ParameterDirection.Output;
            pammedia.Direction = ParameterDirection.Output;
            pamsuma.Direction = ParameterDirection.Output;
            var consulta =
                this.context.Trabajadores.FromSqlRaw(sql, pamoficio
                , pampersonas, pammedia, pamsuma);
            TrabajadoresModel model = new TrabajadoresModel();
            model.Trabajadores = consulta.ToList();
            model.Personas = int.Parse(pampersonas.Value.ToString());
            model.MediaSalarial = int.Parse(pammedia.Value.ToString());
            model.SumaSalarial = int.Parse(pamsuma.Value.ToString());
            return model;
        }
    }
}

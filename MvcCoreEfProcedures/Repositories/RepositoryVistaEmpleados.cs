using MvcCoreEfProcedures.Data;
using MvcCoreEfProcedures.Models;

namespace MvcCoreEfProcedures.Repositories
{
    public class RepositoryVistaEmpleados
    {
        private HospitalContext context;

        public RepositoryVistaEmpleados(HospitalContext context)
        {
            this.context = context;
        }

        public List<VistaEmpleado> GetEmpleados()
        {
            var consulta = from datos in this.context.VistaEmpleados
                           select datos;
            return consulta.ToList();
        }
    }
}

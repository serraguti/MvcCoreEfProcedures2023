using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCoreEfProcedures.Models
{
    [Table("V_EMPLEADOS_DEPT")]
    public class VistaEmpleado
    {
        [Key]
        [Column("CLAVE")]
        public int IdKey { get; set; }
        [Column("APELLIDO")]
        public string Apellido { get; set; }
        [Column("OFICIO")]
        public string Oficio { get; set; }
        [Column("NOMBRE")]
        public string Departamento { get; set; }
        [Column("LOCALIDAD")]
        public string Localidad { get; set; }
    }
}

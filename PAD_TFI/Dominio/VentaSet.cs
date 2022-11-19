namespace PAD_TFI.Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VentaSet")]
    public partial class VentaSet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VentaSet()
        {
            LineaVentaSet = new HashSet<LineaVentaSet>();
            PagoSet = new HashSet<PagoSet>();
        }

        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public int Empleado_Id { get; set; }

        public int Cliente_Id { get; set; }

        public virtual ClienteSet ClienteSet { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LineaVentaSet> LineaVentaSet { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PagoSet> PagoSet { get; set; }

        public virtual VendedorSet VendedorSet { get; set; }
    }
}

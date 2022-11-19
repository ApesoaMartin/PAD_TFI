namespace PAD_TFI.Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DireccionSet")]
    public partial class DireccionSet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DireccionSet()
        {
            ClienteSet = new HashSet<ClienteSet>();
        }

        public int Id { get; set; }

        [Required]
        public string Calle { get; set; }

        public short Numero { get; set; }

        public short Piso { get; set; }

        public byte Dpto { get; set; }

        public int Localidad_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClienteSet> ClienteSet { get; set; }

        public virtual LocalidadSet LocalidadSet { get; set; }
    }
}

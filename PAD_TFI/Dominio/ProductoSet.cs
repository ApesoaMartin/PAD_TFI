namespace PAD_TFI.Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductoSet")]
    public partial class ProductoSet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductoSet()
        {
            DescuentoSet = new HashSet<DescuentoSet>();
        }

        public int Id { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public int Stock { get; set; }

        public decimal PrecioUnitario { get; set; }

        [Required]
        public string SKU { get; set; }

        public string Imagen { get; set; }

        public int Categoria_Id { get; set; }

        public int Marca_Id { get; set; }

        public virtual CategoriaSet CategoriaSet { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DescuentoSet> DescuentoSet { get; set; }

        public virtual MarcaSet MarcaSet { get; set; }
    }
}

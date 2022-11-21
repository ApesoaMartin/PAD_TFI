namespace PAD_TFI.Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DescuentoSet")]
    public partial class DescuentoSet
    {
        public int Id { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public decimal Porcentaje { get; set; }

        public short CompraMinima { get; set; }

		public string Descripcion { get; set; }

        public int? Producto_Id { get; set; }

        public int? Categoria_Id { get; set; }

        public virtual CategoriaSet CategoriaSet { get; set; }

        public virtual ProductoSet ProductoSet { get; set; }
    }
}

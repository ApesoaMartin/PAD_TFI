namespace PAD_TFI.Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LineaVentaSet")]
    public partial class LineaVentaSet
    {
        public int Id { get; set; }

        public short Cantidad { get; set; }

        public int Venta_Id { get; set; }

        public virtual VentaSet VentaSet { get; set; }
    }
}

namespace PAD_TFI.Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PagoSet")]
    public partial class PagoSet
    {
        public int Id { get; set; }

        [Required]
        public string Tipo { get; set; }

        public string NroTarjeta { get; set; }

        public DateTime Fecha { get; set; }

        public int PagoVenta_Pago_Id { get; set; }

        public virtual VentaSet VentaSet { get; set; }
    }
}

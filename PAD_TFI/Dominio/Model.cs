namespace PAD_TFI.Dominio {
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class BaseDeDatos : DbContext {
		public BaseDeDatos()
			: base("name=Model") {
		}

		public virtual DbSet<CategoriaSet> CategoriaSet { get; set; }
		public virtual DbSet<ClienteSet> ClienteSet { get; set; }
		public virtual DbSet<DescuentoSet> DescuentoSet { get; set; }
		public virtual DbSet<DireccionSet> DireccionSet { get; set; }
		public virtual DbSet<LineaVentaSet> LineaVentaSet { get; set; }
		public virtual DbSet<LocalidadSet> LocalidadSet { get; set; }
		public virtual DbSet<MarcaSet> MarcaSet { get; set; }
		public virtual DbSet<PagoSet> PagoSet { get; set; }
		public virtual DbSet<ProductoSet> ProductoSet { get; set; }
		public virtual DbSet<ProvinciaSet> ProvinciaSet { get; set; }
		public virtual DbSet<VendedorSet> VendedorSet { get; set; }
		public virtual DbSet<VentaSet> VentaSet { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			modelBuilder.Entity<CategoriaSet>()
				.HasMany(e => e.ProductoSet)
				.WithRequired(e => e.CategoriaSet)
				.HasForeignKey(e => e.Categoria_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<CategoriaSet>()
				.HasMany(e => e.DescuentoSet)
				.WithOptional(e => e.CategoriaSet)
				.HasForeignKey(e => e.Categoria_Id);

			modelBuilder.Entity<ClienteSet>()
				.HasMany(e => e.VentaSet)
				.WithRequired(e => e.ClienteSet)
				.HasForeignKey(e => e.Cliente_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<DescuentoSet>()
				.Property(e => e.Porcentaje)
				.HasPrecision(18, 0);

			modelBuilder.Entity<DireccionSet>()
				.HasMany(e => e.ClienteSet)
				.WithRequired(e => e.DireccionSet)
				.HasForeignKey(e => e.Direccion_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<LocalidadSet>()
				.HasMany(e => e.DireccionSet)
				.WithRequired(e => e.LocalidadSet)
				.HasForeignKey(e => e.Localidad_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<MarcaSet>()
				.HasMany(e => e.ProductoSet)
				.WithRequired(e => e.MarcaSet)
				.HasForeignKey(e => e.Marca_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ProductoSet>()
				.Property(e => e.PrecioUnitario)
				.HasPrecision(18, 0);

			modelBuilder.Entity<ProductoSet>()
				.HasMany(e => e.DescuentoSet)
				.WithOptional(e => e.ProductoSet)
				.HasForeignKey(e => e.Producto_Id);

			modelBuilder.Entity<ProvinciaSet>()
				.HasMany(e => e.LocalidadSet)
				.WithRequired(e => e.ProvinciaSet)
				.HasForeignKey(e => e.Provincia_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<VendedorSet>()
				.HasMany(e => e.VentaSet)
				.WithRequired(e => e.VendedorSet)
				.HasForeignKey(e => e.Empleado_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<VentaSet>()
				.HasMany(e => e.LineaVentaSet)
				.WithRequired(e => e.VentaSet)
				.HasForeignKey(e => e.Venta_Id)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<VentaSet>()
				.HasMany(e => e.PagoSet)
				.WithRequired(e => e.VentaSet)
				.HasForeignKey(e => e.PagoVenta_Pago_Id)
				.WillCascadeOnDelete(false);
		}
	}
}

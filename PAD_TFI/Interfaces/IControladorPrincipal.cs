namespace PAD_TFI.Interfaces {
	public interface IControladorPrincipal {

		string[] CargarCategorias();
		void CargarTabla();

		void FiltrarTabla(string categoria);
		void VincularPagina(IPrincipal pagina);

		void AgregarACarrito(int productoId, int cantidad);
		void QuitarDeCarrito(int productoId, int cantidad);
		int GetCantidad(int productoId);

	}
}

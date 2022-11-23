using PAD_TFI.Dominio;
using System.Collections.Generic;

namespace PAD_TFI.Interfaces {
	public interface IControladorPrincipal {

		string[] CargarCategorias();
		void CargarTabla();

		void FiltrarTabla(string categoria);
		void VincularPagina(IPrincipal pagina);

		void AgregarACarrito(int productoId, int cantidad);
		void QuitarDeCarrito(int productoId, int cantidad);
		int GetCantidad(int productoId);
		void CalcularDescuento(ProductoSet producto, int cantidad,
				out decimal precioTotal, out decimal descuentoTotal, out int porcentajeDescuento);

		Dictionary<int, int> ObtenerCarrito();
		string ObtenerUrlPaginaPrincipal();

		void VerificarEstadoCompra();
	}
}

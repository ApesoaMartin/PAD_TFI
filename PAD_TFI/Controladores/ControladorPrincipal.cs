using PAD_TFI.Dominio;
using PAD_TFI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAD_TFI.Controladores {
	public class ControladorPrincipal : IControladorPrincipal{

		#region Singleton
		private static volatile ControladorPrincipal _instance;
		private static readonly object _syncLock = new object();

		public ControladorPrincipal() {
			_carrito = new Dictionary<int, int>();
		}

		public static IControladorPrincipal Instance {
			get {
				if (_instance != null) return _instance;
				lock (_syncLock) {
					if (_instance == null) {
						_instance = new ControladorPrincipal();
					}
				}
				return _instance;
			}
		}
		#endregion

		private readonly Dictionary<int, int> _carrito;

		private const int COLUMNAS = 5;

		private IPrincipal _principal;

		public void VincularPagina(IPrincipal pagina) {
			_principal = pagina;
		}

		public string[] CargarCategorias() {
			using (var bd = new BaseDeDatos()) {
				return (from cat in bd.CategoriaSet select cat.Nombre).ToArray();
			}
		}

		public void CargarTabla() {
			using (var bd = new BaseDeDatos()) {

				LlenarTabla(bd.ProductoSet.Include("MarcaSet").Include("DescuentoSet").Include("CategoriaSet"));
			}
		}

		public void CalcularDescuento(ProductoSet producto, int cantidad,
				out decimal precioTotal, out decimal descuentoTotal, out int porcentajeDescuento) {
			var fechaActual = DateTime.Now;

			precioTotal = producto.PrecioUnitario * cantidad;

			descuentoTotal = 0;

			foreach (var desc in producto.DescuentoSet
									.Where((d) => d.FechaInicio <= fechaActual && d.FechaFin >= fechaActual)) {
				descuentoTotal += (producto.PrecioUnitario * desc.Porcentaje / 100.0m) * (cantidad / desc.CompraMinima);
			}

			porcentajeDescuento = (int)(100 * descuentoTotal / precioTotal);
			precioTotal -= descuentoTotal;
		}

		private void LlenarTabla(IEnumerable<ProductoSet> productos) {
			_principal.IniciarTabla(COLUMNAS);
			int cantidad = 0;
			string info;
			string avisoStock;
			foreach (var prod in productos) {
				avisoStock = null;
				cantidad = GetCantidad(prod.Id);
				if (cantidad >= prod.Stock) {
					cantidad = prod.Stock;
					_carrito[prod.Id] = cantidad;
					avisoStock = "Sin stock";
				}
				info = "";
				foreach (var desc in prod.DescuentoSet) {
					info += $"{desc.Descripcion}\n";
				}
				if (prod.DescuentoSet.Count > 0) {
					CalcularDescuento(prod, Math.Max(1, cantidad), out decimal precioTotal, out decimal descuentoTotal, out int porcentajeDescuento);
					if (descuentoTotal > 0) {
						_principal.AgregarCelda(
							prod.Id,
							prod.Imagen,
							prod.Descripcion,
							prod.MarcaSet.Nombre,
							"$" + prod.PrecioUnitario.ToString("N"),
							cantidad,
							info,
							avisoStock,
							"$" + precioTotal.ToString("N"),
							string.Format("(-{0}%)", porcentajeDescuento)
						);
					} else {
						_principal.AgregarCelda(
							prod.Id,
							prod.Imagen,
							prod.Descripcion,
							prod.MarcaSet.Nombre,
							"$" + (prod.PrecioUnitario * Math.Max(1, cantidad)).ToString("N"),
							cantidad,
							info,
							avisoStock
						);
					}
				} else {
					_principal.AgregarCelda(
						prod.Id,
						prod.Imagen,
						prod.Descripcion,
						prod.MarcaSet.Nombre,
						"$" + (prod.PrecioUnitario * Math.Max(1, cantidad)).ToString("N"),
						cantidad,
						info,
						avisoStock
					);
				}
			}
			_principal.TerminarTabla();
		}

		public void FiltrarTabla(string categoria) {
			using (var bd = new BaseDeDatos()) {
				_principal.IniciarTabla(COLUMNAS);

				LlenarTabla(bd.ProductoSet
							  .Include("MarcaSet")
							  .Include("CategoriaSet")
							  .Where((x) => x.CategoriaSet.Nombre == categoria));

				_principal.TerminarTabla();
			}
		}

		public void AgregarACarrito(int productoId, int cantidad) {
			if (_carrito.ContainsKey(productoId)) {
				_carrito[productoId] += cantidad;
			} else {
				_carrito[productoId] = cantidad;
			}
		}

		public void QuitarDeCarrito(int productoId, int cantidad) {
			if (_carrito.ContainsKey(productoId)) {
				if (_carrito[productoId] > cantidad) {
					_carrito[productoId] -= cantidad;
				} else {
					_carrito.Remove(productoId);
				}
			}
		}

		public int GetCantidad(int productoId) {
			return _carrito.ContainsKey(productoId) ? _carrito[productoId] : 0;
		}

        public Dictionary<int, int> ObtenerCarrito()
        {
			return _carrito;
        }

        public string ObtenerUrlPaginaPrincipal()
        {
			return HttpContext.Current.Request.Url.AbsoluteUri;

		}
		  
        public void VerificarEstadoCompra()
        {
			if (ControladorCarrito.Instance.CompraCompletada())
			{
				//_carrito.Clear();

			}


        }
    }
}
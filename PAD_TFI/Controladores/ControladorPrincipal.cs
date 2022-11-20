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

				LlenarTabla(bd.ProductoSet.Include("MarcaSet").Include("DescuentoSet"));
			}
		}

		private void LlenarTabla(IEnumerable<ProductoSet> productos) {
			_principal.IniciarTabla(COLUMNAS);

			foreach (var prod in productos) {
				if (prod.DescuentoSet.Count > 0) {
					var fechaActual = DateTime.Now;
					decimal precioMP = 1;
					foreach (var desc in prod.DescuentoSet
											.Where((d) => d.FechaInicio <= fechaActual && d.FechaFin >= fechaActual)) {
						//TODO: tener en cuenta cantidades
						precioMP -= (desc.Porcentaje / 100.0m) * precioMP;
					}
					var precio = prod.PrecioUnitario * precioMP;
					int descuento = (int)((1 - precioMP) * 100);
					_principal.AgregarCelda(
						prod.Id,
						prod.Imagen,
						prod.Descripcion,
						prod.MarcaSet.Nombre,
						"$" + prod.PrecioUnitario.ToString("N"),
						GetCantidad(prod.Id),
						"$" + precio.ToString("N"),
						string.Format("(-{0}%)", descuento)
					);
				} else {
					_principal.AgregarCelda(
						prod.Id,
						prod.Imagen,
						prod.Descripcion,
						prod.MarcaSet.Nombre,
						"$" + prod.PrecioUnitario.ToString("N"),
						GetCantidad(prod.Id)
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
	}
}
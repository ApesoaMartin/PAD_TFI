using PAD_TFI.Dominio;
using PAD_TFI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAD_TFI.Controladores {
	public class ControladorPrincipal : IControladorPrincipal{

		private const int COLUMNAS = 5;

		private readonly IPrincipal _principal;

		public ControladorPrincipal(IPrincipal pagina) {
			_principal = pagina;
		}

		public string[] CargarCategorias() {
			using (var bd = new BaseDeDatos()) {
				return (from cat in bd.CategoriaSet select cat.Nombre).ToArray();
			}
		}

		public void CargarTabla() {
			using (var bd = new BaseDeDatos()) {

				_principal.IniciarTabla(COLUMNAS);

				foreach (var prod in bd.ProductoSet.Include("MarcaSet").Include("DescuentoSet")) {
					
					/*if (prod.DescuentoSet.Count > 0) {
						var fechaActual = DateTime.Now;
						double precioMP = 1;
						foreach (var desc in prod.DescuentoSet
												.Where((d) => d.FechaInicio >= fechaActual && d.FechaFin <= fechaActual)) {
							precioMP -= (int)desc.Porcentaje / 100.0d;
						}
					}*/

					_principal.AgregarCelda(
						prod.Imagen,
						prod.Descripcion,
						prod.MarcaSet.Nombre,
						"$" + prod.PrecioUnitario.ToString("N")
					);
				}

				_principal.TerminarTabla();
			}
		}

		public void FiltrarTabla(string categoria) {
			using (var bd = new BaseDeDatos()) {
				_principal.IniciarTabla(COLUMNAS);

				foreach (var prod in bd.ProductoSet
									   .Include("MarcaSet")
									   .Include("CategoriaSet")
									   .Where((x) => x.CategoriaSet.Nombre == categoria)) {
					_principal.AgregarCelda(
						prod.Imagen,
						prod.Descripcion,
						prod.MarcaSet.Nombre,
						prod.PrecioUnitario.ToString("N")
					);
				}

				_principal.TerminarTabla();
			}
		}
	}
}
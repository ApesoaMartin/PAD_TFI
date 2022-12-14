using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD_TFI.Interfaces {
	public interface IControladorCarrito {

		List<string> ObtenerProvincias();

		List<string> ObtenerLocalidades();
		void BuscarIDProvincia(int index);

		void BuscarIDLocalidad(int index);

		bool ConfirmarCompra();

		void SetearVista(ICarrito vista);

		void ObtenerCarrito();

		void CargarListadoDeProductos();

		bool ObtenerEstadoDePago();

		string ObtenerURLPago();

		void ErrorEnElPago();

		bool CompraCompletada();

		void IntentoDePago();
	}
}

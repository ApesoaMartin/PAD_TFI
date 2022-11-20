using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD_TFI.Interfaces {
	public interface ICarrito {

		string ObtenerNombre();

		string ObtenerApellido();
		string ObtenerDNI();
		string ObtenerTelefono();

		string ObtenerCorreo();
		string ObtenerCalle();
		string ObtenerAltura();
		string ObtenerPiso();
		string ObtenerDpto();

	}
}

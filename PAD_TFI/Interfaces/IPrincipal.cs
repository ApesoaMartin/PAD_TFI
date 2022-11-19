using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD_TFI.Interfaces {
	public interface IPrincipal {

		void IniciarTabla(int columnas);

		void AgregarCelda(string imageUrl, string descripcion, string marca, string precio, string descuento=null);

		void TerminarTabla();
	}
}

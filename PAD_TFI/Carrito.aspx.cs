using PAD_TFI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PAD_TFI {
	public partial class Carrito : System.Web.UI.Page, ICarrito {

		private IControladorCarrito _controlador;

		protected void Page_Load(object sender, EventArgs e) {

		}
	}
}
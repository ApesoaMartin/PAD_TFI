using System;
using PAD_TFI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PAD_TFI {
	public partial class _Default : Page, IPrincipal {

		private IControladorPrincipal _controlador;

		protected void Page_Load(object sender, EventArgs e) {

		}
	}
}
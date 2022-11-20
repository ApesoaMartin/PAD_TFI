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

		protected void Page_Load(object sender, EventArgs e) 
		{
			List<string> localidades = new List<string> { "","0","1","2", "3","4", "5" };
			List<string> provincias = new List<string> { "","A", "B", "C", "D", "E", "F" };
			//foreach (var item in datos)
			//{
			//    localidadSL.Items.Add(item);
			//}
			localidadSL.DataSource = localidades;
			provinciaSL.DataSource = provincias;
			Page.DataBind();
		}


    }
}
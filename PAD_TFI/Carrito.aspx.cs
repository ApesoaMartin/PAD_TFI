using PAD_TFI.Controladores;
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

        public Carrito()
        {
			_controlador = ControladorCarrito.Instance;
            
        }
                
		protected void Page_Load(object sender, EventArgs e) 
		{
            _controlador.SetearVista(this);
            if (!IsPostBack)
            {
                PagarPanel.Visible = false;
                provinciaSL.DataSource = _controlador.ObtenerProvincias();
                localidadSL.Enabled = false;
                Page.DataBind();
            }
            if (IsPostBack)
            {

            }
            
		}

        protected void localidadSL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (localidadSL.SelectedIndex != 0)
            {
                _controlador.BuscarIDLocalidad(localidadSL.SelectedIndex);
            }
        }

        protected void provinciaSL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (provinciaSL.SelectedIndex != 0)
            {
                _controlador.BuscarIDProvincia(provinciaSL.SelectedIndex);
                localidadSL.DataSource = _controlador.ObtenerLocalidades();
                Page.DataBind();
                localidadSL.Enabled = true;
            }
            else
            {
                localidadSL.Enabled = false;
            }
        }

        protected void confirmacionBTN_Click(object sender, EventArgs e)
        {
            bool resultado = _controlador.ConfirmarCompra();
            if (!resultado)
            {
                Response.Write("alert('Campos Invalidos')");
            }
            else
            {
                pagarBTN.PostBackUrl = _controlador.ObtenerURLPago();
                PagarPanel.Visible = true;
            }
        }

        public string ObtenerNombre()
        {
            return nombreTB.Text;
        }

        public string ObtenerApellido()
        {
            return apellidoTB.Text;
        }

        public string ObtenerDNI()
        {
            return dniTB.Text;
        }

        public string ObtenerTelefono()
        {
            return telefonoTB.Text;
        }

        public string ObtenerCorreo()
        {
            return correoTB.Text;
        }

        public string ObtenerCalle()
        {
            return calleTB.Text;
        }

        public string ObtenerAltura()
        {
            return alturaTB.Text;
        }

        public string ObtenerPiso()
        {
            return pisoTB.Text;
        }

        public string ObtenerDpto()
        {
            return dptoTB.Text;
        }
    }
}
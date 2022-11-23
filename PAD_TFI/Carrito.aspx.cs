using PAD_TFI.Controladores;
using PAD_TFI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PAD_TFI {
	public partial class Carrito : System.Web.UI.Page, ICarrito {

		private IControladorCarrito _controlador;

        public Carrito()
        {
			_controlador = ControladorCarrito.Instance;
            _controlador.SetearVista(this);

        }
                
		protected void Page_Load(object sender, EventArgs e) 
		{
            
            AgregarCabeceras();
            _controlador.ObtenerCarrito();
            _controlador.CargarListadoDeProductos();
            if (!IsPostBack)
            {
                if (!_controlador.ObtenerEstadoDePago())
                {
                    ConfirmacionPanel.Visible = true;
                    PagarPanel.Visible = false;
                    fallaPagoHD.Visible = false;
                    compraConfirmadaHD.Visible = true;
                }
                else
                {
                    _controlador.ErrorEnElPago();
                    pagarBTN.PostBackUrl = _controlador.ObtenerURLPago();
                    ConfirmacionPanel.Visible = false;
                    compraConfirmadaHD.Visible = false;
                    fallaPagoHD.Visible = true;
                    PagarPanel.Visible = true;
                }
                
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
            if (!_controlador.ObtenerEstadoDePago())
            {
                bool resultado = _controlador.ConfirmarCompra();
                if (!resultado)
                {

                    lblError.Text = "Por favor revisar los campos";
                }
                else
                {
                    lblError.Text = "";
                    ConfirmacionPanel.Visible = false;
                    PagarPanel.Visible = true;

                }
            }
            else
            {
                lblError.Text = "Pago de compra previa Pendiente";
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

        public void SetearURLPago(string url)
        {
            pagarBTN.PostBackUrl = url;
        }
        public void AgregarCabeceras()
        {
            TableRow fila = new TableRow();

            TableCell imagenProducto = new TableCell();
            var div = new HtmlGenericControl("div");
            div.Controls.Add(new Label() { Text = "Imagen" });
            imagenProducto.Controls.Add(div);
            fila.Cells.Add(imagenProducto);

            TableCell descripcionProducto = new TableCell();
            var div1 = new HtmlGenericControl("div");
            div1.Controls.Add(new Label() { Text = "Descripción" });
            descripcionProducto.Controls.Add(div1);
            fila.Cells.Add(descripcionProducto);


            TableCell precioUnitarioProducto = new TableCell();
            var div2 = new HtmlGenericControl("div");
            div2.Controls.Add(new Label() { Text = "Precio Unitario" });
            precioUnitarioProducto.Controls.Add(div2);
            fila.Cells.Add(precioUnitarioProducto);

			TableCell cantidadProducto = new TableCell();
			var div4 = new HtmlGenericControl("div");
			div4.Controls.Add(new Label() { Text = "Unidades" });
			cantidadProducto.Controls.Add(div4);
			fila.Cells.Add(cantidadProducto);

			TableCell descuentoProducto = new TableCell();
            var div3 = new HtmlGenericControl("div");
            div3.Controls.Add(new Label() { Text = "Descuento" });
            descuentoProducto.Controls.Add(div3);
            fila.Cells.Add(descuentoProducto);

            TableCell totalProducto = new TableCell();
            var div5 = new HtmlGenericControl("div");
            div5.Controls.Add(new Label() { Text = "Subtotal" });
            totalProducto.Controls.Add(div5);
            fila.Cells.Add(totalProducto);

            ProductsTable.Rows.Add(fila);
        }
        public void AgregarProducto(string urlImagen, string desccripcion, string precioUnitario,string descuento, string cantidad, string precioFinal)
        {
            TableRow fila = new TableRow();

            TableCell imagenProducto = new TableCell();
            System.Web.UI.WebControls.Image imagen = new System.Web.UI.WebControls.Image() { ImageUrl = urlImagen, Width = new Unit(64, UnitType.Pixel), Height = new Unit(64, UnitType.Pixel), ImageAlign = ImageAlign.Middle };
            imagenProducto.Controls.Add(imagen);
            
            fila.Cells.Add(imagenProducto);

            TableCell descripcionProducto = new TableCell();
            var div = new HtmlGenericControl("div");
            div.Controls.Add(new Label() { Text = desccripcion });
            descripcionProducto.Controls.Add(div);
            fila.Cells.Add(descripcionProducto);


            TableCell precioUnitarioProducto = new TableCell();
            var div2 = new HtmlGenericControl("div");
            div2.Controls.Add(new Label() { Text = precioUnitario });
            precioUnitarioProducto.Controls.Add(div2);
            fila.Cells.Add(precioUnitarioProducto);

			TableCell cantidadProducto = new TableCell();
			var div4 = new HtmlGenericControl("div");
			div4.Controls.Add(new Label() { Text = cantidad });
			cantidadProducto.Controls.Add(div4);
			fila.Cells.Add(cantidadProducto);

			TableCell descuentoProducto = new TableCell();
            var div3 = new HtmlGenericControl("div");
            div3.Controls.Add(new Label() { Text = descuento });
            descuentoProducto.Controls.Add(div3);
            fila.Cells.Add(descuentoProducto);

            TableCell totalProducto = new TableCell();
            var div5 = new HtmlGenericControl("div");
            div5.Controls.Add(new Label() { Text = precioFinal });
            totalProducto.Controls.Add(div5);
            fila.Cells.Add(totalProducto);

            ProductsTable.Rows.Add(fila);
        }

        public void ActualizarPrecioFinal(string total)
        {
            TableRow fila = new TableRow();
            TableCell costoTotal = new TableCell();
            var div = new HtmlGenericControl("div");
            div.Controls.Add(new Label() { Text = $"Total a Pagar = {total}" });
            costoTotal.Controls.Add(div);
            fila.Cells.Add(costoTotal);

            ProductsTable.Rows.Add(fila);
        }

        protected void pagarBTN_Click(object sender, EventArgs e)
        {
            
        }
    }
}
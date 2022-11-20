using System;
using PAD_TFI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;

namespace PAD_TFI {
	public partial class _Default : Page, IPrincipal {

		private int _columnas, _ultimaPosicion;
		private TableRow _ultimaFila;
		private string categoria;

		public string[] Categorias { get; set; }

		private IControladorPrincipal _controlador;

		protected void Page_Load(object sender, EventArgs e) {
			categoria = Request["cat"];
			_controlador = Controladores.ControladorPrincipal.Instance;
			_controlador.VincularPagina(this);
			_columnas = 0;
			_ultimaPosicion = 0;
			_ultimaFila = null;

			if (!Page.IsPostBack) {
				CategoriaRepeater.DataSource = _controlador.CargarCategorias();
				CategoriaRepeater.DataBind();
			}

			var action = Request["a"];
			var id = Request["i"];
			if (action == "p") {
				_controlador.AgregarACarrito(int.Parse(id), 1);
			} else if (action == "m") {
				_controlador.QuitarDeCarrito(int.Parse(id), 1);
			}

			if (action != null) {
				//Elimina la ultima accion de la URL.
				//Esto previene que se repita la accion al recargar la página.
				Response.Redirect(categoria == null ? "~/Default" : $"~/Default?cat={categoria}");
			} else {
				if (categoria == null)
					_controlador.CargarTabla();
				else
					_controlador.FiltrarTabla(categoria);
			}
		}

		protected void Categoria_Click(object sender, EventArgs e) {
			Response.Redirect("~/Default?cat=" + (sender as Button).Text);
		}

		public void IniciarTabla(int columnas) {
			_columnas = columnas;
			tablaProductos.Rows.Clear();
			_ultimaPosicion = 0;
			_ultimaFila = new TableRow();
		}

		public void AgregarCelda(int pId, string imageUrl, string descripcion, string marca, string precio, int cantidad, string precioConDescuento=null, string descuento = null) {
			var celda = new TableCell();
			var div = new HtmlGenericControl("div");

			div.Controls.Add(new System.Web.UI.WebControls.Image() { ImageUrl = imageUrl, Width = new Unit(80, UnitType.Percentage), ImageAlign = ImageAlign.Middle });

			div.Controls.Add(new HtmlGenericControl("br"));
			div.Controls.Add(new Label() { Text = descripcion });

			div.Controls.Add(new HtmlGenericControl("br"));
			div.Controls.Add(new Label() { Text = marca });

			if (descuento == null) {
				div.Controls.Add(new HtmlGenericControl("br"));
				div.Controls.Add(new Label() { Text = precio });
			} else {
				div.Controls.Add(new HtmlGenericControl("br"));
				var tempLabel = new Label() { Text = precio };
				tempLabel.ForeColor = System.Drawing.Color.Red;
				tempLabel.Font.Strikeout = true;
				div.Controls.Add(tempLabel);

				div.Controls.Add(new HtmlGenericControl("br"));
				div.Controls.Add(new Label() { Text = precioConDescuento });

				div.Controls.Add(new Label() { Width = new Unit(25, UnitType.Pixel) });

				tempLabel = new Label() { Text = descuento };
				tempLabel.ForeColor = System.Drawing.Color.Green;
				tempLabel.Font.Bold = true;
				div.Controls.Add(tempLabel);
			}

			div.Controls.Add(new HtmlGenericControl("br"));

			var botones = new HtmlGenericControl("div");

			var tempLink = new HyperLink() {
				ID = $"Minus{pId}",
				Text = "-",
				ForeColor=Color.Black,
				Width = new Unit(25, UnitType.Pixel),
				BackColor = Color.Red,
				NavigateUrl = categoria == null ? $"~/Default?a=m&i={pId}" : $"~/Default?cat={categoria}&a=m&i={pId}"
			};
			tempLink.Style.Add("text-align", "center");
			tempLink.Font.Bold = true;
			tempLink.Font.Size = new FontUnit(FontSize.Large);

			botones.Controls.Add(tempLink);

			botones.Controls.Add(new Label() { Width=new Unit(50, UnitType.Pixel) });

			botones.Controls.Add(new Label() { Text = cantidad.ToString() });

			botones.Controls.Add(new Label() { Width = new Unit(50, UnitType.Pixel) });

			tempLink = new HyperLink() {
				ID = $"Plus{pId}",
				Text = "+",
				Width = new Unit(25, UnitType.Pixel),
				BackColor = Color.Green,
				ForeColor = Color.White,
				NavigateUrl = categoria == null ? $"~/Default?a=p&i={pId}" : $"~/Default?cat={categoria}&a=p&i={pId}"
			};
			tempLink.Style.Add("text-align", "center");
			tempLink.Font.Bold = true;
			tempLink.Font.Size = new FontUnit(FontSize.Large);

			botones.Controls.Add(tempLink);

			div.Controls.Add(botones);

			celda.Width = 200;
			celda.BorderStyle = BorderStyle.Solid;
			celda.BorderWidth = 2;
			celda.BorderColor = System.Drawing.Color.Black;
			celda.Controls.Add(div);

			_ultimaFila.Cells.Add(celda);

			_ultimaPosicion++;
			if (_ultimaPosicion == _columnas) {
				_ultimaPosicion = 0;
				tablaProductos.Rows.Add(_ultimaFila);
				_ultimaFila = new TableRow();
			}
		}

		protected void BtnLimpiarFiltro_Click(object sender, EventArgs e) {
			Response.Redirect("~/Default");
		}

		public void TerminarTabla() {
			if (_ultimaPosicion != 0)
				tablaProductos.Rows.Add(_ultimaFila);
			_ultimaFila = null;
			_ultimaPosicion = 0;
		}
	}
}
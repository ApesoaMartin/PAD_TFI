using System;
using PAD_TFI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using PAD_TFI.Controladores;

namespace PAD_TFI {
	public partial class _Default : Page, IPrincipal {

		private int _columnas, _ultimaPosicion;
		private TableRow _ultimaFila;
		private string categoria;

		public string[] Categorias { get; set; }

		private IControladorPrincipal _controlador;

		protected void Page_Load(object sender, EventArgs e) {
			categoria = Request["cat"];
			_controlador = ControladorPrincipal.Instance;
			_controlador.VincularPagina(this);
			_columnas = 0;
			_ultimaPosicion = 0;
			_ultimaFila = null;

			if (!Page.IsPostBack) {
				CategoriaRepeater.DataSource = _controlador.CargarCategorias();
				CategoriaRepeater.DataBind();
				_controlador.VerificarEstadoCompra();
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

		private HtmlGenericControl GetLineBreak() {
			var space = new HtmlGenericControl("div");
			space.Attributes.Add("class", "lineBreak");
			return space;
		}

		public void AgregarCelda(int pId, string imageUrl, string descripcion, string marca, string precio, int cantidad, string infoDescuento, string precioConDescuento=null, string descuento = null) {
			var celda = new TableCell();
			var div = new HtmlGenericControl("div");

			if (!string.IsNullOrWhiteSpace(infoDescuento))
				div.Controls.Add(new Label() { Text = infoDescuento, CssClass = "prodInfoDescuento" });

			div.Controls.Add(new System.Web.UI.WebControls.Image() {
				ImageUrl = imageUrl,
				Width = new Unit(80, UnitType.Percentage),
				ImageAlign = ImageAlign.Middle,
				CssClass = "productoImg"
			});

			div.Controls.Add(GetLineBreak());
			div.Controls.Add(new Label() { Text = descripcion, CssClass="prodDescripcion" });
			div.Controls.Add(GetLineBreak());
			div.Controls.Add(new Label() { Text = marca, CssClass = "prodMarca" });
			div.Controls.Add(GetLineBreak());
			if (descuento == null) {
				div.Controls.Add(new Label() { Text = precio, CssClass = "prodPrecio" });
			} else {
				var tempLabel = new Label() { Text = precio, CssClass = "prodPrecioDescontado" };
				tempLabel.ForeColor = System.Drawing.Color.Red;
				tempLabel.Font.Strikeout = true;
				div.Controls.Add(tempLabel);

				tempLabel = new Label() { Text = descuento, CssClass = "prodDescuento" };
				tempLabel.ForeColor = System.Drawing.Color.Green;
				tempLabel.Font.Bold = true;
				div.Controls.Add(tempLabel);

				div.Controls.Add(GetLineBreak());
				div.Controls.Add(new Label() { Text = precioConDescuento, CssClass = "prodPrecioConDescuento" });
			}

			div.Controls.Add(GetLineBreak());

			var botones = new HtmlGenericControl("div");

			var tempLink = new HyperLink() {
				ID = $"Minus{pId}",
				Text = "-",
				CssClass="botonMenos",
				ForeColor=Color.Black,
				Width = new Unit(25, UnitType.Pixel),
				BackColor = Color.Red,
				NavigateUrl = categoria == null ? $"~/Default?a=m&i={pId}" : $"~/Default?cat={categoria}&a=m&i={pId}"
			};

			tempLink.Style.Add("text-align", "center");
			tempLink.Font.Bold = true;
			tempLink.Font.Size = new FontUnit(FontSize.Large);

			botones.Controls.Add(tempLink);

			botones.Controls.Add(new Label() { Text = cantidad.ToString(), CssClass="prodCantidad"});

			tempLink = new HyperLink() {
				ID = $"Plus{pId}",
				Text = "+",
				CssClass = "botonMas",
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

			celda.CssClass = "celdaProducto";
			celda.Width = 200;
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
using System;
using PAD_TFI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace PAD_TFI {
	public partial class _Default : Page, IPrincipal {

		private int _columnas, _ultimaPosicion;
		private TableRow _ultimaFila;

		public string[] Categorias { get; set; }

		private IControladorPrincipal _controlador;

		protected void Page_Load(object sender, EventArgs e) {
			_controlador = new Controladores.ControladorPrincipal(this);
			_columnas = 0;
			_ultimaPosicion = 0;
			_ultimaFila = null;

			if (!Page.IsPostBack) {
				CategoriaRepeater.DataSource = _controlador.CargarCategorias();
				CategoriaRepeater.DataBind();
				_controlador.CargarTabla();
			}
		}

		protected void Categoria_Click(object sender, EventArgs e) {
			_controlador.FiltrarTabla((sender as Button).Text);
		}

		public void IniciarTabla(int columnas) {
			_columnas = columnas;
			tablaProductos.Rows.Clear();
			_ultimaPosicion = 0;
			_ultimaFila = new TableRow();
		}

		public void AgregarCelda(string imageUrl, string descripcion, string marca, string precio, string descuento = null) {
			var celda = new TableCell();
			var div = new HtmlGenericControl("div");

			div.Controls.Add(new ImageButton() { ImageUrl = imageUrl, Width = new Unit(100, UnitType.Percentage), ImageAlign = ImageAlign.Middle });

			div.Controls.Add(new HtmlGenericControl("br"));
			div.Controls.Add(new Label() { Text = descripcion });

			div.Controls.Add(new HtmlGenericControl("br"));
			div.Controls.Add(new Label() { Text = marca });

			div.Controls.Add(new HtmlGenericControl("br"));
			div.Controls.Add(new Label() { Text = precio });

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
			_controlador.CargarTabla();
		}

		public void TerminarTabla() {
			if (_ultimaPosicion != 0)
				tablaProductos.Rows.Add(_ultimaFila);
			_ultimaFila = null;
			_ultimaPosicion = 0;
		}
	}
}
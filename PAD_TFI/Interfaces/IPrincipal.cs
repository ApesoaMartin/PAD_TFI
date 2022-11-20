namespace PAD_TFI.Interfaces {
	public interface IPrincipal {

		void IniciarTabla(int columnas);

		void AgregarCelda(int pId, string imageUrl, string descripcion, string marca, string precio, int cantidad, string precioConDescuento = null, string descuento = null);

		void TerminarTabla();
	}
}

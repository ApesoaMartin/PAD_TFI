using PAD_TFI.Dominio;
using PAD_TFI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;

namespace PAD_TFI.Controladores {
	public class ControladorCarrito : IControladorCarrito {

		private ICarrito _vista;

        private int _provinciaId;
        private int _localidadId;

        private List<string> _provincias;
        private List<string> _localidades = new List<string>();

        private Dictionary<int, int> _carrito;

        #region Singleton

        private static volatile ControladorCarrito _instance;
        private static readonly object _syncLock = new object();

        public ControladorCarrito()
        {
            MercadoPagoConfig.AccessToken = "TEST-6986966527076860-111318-4cc8718b374ab28a1fb4700b2a7c21ba-72090181";
        }

        public static ControladorCarrito Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncLock)
                {
                    if (_instance == null)
                    {
                        _instance = new ControladorCarrito();
                    }
                }
                return _instance;
            }
        }


        #endregion

        public void SetearVista(ICarrito vista)
        {
            _vista = vista;
        }

        public void ObtenerCarrito()
        {
            _carrito = ControladorPrincipal.Instance.ObtenerCarrito();
        }


        public List<string> ObtenerProvincias()
        {
            if(_provincias != null)_provincias.Clear();
            using (var bd = new BaseDeDatos())
            {
                _provincias = (from prov in bd.ProvinciaSet orderby prov.Nombre ascending select prov.Nombre).ToList();
                _provincias.Insert(0,"");
                return _provincias;
            }
        }

        public List<string> ObtenerLocalidades()
        {
            _localidades.Clear();
            using (var bd = new BaseDeDatos())
            {
                _localidades = (from loc in bd.LocalidadSet where loc.Provincia_Id == _provinciaId select loc.Nombre).ToList();
                _localidades.Insert(0, "");
                return _localidades;
            }
        }

        public void BuscarIDProvincia(int index)
        {
            if(index != 0)
            {
                using (var bd = new BaseDeDatos())
                {
                    string nombreSeleccion = _provincias[index];
                    List<int> consulta = (from prov in bd.ProvinciaSet where prov.Nombre == nombreSeleccion  select prov.Id).ToList();
                    _provinciaId = consulta[0];
                }
            }
            
        }

        public void BuscarIDLocalidad(int index)
        {
            _localidadId = -99;
            if (index != 0)
            {
                using (var bd = new BaseDeDatos())
                {
                    string nombreSeleccion = _localidades[index];
                    List<int> consulta = (from loc in bd.LocalidadSet where loc.Nombre == nombreSeleccion select loc.Id).ToList();
                    _localidadId = consulta[0];
                }
            }
        }

        public bool ConfirmarCompra()
        {
            string nombre = _vista.ObtenerNombre();
            string apellido = _vista.ObtenerApellido();
            string dni = _vista.ObtenerDNI();
            string telefono = _vista.ObtenerTelefono();
            string correo = _vista.ObtenerCorreo();
            string calle = _vista.ObtenerCalle();
            string altura = _vista.ObtenerAltura();

            if(!string.IsNullOrWhiteSpace(nombre)
				&& !string.IsNullOrWhiteSpace(apellido)
				&& !string.IsNullOrWhiteSpace(dni)
				&& !string.IsNullOrWhiteSpace(telefono)
				&& !string.IsNullOrWhiteSpace(correo)
				&& !string.IsNullOrWhiteSpace(calle)
				&& !string.IsNullOrWhiteSpace(altura))
            {
                string piso = _vista.ObtenerPiso();
                string dpto = _vista.ObtenerDpto();
                if (_localidadId >= 0)
                {
                    InsertarCompraEnDB(nombre,apellido,dni,telefono, correo,calle,altura,piso,dpto);
                    CrearPagoEnMercadoLibre();
                }
                

                return true;
            }
            else
            {
                return false;
            }
            
        }

        private void InsertarCompraEnDB(string nombre, string apellido, string dni, string telefono, string correo, string calle, string altura, string piso, string dpto)
        {
            
            using (var bd = new BaseDeDatos())
            { 
                LocalidadSet localidad = bd.LocalidadSet.Find(_localidadId);
                DireccionSet direccion = new DireccionSet();
                direccion.Calle = calle;
                direccion.Numero = (short)int.Parse(altura);
                if(piso!= "")direccion.Piso = (short)int.Parse(piso);
                if(dpto != "")direccion.Dpto = byte.Parse(dpto);
                direccion.LocalidadSet = localidad;

                bd.DireccionSet.Add(direccion);
                bd.SaveChanges();


            }
            using (var bd = new BaseDeDatos())
            {
                DireccionSet direccion =(from dir in bd.DireccionSet orderby dir.Id descending select dir).ToList()[0];
                ClienteSet cliente = new ClienteSet();
                cliente.Nombres = nombre;
                cliente.Apellidos = apellido;
                cliente.DNI = dni;
                cliente.Telefono = telefono;
                cliente.Correo = correo;
                cliente.DireccionSet = direccion;
                bd.ClienteSet.Add(cliente);
                bd.SaveChanges();


            }
        }

        private void CrearPagoEnMercadoLibre()
        {
            List<PreferenceItemRequest> items = new List<PreferenceItemRequest>();

            using (var bd = new BaseDeDatos())
            {
                foreach (var item in _carrito)
                {
                    ProductoSet producto = bd.ProductoSet.Find(item.Key);

                    PreferenceItemRequest itemRequest = new PreferenceItemRequest
                    {
                        Title = producto.Descripcion,
                        Quantity = item.Value,
                        CurrencyId = "ARS",
                        UnitPrice = producto.PrecioUnitario,
                    };
                    items.Add(itemRequest);
                }
            }
                


            var request = new PreferenceRequest
            {
                Items = items,
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = HttpContext.Current.Request.Url.AbsoluteUri,
                },
            };

            // Crea la preferencia usando el client
            var client = new PreferenceClient();
            Preference preference = client.Create(request);

            _vista.SetearURLPago(preference.InitPoint);
        }

        public void CargarListadoDeProductos()
        {
			decimal costoTotal = 0.0m;
            using (var bd = new BaseDeDatos())
            {
				foreach (var item in _carrito) {
					ProductoSet producto = bd.ProductoSet.Find(item.Key);
					decimal precioFinalInicial = producto.PrecioUnitario * item.Value;
					ControladorPrincipal.Instance.CalcularDescuento(producto, item.Value, out decimal precioFinal, out decimal descuentoTotal, out int porcentaje);

					costoTotal += precioFinal;

					_vista.AgregarProducto(producto.Imagen, producto.Descripcion, $"$ {producto.PrecioUnitario.ToString("N")}", $"$ {descuentoTotal.ToString("N")}", item.Value.ToString(), $"$ {precioFinal.ToString("N")}");
				}

            }
            _vista.ActualizarPrecioFinal($"$ {costoTotal.ToString("N")}");
        }
    }
}
using PAD_TFI.Dominio;
using PAD_TFI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAD_TFI.Controladores {
	public class ControladorCarrito : IControladorCarrito {

		private ICarrito _carrito;

        private int _provinciaId;
        private int _localidadId;

        private List<string> _provincias;
        private List<string> _localidades = new List<string>();

        #region Singleton

        private static volatile ControladorCarrito _instance;
        private static readonly object _syncLock = new object();

        public ControladorCarrito()
        {

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
            _carrito = vista;
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
            string nombre = _carrito.ObtenerNombre();
            string apellido = _carrito.ObtenerApellido();
            string dni = _carrito.ObtenerDNI();
            string telefono = _carrito.ObtenerTelefono();
            string correo = _carrito.ObtenerCorreo();
            string calle = _carrito.ObtenerCalle();
            string altura = _carrito.ObtenerAltura();
            if(nombre != "" && apellido != "" && dni != "" && telefono != "" && correo != "" && calle != "" && altura != "")
            {
                string piso = _carrito.ObtenerPiso();
                string dpto = _carrito.ObtenerDpto();
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
                direccion.Piso = (short)int.Parse(piso);
                direccion.Dpto = byte.Parse(dpto);
                direccion.LocalidadSet = localidad;

                bd.DireccionSet.Add(direccion);
                bd.SaveChanges();


            }
            using (var bd = new BaseDeDatos())
            {
                DireccionSet direccion =  bd.DireccionSet.Last();
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

        }


        public string ObtenerURLPago()
        {
            //terminar
            return null;
        }
    }
}
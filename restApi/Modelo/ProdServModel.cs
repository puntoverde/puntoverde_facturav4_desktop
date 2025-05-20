
using contpaqSDK;
using restApi.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace restApi.Modelo
{
    class ProdServModel
    {
        public static List<ProductoServicio> lst_prod_serv()
        {
            List<ProductoServicio> lista = new List<ProductoServicio>();
            int IError;
            IError=Producto.fPosPrimerProducto();
            if (IError == 0)//se posiciona en el primer registro
            {
                do {
                    StringBuilder codigo = new StringBuilder(12);
                    StringBuilder nombre = new StringBuilder(12);
                    StringBuilder precio = new StringBuilder();
                    StringBuilder iva = new StringBuilder();
                    Producto.fLeeDatoProducto("CCODIGOPRODUCTO", codigo,8);
                    Producto.fLeeDatoProducto("CNOMBREPRODUCTO", nombre, 40);
                    Producto.fLeeDatoProducto("CCOSTOESTANDAR", precio, 10);
                    Producto.fLeeDatoProducto("CESEXENTO", iva, 10);
                    lista.Add(new ProductoServicio() { codigo_prod_serv = codigo.ToString(), nombre_prod_serv = nombre.ToString() , precio = precio.ToString(), iva = iva.ToString() });
                }while(Producto.fPosSiguienteProducto() == 0);
            }
            else {
            SDK.MuestraError(IError, "fAltaDocumento");
            }

            return lista;
        }

        public static ProductoServicio getProducto(string codigo_producto) {

            int IError;
            IError = Producto.fBuscaProducto(codigo_producto);
            if (IError == 0)
            {
                StringBuilder codigo = new StringBuilder(12);
                StringBuilder nombre = new StringBuilder(12);
                StringBuilder precio = new StringBuilder();
                StringBuilder iva = new StringBuilder();
                Producto.fLeeDatoProducto("CCODIGOPRODUCTO", codigo, 8);
                Producto.fLeeDatoProducto("CNOMBREPRODUCTO", nombre, 40);
                Producto.fLeeDatoProducto("CCOSTOESTANDAR", precio, 10);
                Producto.fLeeDatoProducto("CESEXENTO", iva, 10); 
                return new ProductoServicio() { codigo_prod_serv = codigo.ToString(), nombre_prod_serv = nombre.ToString(),precio=precio.ToString(),iva=iva.ToString()};
            }
            else {
                SDK.MuestraError(IError, "fBuscaProducto");
                return null;
            }
        }
    }
}

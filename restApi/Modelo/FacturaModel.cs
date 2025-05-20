using contpaqSDK;
using restApi.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**va en la parte de alta movimiento*/
//el trece es el concepto donde se carga el documento de abono y el AC es la serie de esos documento de abono que son los que saldan
////IError = Documento.fAfectaDocto_Param("13", "AC", folio_documento, true);
////if (IError == 0) { Console.WriteLine("Documento creado..."); }
////else { SDK.MuestraError(IError, "fAfectaDocto_Param"); }
///

namespace restApi.Modelo
{
    class FacturaModel
    {

        public static int updateCliente(string codigo_cliente, string razon_social, string rfc, string curp, string forma_pago, string uso_cfdi,string email)
        {
            int IError;//variable para mostrar los errores

            //asignacion del cliente con lo que se trae de la factura
            Clientes.tCteProv cliente = new Clientes.tCteProv();
            cliente.cRazonSocial = razon_social;
            cliente.cRFC = rfc;
            cliente.cCURP = curp;
            cliente.cNombreMoneda = Convert.ToString("Peso Mexicano");
            cliente.cTipoCliente = Convert.ToInt32("1");
            cliente.cLimiteCreditoCliente = Convert.ToInt32("100000");
            cliente.cBanVentaCredito = Convert.ToInt32("1");

            //se actualiza el cliente 
            IError = Clientes.fActualizaCteProv(codigo_cliente, ref cliente);
            if (IError == 0)//si es 0 se actualizo correctamente
            {
                Console.WriteLine("SE ACTUALIZA EL CLIENTE:"+ codigo_cliente);
                //habilita la modificacion del cliente
                IError = Clientes.fEditaCteProv();
                if (IError == 0)
                {
                    Console.WriteLine("HABILITA EDICION DEL CLIENTE:"+ codigo_cliente);
                    //se agrega el correo al cliente
                    IError = Clientes.fSetDatoCteProv("CEMAIL1", email);
                    //modifica el dato de metodo pago
                    IError = Clientes.fSetDatoCteProv("CMETODOPAG", forma_pago);
                    //modifica el dato de uso cfdi
                    IError = Clientes.fSetDatoCteProv("CUSOCFDI", uso_cfdi);
                    //guarda los cambios en el cliente
                    IError = Clientes.fGuardaCteProv();
                }
            }

            return IError;
        }

        public static int insertCliente(string codigo_cliente, string razon_social, string rfc, string curp, string forma_pago, string uso_cfdi,string email) {
            int IError;//variable para mostrar los errores
            int idCliente = 0;

            //asignacion del cliente con lo que se trae de la factura
            Clientes.tCteProv cliente = new Clientes.tCteProv();
            cliente.cCodigoCliente = codigo_cliente;
            cliente.cRazonSocial = razon_social;
            cliente.cRFC = rfc;
            cliente.cCURP = curp;
            cliente.cNombreMoneda = Convert.ToString("Peso Mexicano");
            cliente.cTipoCliente = Convert.ToInt32("1");
            cliente.cLimiteCreditoCliente = Convert.ToInt32("100000");
            cliente.cBanVentaCredito = Convert.ToInt32("1");

            IError = Clientes.fAltaCteProv(ref idCliente, ref cliente);
            if (IError == 0)//en cero significa que el cliente se dio de alta correctamente...
            {
                Console.WriteLine("INSERTA EL CLIENTE:"+codigo_cliente);
                //habilita la modificacion del cliente
                IError = Clientes.fEditaCteProv();
                if (IError == 0)
                {
                    Console.WriteLine("HABILITA EDICION DEL CLIENTE:" + codigo_cliente+" EN CUANDO SE AGREGA");
                    //se agrega el correo al cliente
                    IError = Clientes.fSetDatoCteProv("CEMAIL1", email);
                    //modifica el dato de metodo pago
                    IError = Clientes.fSetDatoCteProv("CMETODOPAG", forma_pago);
                    //modifica el dato de uso cfdi
                    IError = Clientes.fSetDatoCteProv("CUSOCFDI", uso_cfdi);
                    //guarda los cambios en el cliente
                    IError = Clientes.fGuardaCteProv();
                }
            }

            return IError;
        }

        public static int updateDomicilio(string codigo_cliente, string calle, string num_ext, string num_int, string colonia, string cp, string ciudad, string estado, string pais)
        {
            int IError;

            Direccion.tDireccion direccion = new Direccion.tDireccion();
            direccion.cCodCteProv = codigo_cliente;
            direccion.cNombreCalle = calle;
            direccion.cNumeroExterior = num_ext;
            direccion.cNumeroInterior = num_int;
            direccion.cColonia = colonia;
            direccion.cCodigoPostal = cp;
            direccion.cCiudad = ciudad;
            direccion.cEstado = estado;
            direccion.cPais = pais;
            direccion.cTipoDireccion = 1;//0 es fiscal ,1 es envio  aunque es al reves ???

            //como la direccion existe se prosigue a actualizarla 
            IError = Direccion.fActualizaDireccion(ref direccion);

            return IError;
        }

        public static int insertDomicilio(string codigo_cliente, string calle, string num_ext, string num_int, string colonia, string cp, string ciudad, string estado, string pais) {
            int IError;
            int idDireccion = 0;

            Direccion.tDireccion direccion = new Direccion.tDireccion();
            direccion.cCodCteProv = codigo_cliente;
            direccion.cNombreCalle = calle;
            direccion.cNumeroExterior = num_ext;
            direccion.cNumeroInterior = num_int;
            direccion.cColonia = colonia;
            direccion.cCodigoPostal = cp;
            direccion.cCiudad = ciudad;
            direccion.cEstado = estado;
            direccion.cPais = pais;

            //se registra ladireccion
            IError = Direccion.fAltaDireccion(ref idDireccion, ref direccion);

            return IError;
        }
        
        public static string[] crearDocumento(string codigo_cliente, List<Documentos> cargos, string observaciones,string metodo_pago,string fecha_saldar,string folio_pv) {
            //posicion1= folio, posicion2=codigoError,posicion3=estado en compaq,posicion4=uuid 
            string[] response = { "0", "0", "0", "0" };
            int IErrorMovimiento=0;//espara la suma de los errores de los movimientos
            int IError;
            int idDocumento = 0;
            string idConcepto = "42018";//este concepto es fijo a noser que lo quiran cambiar...
            StringBuilder serie = new StringBuilder(12);
            double ifolio = 0;

            IError = Documento.fSiguienteFolio(idConcepto, serie, ref ifolio);
            if (IError == 0)
            {
                Documento.tDocumento docu = new Documento.tDocumento();
                docu.aCodConcepto = idConcepto;
                docu.aSerie = serie.ToString();
                docu.aFolio = ifolio;
                docu.aFecha = DateTime.Today.ToString("MM/dd/yyyy");
                docu.aCodigoCteProv = codigo_cliente;
                docu.aCodigoAgente = "(Ninguno)";
                docu.aSistemaOrigen = 1;
                docu.aTipoCambio = 1;
                docu.aNumMoneda = 1;
                docu.aImporte = 0;
                docu.aDescuentoDoc1 = 0;
                docu.aDescuentoDoc2 = 0;
                docu.aAfecta = 0;

                IError = Documento.fAltaDocumento(ref idDocumento, ref docu);
                if (IError == 0)
                {
                    //se asigna el folio al array para regresarlo esto indica que se creo un documento
                    response[0] = ifolio.ToString();
                    //estado en comercial que indica que se cuenta con un documento
                    response[2] = "1";

                    //SE CREA TXT DE LOGS
                    System.IO.StreamWriter log_pv = new System.IO.StreamWriter(@"C:\Compac\LOGS_PV\"+ ifolio.ToString() + "-log.txt");
                    log_pv.WriteLine("SE CREA CON EXITO EL DOCUMENTO CON FOLIO:"+ifolio.ToString());
                    log_pv.WriteLine("ESTADO DE FACTURA EN (" + response[2] + ") SIGNIFICA QUE EXISTE DOCUMENTO EN COMPAC");
                    log_pv.WriteLine("---------------------------------------------------------------");
                    log_pv.WriteLine("INICIA INSERCION DE MOVIMIENTOS");
                    log_pv.WriteLine("---------------------------------------------------------------");
                    log_pv.Close();//se cierra el archivo

                    Console.WriteLine("SE CREA CON EXITO EL DOCUMENTO CON FOLIO:" + ifolio.ToString());                                      
                    Console.WriteLine("ESTADO DE FACTURA EN ("+response[2]+") SIGNIFICA QUE EXISTE DOCUMENTO EN COMPAC");
                    Console.WriteLine("---------------------------------------------------------------");
                    Console.WriteLine("INICIA INSERCION DE MOVIMIENTOS");
                    Console.WriteLine("---------------------------------------------------------------");

                    cargos.ForEach(mov => {
                        //solamente se registran aquellos que llevan un costo mayor a 0
                        if (mov.precio > 0)
                        {
                            //si el descuento es 0 no tiene descuento y se guarda solo en movimiento
                            if (mov.descuento == 0) { IError = crearMovimiento(mov.codigo_prod_serv, mov.unidades, mov.precio, idDocumento, ifolio); }
                            //si el descuento es diferente de 0 tiene descsuento y se guarda movimineto con descuento
                            else { IError = crearMovimientoDescuento(mov.codigo_prod_serv, mov.unidades, mov.precio, idDocumento, mov.descuento, ifolio); }

                            //suma los IError en cada recorrido al IErrorMovimiento si la suma es mayor a cero ocurrio un error en algun movimiento
                            IErrorMovimiento = IErrorMovimiento + IError;
                        }//fin del if cuando un pago tiene precio 
                        else {
                            Console.WriteLine("ESTE MOVIMIENTO TIENE SALDO DE CERO POR LO CUAL NO SE AGREGA->"+mov.codigo_prod_serv);
                        }
                    });

                    if (IError == 0 && IErrorMovimiento==0)//se asegura que IError sea 0 y tambien IErrorMovimiento 
                    {
                        log_pv = new System.IO.StreamWriter(@"C:\Compac\LOGS_PV\" + ifolio.ToString() + "-log.txt",true);
                        //si entro aqui supone que todos los movimientos se registraron con exito
                        response[2] = "2";//2 indica movimeintos completos guardados
                        log_pv.WriteLine("TODOS LOS MOVIMIENTOS SE GREGARON CON EXITO.");
                        log_pv.WriteLine("ESTADO DE FACTURA EN (" + response[2] + ") SIGNIFICA QUE LOS MOVIMIENTOS SE AGREGARON CON EXITO AL DOCUMENTO EN COMPAC");
                        log_pv.WriteLine("---------------------------------------------------------------");
                        log_pv.WriteLine("INICIA PROCESO DE SALDADO DE DOCUMENTO");
                        log_pv.WriteLine("---------------------------------------------------------------");
                        Console.WriteLine("TODOS LOS MOVIMIENTOS SE GREGARON CON EXITO.");
                        Console.WriteLine("ESTADO DE FACTURA EN (" + response[2] + ") SIGNIFICA QUE LOS MOVIMIENTOS SE AGREGARON CON EXITO AL DOCUMENTO EN COMPAC");
                        Console.WriteLine("---------------------------------------------------------------");
                        Console.WriteLine("INICIA PROCESO DE SALDADO DE DOCUMENTO");
                        Console.WriteLine("---------------------------------------------------------------");
                        log_pv.Close();//CERRO POR PROCESO

                        //si el subtotal de la suma de los movimientos es igual al subtotal del documento realiza el saldado
                        IError = saldarDocumento(codigo_cliente, ifolio, fecha_saldar);
                        if (IError == 0)
                        {
                            log_pv = new System.IO.StreamWriter(@"C:\Compac\LOGS_PV\" + ifolio.ToString() + "-log.txt", true);
                            response[2] = "3";//3 indica que se saldo el documento con exito...
                            Console.WriteLine("ESTADO DE FACTURA EN (" + response[2] + ") SIGNIFICA QUE SE SALDO EL DOCUMENTO EN COMPAC");
                            log_pv.WriteLine("ESTADO DE FACTURA EN (" + response[2] + ") SIGNIFICA QUE SE SALDO EL DOCUMENTO EN COMPAC");
                            ////Console.WriteLine("comienza a timbrar el documento");
                            ////timbrarDocumento(ifolio);
                            ////if (IError == 0) {
                            ////}  
                            log_pv.Close();//CERRO POR PROCESO
                        }
                                                                    
                    }


                    //final ocurrio o no error en movimientos saldar o timbrar el doc esta creado y se modifica sus siguientes atributos
                    //se agrega descripcion al documento
                    Documento.fEditarDocumento();//se ahabilita la edicion del documento
                    Documento.fSetDatoDocumento("CCANTPARCI", metodo_pago);//se agrega el metdoo pago al doc ya sea pue o ppd
                    Documento.fSetDatoDocumento("COBSERVACIONES", observaciones);//se agrega la descripcion
                    Documento.fSetDatoDocumento("CTEXTOEXTRA1", folio_pv);//se integrara el folio de punto verde
                    Documento.fGuardaDocumento();//guarda los cambios en el documento

                    //se libera el documento..
                    Documento.fDesbloqueaDocumento();
                }
                else
                {
                    SDK.MuestraError(IError, "fAltaDocumento");
                }
            }
            else
            {

                SDK.MuestraError(IError, "fSiguienteFolio");
            }
            
            response[1] = IError.ToString();

            return response;
        }

        //retorna 0 si nada salio mal
        public static int crearMovimiento(string codigo_producto_servicio, int unidades, double precio, int id_documento, double folio_documento)
        {
            System.IO.StreamWriter log_pv = new System.IO.StreamWriter(@"C:\Compac\LOGS_PV\" + folio_documento.ToString() + "-log.txt", true);
            int IError;
            int idMovimiento = 0;
            Movimiento.tMovimiento movimiento = new Movimiento.tMovimiento();
            movimiento.aCodAlmacen = "1";//es fijo
            movimiento.aConsecutivo = 1;//es fijo
            movimiento.aCodProdSer = codigo_producto_servicio;
            movimiento.aUnidades = unidades;
            movimiento.aPrecio = Math.Round(precio/1.16,2);

            IError = Movimiento.fAltaMovimiento(id_documento, ref idMovimiento, ref movimiento);
            if (IError == 0)
            {
                //se intenta aplicar esta spolucion para permitir cargos en cero
                /**if (precio == 0)
                {
                    Console.WriteLine("Movimiento en Cero 0");
                    Movimiento.fEditarMovimiento();
                    Movimiento.fSetDatoMovimiento("CPRECIO", "0");
                    Movimiento.fSetDatoMovimiento("CPRECIOCAPTURADO", "0");
                    Movimiento.fSetDatoMovimiento("CNETO", "0");
                    Movimiento.fSetDatoMovimiento("CIMPUESTO1", "0");
                    Movimiento.fSetDatoMovimiento("CTOTAL", "0");
                    Movimiento.fGuardaMovimiento();
                }*/
                //abro documento paso true para escribir sin eliminar lo que ya contiene
                
                log_pv.WriteLine("MOVIMIENTO DADO DE ALTA CON EXITO: IDMOVIMIENTO="+idMovimiento.ToString());
                log_pv.WriteLine("DATOS-> [COD_PROD_SERV:{0},UNIDADES:{1},PRECIO:{2},SUBTOTALPRECIO:{3}]", codigo_producto_servicio, unidades, precio, Math.Round(precio / 1.16, 2));                
                Console.WriteLine("MOVIMIENTO DADO DE ALTA CON EXITO: IDMOVIMIENTO=" + idMovimiento.ToString());
                Console.WriteLine("DATOS-> [COD_PROD_SERV:{0},UNIDADES:{1},PRECIO:{2},SUBTOTALPRECIO:{3}]", codigo_producto_servicio,unidades,precio, Math.Round(precio / 1.16, 2));

                //**********lienas de codigo pra intentar redondear bien***/
                StringBuilder precio_movimiento = new StringBuilder();
                Movimiento.fLeeDatoMovimiento("CPRECIO", precio_movimiento, 10);
                log_pv.WriteLine("DATOS-> [PRECIO DEL PRODUCTO DESDE COMPAQ:{0}]", precio_movimiento);
                Console.WriteLine("DATOS-> [PRECIO DEL PRODUCTO DESDE COMPAQ:{0}]", precio_movimiento);

                StringBuilder neto_movimiento = new StringBuilder();
                Movimiento.fLeeDatoMovimiento("CNETO", neto_movimiento, 10);
                log_pv.WriteLine("DATOS-> [PRECIO DEL PRODUCTO DESDE COMPAQ:{0}]", precio_movimiento);
                Console.WriteLine("DATOS-> [PRECIO DEL PRODUCTO DESDE COMPAQ:{0}]", precio_movimiento);

                StringBuilder total_movimiento = new StringBuilder();
                Movimiento.fLeeDatoMovimiento("CTOTAL", total_movimiento, 10);
                log_pv.WriteLine("DATOS-> [TOTAL DEL PRODUCTO DESDE COMPAQ:{0}]", total_movimiento);
                Console.WriteLine("DATOS-> [TOTAL DEL PRODUCTO DESDE COMPAQ:{0}]", total_movimiento);

                if (Double.Parse(total_movimiento.ToString()) == precio)
                {
                    log_pv.WriteLine("DATOS-> [TOTAL DE COMPAQ {0} ES IGUAL A TOTAL ENVIO:{1}]", total_movimiento, precio);
                    Console.WriteLine("DATOS-> [TOTAL DE COMPAQ {0} ES IGUAL A TOTAL ENVIO:{1}]", total_movimiento, precio);
                }
                else {
                    StringBuilder iva_movimiento = new StringBuilder();
                    Movimiento.fLeeDatoMovimiento("CIMPUESTO1", iva_movimiento, 10);

                    Double restante = precio - Double.Parse(total_movimiento.ToString());

                    log_pv.WriteLine("DATOS-> [TOTAL DE COMPAQ {0} NO ES IGUAL A TOTAL ENVIO:{1} SE MODIFICA EL IVA PARA QUE SEA {2}]", total_movimiento, precio, precio);
                    Console.WriteLine("DATOS-> [TOTAL DE COMPAQ {0} NO ES IGUAL A TOTAL ENVIO:{1} SE MODIFICA EL IVA PARA QUE SEA {2}]", total_movimiento, precio, precio);
                    Movimiento.fSetDatoMovimiento("CIMPUESTO1", (Double.Parse(iva_movimiento.ToString())+restante).ToString());
                    Movimiento.fGuardaMovimiento();
                }
            }
            else
            {
                SDK.MuestraError(IError, ">>>fAltaMovimiento<<<");
                log_pv.WriteLine(SDK.MuestraErrorLogs(IError, ">>>fAltaMovimiento<<<"));
            }
            log_pv.Close();//se cierra el archivo
            return IError;
        }

        //retorna 0 si no paso ningun error
        public static int crearMovimientoDescuento(string codigo_producto_servicio,int unidades,double precio,int id_documento,double descuento,double folio_documento) {

            System.IO.StreamWriter log_pv = new System.IO.StreamWriter(@"C:\Compac\LOGS_PV\" + folio_documento.ToString() + "-log.txt", true);

            int IError = 0;
            int idMovimiento = 0;

            Movimiento.tMovimientoDesc movimiento_desc = new Movimiento.tMovimientoDesc();
            movimiento_desc.aCodAlmacen = "1";
            movimiento_desc.aConsecutivo = 1;
            movimiento_desc.aCodProdSer = codigo_producto_servicio;
            movimiento_desc.aUnidades = unidades;
            movimiento_desc.aPrecio = Math.Round(precio/1.16,2);
            movimiento_desc.aImporteDescto1 = Math.Round(descuento/1.16,2);

            IError = Movimiento.fAltaMovimientoCDesct(id_documento,ref idMovimiento,ref movimiento_desc);
            if (IError == 0)
            {
                
                log_pv.WriteLine("MOVIMIENTO CON DESCUENTO DADO DE ALTA CON EXITO: IDMOVIMIENTO=" + idMovimiento.ToString());
                log_pv.WriteLine("DATOS-> [COD_PROD_SERV:{0},UNIDADES:{1},PRECIO:{2},DESCUENTO:{3},SUBTOTAL:{4},SUBTOTALDESCUENTO:{5}]", codigo_producto_servicio, unidades, precio, descuento, Math.Round(precio / 1.16, 2), Math.Round(descuento / 1.16, 2));
                Console.WriteLine("MOVIMIENTO DADO DE ALTA CON EXITO: IDMOVIMIENTO=" + idMovimiento.ToString());
                Console.WriteLine("DATOS-> [COD_PROD_SERV:{0},UNIDADES:{1},PRECIO:{2},DESCUENTO:{3},SUBTOTAL:{4},SUBTOTALDESCUENTO:{5}]", codigo_producto_servicio, unidades, precio, descuento, Math.Round(precio / 1.16, 2), Math.Round(descuento / 1.16, 2));

                //**********lienas de codigo pra intentar redondear bien***/
                StringBuilder precio_movimiento = new StringBuilder();
                Movimiento.fLeeDatoMovimiento("CPRECIO", precio_movimiento, 30);
                log_pv.WriteLine("DATOS-> [PRECIO DEL PRODUCTO DESDE COMPAQ:{0}]", precio_movimiento);
                Console.WriteLine("DATOS-> [PRECIO DEL PRODUCTO DESDE COMPAQ:{0}]", precio_movimiento);

                StringBuilder neto_movimiento = new StringBuilder();
                Movimiento.fLeeDatoMovimiento("CNETO", neto_movimiento, 30);
                log_pv.WriteLine("DATOS-> [NETO DEL PRODUCTO DESDE COMPAQ:{0}]", neto_movimiento);
                Console.WriteLine("DATOS-> [NETO DEL PRODUCTO DESDE COMPAQ:{0}]", neto_movimiento);

                StringBuilder total_movimiento = new StringBuilder();
                Movimiento.fLeeDatoMovimiento("CTOTAL", total_movimiento, 30);
                log_pv.WriteLine("DATOS-> [TOTAL DEL PRODUCTO DESDE COMPAQ:{0}]", total_movimiento);
                Console.WriteLine("DATOS-> [TOTAL DEL PRODUCTO DESDE COMPAQ:{0}]", total_movimiento);

                if (Double.Parse(total_movimiento.ToString()) == (precio-descuento))
                {
                    log_pv.WriteLine("DATOS-> [TOTAL DE COMPAQ {0} ES IGUAL A TOTAL ENVIO MENOS DESCUENTO:{1}]", total_movimiento, (precio-descuento));
                    Console.WriteLine("DATOS-> [TOTAL DE COMPAQ {0} ES IGUAL A TOTAL ENVIO MENOS DESCUENTO:{1}]", total_movimiento, (precio - descuento));
                }
                else
                {
                    StringBuilder iva_movimiento = new StringBuilder();
                    Movimiento.fLeeDatoMovimiento("CIMPUESTO1", iva_movimiento, 10);

                    Double restante = (precio - descuento) - Double.Parse(total_movimiento.ToString());

                    log_pv.WriteLine("DATOS-> [TOTAL DE COMPAQ {0} NO ES IGUAL A TOTAL ENVIO:{1} SE MODIFICA EL IVA PARA QUE SEA {2}]", total_movimiento, (precio - descuento), (precio - descuento));
                    Console.WriteLine("DATOS-> [TOTAL DE COMPAQ {0} NO ES IGUAL A TOTAL ENVIO:{1} SE MODIFICA EL IVA PARA QUE SEA {2}]", total_movimiento, (precio - descuento), (precio - descuento));
                    Movimiento.fSetDatoMovimiento("CIMPUESTO1", (Double.Parse(iva_movimiento.ToString()) + restante).ToString());
                    Movimiento.fGuardaMovimiento();
                }


            }
            else { SDK.MuestraError(IError, ">>>fAltaMovimientoCDesct<<<");
                  log_pv.WriteLine(SDK.MuestraErrorLogs(IError, ">>>fAltaMovimientoCDesct<<<"));
            }

            log_pv.Close();//se cierra el archivo
            return IError;
        
        }

        //retorna 0 si no efectuo error
        public static int saldarDocumento(string codigo_cliente, double folio_factura,string fecha_pago) {

            //SE ABRE ARCHIVO
            System.IO.StreamWriter log_pv = new System.IO.StreamWriter(@"C:\Compac\LOGS_PV\" + folio_factura.ToString() + "-log.txt", true);           

            int lError;
            int error_interno = 0;
            StringBuilder serie = new StringBuilder("");
            double folio = 0;
            string codConcepto = "13";//es fijo es donde van los docuemntos de saldo 
            string codCte = codigo_cliente;

            StringBuilder subtotal = new StringBuilder();//se almacenara el subtotal del doc recuperado de compac
            StringBuilder iva = new StringBuilder();//es el iva del documento
            StringBuilder total = new StringBuilder();//se almacenara el total del doc recuperado de compac

            Documento.fLeeDatoDocumento("CNETO", subtotal, 30);
            Documento.fLeeDatoDocumento("CIMPUESTO1", iva, 30);
            Documento.fLeeDatoDocumento("CTOTAL", total, 30);

            lError = Documento.fSiguienteFolio(codConcepto, serie, ref folio);
            if (lError == 0)
            {
            Documento.tDocumento lDocto = new Documento.tDocumento();
            Documento.tLlaveDoc factura = new Documento.tLlaveDoc();
            Documento.tLlaveDoc pago = new Documento.tLlaveDoc();

                DateTime datetime;//es temporal solo para validar la fecha
                

                string fechaSaldar = DateTime.TryParse(fecha_pago, out datetime) ? datetime.ToString("MM/dd/yyyy") : DateTime.Today.ToString("MM/dd/yyyy");
                //importe_total es el subtotal y el importe saldar es el total
                //porque importe_total se pasa al doc y compaq le suma el iva y para saldar se indica el total ya con iva
          

                Console.WriteLine("DETALLE DE LOS TOTALES ENVIADOS Y OBTENIDOS DE COMPAQ;");
                Console.WriteLine("IMPORTE SUBTOTAL RECUPERADO DE COMPAC PARA SALDAR:['CNETO']="+subtotal);
                Console.WriteLine("IVA RECUPERADO DE COMPAC['CIMPUESTO1']=" + iva);
                Console.WriteLine("IMPORTE TOTAL RECUPERADO DE COMPAC['CTOTAL']="+ total);

                log_pv.WriteLine("DETALLE DE LOS TOTALES ENVIADOS Y OBTENIDOS DE COMPAQ;");
                log_pv.WriteLine("IMPORTE SUBTOTAL RECUPERADO DE COMPAC PARA SALDAR['CNETO']=" + subtotal);
                log_pv.WriteLine("IVA RECUPERADO DE COMPAC['CIMPUESTO1']=" + iva);
                log_pv.WriteLine("IMPORTE TOTAL RECUPERADO DE COMPAC['CTOTAL']=" + total);


                lDocto.aCodConcepto = codConcepto;
                lDocto.aCodigoCteProv = codCte; ;
                lDocto.aFecha = fechaSaldar;
                lDocto.aFolio = folio;
                lDocto.aImporte = Convert.ToDouble(subtotal.ToString());//es el subtotal compaq suma el iva
                lDocto.aNumMoneda = 1;
                lDocto.aTipoCambio = 1;
                lDocto.aSerie = serie.ToString();
                lDocto.aSistemaOrigen = 6;//indica que se realizo xon una aplicacion externa

                lError = Documento.fAltaDocumentoCargoAbono(ref lDocto);
                if (lError == 0)
                {

                    StringBuilder total_abono = new StringBuilder();//se almacena el total de el doc de abono 
                    Documento.fLeeDatoDocumento("CTOTAL", total_abono, 30);
                    Console.WriteLine("DOCUMENTO DE ABONO: IMPORTE TOTAL RECUPERADO DE COMPAC['CTOTAL']=" + total_abono);
                    log_pv.WriteLine("DOCUMENTO DE ABONO: IMPORTE TOTAL RECUPERADO DE COMPAC['CTOTAL']=" + total_abono);

                    
                    if (Convert.ToDouble(total_abono.ToString()) == Convert.ToDouble(total.ToString()))
                    {                        
                        Console.WriteLine("EL TOTAL DEL PAGO {0} ES IGUAL AL TOTAL DEL LA FACTURA {1} ,NO SE HACE NADA;",total_abono,total);
                        log_pv.WriteLine("EL TOTAL DEL PAGO {0} ES IGUAL AL TOTAL DEL LA FACTURA {1} ,NO SE HACE NADA;",total_abono,total);
                    }
                    else {
                        Console.WriteLine("EL TOTAL DEL PAGO {0} ES DIFERENTE AL TOTAL DEL LA FACTURA {1} ,SE PROCEDE A EMPATAR EL TOTAL DE PAGO", total_abono, total);
                        log_pv.WriteLine("EL TOTAL DEL PAGO {0} ES DIFERENTE AL TOTAL DEL LA FACTURA {1} ,SE PROCEDE A EMPATAR EL TOTAL DE PAGO", total_abono, total);
                        Documento.fSetDatoDocumento("CTOTAL",total.ToString());
                        Documento.fGuardaDocumento();
                    }
                    
                    factura.aCodConcepto = "42018";//es el codigo del concepto de los cargos y es fijo
                    factura.aSerie = "";//no tiene serie
                    factura.aFolio = folio_factura;//es el folio del otro documento que se dio de alta

                    pago.aCodConcepto = codConcepto;//es el concepto 13 que es el documento que se acaba crear
                    pago.aSerie = serie.ToString();
                    pago.aFolio = folio;//es el folio de este documento

                    Console.WriteLine("SE CREO CON EXITO EL DOCUMENTO DE PAGO O ABONO CON EL FOLIO:" + folio);
                    Console.WriteLine("DATOS DE DOC FACTURA -> SERIE:{0}  -  FOLIO:{1}","",folio_factura);
                    Console.WriteLine("DATOS DE DOC ABONOPAGO -> SERIE:{0}  -  FOLIO:{1}", serie, folio);

                    log_pv.WriteLine("SE CREO CON EXITO EL DOCUMENTO DE PAGO O ABONO CON EL FOLIO:" + folio);
                    log_pv.WriteLine("DATOS DE DOC FACTURA -> SERIE:{0}  -  FOLIO:{1}", "", folio_factura);
                    log_pv.WriteLine("DATOS DE DOC ABONOPAGO -> SERIE:{0}  -  FOLIO:{1}", serie, folio);

                    //importe saldar es el total ya con iva
                    lError = Documento.fSaldarDocumento(ref factura, ref pago, Convert.ToDouble(total.ToString()), 1, datetime.ToString("MM/dd/yyyy"));
                    if (lError == 0)
                    {
                        Console.WriteLine("SE SALDA EL DOCUMENTO CON EXITO");
                        Console.WriteLine("DATOS DE SALDADO-> SE SALDA EL DOC {0} CON EL DOC {1} POR EL IMPORTE DE {2} EN LA FECHA {3}",folio_factura,folio,total,datetime.ToString("MM/dd/yyyy"));
                        log_pv.WriteLine("SE SALDA EL DOCUMENTO CON EXITO");
                        log_pv.WriteLine("DATOS DE SALDADO-> SE SALDA EL DOC {0} CON EL DOC {1} POR EL IMPORTE DE {2} EN LA FECHA {3}", folio_factura, folio, total, datetime.ToString("MM/dd/yyyy"));
                    }
                    else
                    {
                        log_pv.WriteLine(SDK.MuestraError(lError, ">>>fSaldarDocumento<<<"));
                        error_interno = Documento.fBuscarDocumento(codConcepto,serie.ToString(),folio.ToString());
                        if (error_interno == 0)
                        {
                            Console.WriteLine("SE BUSCA EL DOC {0} PARA ELIMINARLO Y NO QUEDE VOLANDO.",folio);
                            log_pv.WriteLine("SE BUSCA EL DOC {0} PARA ELIMINARLO Y NO QUEDE VOLANDO.", folio);
                            error_interno = Documento.fBorraDocumento();
                            if (error_interno == 0) { 
                                Console.WriteLine("SE ELIMINA EL DOC {0} DE PAGO ABONO POR NO SALDARSE",folio);
                                log_pv.WriteLine("SE ELIMINA EL DOC {0} DE PAGO ABONO POR NO SALDARSE", folio);
                            }
                            else { log_pv.WriteLine(SDK.MuestraError(error_interno, ">>>fBorraDocumento<<<")); }
                        }
                        else {
                            log_pv.WriteLine(SDK.MuestraError(error_interno, ">>>fBuscarDocumento<<<"));
                        }
                        //vuelve aposicionar el documento al original
                        Console.WriteLine("SE VUELVE A POSICIONAR EN EL DOC {0} DE FACTURA ORIGINAL PARA SEGUIR TRABAJANDO CON EL .", folio_factura);
                        log_pv.WriteLine("SE VUELVE A POSICIONAR EN EL DOC {0} DE FACTURA ORIGINAL PARA SEGUIR TRABAJANDO CON EL .", folio_factura);
                        Documento.fBuscarDocumento("42018", "", folio_factura.ToString());
                    }
                }
                else
                {
                    log_pv.WriteLine(SDK.MuestraError(lError, ">>>fAltaDocumentoCargoAbono<<<"));
                }
            }

            else
            {

                log_pv.WriteLine(SDK.MuestraError(lError, ">>>fSiguienteFolio<<<"));

            }

            log_pv.Close();//se cierra el archivo
            return lError;

        }

        public static int timbrarDocumento(double folio)
        {
            Console.WriteLine("entra a timbrar documento...");
            int IError;
            StringBuilder serie = new StringBuilder("");
            string codConcepto = "42018";//es fijo es donde van los docuemntos
            string password = "PuntoV14";
            StringBuilder uuid = new StringBuilder("");

            IError = Documento.fEmitirDocumento(codConcepto,serie.ToString(),folio,password,"");
            if (IError == 0)
            {
                Console.WriteLine("se timbro con exito el Documento...");
                IError = Documento.fEntregEnDiscoXML(codConcepto, serie.ToString(), folio, 1, "");
                if (IError == 0)
                {
                    Console.WriteLine("Se genero PDF con exito");
                }
                else {
                    SDK.MuestraError(IError, "fEntregEnDiscoXML");
                }                

            }
            else {
                SDK.MuestraError(IError, "fEmitirDocumento");
            }

            return 0;
        }

        public static string[] getUUID(double folio) {
            int IError;
            StringBuilder serie = new StringBuilder("");
            string codConcepto = "42018";//es fijo es donde van los docuemntos
            StringBuilder uuid = new StringBuilder("");

            Console.WriteLine("entra a obtener el uuid");
            IError = Documento.fDocumentoUUID(new StringBuilder(codConcepto), serie, folio,uuid);
            if (IError == 0)
            {
                Console.WriteLine("obtine el uuid:"+ uuid.ToString());
                string[] response= { IError.ToString(), uuid.ToString() };
                return response;
            }
            else { 
                SDK.MuestraError(IError, "fDocumentoUUID");
                string[] response = { IError.ToString(), "" };
                return response;
            }

        }

        public static List<ResponseMovimiento> getMovimientos(string folio)
        {
            int IError = 0;
            List<ResponseMovimiento> lista = new List<ResponseMovimiento>();
            IError = Documento.fBuscarDocumento("42018", "", folio);
            if (IError == 0)
            {
                Console.WriteLine("Documento Encontrado");
                StringBuilder id_documento = new StringBuilder();
                IError = Documento.fLeeDatoDocumento("CIDDOCUMENTO", id_documento, 10);
                if (IError == 0)
                {
                    Console.WriteLine("id docuemnto es:" + id_documento.ToString());
                    IError = Movimiento.fSetFiltroMovimiento(Convert.ToInt32(id_documento.ToString()));
                    if (IError == 0)
                    {
                        Console.WriteLine("filtro aplicado con exito");

                        //se posiciona en el primer movimiento del fitro
                        IError = Movimiento.fPosPrimerMovimiento();
                        if (IError == 0)
                        {
                            do
                            {
                                StringBuilder id_movimiento = new StringBuilder(12);
                                StringBuilder n_movimiento = new StringBuilder(12);
                                StringBuilder precio = new StringBuilder();
                                StringBuilder cantidad = new StringBuilder();
                                StringBuilder precio_total = new StringBuilder();
                                StringBuilder id_producto = new StringBuilder();
                                Movimiento.fLeeDatoMovimiento("CIDMOVIMIENTO", id_movimiento, 8);
                                Movimiento.fLeeDatoMovimiento("CNUMEROMOVIMIENTO", n_movimiento, 40);
                                Movimiento.fLeeDatoMovimiento("CPRECIOCAPTURADO", precio, 10);
                                Movimiento.fLeeDatoMovimiento("CUNIDADESCAPTURADAS", cantidad, 10);
                                Movimiento.fLeeDatoMovimiento("CTOTAL", precio_total, 10);
                                Movimiento.fLeeDatoMovimiento("CIDPRODUCTO", id_producto, 10);
                                Producto.fBuscaIdProducto(Convert.ToInt32(id_producto.ToString()));
                                StringBuilder nombre = new StringBuilder(12);
                                Producto.fLeeDatoProducto("CNOMBREPRODUCTO", nombre, 40);

                                lista.Add(new ResponseMovimiento()
                                {
                                    //id = Convert.ToInt32(id_movimiento.ToString()), 
                                    id = id_movimiento.ToString(),
                                    producto = nombre.ToString(),
                                    precio = precio.ToString(),
                                    cantidad = cantidad.ToString(),
                                    total = precio_total.ToString()
                                });
                            } while (Movimiento.fPosSiguienteMovimiento() == 0);
                        }
                    }
                    else { Console.WriteLine("filtro fallo"); }

                }

            }

            return lista;

        }
    }
}
 
using CapaEntidad1;
using CapaNegocio1;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Linq;


namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }

        public ActionResult Marca()
        {
            return View();
        }

        public ActionResult Producto()
        {
            return View();
        }

        // Métodos para Categorías
        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new CN_Categoria().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdCategoria == 0)
            {
                resultado = new CN_Categoria().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = new CN_Categoria().Eliminar(id, out string mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // Métodos para Marcas
        [HttpGet]
        public JsonResult ListarMarcas()
        {
            List<Marca> oLista = new CN_Marca().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdMarca == 0)
            {
                resultado = new CN_Marca().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Marca().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = new CN_Marca().Eliminar(id, out string mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // Métodos para Productos
        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> oLista = new CN_Producto().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            object resultado;
            string mensaje = string.Empty;
            bool operacionExitosa = true;
            bool guardarImagenExito = true;

            // Deserializa el objeto Producto
            Producto oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

            // Validación del precio
            decimal precio;
            if (decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out precio))
            {
                oProducto.Precio = precio;
            }
            else
            {
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);
            }

            // Si el producto es nuevo, registra el producto
            if (oProducto.IdProducto == 0)
            {
                int idProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);
                if (idProductoGenerado != 0)
                {
                    oProducto.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacionExitosa = false;
                }
            }
            else
            {
                // Si el producto existe, edítalo
                operacionExitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }

            // Si la operación fue exitosa y hay una imagen, intentamos guardarla
            if (operacionExitosa && archivoImagen != null)
            {
                try
                {
                    string rutaGuardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    if (string.IsNullOrEmpty(rutaGuardar))
                    {
                        throw new Exception("La ruta para guardar las imágenes no está configurada.");
                    }

                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombreImagen = string.Concat(oProducto.IdProducto.ToString(), extension);

                    // Guardar la imagen en el servidor
                    archivoImagen.SaveAs(Path.Combine(rutaGuardar, nombreImagen));

                    // Actualizar los datos del producto con la ruta y nombre de la imagen
                    oProducto.RutaImagen = rutaGuardar;
                    oProducto.NombreImagen = nombreImagen;

                    bool respuesta = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    if (!respuesta)
                    {
                        mensaje = "Se guardó el producto pero hubo problemas con la imagen.";
                    }
                }
                catch (Exception ex)
                {
                    mensaje = $"Se guardó el producto pero ocurrió un error al guardar la imagen: {ex.Message}";
                    guardarImagenExito = false;
                }
            }

            return Json(new { operacionExitosa = operacionExitosa, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ImagenProducto(int id) {
            bool conversion;
            Producto oproducto = new CN_Producto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();
            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oproducto.RutaImagen,oproducto.NombreImagen),out conversion);

            return Json(new
            { 
             conversion = conversion,
             textoBase64 = textoBase64,
             extension = Path.GetExtension(oproducto.NombreImagen)
            
            
            
            }, JsonRequestBehavior.AllowGet);













        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string Mensaje = string.Empty;
            respuesta  = new CN_Producto().Eliminar(id, out  Mensaje);
            return Json(new { resultado = respuesta, Mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_Producto.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Cors;
/*
* Nota: la cadena de conexion esta en aspsettings.json y accedemos a ella mediante la interfaz "IConfiguration"
* para poder manipular datos instalamos el siguiente paquete -System.Data.SqlClient-
* 
*/
namespace API_Producto.Controllers
{
    [EnableCors("ReglaCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly string cadenaSQL;
            public ProductoController(IConfiguration config)
        {
            //a traves del "config" vamos a recbir la cadena de conexion a la bd

            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

      

        //Todos los datos
        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            //creando la lista que vamos a retornar
            List<Producto> lista = new List<Producto>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();//abriendo la cadena de conexion 
                    //(procedimiento almacenado, fuente)
                    var cmd = new SqlCommand("sp_lista_productos", conexion);
                    //especificando el tipo de comando
                    cmd.CommandType = CommandType.StoredProcedure;
                    //leyendo el resultado del comando de ejecucion 
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())//mientas "exista siguiente"
                        {
                            lista.Add(new Producto()
                            {
                                //llenando las propiedades del objeto
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                Nombre = rd["Nombre"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Marca"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"])
                            }) ;
                            
                        }
                    }
                }

                //retornando el codgo 200 y el json con la siguiente estructura
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista});
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });
            }
        }

        //--- Obtener dato por ID ---
        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            //creando la lista que vamos a retornar
            List<Producto> lista = new List<Producto>();
            //creando un objeto modelo del tipo Producto
            Producto producto = new Producto();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();//abriendo la cadena de conexion 
                    //(procedimiento almacenado, fuente)
                    var cmd = new SqlCommand("sp_lista_productos", conexion);
                    //especificando el tipo de comando
                    cmd.CommandType = CommandType.StoredProcedure;
                    //leyendo el resultado del comando de ejecucion 
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())//mientas "exista siguiente"
                        {
                            lista.Add(new Producto()
                            {
                                //llenando las propiedades del objeto
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                Nombre = rd["Nombre"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Marca"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"])
                            });

                        }
                    }
                }
                //filtrando por id en la lista
                producto = lista.Where(item => item.IdProducto == idProducto).FirstOrDefault();

                //retornando el codgo 200 y el json con la siguiente estructura
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = producto
                });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = producto });
            }
        }

        //- POST----------------------------------------------------------------

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();//abriendo la cadena de conexion 
                    //(procedimiento almacenado, fuente)
                    var cmd = new SqlCommand("sp_guardar_productos", conexion);
                    //agregando los parametros (ya que el procediemiento asi esta configurado)
                    cmd.Parameters.AddWithValue("codigoBarra", objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.Marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.Categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.Precio);
                    //especificando el tipo de comando
                    cmd.CommandType = CommandType.StoredProcedure;
                    //ejecutando la sentencia
                    cmd.ExecuteNonQuery();
              
                }
                //retornando el codgo 200 y el json con la siguiente estructura
                return StatusCode(StatusCodes.Status200OK, new {mensaje = "ok"});
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        //- PUT----------------------------------------------------------------

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();//abriendo la cadena de conexion 
                    //(procedimiento almacenado, fuente)
                    var cmd = new SqlCommand("sp_editar_producto", conexion);
                    //agregando los parametros (ya que el procediemiento asi esta configurado)
                    //como en el procedimiento podemos aceptar valores nulos entonces validamos
                    //meidante una expresion lambda si es nulo pues que conserve el valor original
                    //de lo contrario lo actualize
                    cmd.Parameters.AddWithValue("idProducto", objeto.IdProducto == 0 ? DBNull.Value : objeto.IdProducto);
                    cmd.Parameters.AddWithValue("codigoBarra", objeto.CodigoBarra is null ? DBNull.Value : objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("nombre", objeto.Nombre is null ? DBNull.Value : objeto.Nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.Marca is null ? DBNull.Value : objeto.Marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.Categoria is null ? DBNull.Value : objeto.Categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.Precio == 0 ? DBNull.Value : objeto.Precio);
                    //especificando el tipo de comando
                    cmd.CommandType = CommandType.StoredProcedure;
                    //ejecutando la sentencia
                    cmd.ExecuteNonQuery();

                }
                //retornando el codgo 200 y el json con la siguiente estructura
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "editado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        //- DELETE ----------------------------------------------------------------

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();//abriendo la cadena de conexion 
                    //(procedimiento almacenado, fuente)
                    var cmd = new SqlCommand("sp_eliminar", conexion);
                    //agregando los parametros (ya que el procediemiento asi esta configurado)
                    cmd.Parameters.AddWithValue("idProducto", idProducto);         
                    //especificando el tipo de comando
                    cmd.CommandType = CommandType.StoredProcedure;
                    //ejecutando la sentencia
                    cmd.ExecuteNonQuery();

                }
                //retornando el codgo 200 y el json con la siguiente estructura
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}

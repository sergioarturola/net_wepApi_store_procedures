namespace API_Producto.Models
{
    public class Producto
    {
        //agregando propiedades que son c/u de las columnas en la tabla
        //para que dejeran de salir subrayados en verde las propiedades (ya que inidica
        //que pueden ser null hacemos doble clic en API_Poducto y en "Nullable" lo cambiamos
        //de "enable" a "disable"
        public int IdProducto { get; set; }
        public string CodigoBarra { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
    }
}

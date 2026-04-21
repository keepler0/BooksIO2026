namespace BooksIO2026.Entities
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime PublishedDate { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public int PublisherId { get; set; }
        public Publisher? Publisher { get; set; }
        //TODO: El libro tambien deberia tener una descripcion o una sinapsis
        //tmabien  se podria agregar como dificultad de lectura, Cantidad de paginas, estado, ISBN, Formato,
    }
}

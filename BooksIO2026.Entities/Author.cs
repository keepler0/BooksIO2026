namespace BooksIO2026.Entities
{
    //[Index(nameof(FirstName), nameof(LastName), IsUnique = true, Name = "IX_Authors_FirstName_LastName")]
    //public class Author
    //{
    //    [Key]
    //    public int AuthorId { get; set; }

    //    [Required(ErrorMessage = "The field {0} is required")]
    //    [StringLength(50, ErrorMessage = "The field {0} must have {2} and {1} characters", MinimumLength = 3)]
    //    public string FirstName { get; set; } = null!;

    //    [Required(ErrorMessage = "The field {0} is required")]
    //    [StringLength(50, ErrorMessage = "The field {0} must have {2} and {1} characters", MinimumLength = 3)]
    //    public string LastName { get; set; } = null!;
    //    public override string ToString()
    //    {
    //        return $"{FirstName} {LastName}";
    //    }

    //}
    //Clase con Data Annotations para validar los campos de la clase Author ademas de indicar el PK
    //pero se considera una mala practica ya que mezcla validaciones y no cumple con POCO lo mejor seria una clase plana o limpia realizando
    //las validaciones con fluent API en el DbContext y la clase AuthorEntityTypeConfiguration que esta en el proyecto DATA carpeta CONFIGURATIONS
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
        public ICollection<Book> Books { get; set; }=new List<Book>();
    }
}

using BooksIO2026.Entities;

namespace BooksIO2026.Service.Interfaces
{
    public interface IAuthorService
    {
        List<Author> GetAll();
        Author? GetById(int id);
        
        //cambiamos la firma de los métodos para que devuelvan una tupla con un booleano indicando el éxito de la operación
        //y una lista de errores en caso de que haya fallado
        (bool Success, List<string> Errors) Add(Author author);
        (bool Success, List<string> Errors) Update(Author author);
        (bool Success, List<string> Errors) Delete(int authorId);
    }
}

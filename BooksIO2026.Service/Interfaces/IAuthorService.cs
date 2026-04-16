using BooksIO2026.Service.DTOs;

namespace BooksIO2026.Service.Interfaces
{
    public interface IAuthorService
    {
        List<AuthorListDto> GetAll();
        AuthorDetailDto? GetById(int id);

        //cambiamos la firma de los métodos para que devuelvan una tupla con un booleano indicando el éxito de la operación
        //y una lista de errores en caso de que haya fallado
        (bool Success, List<string> Errors) Add(AuthorCreateDto authorDto);
        (bool Success, List<string> Errors) Update(AuthorUpdateDto authorDto);
        (bool Success, List<string> Errors) Delete(int authorId);
        AuthorUpdateDto? GetAuthorForUpdate(int id);
    }
}

using BooksIO2026.Service.Common;
using BooksIO2026.Service.DTOs;

namespace BooksIO2026.Service.Interfaces
{
    public interface IAuthorService
    {
        Result<List<AuthorListDto>> GetAll();
        Result<AuthorListDto> GetById(int id);
        Result<AuthorUpdateDto> GetAuthorForUpdate(int id);
        Result Add(AuthorCreateDto authorDto);
        Result Update(AuthorUpdateDto authorDto);
        Result Delete(int authorId);

        //cambiamos la firma de los metodos para que devuelvan un result generico con el tipo de dato esperado en caso de éxito de lo contrario errores
        //List<AuthorListDto> GetAll();
        //AuthorDetailDto? GetById(int id);
        //AuthorUpdateDto? GetAuthorForUpdate(int id);
        //cambiamos la firma de los métodos para que devuelvan una tupla con un booleano indicando el éxito de la operación
        //y una lista de errores en caso de que haya fallado
        //(bool Success, List<string> Errors) Add(AuthorCreateDto authorDto);
        //(bool Success, List<string> Errors) Update(AuthorUpdateDto authorDto);
        //(bool Success, List<string> Errors) Delete(int authorId);

    }
}

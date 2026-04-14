using BooksIO2026.Data.Interfaces;
using BooksIO2026.Data.Repositories;
using BooksIO2026.Entities;
using BooksIO2026.Service.Interfaces;
using BooksIO2026.Service.Validators;

namespace BooksIO2026.Service.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly AuthorValidator _authorValidator;
        public AuthorService()
        {
            _authorRepository = new AuthorRepository();
            _authorValidator = new AuthorValidator();
        }
        public (bool Success, List<string> Errors) Add(Author author)
        {
            var result = _authorValidator.Validate(author);//validamos el autor con la clase AuthorValidator que proviene de FluentValidation
            if (!result.IsValid)//si el resultado no es valido, es decir, si hay errores de validacion
            {
                foreach (var error in result.Errors)//recorremos los errores de validacion
                {
                    var errors = result.Errors.Select(e => e.ErrorMessage).ToList();//los capturamos en una lista de strings
                    return (false, errors);//retornamos false y la lista de errores para mostrar los errores que se presento
                }
            }
            try
            {
                _authorRepository.Add(author);
                return (true, new List<string>());
                //de lo contrario, si el autor es valido, lo agregamos a la base de datos y retornamos true y una lista vacia de errores
            }
            catch (Exception)
            {
                return (false, new List<string>() { "Database error" });
            }
        }

        public (bool Success, List<string> Errors) Delete(int authorId)
        {
            try
            {
                _authorRepository.Delete(authorId);
                return (true, new List<string>());
            }
            catch (Exception)
            {
                return (false, new List<string>() { "Database error" });
            }
        }

        public List<Author> GetAll()
        {
            return _authorRepository.GetAll();
        }

        public Author? GetById(int id)
        {
            return _authorRepository.GetById(id);
        }

        public (bool Success, List<string> Errors) Update(Author author)
        {
            var result = _authorValidator.Validate(author);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                    return (false, errors);
                }
            }
            try
            {
                _authorRepository.Update(author);
                return (true, new List<string>());
            }
            catch (Exception)
            {
                return (true, new List<string>() { "Database error" });
            }
        }
    }
}

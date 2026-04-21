using BooksIO2026.Data;
using BooksIO2026.Data.Interfaces;
using BooksIO2026.Entities;
using BooksIO2026.Service.DTOs;
using BooksIO2026.Service.Interfaces;
using BooksIO2026.Service.Mappers;
using FluentValidation;

namespace BooksIO2026.Service.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IValidator<Author> _authorValidator;
        private readonly IUnitOfWork _unitOfWork;
        public AuthorService(IAuthorRepository authorRepository,
                              IUnitOfWork unitOfWork,
                              IValidator<Author> authorValidator)
        {
            _authorRepository = authorRepository;
            _authorValidator = authorValidator;
            _unitOfWork = unitOfWork;
        }
        public (bool Success, List<string> Errors) Add(AuthorCreateDto authorDto)
        {
            var author = AuthorMapper.ToAuthorEntity(authorDto);

            var result = _authorValidator.Validate(author);//validamos el autor con la clase AuthorValidator que proviene de FluentValidation
            if (!result.IsValid)//si el resultado no es valido, es decir, si hay errores de validacion
            {
                foreach (var error in result.Errors)//recorremos los errores de validacion
                {
                    var errors = result.Errors.Select(e => e.ErrorMessage).ToList();//los capturamos en una lista de strings
                    return (false, errors);//retornamos false y la lista de errores para mostrar los errores que se presento
                }
            }
            if (!_authorRepository.Exist(author.FirstName, author.LastName))
            {
                try
                {
                    _authorRepository.Add(author);
                    _unitOfWork.Save();
                    return (true, new List<string>());
                    //de lo contrario, si el autor es valido, lo agregamos a la base de datos y retornamos true y una lista vacia de errores
                }
                catch (Exception)
                {
                    return (false, new List<string>() { "Database error" });
                }
            }
            else
            {
                return (false, new List<string>() { "Author already exists" });
            }
        }

        public (bool Success, List<string> Errors) Delete(int authorId)
        {
            try
            {
                _authorRepository.Delete(authorId);
                _unitOfWork.Save();
                return (true, new List<string>());
            }
            catch (Exception)
            {
                return (false, new List<string>() { "Database error" });
            }
        }

        //Como ahora usamos AuthorListDto para mostrar la lista de autores tenemos que crear los objetos AuthorListDto con el metodo Select de Linq
        public List<AuthorListDto> GetAll()
        {
            return _authorRepository.GetAll()
                                    .Select(a => AuthorMapper.ToAuthorListDto(a))
                                    .ToList();
        }

        public AuthorUpdateDto? GetAuthorForUpdate(int id)
        {
            var author = _authorRepository.GetById(id);
            if (author is not null)
            {
                return AuthorMapper.ToAuthorUpdateDto(author);
            }
            return null;
        }

        public AuthorDetailDto? GetById(int id)
        {
            var author = _authorRepository.GetById(id);
            if (author is not null)
            {
                return AuthorMapper.ToAuthorDetailDto(author);
            }
            return null;
        }

        public (bool Success, List<string> Errors) Update(AuthorUpdateDto authorDto)
        {
            //var author = AuthorMapper.ToAuthor(authorDto);
            var author = _authorRepository.GetById(authorDto.AuthorId);
            if (author is null) return (false, new List<string>() { "Author not found" });
            author.FirstName = authorDto.FirstName;
            author.LastName = authorDto.LastName;
            var result = _authorValidator.Validate(author);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    var errors = result.Errors
                                       .Select(e => e.ErrorMessage)
                                       .ToList();
                    return (false, errors);
                }
            }
            if (!_authorRepository.Exist(author.FirstName, author.LastName, author.AuthorId))
            {
                try
                {
                    //_authorRepository.Update(author);
                    _unitOfWork.Save();
                    return (true, new List<string>());
                }
                catch (Exception)
                {
                    return (false, new List<string>() { "Database error" });
                }
            }
            else
            {
                return (false, new List<string>() { "Author already exist!" });
            }
        }
    }
}

using BooksIO2026.Data;
using BooksIO2026.Entities;
using BooksIO2026.Service.Common;
using BooksIO2026.Service.DTOs;
using BooksIO2026.Service.Interfaces;
using BooksIO2026.Service.Mappers;
using FluentValidation;

namespace BooksIO2026.Service.Services
{
    public class AuthorService : IAuthorService
    {
        //quitmos el IAuthorRepositorio ya que pasamos la responsabilidad a UNIT OF WORK por lo tanto usamos _unitOfWork.Authors para usar el repositorio


        //private readonly IAuthorRepository _authorRepository;
        private readonly IValidator<Author> _authorValidator;
        private readonly IUnitOfWork _unitOfWork;
        public AuthorService(//IAuthorRepository authorRepository,
                              IUnitOfWork unitOfWork,
                              IValidator<Author> authorValidator)
        {
            //_authorRepository = authorRepository;
            _authorValidator = authorValidator;
            _unitOfWork = unitOfWork;
        }
        public Result Add(AuthorCreateDto authorDto)
        {
            var author = AuthorMapper.ToAuthorEntity(authorDto);

            var result = _authorValidator.Validate(author);//validamos el autor con la clase AuthorValidator que proviene de FluentValidation
            if (!result.IsValid)//si el resultado no es valido, es decir, si hay errores de validacion
            {
                foreach (var error in result.Errors)//recorremos los errores de validacion
                {
                    return Result.Failure(result.Errors.Select(e => e.ErrorMessage).ToList());//los capturamos en una lista de strings
                    //return (false, errors);//retornamos false y la lista de errores para mostrar los errores que se presento
                }
            }

            if (_unitOfWork.Authors.ExistSameName(author.FirstName, author.LastName))
                return Result.Failure("Author already exists");

            try
            {
                _unitOfWork.Authors.Add(author);
                _unitOfWork.Save();
                return Result.Success();
                //return (true, new List<string>());
                //de lo contrario, si el autor es valido, lo agregamos a la base de datos y retornamos true y una lista vacia de errores
            }
            catch (Exception ex)
            {
                return Result.Failure($"Database error...{ex.Message}");
                //return (false, new List<string>() { "Database error" });
            }

        }

        public Result Delete(int authorId)
        {
            var authorInDb=_unitOfWork.Authors.GetById(authorId);
            if (authorInDb is null)
                return Result.Failure("Author not found!");

            if (_unitOfWork.Authors.HasBooks(authorId))
                return Result.Failure("Delete denied...author with associated books");

            try
            {
                _unitOfWork.Authors.Delete(authorId);
                _unitOfWork.Save();
                return Result.Success();
                //return (true, new List<string>());
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
                //return (false, new List<string>() { "Database error" });
            }
        }

        //Como ahora usamos AuthorListDto para mostrar la lista de autores tenemos que crear los objetos AuthorListDto con el metodo Select de Linq
        public List<AuthorListDto> GetAll()
        {
            return _unitOfWork.Authors.GetAll()
                                      .Select(a => AuthorMapper.ToAuthorListDto(a))
                                      .ToList();
        }

        public AuthorUpdateDto? GetAuthorForUpdate(int id)
        {
            var author = _unitOfWork.Authors.GetById(id);
            if (author is not null)
            {
                return AuthorMapper.ToAuthorUpdateDto(author);
            }
            return null;
        }

        public AuthorDetailDto? GetById(int id)
        {
            var author = _unitOfWork.Authors.GetById(id);
            if (author is not null)
            {
                return AuthorMapper.ToAuthorDetailDto(author);
            }
            return null;
        }

        public Result Update(AuthorUpdateDto authorDto)
        {
            var authorToValidate = AuthorMapper.ToAuthorEntity(authorDto);
            var result = _authorValidator.Validate(authorToValidate);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    //var errors = result.Errors
                    //                   .Select(e => e.ErrorMessage)
                    //                   .ToList();
                    //return (false, errors);
                    return Result.Failure(result.Errors.Select(e => e.ErrorMessage).ToList());
                }
            }
            var author = _unitOfWork.Authors.GetById(authorDto.AuthorId);
            if (author is null)
                return Result.Failure("Author not found");
            //return (false, new List<string>() { "Author not found" });
            author.FirstName = authorDto.FirstName;
            author.LastName = authorDto.LastName;
            
            if (_unitOfWork.Authors.ExistSameName(author.FirstName, author.LastName, author.AuthorId))
                return Result.Failure("Author already exist!");
                //return (false, new List<string>() { "Author already exist!" });
            try
            {
                //_authorRepository.Update(author);
                _unitOfWork.Save();
                return Result.Success();
                //return (true, new List<string>());
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
                //return (false, new List<string>() { "Database error" });
            }
        }
    }
}

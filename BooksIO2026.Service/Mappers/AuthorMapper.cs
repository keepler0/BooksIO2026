using BooksIO2026.Entities;
using BooksIO2026.Service.DTOs;

namespace BooksIO2026.Service.Mappers
{
    public static class AuthorMapper
    {
        public static AuthorListDto ToAuthorListDto(Author author)
        {
            return new AuthorListDto
            {
                AuthorId = author.AuthorId,
                FullName = $"{author.FirstName} {author.LastName}"
            };
        }
        public static AuthorUpdateDto ToAuthorUpdateDto(Author author)
        {
            return new AuthorUpdateDto
            {
                AuthorId = author.AuthorId,
                FirstName = author.FirstName,
                LastName = author.LastName
            };
        }
        public static Author ToAuthor(AuthorUpdateDto authorUpdateDto)
        {
            return new Author
            {
                AuthorId = authorUpdateDto.AuthorId,
                FirstName = authorUpdateDto.FirstName,
                LastName = authorUpdateDto.LastName
            };
        }
        public static AuthorDetailDto ToAuthorDetailDto(Author author)
        {
            return new AuthorDetailDto
            {
                AuthorId = author.AuthorId,
                FirstName = author.FirstName,
                LastName = author.LastName
            };
        }
        public static Author ToAuthor(AuthorCreateDto AuthorDto)
        {
            return new Author
            {
                FirstName = AuthorDto.FirstName,
                LastName = AuthorDto.LastName
            };
        }
    }
}

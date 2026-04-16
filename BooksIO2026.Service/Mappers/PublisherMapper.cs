using BooksIO2026.Entities;
using BooksIO2026.Service.DTOs.Publisher;

namespace BooksIO2026.Service.Mappers
{
    public static class PublisherMapper
    {
        public static PublisherListDto ToPublisherListDto(Publisher publisher)
        {
            return new PublisherListDto
            {
                PublisherId = publisher.PublisherId,
                Name = publisher.Name,
                Country = publisher.Country
            };
        }
        public static PublisherUpdateDto ToPublisherUpdateDto(Publisher publisher)
        {
            return new PublisherUpdateDto
            {
                PublisherId = publisher.PublisherId,
                Name = publisher.Name,
                Country = publisher.Country,
                FoundedDate = publisher.FoundedDate,
                Email = publisher.Email,
                IsActive = publisher.IsActive
            };
        }
        public static PublisherDetailDto ToPublisherDetailDto(Publisher publisher)
        {
            return new PublisherDetailDto
            {
                PublisherId = publisher.PublisherId,
                Name = publisher.Name,
                Country = publisher.Country,
                FoundedDate = publisher.FoundedDate,
                IsActive = publisher.IsActive
            };
        }
        public static Publisher ToPublisher(PublisherUpdateDto publisherDto)
        {
            return new Publisher
            {
                PublisherId = publisherDto.PublisherId,
                Name = publisherDto.Name,
                Country = publisherDto.Country,
                FoundedDate = publisherDto.FoundedDate,
                Email = publisherDto.Email,
                IsActive = publisherDto.IsActive
            };
        }
        public static Publisher ToPublisher(PublisherCreateDto publisherDto)
        {
            return new Publisher
            {
                Name = publisherDto.Name,
                Country = publisherDto.Country,
                FoundedDate = publisherDto.FoundedDate,
                Email = publisherDto.Email,
                IsActive = true //TODO: preguntar si esto es valido ya que esto era una propiedad donde solo el sistema asignaba
            };
        }
    }
}

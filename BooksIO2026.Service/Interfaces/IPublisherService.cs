using BooksIO2026.Service.DTOs.Publisher;

namespace BooksIO2026.Service.Interfaces
{
    public interface IPublisherService
    {
        List<PublisherListDto> GetAll();
        PublisherDetailDto? GetById(int id);
        PublisherUpdateDto? GetPublisherForUpdate(int id);
        (bool success, List<string> Errors) Add(PublisherCreateDto publisherDto);
        (bool success, List<string> Errors) Update(PublisherUpdateDto publisherDto);
        (bool success, List<string> Errors) Delete(int id);
    }
}

using BooksIO2026.Service.Common;
using BooksIO2026.Service.DTOs.Publisher;

namespace BooksIO2026.Service.Interfaces
{
    public interface IPublisherService
    {
        Result<List<PublisherListDto>> GetAll();
        Result<PublisherDetailDto> GetById(int id);
        Result<PublisherUpdateDto> GetPublisherForUpdate(int id);
        Result<PublisherDetailDto> GetPublisherDetails(int id);
        Result Add(PublisherCreateDto publisherDto);
        Result Update(PublisherUpdateDto publisherDto);
        Result Delete(int id);

        //List<PublisherListDto> GetAll();
        //PublisherDetailDto? GetById(int id);
        //PublisherUpdateDto? GetPublisherForUpdate(int id);
        //(bool success, List<string> Errors) Add(PublisherCreateDto publisherDto);
        //(bool success, List<string> Errors) Update(PublisherUpdateDto publisherDto);
        //(bool success, List<string> Errors) Delete(int id);
    }
}

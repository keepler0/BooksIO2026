using BooksIO2026.Data;
using BooksIO2026.Entities;
using BooksIO2026.Service.Common;
using BooksIO2026.Service.DTOs.Publisher;
using BooksIO2026.Service.Interfaces;
using BooksIO2026.Service.Mappers;
using FluentValidation;

namespace BooksIO2026.Service.Services
{
    public class PublisherService : IPublisherService
    {
        //private readonly IPublisherRepository _publisherRepository;
        private readonly IValidator<Publisher> _publisherValidator;
        private readonly IUnitOfWork _unitOfWork;

        public PublisherService(//IPublisherRepository publisherRepository,
                                IValidator<Publisher> publisherValidator,
                                IUnitOfWork unitOfWork)
        {
            //_publisherRepository = publisherRepository;
            _publisherValidator = publisherValidator;
            _unitOfWork = unitOfWork;
        }

        public Result Add(PublisherCreateDto publisherDto)
        {
            var publisher = PublisherMapper.ToPublisher(publisherDto);
            var result = _publisherValidator.Validate(publisher);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    return Result.Failure(result.Errors.Select(error => error.ErrorMessage).ToList());
                    //var errors = result.Errors.Select(error => error.ErrorMessage).ToList();
                    //return (false, errors);
                }
            }
            if (!_unitOfWork.Publishers.Exist(publisher.Name, publisher.Country))
            {
                try
                {
                    _unitOfWork.Publishers.Add(publisher);
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
            return Result.Failure("Publisher already exist");
            //return (false, new List<string>() { "Publisher already exist" });
        }

        public Result Delete(int id)
        {
            if (!_unitOfWork.Publishers.Exist(null, null, id))
                return Result.Failure("Publisher not found");
                //return (false, new List<string>() { "Publisher not found" });

            if (_unitOfWork.Publishers.HasBooks(id))
                return Result.Failure("Delete denied...the publisher has associated books");
            try
            {
                _unitOfWork.Publishers.Delete(id);
                _unitOfWork.Save();
                return Result.Success();
                //return (true, new List<string>());
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
                //return (true, new List<string>() { "Database error" });
            }
        }

        public List<PublisherListDto> GetAll()
        {
            return _unitOfWork.Publishers.GetAll()
                                         .Select(p => PublisherMapper.ToPublisherListDto(p))
                                         .ToList();
        }

        public PublisherDetailDto? GetById(int id)
        {
            var publisher = _unitOfWork.Publishers.GetById(id);
            //if (publisher is not null)
            //{
            //    return PublisherMapper.ToPublisherDetailDto(publisher);
            //}
            //return null;
            return publisher is not null ? PublisherMapper.ToPublisherDetailDto(publisher) : null;
        }

        public PublisherUpdateDto? GetPublisherForUpdate(int id)
        {
            var publisher = _unitOfWork.Publishers.GetById(id);
            //if (publisher is not null)
            //{
            //    return PublisherMapper.ToPublisherUpdateDto(publisher);
            //}
            return publisher is not null ? PublisherMapper.ToPublisherUpdateDto(publisher) : null;
        }

        public Result Update(PublisherUpdateDto publisherDto)
        {
            var publisher = _unitOfWork.Publishers.GetById(publisherDto.PublisherId);
            if (publisher is null)
                return Result.Failure("Publisher not found");//return (false, new List<string>() { "Publisher not found" });
            var publisherToValidate = PublisherMapper.ToPublisher(publisherDto);
            var result = _publisherValidator.Validate(publisherToValidate);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    return Result.Failure(result.Errors
                                                .Select(e => e.ErrorMessage)
                                                .ToList());
                    //var errors = result.Errors
                    //                  .Select(e => e.ErrorMessage)
                    //                  .ToList();
                    //return (false, errors);
                }
            }
            publisher.Name = publisherDto.Name;
            publisher.Country = publisherDto.Country;
            publisher.FoundedDate = publisherDto.FoundedDate;
            publisher.Email = publisherDto.Email;
            publisher.IsActive = publisherDto.IsActive;
            
            if (_unitOfWork.Publishers.Exist(publisherDto.Name,
                                              publisherDto.Country,
                                              publisherDto.PublisherId))
            {
                return Result.Failure("Publisher already exist!");
                //return (false, new List<string>() { "Publisher already exist!" });
            }
            try
            {
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

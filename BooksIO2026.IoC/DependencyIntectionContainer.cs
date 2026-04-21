using BooksIO2026.Data;
using BooksIO2026.Data.Interfaces;
using BooksIO2026.Data.Repositories;
using BooksIO2026.Entities;
using BooksIO2026.Service.Interfaces;
using BooksIO2026.Service.Services;
using BooksIO2026.Service.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BooksIO2026.IoC
{
    public static class DependencyIntectionContainer
    {
        public static IServiceProvider Configure()
        {
            var services = new ServiceCollection();
            services.AddDbContext<BooksDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region Author
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IValidator<Author>, AuthorValidator>();
            #endregion
            #region Publisher
            services.AddScoped<IPublisherRepository, PublisherRepository>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddScoped<IValidator<Publisher>, PublisherValidator>();
            #endregion
            #region Book
            services.AddScoped<IValidator<Book>, BookValidator>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBookService, BookService>();
            #endregion

            return services.BuildServiceProvider();

        }
    }
}

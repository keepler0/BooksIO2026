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
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IValidator<Author>, AuthorValidator>();

            return services.BuildServiceProvider();

        }
    }
}

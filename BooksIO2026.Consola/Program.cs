using BooksIO2026.IoC;
using BooksIO2026.Service.DTOs;
using BooksIO2026.Service.Interfaces;
using BooksIO2026.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BooksIO2026.Consola
{
    internal class Program
    {
        static IServiceProvider serviceProvider = DependencyIntectionContainer.Configure();
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Library Manager");
                Console.WriteLine("[1] Authors");
                Console.WriteLine("[2] Books");
                Console.WriteLine("[0] Exit");
                Console.Write("Select an option: ");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "0":
                        Environment.Exit(0);
                        break;
                    case "1":
                        AuthorMenu();
                        break;
                    case "2":
                        BooksMenu();
                        break;
                }

            } while (true);
        }

        private static void BooksMenu()
        {
            throw new NotImplementedException();
        }

        private static void AuthorMenu()
        {
            using (var scoped = serviceProvider.CreateScope())
            {
                var authorService = scoped.ServiceProvider.GetRequiredService<IAuthorService>();
                do
                {
                    Console.Clear();
                    Console.WriteLine("Authors menu");
                    Console.WriteLine("[1] List of authors");
                    Console.WriteLine("[2] Add author");
                    Console.WriteLine("[3] Delete author");
                    Console.WriteLine("[4] Update author");
                    Console.WriteLine("[0] Exit");
                    Console.Write("Select an option: ");
                    var option = Console.ReadLine();
                    switch (option)
                    {
                        case "0":
                            Console.Clear();
                            return;
                        case "1":
                            ListAuthors(authorService);
                            break;
                        case "2":
                            AddAuthor(authorService);
                            break;
                        case "3":
                            DeleteAuthor(authorService);
                            break;
                        case "4":
                            UpdateAuthor(authorService);
                            break;

                    }
                } while (true);
            }
        }

        private static void UpdateAuthor(IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("Update an author");
            ShowAuthors(authorService);
            Console.Write("Select an ID to update");
            var authorId = int.Parse(Console.ReadLine()!);
            var authorToUpdate = authorService.GetAuthorForUpdate(authorId);
            if (authorToUpdate is not null)
            {
                Console.Write($"Are you sure to update {authorToUpdate.FullName}? : ");
                var yesOrNo = Console.ReadLine()!;
                if (yesOrNo.ToUpper() == "NO") return;
                var fullname = CreateFullName();
                authorToUpdate.FirstName = string.IsNullOrEmpty(fullname.Item1) ? authorToUpdate.FirstName : fullname.Item1;
                authorToUpdate.LastName = string.IsNullOrEmpty(fullname.Item2) ? authorToUpdate.LastName : fullname.Item2;

                var result = authorService.Update(authorToUpdate);
                if (!result.Success)//este bloque se ejecutara si la actualización no fue exitosa, es decir, si hubo errores
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine(error);//mostramos los errores
                    }
                }
                else
                {
                    Console.WriteLine("Author seccessfully updated!");
                }
            }
            else
            {
                Console.WriteLine("ERROR: The author was not found.");
            }
            CleanScreen();
        }

        private static (string, string) CreateFullName()
        {
            Console.Write("Set new first name: ");
            var firstName = Console.ReadLine()!;
            Console.Write("Set new Last name: ");
            var lastName = Console.ReadLine()!;
            return (firstName, lastName);
        }

        private static void DeleteAuthor(IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("Delete actor");
            Console.WriteLine("List of available authors");

            ShowAuthors(authorService);
            Console.Write("Select an ID to delete: ");
            var id = int.Parse(Console.ReadLine()!);
            var authorToDelete = authorService.GetById(id);
            Console.Write($"Are you sure to delete the author: {authorToDelete?.FullName}? : ");
            var yesOrNo = Console.ReadLine()!;
            if (yesOrNo.ToUpper() == "NO") return;
            var result = authorService.Delete(id);
            if (!result.Success)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error);
                }
            }
            else
            {
                Console.WriteLine("Author deleted succesfully");
            }
            CleanScreen();
        }

        private static void AddAuthor(IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("Add a new author");
            var fullname = CreateFullName();
            var newAuthorDto = new AuthorCreateDto
            {
                FirstName = fullname.Item1,
                LastName = fullname.Item2
            };
            var result = authorService.Add(newAuthorDto);
            if (!result.Success)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error);
                }
            }
            else
            {
                Console.WriteLine("Author added succesfully!");
            }
            CleanScreen();
        }

        private static void CleanScreen()
        {
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();
        }

        private static void ListAuthors(IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("List of authors");
            ShowAuthors(authorService);
            CleanScreen();
        }

        private static void ShowAuthors(IAuthorService authorService)
        {
            var authorsList = authorService.GetAll();
            foreach (var authorListDto in authorsList)
            {
                Console.WriteLine($"ID: {authorListDto.AuthorId,4} Name: {authorListDto.FullName,-30}");
            }
        }
    }
}

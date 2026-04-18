using BooksIO2026.IoC;
using BooksIO2026.Service.DTOs;
using BooksIO2026.Service.DTOs.Publisher;
using BooksIO2026.Service.Interfaces;
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
                Console.WriteLine("[3] Publishers");
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
                    case "3":
                        PublishersMenu();
                        break;
                }
                Console.Clear();
            } while (true);
        }
        #region Publisher
        private static void PublishersMenu()
        {
            using (var scoped = serviceProvider.CreateScope())
            {
                var publisherService = scoped.ServiceProvider.GetRequiredService<IPublisherService>();
                do
                {
                    Console.Clear();
                    Console.WriteLine("Publishers menu");
                    Console.WriteLine("[1] List of publishers");
                    Console.WriteLine("[2] Add publisher");
                    Console.WriteLine("[3] Update publisher");
                    Console.WriteLine("[4] Delete publisher");
                    Console.WriteLine("[0] Exit");
                    Console.Write("Select an option: ");
                    var option = Console.ReadLine();
                    switch (option)
                    {
                        case "0":
                            Console.Clear();
                            return;
                        case "1":
                            ListPublishers(publisherService);
                            break;
                        case "2":
                            AddPublisher(publisherService);
                            break;
                        case "3":
                            UpdatePublisher(publisherService);
                            break;
                        case "4":
                            DeletePublisher(publisherService);
                            break;

                    }
                } while (true);
            }
        }

        private static void DeletePublisher(IPublisherService publisherService)
        {
            Console.Clear();
            Console.WriteLine("Delete an publisher");
            Console.WriteLine("List of available publishers");

            ShowPublishers(publisherService);
            Console.Write("Select an ID to delete: ");
            var id = int.Parse(Console.ReadLine()!);
            var publisherToDelete = publisherService.GetById(id);
            Console.Write($"Are you sure to delete the publisher: {publisherToDelete?.Name}? : ");
            var yesOrNo = Console.ReadLine()!;
            if (yesOrNo.ToUpper() == "NO") return;
            var result = publisherService.Delete(id);
            if (!result.success)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error);
                }
            }
            else
            {
                Console.WriteLine("Publisher deleted succesfully");
            }
            CleanScreen();
        }

        private static void UpdatePublisher(IPublisherService publisherService)
        {
            Console.Clear();
            Console.WriteLine("Update an publisher");
            ShowPublishers(publisherService);
            Console.Write("Select an ID to update");
            var publisherId = int.Parse(Console.ReadLine()!);
            var publisherToUpdate = publisherService.GetPublisherForUpdate(publisherId);
            if (publisherToUpdate is not null)
            {
                Console.Write($"Are you sure to update {publisherToUpdate.Name}? : ");
                var yesOrNo = Console.ReadLine()!;
                if (yesOrNo.ToUpper() == "NO") return;
                var publisherData = publisherMaker(publisherToUpdate);
                publisherToUpdate.Name = publisherData.Item1;
                publisherToUpdate.Country = publisherData.Item2;
                publisherToUpdate.FoundedDate = publisherData.Item3;
                publisherToUpdate.Email = publisherData.Item4;
                publisherToUpdate.IsActive = publisherData.Item5;

                var result = publisherService.Update(publisherToUpdate);
                if (!result.success)//este bloque se ejecutara si la actualización no fue exitosa, es decir, si hubo errores
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

        private static void AddPublisher(IPublisherService publisherService)
        {
            Console.Clear();
            Console.WriteLine("Add a new publisher");
            var publisherData = publisherMaker();
            var newPulisherDto = new PublisherCreateDto
            {
                Name = publisherData.Item1,
                Country = publisherData.Item2,
                FoundedDate = publisherData.Item3,
                Email = publisherData.Item4
            };
            var result = publisherService.Add(newPulisherDto);
            if (!result.success)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error);
                }
            }
            else
            {
                Console.WriteLine("Publisher added succesfully!");
            }
            CleanScreen();
        }

        private static (string, string, DateTime, string?, bool) publisherMaker(PublisherUpdateDto? publisherDto = null)
        {
            Console.WriteLine("Insert the publisher's information");
            Console.Write("Name : ");
            var name = Console.ReadLine()!;
            Console.Write("Country : ");
            var country = Console.ReadLine()!;
            Console.Write("Founded date: ");
            var foudedDate = DateTime.Parse(Console.ReadLine()!);
            Console.Write("Email: ");
            var email = Console.ReadLine();

            if (publisherDto is not null)
            {
                Console.Write("Is it currently active? [yes] o [no]:");
                var isActive = Console.ReadLine()!.ToUpper() == "YES" ? true : false;
                return (string.IsNullOrEmpty(name) ? publisherDto.Name : name,
                        string.IsNullOrEmpty(country) ? publisherDto.Country : country,
                        foudedDate,
                        string.IsNullOrEmpty(email) ? publisherDto.Email : email, isActive);

            }
            return (name, country, foudedDate, email, true);
        }

        private static void ListPublishers(IPublisherService publisherService)
        {
            Console.Clear();
            Console.WriteLine("List of Publishers");
            ShowPublishers(publisherService);
            CleanScreen();
        }
        private static void ShowPublishers(IPublisherService publisherService)
        {
            var publisherList = publisherService.GetAll();
            foreach (var publisherListDto in publisherList)
            {
                Console.WriteLine($"ID: {publisherListDto.PublisherId,4} Name: {publisherListDto.Name,-20} Country: {publisherListDto.Country,-20}");
            }
        }
        #endregion
        private static void BooksMenu()
        {
            throw new NotImplementedException();
        }
        #region Author Menu
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
                    Console.WriteLine("[3] Update author");
                    Console.WriteLine("[4] Delete author");
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
                            UpdateAuthor(authorService);
                            break;
                        case "4":
                            DeleteAuthor(authorService);
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
        #endregion
        private static void CleanScreen()
        {
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
            Console.Clear();
        }
    }
}

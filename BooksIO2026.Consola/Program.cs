using BooksIO2026.IoC;
using BooksIO2026.Service.DTOs;
using BooksIO2026.Service.DTOs.Book;
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
            #region MainMenu
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
            #endregion
        }
        #region PublisherMenu
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
                    Console.WriteLine("[5] View publisher details");
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
                        case "5":
                            ShowPublisherData(publisherService);
                            break;
                    }
                } while (true);
            }
        }

        private static void ShowPublisherData(IPublisherService publisherService)
        {
            Console.Clear();
            Console.WriteLine("=== Publisher Details ===\n");

            var publishersResult = publisherService.GetAll();

            if (publishersResult.Isfailure)
            {
                ShowErrors(publishersResult.Errors);
                CleanScreen();
                return;
            }

            foreach (var publisher in publishersResult.Value!)
            {
                Console.WriteLine($"{publisher.PublisherId} - {publisher.Name}");
            }

            Console.WriteLine();

            int publisherId;
            while (true)
            {
                Console.Write("Select a Publisher ID (0 to quit): ");
                var input = Console.ReadLine();

                if (!int.TryParse(input, out publisherId))
                {
                    Console.WriteLine("You must enter a number.");
                    continue;
                }
                if (publisherId == 0) return;
                var exists = publishersResult.Value!.Any(p => p.PublisherId == publisherId);

                if (!exists)
                {
                    Console.WriteLine("The ID does not correspond to a listed publisher.");
                    continue;
                }

                break;
            }

            var result = publisherService.GetPublisherDetails(publisherId);

            Console.Clear();
            Console.WriteLine("=== Publisher Details ===\n");

            if (result.Isfailure)
            {
                ShowErrors(result.Errors);
                Console.ReadLine();
            }
            else
            {
                var publisher = result.Value!;

                Console.WriteLine($"Id: {publisher.PublisherId}");
                Console.WriteLine($"Name: {publisher.Name}");
                Console.WriteLine($"Country: {publisher.Country}");
                Console.WriteLine($"Founded Date: {publisher.FoundedDate:dd/MM/yyyy}");
                Console.WriteLine($"Email: {publisher.Email ?? "Not provided"}");
                Console.WriteLine();

                Console.WriteLine("--- BOOKS ---");

                if (!publisher.Books.Any())
                {
                    Console.WriteLine("No associated books.");
                }
                else
                {
                    foreach (var book in publisher.Books)
                    {
                        Console.WriteLine($"{book.BookId} - {book.Title}");
                    }
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static void DeletePublisher(IPublisherService publisherService)
        {
            Console.Clear();
            Console.WriteLine("Delete an publisher");
            Console.WriteLine("List of available publishers");

            ShowPublishers(publisherService);
            Console.Write("Select an ID to delete: ");
            var id = int.Parse(Console.ReadLine()!);
            var publisherResult = publisherService.GetById(id);
            if (publisherResult.Isfailure)
            {
                ShowErrors(publisherResult.Errors);
                return;
            }
            var publisherToDelete = publisherResult.Value!;
            Console.Write($"Are you sure to delete the publisher: {publisherToDelete?.Name}? : ");
            var yesOrNo = Console.ReadLine()!;
            if (yesOrNo.ToUpper() == "NO") return;
            var result = publisherService.Delete(id);
            if (result.Isfailure)
            {
                ShowErrors(result.Errors);
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
            var publisherResult = publisherService.GetPublisherForUpdate(publisherId);
            if (publisherResult.Isfailure)
            {
                ShowErrors(publisherResult.Errors);
                return;
            }
            var publisherToUpdate = publisherResult.Value!;
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
            if (result.Isfailure)//este bloque se ejecutara si la actualización no fue exitosa, es decir, si hubo errores
            {
                ShowErrors(result.Errors);
            }
            else
            {
                Console.WriteLine("Author seccessfully updated!");
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
            if (result.Isfailure)
            {
                ShowErrors(result.Errors);
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
            var publisherResult = publisherService.GetAll();
            if (publisherResult.Isfailure)
            {
                ShowErrors(publisherResult.Errors);
                return;
            }
            var publisherList = publisherResult.Value!;
            foreach (var publisherListDto in publisherList)
            {
                Console.WriteLine($"ID: {publisherListDto.PublisherId,4} Name: {publisherListDto.Name,-20} Country: {publisherListDto.Country,-20}");
            }
        }
        #endregion

        #region BookMenu
        private static void BooksMenu()
        {
            using (var scoped = serviceProvider.CreateScope())
            {
                var bookService = scoped.ServiceProvider.GetRequiredService<IBookService>();
                var authorService = scoped.ServiceProvider.GetRequiredService<IAuthorService>();
                var publisherService = scoped.ServiceProvider.GetRequiredService<IPublisherService>();
                do
                {
                    Console.Clear();
                    Console.WriteLine("Books menu");
                    Console.WriteLine("[1] List of books");//TODO: Agregar mas espacio entre registro para mas legibilidad
                    Console.WriteLine("[2] Add new book");
                    Console.WriteLine("[3] Update an book");
                    Console.WriteLine("[4] Delete an book");
                    Console.WriteLine("[5] View book details");
                    Console.WriteLine("[6] Books grouped by publisher");
                    Console.WriteLine("[7] Top 10 more expensive books");
                    Console.WriteLine("[0] Exit");
                    Console.Write("Select an option: ");
                    var option = Console.ReadLine();
                    switch (option)
                    {
                        case "0":
                            Console.Clear();
                            return;
                        case "1":
                            ListBooks(bookService);
                            break;
                        case "2":
                            AddBook(bookService, publisherService, authorService);
                            break;
                        case "3":
                            UpdateBook(bookService, publisherService, authorService);
                            break;
                        case "4":
                            DeleteBook(bookService);
                            break;
                        case "5":
                            ShowBookDetails(bookService);
                            break;
                        case "6":
                            ShowBooksGroupedByPublisher(bookService);
                            break;
                        case "7":
                            ShowMoreExpensiveBooks(bookService);
                            break;
                    }
                } while (true);
            }
        }

        private static void ShowMoreExpensiveBooks(IBookService bookService)
        {
            Console.Clear();
            Console.WriteLine("=== TOP 10 Books More Expensive ===\n");

            var resultT = bookService.GetMoreExpensiveBooks();

            if (resultT.Isfailure)
            {
                ShowErrors(resultT.Errors);
            }
            var books = resultT.Value;
            Console.WriteLine(
                $"{"ID",-5} {"TITLE",-30} {"AUTHOR",-25} {"PUBLISHER",-20} {"PRICE",10}");

            Console.WriteLine(new string('-', 95));

            foreach (var book in books!)
            {
                Console.WriteLine(
                    $"{book.BookId,-5} " +
                    $"{book.Title,-30} " +
                    $"{book.AuthorName,-25} " +
                    $"{book.PublisherName,-20} " +
                    $"{book.Price,10}"
                );
            }
            CleanScreen();
        }

        private static void ShowBooksGroupedByPublisher(IBookService bookService)
        {
            Console.Clear();
            Console.WriteLine("BOOKS GROUPED BY PUBLISHER");
            Console.WriteLine(new string('-', 90));

            var resultT = bookService.GetBooksGroupedByPublisher();

            if (!resultT.IsSuccess || resultT.Value == null || !resultT.Value.Any())
            {
                Console.WriteLine("No records found.");
                Console.ReadKey();
                return;
            }

            var groupedBooks = resultT.Value.ToList();

            Console.WriteLine(
                $"{"#",-5}" +
                $"{"Publisher",-25}" +
                $"{"Books",10}" +
                $"{"Stock",10}");

            Console.WriteLine(new string('-', 90));

            for (int i = 0; i < groupedBooks.Count; i++)
            {
                var item = groupedBooks[i];

                Console.WriteLine(
                    $"{i + 1,-5}" +
                    $"{item.PublisherName,-25}" +
                    $"{item.TotalCount,10}" +
                    //$"{item.TotalStock,10}");
                    $"{item.AveragePrice,10}");
            }

            Console.WriteLine(new string('-', 90));
            Console.WriteLine();
            Console.Write("Would you like to view details for a publisher? (Y/N): ");

            var answer = Console.ReadLine()?.Trim().ToUpper();

            if (answer != "Y")
                return;

            Console.Write("Enter publisher number: ");

            if (!int.TryParse(Console.ReadLine(), out int option) ||
                option < 1 || option > groupedBooks.Count)
            {
                Console.WriteLine("Invalid option.");
                Console.ReadKey();
                return;
            }

            var selectedPublisherId = groupedBooks[option - 1].PublisherId;
            var selectedPublisherName = groupedBooks[option - 1].PublisherName;

            ShowPublisherBooksDetail(bookService, selectedPublisherId, selectedPublisherName);
        }

        private static void ShowPublisherBooksDetail(IBookService bookService, int selectedPublisherId, string selectedPublisherName)
        {
            Console.Clear();
            Console.WriteLine($"BOOK DETAILS - {selectedPublisherName.ToUpper()}");
            Console.WriteLine(new string('-', 100));

            var resultT = bookService.GetBooksByPublisher(selectedPublisherId);

            if (!resultT.IsSuccess || resultT.Value == null || !resultT.Value.Any())
            {
                Console.WriteLine("No books found for this publisher.");
                Console.ReadKey();
                return;
            }

            var books = resultT.Value;

            Console.WriteLine(
                $"{"Title",-40}" +
                $"{"Author",-25}" +
                $"{"Price",15}" +
                $"{"Stock",10}");

            Console.WriteLine(new string('-', 100));

            foreach (var book in books)
            {
                Console.WriteLine(
                    $"{book.Title,-40}" +
                    $"{book.AuthorName,-25}" +
                    $"{book.Price,15}");
                    //$"{book.Stock,10}");
            }

            Console.WriteLine(new string('-', 100));
            Console.ReadKey();
        }

        private static void ShowBookDetails(IBookService bookService)
        {
            Console.Clear();
            ShowBooks(bookService);
            Console.Write("Select an ID: ");
            var id = int.Parse(Console.ReadLine()!);
            var bookResult = bookService.GetDetail(id);
            if (bookResult.Isfailure)
            {
                ShowErrors(bookResult.Errors);
                return;
            }
            var bookDto = bookResult.Value!;
            var isActiveMessage = bookDto.IsActive ? "Yes"
                                                   : "No";
            Console.Clear();
            Console.WriteLine($" ID: {bookDto.BookId}\n Title: {bookDto.Title}\n Price: ${bookDto.Price}\n Published date: {bookDto.PublishedDate.ToShortDateString()}\n Author: {bookDto.AuthorName}\n Publisher: {bookDto.PublisherName}\n Available?: {isActiveMessage}");
            CleanScreen();
        }

        private static void DeleteBook(IBookService bookService)
        {
            Console.Clear();
            Console.WriteLine("Delete an publisher");
            Console.WriteLine("List of available publishers");

            ShowBooks(bookService);
            Console.Write("Select an ID to delete: ");
            var id = int.Parse(Console.ReadLine()!);
            var bookResult = bookService.GetById(id);
            if (bookResult.Isfailure)
            {
                ShowErrors(bookResult.Errors);
                return;
            }
            var bookToDelete = bookResult.Value!;
            Console.Write($"Are you sure to delete the book: {bookToDelete?.Title}? : ");
            var yesOrNo = Console.ReadLine()!;
            if (yesOrNo.ToUpper() == "NO") return;
            var result = bookService.Delete(id);
            if (result.Isfailure)
            {
                ShowErrors(result.Errors);
            }
            else
            {
                Console.WriteLine("Book deleted succesfully");
            }
            CleanScreen();
        }

        private static void UpdateBook(IBookService bookService,
                                       IPublisherService publisherService,
                                       IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("Update an book");
            ShowBooks(bookService);
            Console.Write("Select an ID to update");
            var bookId = int.Parse(Console.ReadLine()!);
            var bookResult = bookService.GetBookForUpdate(bookId);
            if (bookResult.Isfailure)
            {
                ShowErrors(bookResult.Errors);
                return;
            }
            var bookToUpdate = bookResult.Value!;
            Console.Write($"Are you sure to update {bookToUpdate.Title}? : ");
            var yesOrNo = Console.ReadLine()!;
            if (yesOrNo.ToUpper() == "NO") return;
            var bookData = BookMaker(bookToUpdate);
            bookToUpdate.Title = bookData.Item1;
            bookToUpdate.Price = bookData.Item2;
            bookToUpdate.PublishedDate = bookData.Item3;
            bookToUpdate.IsActive = bookData.Item4;
            bookToUpdate.AuthorId = SelectAuthorId(authorService);
            bookToUpdate.PublisherId = SelectPublisherId(publisherService);
            var result = bookService.Update(bookToUpdate);
            if (result.Isfailure)
            {
                ShowErrors(result.Errors);
            }
            else
            {
                Console.WriteLine("Author seccessfully updated!");
            }
            CleanScreen();
        }

        private static void AddBook(IBookService bookService,
                                    IPublisherService publisherService,
                                    IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("Add a new book");
            var bookData = BookMaker(null);
            int authorId = SelectAuthorId(authorService);
            int publisherId = SelectPublisherId(publisherService);
            var newBookDto = new BookCreateDto
            {
                Title = bookData.Item1,
                Price = bookData.Item2,
                PublishedDate = bookData.Item3,
                IsActive = bookData.Item4,
                AuthorId = authorId,
                PublisherId = publisherId
            };
            var result = bookService.Add(newBookDto);
            if (result.Isfailure)
            {
                ShowErrors(result.Errors);
            }
            else
            {
                Console.WriteLine("Author added succesfully!");
            }
            CleanScreen();
        }

        private static int SelectPublisherId(IPublisherService publisherService)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Select the book publisher's ID");
                ShowPublishers(publisherService);
                Console.Write("id: ");
                var id = int.Parse(Console.ReadLine()!);
                var PublishersIDs = publisherService.GetAll()
                                                    .Value!
                                                    .Select(p => p.PublisherId)
                                                    .ToList();
                if (PublishersIDs.Contains(id))
                {
                    return id;
                }
                Console.WriteLine("The publisher ID does not exist, please try again....");
                Thread.Sleep(1000);
            } while (true);
        }

        private static int SelectAuthorId(IAuthorService authorService)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Select the book author's ID");
                ShowAuthors(authorService);
                Console.Write("id: ");
                var id = int.Parse(Console.ReadLine()!);
                var AuthorsIds = authorService.GetAll()
                                              .Value!
                                              .Select(a => a.AuthorId)
                                              .ToList();
                if (AuthorsIds.Contains(id))
                {
                    return id;
                }
                Console.WriteLine("The author ID does not exist, please try again....");
                Thread.Sleep(1000);
            } while (true);
        }

        private static (string, decimal, DateTime, bool) BookMaker(BookUpdateDto? bookDto)
        {
            Console.Write("Title: ");
            var title = Console.ReadLine();
            Console.Write("Price: ");
            var price = decimal.Parse(Console.ReadLine()!);
            Console.Write("Published Date: ");
            var publishedDate = DateTime.Parse(Console.ReadLine()!);
            Console.Write("Available?: ");
            var isActive = Console.ReadLine()!.ToUpper() == "YES" ? true : false;
            if (bookDto is not null)
            {
                title = string.IsNullOrEmpty(title) ? bookDto.Title : title;
                price = price < 0 ? bookDto.Price : price;
            }
            return (title!, price, publishedDate, isActive);
        }
        //TODO continuar con el video 33:13

        private static void ListBooks(IBookService bookService)
        {
            Console.Clear();
            Console.WriteLine("List of books");
            ShowBooks(bookService);
            CleanScreen();
        }

        private static void ShowBooks(IBookService bookService)
        {
            var booksResult = bookService.GetAll();
            if (booksResult.Isfailure)
            {
                ShowErrors(booksResult.Errors);
                return;
            }
            var booksList= booksResult.Value!;
            foreach (var book in booksList)
            {
                Console.WriteLine($" ID: {book.BookId}\n Title: {book.Title}\n Price: ${book.Price}");
                Console.WriteLine();
            }
        }
        #endregion

        #region AuthorMenu
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
                    Console.WriteLine("[5] View author details");
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
                        case "5":
                            ShowAuthorDetails(authorService);
                            break;
                    }
                } while (true);
            }
        }

        private static void ShowAuthorDetails(IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("Author's Details");

            var authorsResult = authorService.GetAll();

            if (authorsResult.Isfailure)
            {
                ShowErrors(authorsResult.Errors);
            }

            foreach (var author in authorsResult.Value!)
            {
                Console.WriteLine($"{author.AuthorId,2} - {author.FullName}");
            }

            Console.WriteLine();

            int authorId;
            while (true)
            {
                Console.Write("Select An Author ID to view details (0 to quit): ");
                var input = Console.ReadLine();

                if (!int.TryParse(input, out authorId))
                {
                    Console.WriteLine("You must enter a number.");
                    continue;
                }
                if (authorId == 0) return;
                var exists = authorsResult.Value!.Any(a => a.AuthorId == authorId);

                if (!exists)
                {
                    Console.WriteLine("The ID does not correspond to a listed author.");
                    continue;
                }

                break;
            }

            var result = authorService.GetAuthorDetails(authorId);

            Console.Clear();
            Console.WriteLine("=== Author Details ===\n");

            if (result.Isfailure)
            {
                ShowErrors(result.Errors);
                CleanScreen();
                return;

            }
            var authorDto = result.Value!;

            Console.WriteLine($"Id: {authorDto.AuthorId}");
            Console.WriteLine($"FullName: {authorDto.FirstName} {authorDto.LastName}");
            Console.WriteLine();

            Console.WriteLine("--- Books ---");

            if (!authorDto.books.Any())
            {
                Console.WriteLine("No associated books.");
            }
            else
            {
                foreach (var book in authorDto.books)
                {
                    Console.WriteLine($"{book.BookId} - {book.Title}");
                }
            }
            CleanScreen();
        }

        private static void UpdateAuthor(IAuthorService authorService)
        {
            Console.Clear();
            Console.WriteLine("Update an author");
            ShowAuthors(authorService);
            Console.Write("Select an ID to update");
            var authorId = int.Parse(Console.ReadLine()!);
            var authorResult = authorService.GetAuthorForUpdate(authorId);
            if (authorResult.Isfailure)
            {
                ShowErrors(authorResult.Errors);
                return;
            }
            var authorToUpdate = authorResult.Value!;
            {
                Console.Write($"Are you sure to update {authorToUpdate.FullName}? : ");
                var yesOrNo = Console.ReadLine()!;
                if (yesOrNo.ToUpper() == "NO") return;
                var fullname = CreateFullName();
                authorToUpdate.FirstName = string.IsNullOrEmpty(fullname.Item1) ? authorToUpdate.FirstName : fullname.Item1;
                authorToUpdate.LastName = string.IsNullOrEmpty(fullname.Item2) ? authorToUpdate.LastName : fullname.Item2;

                var result = authorService.Update(authorToUpdate);
                if (result.Isfailure)//este bloque se ejecutara si la actualización no fue exitosa, es decir, si hubo errores
                {
                    //foreach (var error in result.Errors)
                    //{
                    //    Console.WriteLine(error);//mostramos los errores
                    //}
                    ShowErrors(result.Errors);
                }
                else
                {
                    Console.WriteLine("Author seccessfully updated!");
                }
            }
            CleanScreen();
        }

        private static void ShowErrors(List<string> errors)
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }
            Thread.Sleep(1000);
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
            var authorResult = authorService.GetById(id);
            if (authorResult.Isfailure)
            {
                ShowErrors(authorResult.Errors);
                return;
            }
            var authorToDelete = authorResult.Value!;
            Console.Write($"Are you sure to delete the author: {authorToDelete?.FullName}? : ");
            var yesOrNo = Console.ReadLine()!;
            if (yesOrNo.ToUpper() == "NO") return;
            var result = authorService.Delete(id);
            if (result.Isfailure)
            {
                //foreach (var error in result.Errors)
                //{
                //    Console.WriteLine(error);
                //}
                ShowErrors(result.Errors);
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
            if (result.Isfailure)
            {
                //foreach (var error in result.Errors)
                //{
                //    Console.WriteLine(error);
                //}
                ShowErrors(result.Errors);
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
            var authorsResult = authorService.GetAll();
            if (authorsResult.Isfailure)
            {
                ShowErrors(authorsResult.Errors);
                return;
            }
            var authorsList = authorsResult.Value!;
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

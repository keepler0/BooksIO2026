using BooksIO2026.Entities;
using BooksIO2026.Service.Interfaces;
using BooksIO2026.Service.Services;

namespace BooksIO2026.Consola
{
    internal class Program
    {
        static IAuthorService authorService = new AuthorService();
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
                case "1":
                    ListAuthors();
                    break;
                case "2":
                    AddAuthor();
                    break;
                case "3":
                    DeleteAuthor();
                    break;
                case "4":
                    UpdateAuthor();
                    break;
            }
        }

        private static void UpdateAuthor()
        {
            Console.Clear();
            Console.WriteLine("Update an author");
            ShowAuthors();

            Console.WriteLine("Select an ID to updte");
            var authorId = int.Parse(Console.ReadLine()!);
            var authorToUpdate = authorService.GetById(authorId);
            if (authorToUpdate is not null)
            {
                Console.WriteLine($"Are you sure to update {authorToUpdate.ToString()}?");
                var yesOrNo = Console.ReadLine()!;
                if (yesOrNo.ToUpper() == "NO") return;
                authorToUpdate = BuildAuthor(authorToUpdate);

                var result= authorService.Update(authorToUpdate);
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
        }

        private static Author BuildAuthor(Author? author = null)
        {
            Console.Write("Set new first name: ");
            var firstName = Console.ReadLine()!;
            Console.Write("Set new Last name");
            var LastName = Console.ReadLine()!;
            if (author is not null)
            {
                author.FirstName = string.IsNullOrWhiteSpace(firstName) ? author.FirstName : firstName;
                author.LastName = string.IsNullOrWhiteSpace(LastName) ? author.LastName : LastName;
                return author;
            }
            return new Author
            {
                FirstName = firstName,
                LastName = LastName
            };
        }

        private static void DeleteAuthor()
        {
            Console.Clear();
            Console.WriteLine("Delete actor");
            Console.WriteLine("List of available authors");

            ShowAuthors();
            Console.Write("Select an ID to delete: ");
            var id = int.Parse(Console.ReadLine()!);
            Console.WriteLine($"Are you sure to delete the author?");
            var yesOrNo = Console.ReadLine()!;
            if (yesOrNo.ToUpper() == "NO") return;
            var result= authorService.Delete(id);
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
            Console.WriteLine("Press any key to continue....");
            Console.ReadKey();
        }

        private static void AddAuthor()
        {
            Console.Clear();
            Console.WriteLine("Add a new author");
            var newAuthor = BuildAuthor();
            var result=authorService.Add(newAuthor);
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
             Console.WriteLine("Press any key to continue....");
             Console.ReadKey();

        }


        private static void ListAuthors()
        {
            Console.Clear();
            Console.WriteLine("List of authors");
            Console.WriteLine("ID  FullName");
            ShowAuthors();
        }

        private static void ShowAuthors()
        {
            var authorsList = authorService.GetAll();
            foreach (var author in authorsList)
            {
                Console.WriteLine($"{author.AuthorId} {author.ToString()}");
            }
        }
    }
}

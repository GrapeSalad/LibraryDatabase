using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using Library;
using Library.Objects;

namespace Library.Module
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["Home.cshtml"];
      };

      Get["/librarian"] = _ => {
        return View["librarian.cshtml"];
      };

      Get["/book/new"] = _ => {
        List<Book> allBooks = Book.GetAll();
        return View["book.cshtml", allBooks];
      };

      Get["/author/new"] = _ => {
        List<Author> allAuthors = Author.GetAll();
        return View["author.cshtml", allAuthors];
      };

      Get["/genre/new"] = _ => {
        List<Genre> allGenres = Genre.GetAll();
        return View["genre.cshtml", allGenres];
      };

      Post["/book/new"] = _ => {
        Book newBook = new Book(Request.Form["book-name"]);
        newBook.Save();
        return View["success.cshtml"];
      };

      Post["/author/new"] = _ => {
        Author newAuthor = new Author(Request.Form["author-name"]);
        newAuthor.Save();
        return View["success.cshtml"];
      };

      Post["/genre/new"] = _ => {
        Genre newGenre = new Genre(Request.Form["genre-name"]);
        newGenre.Save();
        return View["success.cshtml"];
      };

    }
  }
}

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
      //HOME PAGE
      Get["/"] = _ => {
        return View["Home.cshtml"];
      };
      //VIEW LIBRARIAN ACCESS PAGE - BOOK, AUTHOR, GENRE edits
      Get["/librarian"] = _ => {
        return View["librarian.cshtml"];
      };
      //VIEW ALL BOOKS AND ADD NEW BOOK
      Get["/book/new"] = _ => {
        List<Book> allBooks = Book.GetAll();
        return View["book.cshtml", allBooks];
      };
      //VIEW ALL AUTHORS AND ADD NEW AUTHOR
      Get["/author/new"] = _ => {
        List<Author> allAuthors = Author.GetAll();
        return View["author.cshtml", allAuthors];
      };
      //VIEW ALL GENRES AND ADD NEW GENRE
      Get["/genre/new"] = _ => {
        List<Genre> allGenres = Genre.GetAll();
        return View["genre.cshtml", allGenres];
      };
      //FORM TO ADD NEW BOOK TO DATABASE library TABLE books
      Post["/book/new"] = _ => {
        Book newBook = new Book(Request.Form["book-title"]);
        newBook.Save();
        return View["success.cshtml"];
      };
      //FORM TO ADD NEW AUTHOR TO DATABASE library TABLE authors
      Post["/author/new"] = _ => {
        Author newAuthor = new Author(Request.Form["author-name"]);
        newAuthor.Save();
        return View["success.cshtml"];
      };
      //FORM TO ADD NEW GENRE TO DATABASE library TABLE genres
      Post["/genre/new"] = _ => {
        Genre newGenre = new Genre(Request.Form["genre-name"]);
        newGenre.Save();
        return View["success.cshtml"];
      };
//BOOKS EDITING AND VIEWING
      //VIEW SINGLE BOOK, PASS INFORMATION TO THE PAGE TO VIEW NEEDED DETAILS
      Get["/book/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Book SelectedBook = Book.Find(parameters.id);
        List<Author> allAuthors = Author.GetAll();
        List<Genre> allGenres = Genre.GetAll();
        model.Add("book", SelectedBook);
        model.Add("author", allAuthors);
        model.Add("genre", allGenres);
        return View["book_edit.cshtml", model];
      };
      //ASSOCIATES AUTHOR WITH BOOK, VIA THE JOIN TABLE ACCESSED BY THE AddAuthorToJoinTable METHOD
      Post["/book/{id}/edit_author"] = _ => {
        Author newAuthor = Author.Find(Request.Form["author-id"]);
        Book currentBook = Book.Find(Request.Form["book-id"]);
        currentBook.AddAuthorToJoinTable(newAuthor);
        return View["success.cshtml"];
      };
      //EDIT TITLE OF THE CURRENT BOOK
      Patch["/book/{id}/edit_title"] = parameters => {
        Book SelectedBook = Book.Find(parameters.id);
        string newName = Request.Form["book-title"];
        SelectedBook.Update(newName);
        return View["success.cshtml"];
      };
      //ASSOCIATES GENRE WITH BOOK, VIA THE JOIN TABLE ACCESSED BY THE AddGenreToBook METHOD
      Post["/book/{id}/edit_genre"] = _ => {
        Genre newGenre = Genre.Find(Request.Form["genre-id"]);
        Book currentBook = Book.Find(Request.Form["book-id"]);
        currentBook.AddGenreToBook(newGenre);
        return View["success.cshtml"];
      };
  //AUTHORS EDITING AND VIEWING
      Get["/author/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Author selectedAuthor = Author.Find(parameters.id);
        List<Book> allBooks = Book.GetAll();
        List<Genre> allGenres = Genre.GetAll();
        model.Add("author", selectedAuthor);
        model.Add("books", allBooks);
        model.Add("genres", allGenres);
        return View["author_edit.cshtml", model];
      };

      Patch["/author/{id}/edit_name"] = parameters => {
        Author SelectedAuthor = Author.Find(parameters.id);
        string newName = Request.Form["author-name"];
        SelectedAuthor.Update(newName);
        return View["success.cshtml"];
      };

      Post["/author/{id}/edit_book"] = _ => {
        Book newBook = Book.Find(Request.Form["book-id"]);
        Author currentAuthor = Author.Find(Request.Form["author-id"]);
        currentAuthor.AddBook(newBook);
        return View["success.cshtml"];
      };

      Get["/genre/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Genre selectedGenre = Genre.Find(parameters.id);
        List<Book> allBooks = Book.GetAll();
        List<Author> allAuthors = Author.GetAll();
        model.Add("genre", selectedGenre);
        model.Add("book", allBooks);
        model.Add("author", allAuthors);
        return View["genre_edit.cshtml", model];
      };

      Patch["/genre/{id}/edit_name"] = parameters => {
        Genre SelectedGenre = Genre.Find(parameters.id);
        string newName = Request.Form["genre-name"];
        SelectedGenre.Update(newName);
        return View["success.cshtml"];
      };

      Post["/genre/{id}/edit_book"] = _ => {
        Book newBook = Book.Find(Request.Form["book-id"]);
        Genre currentGenre = Genre.Find(Request.Form["genre-id"]);
        currentGenre.AddBookToBooks_GenresJoinTable(newBook);
        return View["success.cshtml"];
      };

      Post["/genre/{id}/edit_author"] = _ => {
        Author newAuthor = Author.Find(Request.Form["author-id"]);
        Genre currentGenre = Genre.Find(Request.Form["genre-id"]);
        currentGenre.AddAuthorToGenre_AuthorJoinTable(newAuthor);
        return View["success.cshtml"];
      };

      Post["/author/{id}/edit_genre"] = _ => {
        Genre newGenre = Genre.Find(Request.Form["genre-id"]);
        Author currentAuthor = Author.Find(Request.Form["author-id"]);
        currentAuthor.AddGenre(newGenre);
        return View["success.cshtml"];
      };

    }
  }
}

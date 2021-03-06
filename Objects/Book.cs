using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using Library;

namespace Library.Objects
{
  public class Book
  {
    private int _id;
    private string _title;

    public Book(string Title, int Id = 0)
    {
      _id = Id;
      _title = Title;
    }

    public override bool Equals(System.Object otherBook)
    {
      if (!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool idEquality = this.GetId() == newBook.GetId();
        bool titleEquality = this.GetTitle() == newBook.GetTitle();
        return (idEquality && titleEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetTitle()
    {
      return _title;
    }
    public void SetId(int Id)
    {
      _id = Id;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books (title) OUTPUT INSERTED.id VALUES (@BookTitle)", conn);

      SqlParameter titleParam = new SqlParameter();
      titleParam.ParameterName = "@BookTitle";
      titleParam.Value = this.GetTitle();

      cmd.Parameters.Add(titleParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM books;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Book> GetAll()
    {
      List<Book> allBooks = new List<Book>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookName = rdr.GetString(1);
        Book newBook = new Book(bookName, bookId);
        allBooks.Add(newBook);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allBooks;
    }

    public static List<Book> GetAllAvailable()
    {
      List<Book> availableBooks = new List<Book>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books JOIN copies ON books.id = copies.book_id AND copies.status = 0;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookName = rdr.GetString(1);
        Book newBook = new Book(bookName, bookId);
        availableBooks.Add(newBook);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return availableBooks;
    }

    public static Book Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE id = @BookId", conn);
      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = id.ToString();
      cmd.Parameters.Add(bookIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundBookId = 0;
      string foundBookTitle = null;

      while(rdr.Read())
      {
        foundBookId = rdr.GetInt32(0);
        foundBookTitle = rdr.GetString(1);
      }
      Book foundBook = new Book(foundBookTitle, foundBookId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundBook;
    }

    public List<Copy> GetCopies()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM copies WHERE book_id = @BookId;", conn);
      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetId();

      cmd.Parameters.Add(bookIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Copy> copies = new List<Copy> {};
      while(rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        bool copyStatus = rdr.GetBoolean(1);
        int bookId = rdr.GetInt32(2);
        Copy newCopy = new Copy(copyStatus, bookId, copyId);
        copies.Add(newCopy);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return copies;
    }

    public void AddAuthorToJoinTable(Author newAuthor)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books_authors (book_id, author_id) OUTPUT INSERTED.author_id VALUES (@BookId, @AuthorId);", conn);

      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = newAuthor.GetId();
      cmd.Parameters.Add(authorIdParameter);

      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetId();
      cmd.Parameters.Add(bookIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        newAuthor.SetId(rdr.GetInt32(0));
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Author> GetAuthors()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT authors.* FROM books JOIN books_authors ON (books.id = books_authors.book_id) JOIN authors ON (books_authors.author_id = authors.id) WHERE books.id = @BookId;", conn);
      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(bookIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Author> authors = new List<Author>{};

      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorName = rdr.GetString(1);
        Author newAuthor = new Author(authorName, authorId);
        authors.Add(newAuthor);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return authors;
    }

    public void AddGenreToBook(Genre newGenre)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books_genres (book_id, genre_id) OUTPUT INSERTED.genre_id VALUES (@BookId, @GenreId);", conn);

      SqlParameter genreIdParameter = new SqlParameter();
      genreIdParameter.ParameterName = "@GenreId";
      genreIdParameter.Value = newGenre.GetId();
      cmd.Parameters.Add(genreIdParameter);

      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetId();
      cmd.Parameters.Add(bookIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        newGenre.SetId(rdr.GetInt32(0));
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Genre> GetGenresByBookId()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT genres.* FROM books JOIN books_genres ON (books.id = books_genres.book_id) JOIN genres ON (books_genres.genre_id = genres.id) WHERE books.id = @BookId;", conn);
      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(bookIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Genre> genres = new List<Genre>{};

      while(rdr.Read())
      {
        int genreId = rdr.GetInt32(0);
        string genreName = rdr.GetString(1);
        Genre newGenre = new Genre(genreName, genreId);
        genres.Add(newGenre);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return genres;
    }

    public void Update(string newTitle)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("UPDATE books SET title = @NewTitle OUTPUT INSERTED.title WHERE id = @BookId;", conn);

      SqlParameter newTitleParameter = new SqlParameter();
      newTitleParameter.ParameterName = "@NewTitle";
      newTitleParameter.Value = newTitle;
      cmd.Parameters.Add(newTitleParameter);

      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = this.GetId();
      cmd.Parameters.Add(bookIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        this._title = rdr.GetString(0);
      }

      if (rdr != null)
      {
        rdr.Close();
      }

      if (conn != null)
      {
        conn.Close();
      }
    }

  }
}

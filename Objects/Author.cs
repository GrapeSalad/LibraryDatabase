using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using Library;

namespace Library.Objects
{
  public class Author
  {
    private int _id;
    private string _name;

    public Author(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherAuthor)
    {
      if (!(otherAuthor is Author))
      {
        return false;
      }
      else
      {
        Author newAuthor = (Author) otherAuthor;
        bool idEquality = this.GetId() == newAuthor.GetId();
        bool nameEquality = this.GetName() == newAuthor.GetName();
        return (idEquality && nameEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public void SetId(int Id)
    {
      _id = Id;
    }
    public string GetName()
    {
      return _name;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO authors (name) OUTPUT INSERTED.id VALUES (@AuthorName)", conn);

      SqlParameter nameParam = new SqlParameter();
      nameParam.ParameterName = "@AuthorName";
      nameParam.Value = this.GetName();

      cmd.Parameters.Add(nameParam);

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
      SqlCommand cmd = new SqlCommand("DELETE FROM authors;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Author> GetAll()
    {
      List<Author> allAuthors = new List<Author>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM authors;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorName = rdr.GetString(1);
        Author newAuthor = new Author(authorName, authorId);
        allAuthors.Add(newAuthor);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allAuthors;
    }

    public static Author Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM authors WHERE id = @AuthorId", conn);
      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = id.ToString();
      cmd.Parameters.Add(authorIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundAuthorId = 0;
      string foundAuthorName = null;

      while(rdr.Read())
      {
        foundAuthorId = rdr.GetInt32(0);
        foundAuthorName = rdr.GetString(1);
      }
      Author foundAuthor = new Author(foundAuthorName, foundAuthorId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundAuthor;
    }

    public void AddBook(Book newBook)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books_authors (book_id, author_id) OUTPUT INSERTED.book_id VALUES (@BookId, @AuthorId);", conn);

      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = newBook.GetId();
      cmd.Parameters.Add(bookIdParameter);

      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = this.GetId();
      cmd.Parameters.Add(authorIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        newBook.SetId(rdr.GetInt32(0));
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

    public void AddGenre(Genre newGenre)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO genres_authors (genre_id, author_id) OUTPUT INSERTED.genre_id VALUES (@GenreId, @AuthorId);", conn);

      SqlParameter genreIdParameter = new SqlParameter();
      genreIdParameter.ParameterName = "@GenreId";
      genreIdParameter.Value = newGenre.GetId();
      cmd.Parameters.Add(genreIdParameter);

      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = this.GetId();
      cmd.Parameters.Add(authorIdParameter);

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

    public List<Book> GetBooks()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT books.* FROM authors JOIN books_authors ON (authors.id = books_authors.author_id) JOIN books ON (books_authors.book_id = books.id) WHERE authors.id = @AuthorId;", conn);
      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(authorIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Book> books = new List<Book>{};

      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookTitle = rdr.GetString(1);
        Book newBook = new Book(bookTitle, bookId);
        books.Add(newBook);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return books;
    }

    public List<Genre> GetGenresByAuthorId()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT genres.* FROM authors JOIN genres_authors ON (authors.id = genres_authors.author_id) JOIN genres ON (genres_authors.genre_id = genres.id) WHERE authors.id = @AuthorId;", conn);
      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(authorIdParameter);

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


  }
}

using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using Library;

namespace Library.Objects
{
  public class Genre
  {
    private int _id;
    private string _name;

    public Genre(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherGenre)
    {
      if (!(otherGenre is Genre))
      {
        return false;
      }
      else
      {
        Genre newGenre = (Genre) otherGenre;
        bool idEquality = this.GetId() == newGenre.GetId();
        bool nameEquality = this.GetName() == newGenre.GetName();
        return (idEquality && nameEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetId(int Id)
    {
      _id = Id;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO genres (name) OUTPUT INSERTED.id VALUES (@GenreName)", conn);

      SqlParameter nameParam = new SqlParameter();
      nameParam.ParameterName = "@GenreName";
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
      SqlCommand cmd = new SqlCommand("DELETE FROM genres;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Genre> GetAll()
    {
      List<Genre> allGenres = new List<Genre>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM genres;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int genreId = rdr.GetInt32(0);
        string genreName = rdr.GetString(1);
        Genre newGenre = new Genre(genreName, genreId);
        allGenres.Add(newGenre);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allGenres;
    }

    public static Genre Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM genres WHERE id = @GenreId", conn);
      SqlParameter genreIdParameter = new SqlParameter();
      genreIdParameter.ParameterName = "@GenreId";
      genreIdParameter.Value = id.ToString();

      cmd.Parameters.Add(genreIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundGenreId = 0;
      string foundGenreTitle = null;

      while(rdr.Read())
      {
        foundGenreId = rdr.GetInt32(0);
        foundGenreTitle = rdr.GetString(1);
      }
      Genre foundGenre = new Genre(foundGenreTitle, foundGenreId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundGenre;
    }

    public void AddBookToBooks_GenresJoinTable(Book newBook)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books_genres (book_id, genre_id) OUTPUT INSERTED.book_id VALUES (@BookId, @GenreId);", conn);

      SqlParameter bookIdParameter = new SqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = newBook.GetId();

      SqlParameter genreIdParameter = new SqlParameter();
      genreIdParameter.ParameterName = "@GenreId";
      genreIdParameter.Value = this.GetId();

      cmd.Parameters.Add(bookIdParameter);
      cmd.Parameters.Add(genreIdParameter);

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

    public void AddAuthorToGenre_AuthorJoinTable(Author newAuthor)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO genres_authors (genre_id, author_id) OUTPUT INSERTED.author_id VALUES (@GenreId, @AuthorId);", conn);

      SqlParameter authorIdParameter = new SqlParameter();
      authorIdParameter.ParameterName = "@AuthorId";
      authorIdParameter.Value = newAuthor.GetId();

      SqlParameter genreIdParameter = new SqlParameter();
      genreIdParameter.ParameterName = "@GenreId";
      genreIdParameter.Value = this.GetId();

      cmd.Parameters.Add(authorIdParameter);
      cmd.Parameters.Add(genreIdParameter);

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

    public List<Book> GetBooksByGenreId()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT books.* FROM genres JOIN books_genres ON (genres.id = books_genres.genre_id) JOIN books ON (books_genres.book_id = books.id) WHERE genres.id = @GenreId;", conn);
      SqlParameter genreIdParameter = new SqlParameter();
      genreIdParameter.ParameterName = "@GenreId";
      genreIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(genreIdParameter);

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

    public List<Author> GetAuthorsByGenreId()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT authors.* FROM genres JOIN genres_authors ON (genres.id = genres_authors.genre_id) JOIN authors ON (genres_authors.author_id = authors.id) WHERE genres.id = @GenreId;", conn);
      SqlParameter genreIdParameter = new SqlParameter();
      genreIdParameter.ParameterName = "@GenreId";
      genreIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(genreIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Author> authors = new List<Author>{};

      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorTitle = rdr.GetString(1);
        Author newAuthor = new Author(authorTitle, authorId);
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

    public void Update(string newName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("UPDATE genres SET name = @NewName OUTPUT INSERTED.name WHERE id = @GenreId;", conn);

      SqlParameter newNameParameter = new SqlParameter();
      newNameParameter.ParameterName = "@NewName";
      newNameParameter.Value = newName;
      cmd.Parameters.Add(newNameParameter);

      SqlParameter genreIdParameter = new SqlParameter();
      genreIdParameter.ParameterName = "@GenreId";
      genreIdParameter.Value = this.GetId();
      cmd.Parameters.Add(genreIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        this._name = rdr.GetString(0);
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

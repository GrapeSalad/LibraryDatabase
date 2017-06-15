using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Library;
using Library.Objects;

namespace LibraryTests
{
  [Collection("LibraryTests")]
  public class AuthorTest : IDisposable
  {
    public AuthorTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Book.DeleteAll();
      Author.DeleteAll();
      Genre.DeleteAll();
      Patron.DeleteAll();
      Copy.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
     int result = Author.GetAll().Count;
     Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfAuthorsAreTheSame()
    {
     Author firstAuthor = new Author("Christopher Tolkien");
     Author secondAuthor = new Author("Christopher Tolkien");
     Assert.Equal(firstAuthor, secondAuthor);
    }

    [Fact]
    public void Save_AuthorSavesToDatabase_True()
    {
      //Arrange
      Author testAuthor = new Author("Christopher Tolkien");
      testAuthor.Save();

      //Act
      List<Author> result = Author.GetAll();
      List<Author> testList = new List<Author>{testAuthor};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Save_AssignsIdToObjectId_True()
    {
      //Arrange
      Author testAuthor = new Author("Christopher Tolkien");
      testAuthor.Save();

      //Act
      Author savedAuthor = Author.GetAll()[0];

      int result = savedAuthor.GetId();
      int testId = testAuthor.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Find_FindsAuthorInDatabase_True()
    {
      //Arrange
      Author testAuthor = new Author("Christopher Tolkien");
      testAuthor.Save();

      //Act
      Author result = Author.Find(testAuthor.GetId());

      //Assert
      Assert.Equal(testAuthor, result);
    }

    [Fact]
    public void GetBooks_ReturnsAllAuthors_BookList()
    {

      Author testAuthor = new Author("Christopher Tolkien");
      testAuthor.Save();
      Book testBook1 = new Book("My Tentacle Lover 2, the Re-Tentacling");
      testBook1.Save();
      Book testBook2 = new Book("The Life of Guy");
      testBook2.Save();

      testAuthor.AddBook(testBook1);
      testAuthor.AddBook(testBook2);
      List<Book> savedBooks = testAuthor.GetBooks();
      List<Book> testList = new List<Book> {testBook1, testBook2};

      Assert.Equal(testList, savedBooks);
    }

    [Fact]
    public void GetGenres_ReturnsAllAuthors_GenreList()
    {

      Author testAuthor = new Author("Christopher Tolkien");
      testAuthor.Save();
      Genre testGenre1 = new Genre("Harlequin-Romance");
      testGenre1.Save();
      Genre testGenre2 = new Genre("Harlequin-Romance");
      testGenre2.Save();

      testAuthor.AddGenre(testGenre1);
      testAuthor.AddGenre(testGenre2);
      List<Genre> savedGenres = testAuthor.GetGenresByAuthorId();
      List<Genre> testList = new List<Genre> {testGenre1, testGenre2};

      Assert.Equal(testList, savedGenres);
    }

  }
}

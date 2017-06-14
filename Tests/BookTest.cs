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
  public class BookTest : IDisposable
  {
    public BookTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
     Book.DeleteAll();
     Author.DeleteAll();
     Genre.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
     int result = Book.GetAll().Count;
     Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfBooksAreTheSame()
    {
     Book firstBook = new Book("My Tentacle Lover 2, the Re-Tentacling");
     Book secondBook = new Book("My Tentacle Lover 2, the Re-Tentacling");
     Assert.Equal(firstBook, secondBook);
    }

    [Fact]
    public void Save_BookSavesToDatabase_True()
    {
      //Arrange
      Book testBook = new Book("The Life of Guy");
      testBook.Save();

      //Act
      List<Book> result = Book.GetAll();
      List<Book> testList = new List<Book>{testBook};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Save_AssignsIdToObjectId_True()
    {
      //Arrange
      Book testBook = new Book("My Tentacle Lover 2, the Re-Tentacling");
      testBook.Save();

      //Act
      Book savedBook = Book.GetAll()[0];

      int result = savedBook.GetId();
      int testId = testBook.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Find_FindsBookInDatabase_True()
    {
      //Arrange
      Book testBook = new Book("My Tentacle Lover 2, the Re-Tentacling");
      testBook.Save();

      //Act
      Book result = Book.Find(testBook.GetId());

      //Assert
      Assert.Equal(testBook, result);
    }

    [Fact]
    public void GetAuthors_ReturnsAllBooks_AuthorList()
    {

      Book testBook = new Book("My Tentacle Lover 2, the Re-Tentacling");
      testBook.Save();
      Author testAuthor1 = new Author("Christopher Tolkien");
      testAuthor1.Save();
      Author testAuthor2 = new Author("Douglas Adams");
      testAuthor2.Save();

      testBook.AddAuthorToJoinTable(testAuthor1);
      testBook.AddAuthorToJoinTable(testAuthor2);

      List<Author> savedAuthors = testBook.GetAuthors();
      List<Author> testList = new List<Author> {testAuthor1, testAuthor2};
      // CONSOLE LOGGING LIST ITEMS (GETTING IDs)
      // Console.WriteLine("savedAuthors list id = {0}, {1}", savedAuthors[0].GetId(), savedAuthors[1].GetId());
      // Console.WriteLine("testList list id = {0}, {1}", testList[0].GetId(), testList[1].GetId());

      Assert.Equal(testList, savedAuthors);
    }

    [Fact]
    public void GetGenres_ReturnsAll_BookGenresList()
    {
      Book testBook = new Book("My Tentacle Lover 2, the Re-Tentacling");
      testBook.Save();
      Genre testGenre = new Genre("Sci-Fi");
      testGenre.Save();
      Genre testGenre2 = new Genre("Harlequin-Romance");
      testGenre2.Save();
      Genre testGenre3 = new Genre("Murder-Mystery");
      testGenre3.Save();

      testBook.AddGenreToBook(testGenre);
      testBook.AddGenreToBook(testGenre2);
      testBook.AddGenreToBook(testGenre3);

      List<Genre> savedGenres = testBook. GetGenresByBookId();
      List<Genre> testList = new List<Genre> {testGenre, testGenre2, testGenre3};

      Assert.Equal(testList, savedGenres);
    }


  }
}

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
  public class GenreTest : IDisposable
  {
    public GenreTest()
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
     int result = Genre.GetAll().Count;
     Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfGenresAreTheSame()
    {
     Genre firstGenre = new Genre("Sci-Fi-Harlequin-Romance-Murder-Mystery");
     Genre secondGenre = new Genre("Sci-Fi-Harlequin-Romance-Murder-Mystery");
     Assert.Equal(firstGenre, secondGenre);
    }

    [Fact]
    public void Save_GenreSavesToDatabase_True()
    {
      //Arrange
      Genre testGenre = new Genre("Sci-Fi-Harlequin-Romance-Murder-Mystery");
      testGenre.Save();

      //Act
      List<Genre> result = Genre.GetAll();
      List<Genre> testList = new List<Genre>{testGenre};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Save_AssignsIdToObjectId_True()
    {
      //Arrange
      Genre testGenre = new Genre("Sci-Fi-Harlequin-Romance-Murder-Mystery");
      testGenre.Save();

      //Act
      Genre savedGenre = Genre.GetAll()[0];

      int result = savedGenre.GetId();
      int testId = testGenre.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Find_FindsGenreInDatabase_True()
    {
      //Arrange
      Genre testGenre = new Genre("Sci-Fi-Harlequin-Romance-Murder-Mystery");
      testGenre.Save();

      //Act
      Genre result = Genre.Find(testGenre.GetId());

      //Assert
      Assert.Equal(testGenre, result);
    }

    [Fact]
    public void GetBooks_ReturnsAll_GenresBookList()
    {
      Genre testGenre = new Genre("Sci-Fi-Harlequin-Romance-Murder-Mystery");
      testGenre.Save();
      Book testBook1 = new Book("My Tentacle Lover 2, the Re-Tentacling");
      testBook1.Save();
      Book testBook2 = new Book("The Life of Guy");
      testBook2.Save();

      testGenre.AddBookToBooks_GenresJoinTable(testBook1);
      testGenre.AddBookToBooks_GenresJoinTable(testBook2);
      List<Book> savedBooks = testGenre. GetBooksByGenreId();
      List<Book> testList = new List<Book> {testBook1, testBook2};

      Assert.Equal(testList, savedBooks);
    }

    [Fact]
    public void GetAuthors_ReturnsAll_Genres_AuthorList()
    {
      Genre testGenre = new Genre("Harlequin-Romance");
      testGenre.Save();
      Author testAuthor1 = new Author("Christopher Tolkien");
      testAuthor1.Save();
      Author testAuthor2 = new Author("Douglas Adams");
      testAuthor2.Save();

      testGenre.AddAuthorToGenre_AuthorJoinTable(testAuthor1);
      testGenre.AddAuthorToGenre_AuthorJoinTable(testAuthor2);

      List<Author> savedAuthors = testGenre.GetAuthorsByGenreId();
      List<Author> testList = new List<Author> {testAuthor1, testAuthor2};

      Assert.Equal(testList, savedAuthors);
    }

  }
}

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

  }
}

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
  public class CopyTest : IDisposable
  {
    public CopyTest()
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
     int result = Copy.GetAll().Count;
     Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfCopiesAreTheSame()
    {
     Copy firstCopy = new Copy(false, 1);
     Copy secondCopy = new Copy(false, 1);
     Assert.Equal(firstCopy, secondCopy);
    }

    [Fact]
    public void Find_FindsCopyInDatabase_True()
    {
      //Arrange
      Copy testCopy = new Copy(true, 3);
      testCopy.Save();

      //Act
      Copy result = Copy.Find(testCopy.GetId());

      //Assert
      Assert.Equal(testCopy, result);
    }
  }
}

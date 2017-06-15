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
  public class PatronTest : IDisposable
  {
    public PatronTest()
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
     int result = Patron.GetAll().Count;
     Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfPatronsAreTheSame()
    {
     Patron firstPatron = new Patron("Guy Anderson");
     Patron secondPatron = new Patron("Guy Anderson");
     Assert.Equal(firstPatron, secondPatron);
    }

    [Fact]
    public void Find_FindsPatronInDatabase_True()
    {
      //Arrange
      Patron testPatron = new Patron("Bob");
      testPatron.Save();

      //Act
      Patron result = Patron.Find(testPatron.GetId());

      //Assert
      Assert.Equal(testPatron, result);
    }

    [Fact]
    public void AddCopy_ByPatronID_True()
    {
      Patron testPatron = new Patron("Bob");
      testPatron.Save();
      Copy testCopy1 = new Copy(true, 1);
      testCopy1.Save();
      Copy testCopy2 = new Copy(true, 3);
      testCopy2.Save();

      testPatron.AddCopyByPatronID(testCopy1);
      testPatron.AddCopyByPatronID(testCopy2);

      List<Copy> savedCopies = testPatron.GetCopies();
      List<Copy> testList = new List<Copy> {testCopy1, testCopy2};
      // CONSOLE LOGGING LIST ITEMS (GETTING IDs)
      // Console.WriteLine("savedAuthors list id = {0}, {1}", savedAuthors[0].GetId(), savedAuthors[1].GetId());
      // Console.WriteLine("testList list id = {0}, {1}", testList[0].GetId(), testList[1].GetId());

      Assert.Equal(testList, savedCopies);
    }

  }
}

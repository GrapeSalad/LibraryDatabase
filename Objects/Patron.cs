using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using Library;

namespace Library.Objects
{
  public class Patron
  {
    private int _id;
    private string _name;

    public Patron(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherPatron)
    {
      if (!(otherPatron is Patron))
      {
        return false;
      }
      else
      {
        Patron newPatron = (Patron) otherPatron;
        bool idEquality = this.GetId() == newPatron.GetId();
        bool nameEquality = this.GetName() == newPatron.GetName();
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

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM patrons;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM patrons;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int patronId = rdr.GetInt32(0);
        string patronName = rdr.GetString(1);
        Patron newPatron = new Patron(patronName, patronId);
        allPatrons.Add(newPatron);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allPatrons;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO patrons (name) OUTPUT INSERTED.id VALUES (@PatronName)", conn);

      SqlParameter nameParam = new SqlParameter();
      nameParam.ParameterName = "@PatronName";
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

    public static Patron Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE id = @PatronId", conn);
      SqlParameter patronIdParameter = new SqlParameter();
      patronIdParameter.ParameterName = "@PatronId";
      patronIdParameter.Value = id.ToString();
      cmd.Parameters.Add(patronIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundPatronId = 0;
      string foundPatronName = null;

      while(rdr.Read())
      {
        foundPatronId = rdr.GetInt32(0);
        foundPatronName = rdr.GetString(1);
      }
      Patron foundPatron = new Patron(foundPatronName, foundPatronId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundPatron;
    }

    public void AddCopyByPatronID(Copy newCopy)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO checkouts (copy_id, patron_id) OUTPUT INSERTED.copy_id VALUES (@CopyId, @PatronId);", conn);

      SqlParameter copyIdParameter = new SqlParameter();
      copyIdParameter.ParameterName = "@CopyId";
      copyIdParameter.Value = newCopy.GetId();
      cmd.Parameters.Add(copyIdParameter);

      SqlParameter patronIdParameter = new SqlParameter();
      patronIdParameter.ParameterName = "@PatronId";
      patronIdParameter.Value = this.GetId();
      cmd.Parameters.Add(patronIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();
      while (rdr.Read())
      {
        newCopy.SetId(rdr.GetInt32(0));
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

    public List<Copy> GetCopies()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT copies.* FROM patrons JOIN checkouts ON (patrons.id = checkouts.patron_id) JOIN copies ON (checkouts.copy_id = copies.id) WHERE patrons.id = @PatronId;", conn);
      SqlParameter patronIdParameter = new SqlParameter();
      patronIdParameter.ParameterName = "@PatronId";
      patronIdParameter.Value = this.GetId().ToString();

      cmd.Parameters.Add(patronIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Copy> copies = new List<Copy>{};

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

  }
}

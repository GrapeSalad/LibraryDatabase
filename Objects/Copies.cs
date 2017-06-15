using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using Library;

namespace Library.Objects
{
  public class Copy
  {
    private int _id;
    private bool _status;
    private int _bookId;

    public Copy(bool Status, int BookId, int Id = 0)
    {
      _id = Id;
      _status = Status;
      _bookId = BookId;
    }

    public override bool Equals(System.Object otherCopy)
    {
      if (!(otherCopy is Copy))
      {
        return false;
      }
      else
      {
        Copy newCopy = (Copy) otherCopy;
        bool idEquality = this.GetId() == newCopy.GetId();
        bool statusEquality = this.GetStatus() == newCopy.GetStatus();
        bool bookIdEquality = this.GetBookId() == newCopy.GetBookId();
        return (idEquality && statusEquality && bookIdEquality);
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
    public bool GetStatus()
    {
      return _status;
    }
    public void SetStatus(bool Status)
    {
      _status = Status;
    }
    public int GetBookId()
    {
      return _bookId;
    }
    public void SetBookId(int BookId)
    {
      _bookId = BookId;
    }

    public void Save()
    {

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO copies (status, book_id) OUTPUT INSERTED.id VALUES (@CopyStatus, @BookId)", conn);

      SqlParameter statusParam = new SqlParameter();
      statusParam.ParameterName = "@CopyStatus";
      statusParam.Value = this.GetStatus();

      SqlParameter bookIdParam = new SqlParameter();
      bookIdParam.ParameterName = "@BookId";
      bookIdParam.Value = this.GetBookId();

      cmd.Parameters.Add(bookIdParam);
      cmd.Parameters.Add(statusParam);

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
      SqlCommand cmd = new SqlCommand("DELETE FROM copies;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<Copy> GetAll()
    {
      List<Copy> allCopies = new List<Copy>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM copies;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int copyId = rdr.GetInt32(0);
        bool copyStatus = rdr.GetBoolean(1);
        Copy newCopy = new Copy(copyStatus, copyId);
        allCopies.Add(newCopy);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCopies;
    }

    public static Copy Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM copies WHERE id = @CopyId", conn);
      SqlParameter copyIdParameter = new SqlParameter();
      copyIdParameter.ParameterName = "@CopyId";
      copyIdParameter.Value = id.ToString();
      cmd.Parameters.Add(copyIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCopyId = 0;
      int foundCopyBookId = 0;
      bool foundCopyStatus = false;

      while(rdr.Read())
      {
        foundCopyId = rdr.GetInt32(0);
        foundCopyStatus = rdr.GetBoolean(1);
        foundCopyBookId = rdr.GetInt32(2);
      }

      Copy foundCopy = new Copy(foundCopyStatus, foundCopyBookId, foundCopyId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCopy;
    }
  }
}

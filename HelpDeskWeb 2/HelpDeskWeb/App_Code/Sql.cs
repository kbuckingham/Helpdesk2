using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for Sql
/// </summary>
public static class Sql
{
    #region Properties

    public static string CCString()
    {
        return ConfigurationManager.ConnectionStrings["CCWEBConnectionString"].ConnectionString;
    }

    //public static string HDString()
    //{
    //    return ConfigurationManager.ConnectionStrings["HelpdeskConnectionString"].ConnectionString;
    //}

    public static string SHString()
    {
        return ConfigurationManager.ConnectionStrings["STUDENTHELPConnectionString"].ConnectionString;
    }

    public static string TIString()
    {
        return ConfigurationManager.ConnectionStrings["TRACKITConnectionString"].ConnectionString;
    }

    #endregion

    #region Attributes

 
    #endregion

    #region Constructors

    static Sql()
    {

    }

    #endregion

    #region Methods
        
    public static DataTable CCSelect(string command)
    {
        return _innerFunc("CCWEBConnectionString", command);
    }
      
    public static DataTable SHSelect(string command)
    {
        return _innerFunc("STUDENTHELPConnectionString", command);
    }

    public static DataTable TISelect(string command)
    {
        return _innerFunc("TRACKITConnectionString", command);
    }

    public static DataTable JenzSelect(string command)
    {
        return _innerFunc("SQLConnectionString", command);
    }

    public static DataTable _innerFunc(string connectionstring, string command)
    {
        #region Building the connection string

        string ConnectionString = ConfigurationManager.ConnectionStrings[connectionstring].ConnectionString;

        #endregion
            
        #region Try to establish a connection to the database

        SqlConnection SQLConnection = new SqlConnection();

        try
        {
            SQLConnection.ConnectionString = ConnectionString;
            SQLConnection.Open();

            // You can get the server version 
            // SQLConnection.ServerVersion
        }
        catch (Exception Ex)
        {
            // Try to close the connection
            if (SQLConnection != null)
                SQLConnection.Dispose();

            // Create a (useful) error message
            string ErrorMessage = "A error occurred while trying to connect to the server.";
            ErrorMessage += Environment.NewLine;
            ErrorMessage += Environment.NewLine;
            ErrorMessage += Ex.Message;

        }

        #endregion

        #region Execute a SQL query

        // Create a SqlDataAdapter to get the results as DataTable
        SqlDataAdapter SQLDataAdapter = new SqlDataAdapter(command, SQLConnection);

        // Create a new DataTable
        DataTable dtResult = new DataTable();

        // Fill the DataTable with the result of the SQL statement
        SQLDataAdapter.Fill(dtResult);


        #endregion

        #region Close the database link
        // We don't need the data adapter any more
        SQLDataAdapter.Dispose();
        SQLConnection.Close();
        SQLConnection.Dispose();

        #endregion

        return dtResult;

    }


    #endregion

}
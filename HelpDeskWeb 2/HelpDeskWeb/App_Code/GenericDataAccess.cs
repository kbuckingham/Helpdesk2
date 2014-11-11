using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.Sql;
using System.Data.SqlClient;

/// <summary>
/// Class contains generic data access functionality to be accessed from
/// the business tier
/// </summary>

public static class GenericDataAccess
{
    // static constructor

    static GenericDataAccess()
    {
    //
    // TODO: Add constructor logic here
    //
    }

        //THIS IS TO PULL DATASETS INSTEAD OF DATATABLES!!!!
        public static DataSet doDSQuery(string SQLQuery)
        {
            using (SqlConnection sqlConn = new SqlConnection(CCConfiguration.DbConnectionString))
            {
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close();

                sqlConn.Open();

                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataSet dataset = new DataSet();
                    adapter.SelectCommand = new SqlCommand(SQLQuery, sqlConn);
                    adapter.Fill(dataset);

                    sqlConn.Close();
                    return dataset;
                }
                catch (SqlException se)
                {
                    sqlConn.Dispose();
                    throw new Exception(se.Message);
                }
                catch (Exception ex)
                {
                    sqlConn.Dispose();
                    throw new Exception("There was a  general exception: " + ex.Message);
                }
            }
        }

    // executes a command and returns the results as a DataTable object
    public static DataTable ExecuteSelectCommand(DbCommand command)
    {
            // The DataTable to be returned (databales are used to store the information localy)
            DataTable table;
            // Execute the command, making sure the connection gets closed in the
            // end
        try
        {
            // Open the data connection
            command.Connection.Open();

            // Execute the command and save the results in a DataTable
            DbDataReader reader = command.ExecuteReader();

            table = new DataTable();
            table.Load(reader);
            // Close the reader
            reader.Close();
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            // Close the connection
            command.Connection.Close();
        }
        return table;
    }

    // creates and prepares a new DbCommand object on a new connection
    public static DbCommand CreateCommand()
    {
        // Obtain the database provider name
        string dataProviderName = CCConfiguration.DbProviderName;

        // Obtain the database connection string
        string connectionString = CCConfiguration.DbConnectionString;

        // Create a new data provider factory
        DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);

        // Obtain a database-specific connection object
        DbConnection conn = factory.CreateConnection();

        // Set the connection string
        conn.ConnectionString = connectionString;

        // Create a database-specific command object
        DbCommand comm = conn.CreateCommand();

        // Set the command type to stored procedure
        comm.CommandType = CommandType.StoredProcedure;

        // Return the initialized command object
        return comm;
    }

    // creates and prepares a new DbCommand object on a new connection
    public static DbCommand CreateCommand2()
    {
        // Obtain the database provider name
        string dataProviderName = KeyDBConfig.DbProviderName;

        // Obtain the database connection string
        string connectionString = KeyDBConfig.DbConnectionString;

        // Create a new data provider factory
        DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);

        // Obtain a database-specific connection object
        DbConnection conn = factory.CreateConnection();

        // Set the connection string
        conn.ConnectionString = connectionString;

        // Create a database-specific command object
        DbCommand comm = conn.CreateCommand();

        // Set the command type to stored procedure
        comm.CommandType = CommandType.StoredProcedure;

        // Return the initialized command object
        return comm;
    }

    public static DbCommand CreateCommand3()
    {
        // Obtain the database provider name
        string dataProviderName = ConfigurationManager.ConnectionStrings
        ["CCWEBConnectionString"].ProviderName;

        // Obtain the database connection string
        string connectionString = ConfigurationManager.ConnectionStrings
        ["CCWEBConnectionString"].ConnectionString;

        // Create a new data provider factory
        DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);

        // Obtain a database-specific connection object
        DbConnection conn = factory.CreateConnection();

        // Set the connection string
        conn.ConnectionString = connectionString;

        // Create a database-specific command object
        DbCommand comm = conn.CreateCommand();

        // Set the command type to stored procedure
        comm.CommandType = CommandType.StoredProcedure;

        // Return the initialized command object
        return comm;
    }

    public static int ExecuteNonQuery(DbCommand command)
    {
        // The number of affected rows 
        int affectedRows = -1;
        // Execute the command making sure the connection gets closed in the end
        try
        {
            // Open the connection of the command
            command.Connection.Open();
            // Execute the command and get the number of affected rows
            affectedRows = command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            // Log eventual errors and rethrow them
            throw;
        }
        finally
        {
            // Close the connection
            command.Connection.Close();
        }
        // return the number of affected rows

        return affectedRows;
    }

    // execute a select command and return a single result as a string
    public static string ExecuteScalar(DbCommand command)
    {
        // The value to be returned 
        string value = "";
        // Execute the command making sure the connection gets closed in the end
        try
        {

            // Open the connection of the command
            command.Connection.Open();
            // Execute the command and get the number of affected rows
            value = command.ExecuteScalar().ToString();
        }
        catch (Exception ex)
        {
            // Log eventual errors and rethrow them
            throw;
        }
        finally
        {
            // Close the connection
            command.Connection.Close();
        }
        // return the result
        return value;
    }





}
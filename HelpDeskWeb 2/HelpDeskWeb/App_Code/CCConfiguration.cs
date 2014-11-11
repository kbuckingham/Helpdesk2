using System.Configuration;

/// <summary>
/// Repository for CC configuration settings
/// These are pulled from the web config file and stored for later use here
/// </summary>
 
public static class CCConfiguration
{
    // Caches the connection string
    private static string dbConnectionString;
    // Caches the data provider name
    private static string dbProviderName;
    // Store the name of my site
    private readonly static string siteName;

static CCConfiguration()
{
  


    siteName = ConfigurationManager.AppSettings["SiteName"];

    dbConnectionString = ConfigurationManager.ConnectionStrings
    ["CCWEBConnectionString"].ConnectionString;

    dbProviderName = ConfigurationManager.ConnectionStrings
    ["CCWEBConnectionString"].ProviderName;
}

// Returns the connection string for the CC database
public static string DbConnectionString
{
    get
    {
    return dbConnectionString;
    }
}

// Returns the data provider name
public static string DbProviderName
{
    get
    {
    return dbProviderName;
    }
}

}




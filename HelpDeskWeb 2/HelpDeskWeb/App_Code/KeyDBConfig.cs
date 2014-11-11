using System.Configuration;

/// <summary>
/// Repository for CC configuration settings
/// These are pulled from the web config file and stored for later use here
/// </summary>

public static class KeyDBConfig
{
    // Caches the connection string
    private static string dbConnectionString;
    // Caches the data provider name
    private static string dbProviderName;
    // Store the name of my site
    private readonly static string siteName;

    static KeyDBConfig()
    {



        siteName = ConfigurationManager.AppSettings["SiteName"];

        dbConnectionString = ConfigurationManager.ConnectionStrings
        ["STUDENTHELPConnectionString"].ConnectionString;

        dbProviderName = ConfigurationManager.ConnectionStrings
        ["STUDENTHELPConnectionString"].ProviderName;
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

    // Returns the address of the mail server
    public static string MailServer
    {
        get
        {
            return ConfigurationManager.AppSettings["MailServer"];
        }
    }

    // Returns the email username
    public static string MailUsername
    {
        get
        {
            return ConfigurationManager.AppSettings["MailUsername"];
        }
    }

    // Returns the email password
    public static string MailPassword
    {
        get
        {
            return ConfigurationManager.AppSettings["MailPassword"];
        }
    }

    // Returns the email password
    public static string MailFrom
    {
        get
        {
            return ConfigurationManager.AppSettings["MailFrom"];
        }
    }

    // Send error log emails?
    public static bool EnableErrorLogEmail
    {
        get
        {
            return bool.Parse(ConfigurationManager.AppSettings["EnableErrorLogEmail"]);
        }
    }

    // Returns the email address where to send error reports
    public static string ErrorLogEmail
    {
        get
        {
            return ConfigurationManager.AppSettings["ErrorLogEmail"];
        }
    }


    // Returns the length of product descriptions in products lists
    public static string SiteName
    {
        get
        {
            return siteName;
        }
    }

    // Returns the email address for customers to contact the site
    public static string CustomerServiceEmail
    {
        get
        {
            return
            ConfigurationManager.AppSettings["CustomerServiceEmail"];
        }
    }
    // The "from" address for auto-generated order processor emails
    public static string OrderProcessorEmail
    {
        get
        {
            return
            ConfigurationManager.AppSettings["OrderProcessorEmail"];
        }
    }

    // The email address to use to contact the supplier
    public static string SupplierEmail
    {
        get
        {
            return ConfigurationManager.AppSettings["SupplierEmail"];
        }
    }

}




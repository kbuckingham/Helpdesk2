using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.DirectoryServices;
using System.Configuration;
using System.Security.Principal;

/// <summary>
/// Summary description for AD
/// </summary>
public static class AD
{
    #region Properties

    //Returns "pwinkle"
    public static string UserName
    {
        get { return WindowsIdentity.GetCurrent().Name.Remove(0, 5); }
    }

    //Returns "COFO\pwinkle"
    public static string DUserName
    {
        get { return WindowsIdentity.GetCurrent().Name; }
    }

    //Returns "Paul Winkle"
    public static string FullName
    {
        get { return ToFullName(WindowsIdentity.GetCurrent().Name.Remove(0, 5)); }
    }

    //Returns Current users phone Number
    public static string Phone
    {
        get { return GetPhone(WindowsIdentity.GetCurrent().Name.Remove(0, 5)); }
    }

    //Returns Current Help Desk Manager
    public static string HDM
    { get { return @"COFO\pwinkle"; } }

    public static string HDMshort
    { get { return @"pwinkle"; } }

    public static bool IsHDM
    { get { return DUserName == HDM; } }

    //Returns Current Help Desk Developer
    public static string HDD
    { get { return @"COFO\jjohnson"; } }

    public static string HDDshort
    { get { return @"jjohnson"; } }

    public static bool IsHDD
    { get { return DUserName == HDD; } }

    //Returns Current Network Manager
    public static string NM
    { get { return @"COFO\dthompson"; } }

    public static string NMshort
    { get { return @"dthompson"; } }

    public static bool IsNM
    { get { return DUserName == NM; } }

    //Returns Helpdesk
    public static string Helpdesk
    { get { return @"COFO\helpdesk"; } }

    public static string Helpdeskshort
    { get { return @"helpdesk"; } }

    public static bool IsHelpdesk
    { get { return DUserName == Helpdesk; } }

    //RETURNS CCStudents Group
    public static string ccstudents
    { get { return @"COFO\CC_Students"; } }

    public static bool IsinCCStudents
    { get { return InGroup(ccstudents); } }
    
    //RETURNS CCSTUDENT2 Group
    public static string ccstudents2
    { get { return @"COFO\ccstudents2"; } }

    public static bool IsinCCStudents2
    { get { return InGroup(ccstudents2); } }

    //Returns Domain Admins Group
    public static string admins
    { get { return @"COFO\Domain Admins"; } }

    public static bool IsinDomainAdmins
    { get { return InGroup(admins); } }

    #endregion

    #region Attributes

    #endregion

    #region Constructors

    static AD()
    {
    }

    #endregion

    #region Member Methods

    #endregion

    #region Static Methods


    public static string StudentFullName(string username)
    {
        string Name = "";
        DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://student.cofo.edu");
        directoryEntry.Username = ConfigurationManager.AppSettings["User"];
        directoryEntry.Password = Cryptography.Decrypt(ConfigurationManager.AppSettings["Pass"], ConfigurationManager.AppSettings["cKey"]);
        directoryEntry.AuthenticationType = AuthenticationTypes.Secure;

        try
        {
            DirectorySearcher search = new DirectorySearcher(directoryEntry);
            search.Filter = "(SAMAccountName=" + username + ")";
            search.PropertiesToLoad.Add("name");

            SearchResult result = search.FindOne();

            if (result != null)
            {
                Name += result.Properties["name"][0].ToString();

            }
            else
            {
                Name += "Unknown User";
            }
        }
        catch (Exception ex)
        {
            Name = "Error Getting Name";
        }

        return Name;
    }

    //[ToFullName]
    //       Gets: User Name
    //    Returns: Full Name
    //Description: Function gets the full name of a given user name.
    public static string ToFullName(string username)
    {
        string Name = "";
        DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://COFO.edu");
        directoryEntry.Username = ConfigurationManager.AppSettings["User"];
        directoryEntry.Password = Cryptography.Decrypt(ConfigurationManager.AppSettings["Pass"], ConfigurationManager.AppSettings["cKey"]);
        directoryEntry.AuthenticationType = AuthenticationTypes.Secure;

        try
        {
            DirectorySearcher search = new DirectorySearcher(directoryEntry);
            search.Filter = "(SAMAccountName=" + username + ")";
            search.PropertiesToLoad.Add("name");

            SearchResult result = search.FindOne();

            if (result != null)
                Name += result.Properties["name"][0].ToString();
            else
                Name += "Unknown User";
        }
        catch (Exception ex)
        {
            Name = "Error Getting Name : " + ex;
        }

        return Name;
    }

    public static string ToLastFirstName(string username)
    {
        string Name = "";
        DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://COFO.edu");
        directoryEntry.Username = ConfigurationManager.AppSettings["User"];
        directoryEntry.Password = Cryptography.Decrypt(ConfigurationManager.AppSettings["Pass"], ConfigurationManager.AppSettings["cKey"]);
        directoryEntry.AuthenticationType = AuthenticationTypes.Secure;

        try
        {
            DirectorySearcher search = new DirectorySearcher(directoryEntry);
            search.Filter = "(SAMAccountName=" + username + ")";
            search.PropertiesToLoad.Add("name");
            search.PropertiesToLoad.Add("sn");
            search.PropertiesToLoad.Add("givenName");

            SearchResult result = search.FindOne();

            if (result != null)
                Name = result.Properties["sn"][0].ToString() + ", " + result.Properties["givenName"][0].ToString();
            else
                Name = "Unknown User";
        }
        catch (Exception ex)
        {
            Name = "Error Getting Name : " + ex;
        }

        return Name;
    }

    //[ToUserName]
    //       Gets: Full Name
    //    Returns: User Name
    //Description: Function gets the user name of a given full name.
    public static string ToUserName(string fullname)
    {
        string Name = "";
        DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://COFO.edu");
        directoryEntry.Username = ConfigurationManager.AppSettings["User"];
        directoryEntry.Password = Cryptography.Decrypt(ConfigurationManager.AppSettings["Pass"], ConfigurationManager.AppSettings["cKey"]);
        directoryEntry.AuthenticationType = AuthenticationTypes.Secure;

        try
        {
            DirectorySearcher search = new DirectorySearcher(directoryEntry);
            search.Filter = "(name=" + fullname + ")";
            search.PropertiesToLoad.Add("SAMAccountName");

            SearchResult result = search.FindOne();

            if (result != null)
                Name += result.Properties["SAMAccountName"][0].ToString();
            else
                Name += "Unknown User";
        }
        catch (Exception ex)
        {
            Name = "Error Getting Name: " + ex;
        }

        return Name;
    }
    
    //THIS METHOD NEEDS TO BE ADDRESSED BETTER
    //Given a username is user a Domain Admin?
    public static bool DomainAdmin(string username)
    {
        ArrayList Admins = new ArrayList();

        Admins.Add("pwinkle");
        Admins.Add("dthompson");
        Admins.Add("pettit");
        Admins.Add("kpettit");
        Admins.Add("henderson");
        Admins.Add("fyoungblood");

        if (Admins.Contains(username))
            return true;
        else
            return false;
    }

    //[InGroup]
    //       Gets: A name of a group
    //    Returns: True or False
    //Description: This function is used to see if the current user is a member of
    // the group given.
    public static bool InGroup(string group)
    {
        ArrayList al = new ArrayList();
        al = Groups();
        foreach (string s in al)
            if (s == group)
                return true;
        return false;
    }

    //[Groups]
    //       Gets: None
    //    Returns: An array of groups that the current logged in user is a member of.
    //Description: This function is used to retrieve all the roles in AD that the current
    // user is a member of. 
    public static ArrayList Groups()
    {
        ArrayList groups = new ArrayList();
        foreach (System.Security.Principal.IdentityReference group in System.Web.HttpContext.Current.Request.LogonUserIdentity.Groups)
        {
            groups.Add(group.Translate(typeof(System.Security.Principal.NTAccount)).ToString());
        }
        return groups;
    }

    //[GetPhone]
    //       Gets: Username
    //    Returns: Phone Number
    //Description: Function is used to get the Phone extension listed in AD for
    //  a given user.
    public static string GetPhone(string username)
    {
        string Name = "";
        DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://COFO.edu");
        directoryEntry.Username = ConfigurationManager.AppSettings["User"];
        directoryEntry.Password = Cryptography.Decrypt(ConfigurationManager.AppSettings["Pass"], ConfigurationManager.AppSettings["cKey"]);
        directoryEntry.AuthenticationType = AuthenticationTypes.Secure;

        try
        {
            DirectorySearcher search = new DirectorySearcher(directoryEntry);
            search.Filter = "(SAMAccountName=" + username + ")";
            search.PropertiesToLoad.Add("telephoneNumber");

            SearchResult result = search.FindOne();

            if (result != null)
                Name += result.Properties["telephoneNumber"][0].ToString();
            else
                Name += " - ";
        }
        catch (Exception ex)
        {
            Name = " - "; //+ ex;
        }

        return Name;
    }

    //[GetEmail]
    //       Gets: Username
    //    Returns: Email address
    //Description: Function is used to get the E-mail address listed in AD for
    //  a given user.
    public static string GetEmail(string username)
    {
        DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://COFO.edu");
        directoryEntry.Username = ConfigurationManager.AppSettings["User"];
        directoryEntry.Password = Cryptography.Decrypt(ConfigurationManager.AppSettings["Pass"], ConfigurationManager.AppSettings["cKey"]);
        directoryEntry.AuthenticationType = AuthenticationTypes.Secure;

        try
        {
            DirectorySearcher search = new DirectorySearcher(directoryEntry);
            search.Filter = "(SAMAccountName=" + username + ")";
            search.PropertiesToLoad.Add("mail");

            SearchResult result = search.FindOne();
            return result.Properties["mail"][0].ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return "Invalid Username";
        }
    }

    //[AllEmployees]  (((((DO NOT USE DEPRECATED)) use AllEmployeesTB()
    //       Gets: none
    //    Returns: An array of fullnames of the current Faculty and staff members in Active Directory
    //Description: Function is used to retrieve a list of all current employees of the College
    //  
    [Obsolete("Not used anymore use AllEmployeesTB()", true)]
    public static ArrayList AllEmployees()
    {
        ArrayList names = new ArrayList();

        //Connect to Active Directory with the web.config credentials
        DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://DC=cofo,DC=edu");
        directoryEntry.Username = ConfigurationManager.AppSettings["User"];
        directoryEntry.Password = Cryptography.Decrypt(ConfigurationManager.AppSettings["Pass"], ConfigurationManager.AppSettings["cKey"]);
        directoryEntry.AuthenticationType = AuthenticationTypes.Secure;


        try
        {
            //Search for all users in Staff or Faculty that are active (512)
            DirectorySearcher search2 = new DirectorySearcher(directoryEntry);
            search2.Filter = "(&(|(memberOf=CN=Faculty,OU=DC Faculty,OU=Groups,DC=cofo,DC=edu)(memberOf=CN=Staff,OU=DC Staff,OU=Groups,DC=cofo,DC=edu)(memberOf=CN=Sodexo Staff,OU=Email Groups,OU=Groups,DC=cofo,DC=edu)(memberOf=CN=SOFO All Faculty,OU=Email Groups,OU=Groups,DC=cofo,DC=edu)(memberOf=CN=Computer Center Staff,OU=Administration,DC=cofo,DC=edu))(|(userAccountControl=512)(userAccountControl=66048)))";
            search2.PageSize = 2000;
            search2.PropertiesToLoad.Add("name");
            //Add each name to the Array
            foreach (SearchResult result in search2.FindAll())
            {
                names.Add(result.Properties["name"][0].ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        //New array to house the non-duplicate records
        ArrayList noDups = new ArrayList();

        //For each item in the original array, place it in the new array if is not in it already
        foreach (string strItem in names)
            if (!noDups.Contains(strItem.Trim()))
                noDups.Add(strItem.Trim());

        //Sort the new array
        noDups.Sort();

        return noDups;
    }

    //[AllEmployees]
    //       Gets: none
    //    Returns: An array of fullnames of the current Faculty and staff members in Active Directory
    //Description: Function is used to retrieve a list of all current employees of the College
    //  
    public static DataTable AllEmployeesTB()
    {
        //Create the DataTable
        DataTable staff = new DataTable();

        //Connect to Active Directory with the web.config credentials
        DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://DC=cofo,DC=edu");
        directoryEntry.Username = ConfigurationManager.AppSettings["User"];
        directoryEntry.Password = Cryptography.Decrypt(ConfigurationManager.AppSettings["Pass"], ConfigurationManager.AppSettings["cKey"]);
        directoryEntry.AuthenticationType = AuthenticationTypes.Secure;

        try
        {
            //Search for all users in Staff or Faculty that are active (512)
            DirectorySearcher search = new DirectorySearcher(directoryEntry);
            search.Filter = "(&(|(memberOf=CN=Faculty,OU=DC Faculty,OU=Groups,DC=cofo,DC=edu)(memberOf=CN=Staff,OU=DC Staff,OU=Groups,DC=cofo,DC=edu)(memberOf=CN=Sodexo Staff,OU=Email Groups,OU=Groups,DC=cofo,DC=edu)(memberOf=CN=SOFO All Faculty,OU=Email Groups,OU=Groups,DC=cofo,DC=edu)(memberOf=CN=Counseling Interns,OU=Misc,OU=Groups,DC=cofo,DC=edu)(memberOf=CN=Computer Center Staff,OU=Administration,DC=cofo,DC=edu))(|(userAccountControl=512)(userAccountControl=66048)))";
            search.PageSize = 1000;
            search.CacheResults = true;
            search.PropertiesToLoad.Add("givenName");
            search.PropertiesToLoad.Add("sn");
            search.PropertiesToLoad.Add("name");
            search.PropertiesToLoad.Add("SAMAccountName");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("telephoneNumber");
            search.PropertiesToLoad.Add("description");

            SearchResultCollection allResults = search.FindAll();

            ////add columns for each property in results
            foreach (string colName in allResults.PropertiesLoaded)
            {
                staff.Columns.Add(colName, colName.GetType());
            }
            //Add custom Columns
            staff.Columns.Add("LastFirstName", typeof(string));

            //loop to add records to DataTable
            foreach (SearchResult result in allResults)
            {
                int tmp = result.Properties.Count;
                if (result.Properties.Contains("givenName"))
                {
                    DataRow row = staff.NewRow();
                    foreach (string columnName in search.PropertiesToLoad)
                    {
                        if (result.Properties.Contains(columnName))
                            row[columnName] = result.Properties[columnName][0].ToString();
                        else
                            row[columnName] = "";
                    }

                    row["LastFirstName"] = row["sn"].ToString() + ", " + row["givenName"].ToString();
                    staff.Rows.Add(row);
                }
            }

            staff.DefaultView.Sort = "sn ASC";
            staff = staff.DefaultView.ToTable();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return staff;
    }

    //[AllCCStudents]
    //       Gets: none
    //    Returns: a Data Table containing FirstName, LastName, Username, Email of all members in the CC_students group in Active Directory
    //Description: Function is used to retrieve a list of all current CC Students of the College
    //  
    public static DataTable AllCCStudents()
    {
        //Create the DataTable
        DataTable students = new DataTable();

        //Connect to Active Directory with the web.config credentials
        DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://DC=cofo,DC=edu");
        directoryEntry.Username = ConfigurationManager.AppSettings["User"];
        directoryEntry.Password = Cryptography.Decrypt(ConfigurationManager.AppSettings["Pass"], ConfigurationManager.AppSettings["cKey"]);
        directoryEntry.AuthenticationType = AuthenticationTypes.Secure;

        try
        {
            //Search for all users in CC_Students
            DirectorySearcher search = new DirectorySearcher(directoryEntry);
            search.Filter = "(|(memberOf=CN=CC_Students,OU=CC Students,OU=Administration,DC=cofo,DC=edu))";
            search.PageSize = 1000;
            search.CacheResults = true;
            search.PropertiesToLoad.Add("givenName");
            search.PropertiesToLoad.Add("sn");
            search.PropertiesToLoad.Add("SAMAccountName");
            search.PropertiesToLoad.Add("mail");

            SearchResultCollection allResults = search.FindAll();

            ////add columns for each property in results
            foreach (string colName in allResults.PropertiesLoaded)
            {
                students.Columns.Add(colName, colName.GetType());
            }


            //loop to add records to DataTable
            foreach (SearchResult result in allResults)
            {
                int tmp = result.Properties.Count;
                if (result.Properties.Contains("givenName"))
                {
                    DataRow row = students.NewRow();
                    foreach (string columnName in search.PropertiesToLoad)
                    {
                        if (result.Properties.Contains(columnName))
                            row[columnName] = result.Properties[columnName][0].ToString();
                        else
                            row[columnName] = "";
                    }
                    students.Rows.Add(row);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        //RETURN TABLE
        return students;
    }


    //[GetPath]
    public static string GetPath(string computername) 
    {
        string path = "";

        DirectoryEntry entry = new DirectoryEntry("LDAP://DC=cofo,DC=edu");
        entry.Username = ConfigurationManager.AppSettings["User"];
        entry.Password = Cryptography.Decrypt(ConfigurationManager.AppSettings["Pass"], ConfigurationManager.AppSettings["cKey"]);
        entry.AuthenticationType = AuthenticationTypes.Secure;

        try
        {
            DirectorySearcher mySearcher = new DirectorySearcher(entry);
            mySearcher.Filter = ("(&(objectClass=computer)(name=" + computername + "))");
            mySearcher.SizeLimit = int.MaxValue;
            mySearcher.PageSize = int.MaxValue;

            foreach (SearchResult resEnt in mySearcher.FindAll())
            {
                path = resEnt.GetDirectoryEntry().Path;
            }

            mySearcher.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        entry.Dispose();

        return path;
    }

    public static string GetshortPath(string computername)
    {
        return GetPath(computername).Replace("LDAP://CN=", "").Replace(",DC=cofo,DC=edu", "");
    }
    
    #endregion
}
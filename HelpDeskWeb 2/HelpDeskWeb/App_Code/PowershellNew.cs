using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Web;

/// <summary>
/// Summary description for Powershell
/// </summary>
namespace PowershellNew
{
    public class Powershell
    {
        #region Constructor

        public Powershell()
        {
        }

        #endregion

        #region Static Methods

        public static void RunScript(String script)
        {
            PowerShell shell = PowerShell.Create();
            script += " -Credential $Cred";
            shell.Commands.AddScript(script);
            shell.Invoke();
        }

        public static string RunScriptWithOutput(string script)
        {
            PowerShell shell = PowerShell.Create();
            shell.Commands.AddScript(script);
            string output = "";

            foreach (PSObject result in shell.Invoke())
            {
                output += result.ToString() + "<br/>";
            }

            return output;
        }

        public static void RunScriptNoCred(String script)
        {
            PowerShell shell = PowerShell.Create();
            shell.Commands.AddScript(script);
            shell.Invoke();
        }

        public static string GetCredentials()
        {
            string CredentialString;
            CredentialString = "$Username = \"cofo\\helpdeskweb\"\n";
            CredentialString = CredentialString + "$Password = \"2010Bolger\"\n";
            CredentialString = CredentialString + "$SecurePass  = ConvertTo-SecureString -AsPlainText $Password -Force\n";
            CredentialString = CredentialString + "$Cred = New-Object System.Management.Automation.PSCredential ($Username,$SecurePass)\n";
            return CredentialString;
        }

        public static string GetCredentials(string username, string password)
        {
            string CredentialString;
            CredentialString = "$Username = \"cofo\\" + username + "\"\n";
            CredentialString = CredentialString + "$Password = \"" + password + "\"\n";
            CredentialString = CredentialString + "$SecurePass  = ConvertTo-SecureString -AsPlainText $Password -Force\n";
            CredentialString = CredentialString + "$Cred = New-Object System.Management.Automation.PSCredential ($Username,$SecurePass)\n";
            return CredentialString;
        }

        #endregion

        #region Powershell Commands

        public static void RestartComputer(string computername)
        {
            string CredString = GetCredentials();
            RunScript(CredString + " restart-computer -computername " + computername + " -force");
        }

        public static void RestartComputer(string computername, string username, string password)
        {
            string CredString = GetCredentials(username, password);
            RunScript(CredString + " restart-computer -computername " + computername + " -force");
        }

        public static void ShutdownComputer(String computername)
        {
            string credstring = GetCredentials();
            RunScript(credstring + " stop-computer -computername " + computername + " -force");
        }

        public static void RestartComputerNoCred(string computername)
        {
            RunScriptNoCred("restart-computer -computername " + computername);
        }

        public static void UnlockAccount(string accountname)
        {
            string CredString = GetCredentials();
            RunScript(CredString + " Unlock-ADAccount " + accountname + " -server cofo.edu");
        }

        public static string ListOldComputers(string Date)
        {
            string CredString = GetCredentials();
            string script = "$time = \"" + Date + "\"\n";
            script += "$time = get-date ($time)\n";
            script += "$date = get-date ($time) -UFormat %d.%m.%y\n";
            script += "Get-ADComputer -Filter {LastLogonTimeStamp -lt $time} -and -filter {Name -notlike \"*OU=Not Connected*\"} -Properties LastLogonTimeStamp -Server cofo -Credential $cred\n";
            return RunScriptWithOutput(CredString + " " + script);
        }
        //get-wmiobject win32_desktopmonitor -computername // Look into this some more. 
        //LastLogonTimeStamp -lt $time
        #endregion

    }
}
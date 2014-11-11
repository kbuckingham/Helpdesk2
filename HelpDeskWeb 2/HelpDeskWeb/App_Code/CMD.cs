using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

/// <summary>
/// Summary description for CMD
/// </summary>
public static class CMD
{
	static CMD()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static void DoCommand(string file, string arguments)
    {
        ProcessStartInfo pi = new ProcessStartInfo();
        pi.FileName = file;
        pi.Arguments = arguments;
        pi.UseShellExecute = false;
        Process p = new Process();
        p.StartInfo = pi;
        p.Start();    
    }
}
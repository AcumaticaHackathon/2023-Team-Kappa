using Acumatica.Auth.Api;
using Acumatica.Default_22_200_001.Api;
using Acumatica.Default_22_200_001.Model;
using System;
using System.Collections.Generic;

static void DeleteTestLeads(string siteURL, string username, string password, string tenant = null, string branch = null, string locale = null)
{
    var authApi = new AuthApi(siteURL);

    try
    {
        var configuration = authApi.LogIn(username, password, tenant, branch, locale);

        var leadsApi = new LeadApi(configuration);

        Console.WriteLine("Reading Leads...");
        var leads = leadsApi.GetList();

        var newLeads = leads.Where(x => x.LastModifiedDateTime >= DateTime.Now.AddDays(-3));

        Console.WriteLine($"Found {newLeads.Count()} Leads for Deletion...");

        foreach (var lead in newLeads)
        {
            Console.WriteLine("Deleting Lead ID: " + lead.LeadID.Value + ";");

            leadsApi.DeleteById(lead.ID);

        }
        Console.WriteLine("Lead Deletion Complete...");

    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    finally
    {
        //we use logout in finally block because we need to always logout, even if the request failed for some reason
        if (authApi.TryLogout())
        {
            Console.WriteLine("Logged out successfully.");
        }
        else
        {
            Console.WriteLine("An error occured during logout.");
        }
    }
}



Console.WriteLine("siteUrl:");
string siteUrl = Console.ReadLine();

Console.WriteLine("user:");
string user = Console.ReadLine();

Console.WriteLine("password:");
string password = Console.ReadLine();

Console.WriteLine("branch:");
string branch = Console.ReadLine();

DeleteTestLeads(siteURL: siteUrl, username: user, password: password, branch: branch);
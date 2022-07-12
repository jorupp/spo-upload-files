using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SharePoint.Client;
using PnP.Framework;
using System.Reflection;
using System.Security;

var host = Host.CreateDefaultBuilder(args)
    .Build();

var config = host.Services.GetService<IConfiguration>()!;

var site = config["Site"];
var libTitle = config["DocumentLibraryTitle"];
// set up via https://docs.microsoft.com/en-us/sharepoint/dev/solution-guidance/security-apponly-azuread - "Regiser an application to integrate with Azure AD"
var clientId = config["ClientId"];
var tenantName = config["TenantName"];
var certPath = config["CertificatePath"];
var certPassword = config["CertificatePassword"];
//var clientSecret = config["ClientSecret"];
//var username = config["Username"];
//var password = config["Password"];
//var ssPassword = new SecureString();
//foreach (var c in password)
//    ssPassword.AppendChar(c);
//ssPassword.MakeReadOnly();

//foreach(var grower in new [] { "Grower 1", "Grower 2", "Grower 3"})
//{

//}

async Task<byte[]> GetFile(string filename)
{
    var asm = Assembly.GetExecutingAssembly();
    var ms = new MemoryStream();
    await asm.GetManifestResourceStream($"{asm.GetName().Name!}.{filename}")!.CopyToAsync(ms);
    return ms.ToArray();
}

try
{
    // username/password auth is giving "Microsoft.Identity.Client.MsalClientException: Unsupported User Type 'Unknown'. Please see https://aka.ms/msal-net-up."
    //var am = AuthenticationManager.CreateWithCredentials(username, ssPassword);
    //using var context = await am.GetContextAsync(site);

    // client-secret didn't work either, I could create the context, but operations failed with "The remote server returned an error: (401) Unauthorized."
    //using var context = new AuthenticationManager().GetACSAppOnlyContext(site, clientId, clientSecret);

    var authManager = new AuthenticationManager(clientId, certPath, certPassword, tenantName);
    using var context = await authManager.GetContextAsync(site);

    Console.WriteLine("Context created");

    var lists = context.Web.Lists;
    context.Load(lists);
    await context.ExecuteQueryAsync();
    Console.WriteLine($"Lists: {string.Join(", ", lists.Select(i => i.Title))}");

    //var pdfContent = await GetFile("PdfDoc.pdf");
    //var wordContent = await GetFile("WordDoc.docx");
    //Console.WriteLine("File content read");

    //var list = context.Web.Lists.GetByTitle(libTitle);
    //var rootFolder = list.RootFolder;
    //context.Load(rootFolder);
    //context.Load(rootFolder.Folders);
    //await context.ExecuteQueryAsync();

    //var grower = "Grower1";

    //var fci = new FileCreationInformation
    //{
    //    Content = pdfContent,
    //    Overwrite = true,
    //    Url = "PdfDoc.pdf",
    //};

    ////var folder = context.Web.GetFolderByServerRelativeUrl("/")
    ////var folder = rootFolder.Folders.FirstOrDefault(i => i.Name == grower);
    //var folder = await rootFolder.EnsureFolderAsync(grower); // TODO: this makes a call, probably want to avoid that within the loop for the real thing
    //var file = folder.Files.Add(fci);
    //context.Load(file);
    //Console.WriteLine("Creating file...");
    //await context.ExecuteQueryAsync();

    //Console.WriteLine("Successfully created file");
}
catch(Exception ex)
{
    Console.WriteLine($"Error: {ex}");
}
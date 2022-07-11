using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SharePoint.Client;
using PnP.Framework;
using System.Reflection;

var host = Host.CreateDefaultBuilder(args)
    .Build();

var config = host.Services.GetService<IConfiguration>()!;

var site = config["Site"];
var libTitle = config["DocumentLibraryTitle"];
var clientId = config["ClientId"];
var clientSecret = config["ClientSecret"];

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
    using var context = new AuthenticationManager().GetACSAppOnlyContext(site, clientId, clientSecret);

    var pdfContent = await GetFile("PdfDoc.pdf");
    var wordContent = await GetFile("WordDoc.docx");

    var grower = "Grower 1";

    var fci = new FileCreationInformation
    {
        Content = pdfContent,
        Overwrite = true,
        Url = "PdfDoc.pdf",
    };
    var folder = context.Web.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(grower));
    var file = folder.Files.Add(fci);
    context.Load(file);
    await context.ExecuteQueryAsync();

    Console.WriteLine("Successfully created file");
}
catch(Exception ex)
{
    Console.WriteLine($"Error: {ex}");
}
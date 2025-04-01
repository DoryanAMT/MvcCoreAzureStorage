using Azure.Data.Tables;
using Azure.Storage.Blobs;
using MvcCoreAzureStorage.Services;

var builder = WebApplication.CreateBuilder(args);

string azureKeys =
    builder.Configuration.GetValue<string>("AzureKeys:StorageAccount");
TableServiceClient tableServiceClient =
    new TableServiceClient(azureKeys);
builder.Services.AddTransient<TableServiceClient>
    (x => tableServiceClient);
builder.Services.AddTransient<ServiceStorageTables>();


BlobServiceClient blobServiceClient = new BlobServiceClient(azureKeys);
builder.Services.AddTransient<BlobServiceClient>
    (x => blobServiceClient);
builder.Services.AddTransient<ServiceStorageBlobs>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ServiceStorageFiles>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

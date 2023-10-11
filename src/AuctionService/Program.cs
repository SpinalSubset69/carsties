using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(options => 
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Look for any class that inherits from Profile
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

// Seed Database
try{
    DbInitializer.InitDb(app);
}catch(Exception e){
    Console.WriteLine(e);
}

app.Run();

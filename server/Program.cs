using Microsoft.EntityFrameworkCore;
using server.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if(builder.Environment.IsDevelopment()) {
    builder.Configuration.AddUserSecrets<Program>();
}

var connString = builder.Configuration.GetConnectionString("DefaultPostgreSQLConnection");
builder.Services.AddDbContext<ApplicationDBContext>(opt => {
    opt.UseNpgsql(connString);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using(var scope = app.Services.CreateScope()) {
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    dbContext.Database.EnsureCreated();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

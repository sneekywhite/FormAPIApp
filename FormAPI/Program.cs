using FormAPI.Data;
using FormAPI.Dto.Request;
using FormAPI.Implementation;
using FormAPI.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<dbContext>();

//connection string
string connString = builder.Configuration.GetConnectionString("dbconn");
builder.Services.AddDbContext<dbContext>(options => options.UseSqlServer(connString));

//jwtsetting
var _jwtSetting = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSetting>(_jwtSetting);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ApplyMigrations();

app.Run();


void ApplyMigrations()
{
    using (var Scope = app.Services.CreateScope())
    {
        var _db = Scope.ServiceProvider.GetRequiredService<dbContext>();

        if(_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}

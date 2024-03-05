using AutoMapper;
using FormAPI.Config;
using FormAPI.Data;
using FormAPI.Dto.Request;
using FormAPI.Implementation;
using FormAPI.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAuth, Auth>();

builder.Services.AddScoped<dbContext>();

//connection string
string connString = builder.Configuration.GetConnectionString("dbconn");
builder.Services.AddDbContext<dbContext>(options => options.UseSqlServer(connString));

//AutoMapping
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//jwtsetting
var _jwtSetting = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSetting>(_jwtSetting);

//Authentication
var _authkey = builder.Configuration.GetValue<string>("JwtSettings:SecurityKey");
builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authkey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

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

using System.Text;
using ecommerceApi.Data;
using ecommerceApi.Entities;
using ecommerceApi.Extensions;
using ecommerceApi.Middleware;
using ecommerceApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment environment = builder.Environment;
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put Bearer + your token in the box below",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme, Array.Empty<string>()
        }
    });
});

string connString;
if (environment.IsDevelopment())
    connString = configuration.GetConnectionString("DefaultConnection");
else
{
    // Use connection string provided at runtime.
    connString = Environment.GetEnvironmentVariable("PGDB_URL");

}

builder.Services.AddDbContext<StoreContext>(options =>
options.UseSqlServer(connString));



builder.Services.AddCors();
builder.Services.AddIdentityCore<User>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequiredLength = 6;
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
})
.AddRoles<Role>()
.AddEntityFrameworkStores<StoreContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(opt => {
              opt.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = false,
                  ValidateAudience = false,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.
                      GetBytes(configuration["JWTSettings:TokenKey"]))
              };
          });



builder.Services.AddAuthorization();
builder.Services.AddScoped<TokenService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
});

app.UseDefaultFiles();





 

if (environment.IsDevelopment())
{
    string directory = Path.Combine(environment.ContentRootPath, "..\\var\\lib\\Upload\\Images");

    if (!Directory.Exists(directory))
    {
        Directory.CreateDirectory(directory);
    }

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(environment.ContentRootPath, "..\\var\\lib\\Upload\\Images")),
        RequestPath = "/Images"
    });

}
else
{
    string directory = Path.Combine(environment.ContentRootPath, "..//var//lib//Upload//Images");

    if (!Directory.Exists(directory))
    {
        Directory.CreateDirectory(directory);
    }

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(environment.ContentRootPath, "..//var//lib//Upload//Images")),
        RequestPath = "/Images"
    });

}



if (environment.IsDevelopment())
{
    app.UseCors(opt =>
    {
        opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000");
    });

}else
{
    app.UseCors(opt =>
    {
        opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://blushgallery.com");
        opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://*.blushgallery.com")
                .SetIsOriginAllowedToAllowWildcardSubdomains(); ;

    });
}



app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


var scop = app.Services.CreateScope();
var context = scop.ServiceProvider.GetRequiredService<StoreContext>();
var userManager = scop.ServiceProvider.GetRequiredService<UserManager<User>>();
var logger = scop.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
    await context.Database.MigrateAsync();
    await DbInitializer.Inilialize(context, userManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "problem in migrating data");

}

app.Run();

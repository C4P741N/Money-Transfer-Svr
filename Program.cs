using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using money_transfer_server_side.JsonExtractors;
using money_transfer_server_side.Redirectors;
using money_transfer_server_side.Utils;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMts_TransactionManager, Mts_TransactionManager>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//builder.WebHost.UseUrls("http://*:80", "https://*.443");

//JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        RequireExpirationTime = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        //ClockSkew = TimeSpan.FromSeconds(30),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
//Cookie
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//  .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
//  {
//      options.Cookie.Name = "Mtss.Cookies";
//      options.SlidingExpiration = true;
//      options.ExpireTimeSpan = new TimeSpan(0, 1, 0); // Expires in 1 minute
//      options.Events.OnRedirectToLogin = (context) =>
//      {
//          context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//          return Task.CompletedTask;
//      };
//      options.Cookie.HttpOnly = true;
//      // Only use this when the sites are on different domains
//      //options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
//  });

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
//    options.Events = new CookieAuthenticationEvents
//    {
//        OnSigningOut = context =>
//        {
//            // Set the .AspNetCore.Cookies value as a response header
//            context.Response.Headers.SetCookie = context.Properties.GetTokens().First(t => t.Name == ".AspNetCore.Cookies").Value;
//            return Task.CompletedTask;
//        }
//    };
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(webApi =>
{
    webApi      
    .WithOrigins("http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithExposedHeaders()
    .AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

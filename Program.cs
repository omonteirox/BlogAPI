using Blog.Data;
using BlogAPI;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ConfigureAuthentication(builder);
ConfigureMvc(builder);
ConfigureServices(builder);
var app = builder.Build();
LoadConfiguration(app);

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

static void LoadConfiguration(WebApplication app)
{
    var smtp = new Configuration.SmtpConfiguration();
    app.Configuration.GetSection("Smtp").Bind(smtp);
    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
    Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");
    Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
    Configuration.Smtp = smtp;
}

static void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<BlogDataContext>();
    builder.Services.AddTransient<TokenService>();
    builder.Services.AddTransient<EmailService>();
}

static void ConfigureMvc(WebApplicationBuilder builder)
{
    builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });
}
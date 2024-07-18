using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OnlineEducationPlatform;
using OnlineEducationPlatform.Persistence;
using OnlineEducationPlatform.Persistence.Data;
using OnlineEducationPlatform.Persistence.Repository;
using OnlineEducationPlatform.Shared.Entities;
using OnlineEducationPlatform.Shared.Identity;
using OnlineEducationPlatform.Shared.Interfaces;
using OnlineEducationPlatform.Shared.Middleware;
using OnlineEducationPlatform.Shared.Profiles;
using OnlineEducationPlatform.Shared.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .AddConsole();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"]
            ?? throw new InvalidOperationException("Jwt Issuer is missing"),
        ValidAudience = builder.Configuration["JwtSettings:Audience"]
            ?? throw new InvalidOperationException("Jwt Audience is missing"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]
            ?? throw new InvalidOperationException("Secret key is missing.")))
    };
});

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policy.SuperAdmin, policy => policy.RequireClaim("role", Role.SuperAdmin));
    options.AddPolicy(Policy.TenantAdmin, policy => policy.RequireClaim("role", Role.TenantAdmin));
    options.AddPolicy(Policy.CourseCreator, policy => policy.RequireClaim("role", Role.CourseCreator));
});

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var result = new BadRequestObjectResult(context.ModelState);
            return result;
        };
    });

builder.Services.AddSwaggerGen();
builder.Services
    .AddScoped<IUserService, UserService2>()
    //.AddScoped(typeof(IGenericService<>), typeof(GenericService<>))
    .AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>))
    .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
    .AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>))
    .AddScoped<DefaultJwtSecurityTokenHandler>()
    .AddScoped(typeof(IGenericService<Course>), typeof(CourseService))
    ;

// register auto mapper
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new CourseProfile());
    mc.AddProfile(new UserProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

await SeedData.InitializeAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    //app.UseMiddleware<RequestBodyLoggingMiddleware>();
    //app.UseMiddleware<RequestResponseLoggingMiddleware>();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.Identity.Web;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.


////builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
////    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
////        .EnableTokenAcquisitionToCallDownstreamApi()
////            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
////            .AddInMemoryTokenCaches()
////            .AddDownstreamApi("DownstreamApi", builder.Configuration.GetSection("DownstreamApi"))
////            .AddInMemoryTokenCaches();

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthentication();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

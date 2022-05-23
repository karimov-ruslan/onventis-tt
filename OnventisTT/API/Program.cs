using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnventisTT;
using OnventisTT.Messaging;
using OnventisTT.Messaging.Abstraction;
using OnventisTT.Services;
using OnventisTT.Services.Abstraction;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Host.UseSerilog((ctx, lc) => lc
           .WriteTo.Console()
           .ReadFrom.Configuration(ctx.Configuration));

    var connectionString = builder.Configuration.GetConnectionString("WebApiDatabase");
    builder.Services.AddDbContext<MyDbContext>(x => x.UseSqlite(connectionString));

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

    builder.Services.AddAutoMapper(cfg =>
    {
        cfg.CreateMap<OnventisTT.WebModels.InvoiceModel, OnventisTT.Services.Models.InvoiceModel>();
        cfg.CreateMap<OnventisTT.WebModels.InvoiceLineModel, OnventisTT.Services.Models.InvoiceLineModel>();
        cfg.CreateMap<OnventisTT.Services.Models.InvoiceModel, OnventisTT.Infrastructure.Entities.Invoice>();
        cfg.CreateMap<OnventisTT.Services.Models.InvoiceLineModel, OnventisTT.Infrastructure.Entities.InvoiceLine>();
    },
    AppDomain.CurrentDomain.GetAssemblies());
    
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });

    builder.Services.AddScoped<IInvoiceService, InvoiceService>();
    builder.Services.AddScoped<IMessageSender, RabbitMqMessageSender>();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
        dbContext.Database.Migrate();
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();

    app.UseAuthentication();
    app.UseAuthorization();
    
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}


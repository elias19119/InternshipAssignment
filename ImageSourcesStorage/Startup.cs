namespace ImageSourcesStorage
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;
    using System.Text;
    using System.Text.Json.Serialization;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using ImageSourcesStorage.DataAccessLayer;
    using ImageSourcesStorage.DataAccessLayer.Models;
    using ImageSourcesStorage.DataAccessLayer.Validators;
    using ImageSourcesStorage.Mappings;
    using ImageSourcesStorage.Validators;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Azure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.Filters;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(this.Configuration.GetSection("Storage:ConnectionString").Value);
            });
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("ImageSourceDatabase")));
            services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddSwaggerGen(c =>
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "ImageSourcesStorage.xml");
                c.IncludeXmlComments(filePath, includeControllerXmlComments: true);

                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT", // Optional
                };

                var securityRequirement = new OpenApiSecurityRequirement
                {
                     {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                   Type = ReferenceType.SecurityScheme,
                                   Id = "oauth2",
                                },
                            },
                            new string[] {}
                     },
                };

                c.AddSecurityDefinition("oauth2", securityScheme);
                c.AddSecurityRequirement(securityRequirement);
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            services.AddMvc(opt =>
            {
                opt.SuppressAsyncSuffixInActionNames = false;
                opt.Filters.Add(typeof(ValidationFilter));
            }).AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddTransient(typeof(IUserRepository), typeof(UserRepository));
            services.AddTransient(typeof(IBoardRepository), typeof(BoardRepository));
            services.AddTransient(typeof(IPinRepository), typeof(PinRepository));
            services.AddTransient(typeof(IPinBoardRepository), typeof(PinBoardRepository));
            services.AddTransient(typeof(ICredentials), typeof(CredentailsRepository));
            services.AddTransient<IStorage, Storage>();
            services.AddAutoMapper(typeof(Startup));
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); 
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = this.Configuration["JwtIssuer"],
                        ValidAudience = this.Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["JwtKey"])),
                        ClockSkew = TimeSpan.Zero, // remove delay of token when expire
                    };
                });
            services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<DataContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pins");
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

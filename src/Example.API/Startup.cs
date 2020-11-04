using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Example.Domain.Mappings;
using Example.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Example.Domain.Helpers.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;
using Example.Domain.Interfaces;
using Example.Infrastructure.Repositories;
using Example.Infrastructure.Auth;
using Example.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Example.Domain.Enums;

namespace Example.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ExampleDbContext>(opt =>
            {
                opt.UseSqlServer(_configuration.GetConnectionString("Docker")
                    //x => x.MigrationsAssembly("Example.Infrastructure.Persistence")
                    ); 
            });

            services.AddAutoMapper(typeof(DefaultMappingProfile));

            var authSettings = _configuration.GetSection(nameof(AuthSettings));
            services.Configure<AuthSettings>(authSettings);

            var signingKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings[nameof(AuthSettings.SecretKey)]));

            var jwtOptions = _configuration.GetSection(nameof(JwtOptions));
            services.Configure<JwtOptions>(options =>
            {
                options.Issuer = jwtOptions[nameof(JwtOptions.Issuer)];
                options.Audience = jwtOptions[nameof(JwtOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                options.ExpireMinute = int.Parse(jwtOptions[nameof(JwtOptions.ExpireMinute)]);
                options.RefreshMinute = int.Parse(jwtOptions[nameof(JwtOptions.RefreshMinute)]);
            });

            // es ufro strict sheidzleba iyos but it's a demo
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidIssuer = jwtOptions[nameof(JwtOptions.Issuer)],

                ValidateAudience = false,
                ValidAudience = jwtOptions[nameof(JwtOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.ClaimsIssuer = jwtOptions[nameof(JwtOptions.Issuer)];
                options.TokenValidationParameters = tokenValidationParameters;
                options.SaveToken = true;

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // amit realurad front ends vagebineb martivad rom tokens dro gauvida
                        // kinda gets the job done
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
                            context.Response.Headers.Add("Access-Control-Expose-Headers", "*");
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            // just enable for everyone for now
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyEnum.ContactOwner.ToString(),
                        policy => policy.Requirements.Add(new ContactBelongsToUserRequirement())
                    );
                options.AddPolicy(PolicyEnum.PhoneNumberOwner.ToString(),
                        policy => policy.Requirements.Add(new PhoneNumberBelongsToUserRequirement())
                    );
            });

            // Auth
            services.AddSingleton<IAuthorizationHandler, ContactAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, PhoneNumberAuthorizationHandler>();

            // token generator services
            services.AddSingleton<IJwtTokenHandler, JwtTokenHandler>();
            services.AddSingleton<ITokenFactory, TokenFactory>();
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddSingleton<IJwtTokenValidator, JwtTokenValidator>();

            // helpers
            services.AddSingleton<IHashHelper, HashHelper>();

            // repos
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IContactsRepository, ContactsRepository>();
            services.AddScoped<IPhoneNumbersRepository, PhoneNumbersRepository>();

            services.AddAuthorization();
            services.AddControllers()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    // ubralod internals davabruneb tu rame exception moxdeba
                    // globalurad rom davichiro exception
                    // ubralo fixia am shemtxvevashi rom bevri try catch ar dawero tundac
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        // aq shegvidzlia davichirot nebismieri exception
                        // amis realurad calke gatanac sheidzleba mtlianad magram simartivistvis aq davtov
                        // bevri patara proeqtia mainc

                    }
                    await context.Response.StartAsync();
                });
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseCookieAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

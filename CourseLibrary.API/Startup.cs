using CourseLibrary.API.DbContexts;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CourseLibrary.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost");
                    });
            });
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                //setupAction.OutputFormatters.Add(
                //    new XmlDataContractSerializerOutputFormatter());
            }).AddXmlDataContractSerializerFormatters()
           .ConfigureApiBehaviorOptions(setupAction =>
           {
               setupAction.InvalidModelStateResponseFactory = contexet =>
               {
                   // create a problem details object 
                   var problemDetailsFactory = contexet.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                   var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(contexet.HttpContext, contexet.ModelState);

                   // add additional info added by default
                   problemDetails.Detail = "see the errors fields for details.";
                   problemDetails.Instance = contexet.HttpContext.Request.Path;

                   //find out which status code to use 
                   var actionExecutingContext = contexet as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                   //if there are modelstate errors & all arguments were correctly 
                   // found / parsed we're dealing with validation errors 
                   if ((contexet.ModelState.ErrorCount > 0) && (actionExecutingContext?.ActionArguments.Count == contexet.ActionDescriptor.Parameters.Count))
                   {
                       problemDetails.Type = "https://courseLibrary/modelvalidationproblem"; // custom type
                       problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                       problemDetails.Title = "One or more validation errors occurred";
                       return new UnprocessableEntityObjectResult(problemDetails)
                       {
                           ContentTypes = { "application/problem+json" } // from rfc standards 
                        };
                   }

                   // if one of the arguments wasn't correctly found / couldn't be parsed
                   // we're dealing with null/unparseable input 
                   problemDetails.Status = StatusCodes.Status400BadRequest;
                   problemDetails.Title = "One or more errors on input occurred.";
                   return new BadRequestObjectResult(problemDetails)
                   {
                       ContentTypes = { "application/problem+json" }
                   };
               };
           });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ICourseLibraryRepository, CourseLibraryRepository>();

            services.AddDbContext<CourseLibraryContext>(options =>
            {
                options.UseSqlServer(
                    @"Server=DESKTOP-AT22ON3;Database=CourseLibraryDB;Trusted_Connection=True;");
            }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseExceptionHandler(); //default middele ware used for handilling exception in production 
               //if we want to custom message for 500 exception 
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An Unexpected Fault happened. Try again later");
                    });
                });
            }

            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

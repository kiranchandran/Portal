using CompanyName.Data.Contexts;
using CompanyName.Data.Seeds;

namespace CompanyName.Api.Extensions
{
    /// <summary>
    /// Extensions for <see cref="WebApplicationBuilder"/>.
    /// </summary>
    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure the services.
        /// </summary>
        /// <param name="builder"><see cref="WebApplicationBuilder"/>.</param>
        /// <returns></returns>
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.ConfigureServices(builder.Configuration);
            builder.Services.ConfigureAuthentication(builder.Configuration);
            return builder.Build();
        }

        /// <summary>
        /// Configure the pipeline.
        /// </summary>
        /// <param name="app"><see cref="WebApplication"/></param>
        /// <returns></returns>
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
            }
            app.UseSwagger();
            app.UseSwaggerUI();

            app.ConfigureCors();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        /// <summary>
        /// Execute the seeding for the initial load.
        /// </summary>
        /// <param name="app"><see cref="WebApplication"/>.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static WebApplication ConfigureSeed(this WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<DataContext>();
                if (context == null)
                {
                    throw new Exception("Context is null. Filed to seed data.");
                }
                var dataInitialiser = new DataInitialiser(context);
                dataInitialiser.SeedInitialData();
            }

            return app;
        }

        private static void ConfigureCors(this WebApplication app)
        {
            app.UseCors("CorsPolicy");
        }
    }
}

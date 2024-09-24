namespace Talabat.APIs.Extentions
{
    public static class SwaggerServicesExtention
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen();

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app) 
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddHttpClient();
        services.AddSingleton<IConfiguration>(provider => Configuration);
        services.AddTransient<DotNetEmailClassifierApi.Services.AiServiceClient>();

        // Register SchemaProvider with path from configuration
        var schemaPath = Configuration["Schema:SpamEvaluationPath"] ?? "Resources/SpamEvaluation.schema.json";
        services.AddSingleton(new DotNetEmailClassifierApi.Services.SchemaProvider(schemaPath));

        var issueTypesPath = Configuration["Issues:IssueTypesPath"] ?? "Resources/IssueTypes.json";
        services.AddSingleton(new DotNetEmailClassifierApi.Services.IssueTypesProvider(issueTypesPath));
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
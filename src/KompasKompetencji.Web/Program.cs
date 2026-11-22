using KompasKompetencji.Web.Auth;
using KompasKompetencji.Web.Assistant;
using KompasKompetencji.Web.Assistant.KnowledgeBase;
using KompasKompetencji.Web.Assistant.Privacy;
using KompasKompetencji.Web.Components;
using KompasKompetencji.Web.Data;
using KompasKompetencji.Web.Demo;
using KompasKompetencji.Web.Evaluations;
using KompasKompetencji.Web.Evaluations.Pdf;
using Microsoft.Extensions.AI;
using KompasKompetencji.Web.Organization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'ConnectionStrings:Default' is missing.");

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sql => sql
        .EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(15), errorNumbersToAdd: null)
        .CommandTimeout(120)));

builder.Services.Configure<DemoOptions>(builder.Configuration.GetSection(DemoOptions.SectionName));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = AuthEndpoints.LoginPagePath;
        options.AccessDeniedPath = "/brak-dostepu";
        options.ReturnUrlParameter = "returnUrl";
        options.Cookie.Name = "kompas.auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddSingleton<LoginTicketStore>();
builder.Services.AddSingleton<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrganizationService>();
builder.Services.AddScoped<EvaluationService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRadzenComponents();

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

builder.Services.AddSingleton(_ => KnowledgeBaseStore.Load(KnowledgeBaseStore.DefaultPath));
builder.Services.AddSingleton<Bm25Retriever>();
builder.Services.AddSingleton<EmployeeNameSource>();
builder.Services.AddChatClient(AssistantClientFactory.Create)
    .Use((inner, services) => new KnowledgeBaseChatClient(inner, services.GetRequiredService<Bm25Retriever>()))
    .Use((inner, services) => new PseudonymizingChatClient(inner, services.GetRequiredService<EmployeeNameSource>().GetAsync))
    .UseLogging();
builder.Services.AddKeyedChatClient(NoteCriterionClassifier.ClientKey, AssistantClientFactory.Create)
    .Use((inner, services) => new PseudonymizingChatClient(inner, services.GetRequiredService<EmployeeNameSource>().GetAsync))
    .UseLogging();
builder.Services.AddScoped<NoteCriterionClassifier>();
builder.Services.AddSingleton<ILocalizer, PolishRadzenLocalizer>();
builder.Services.AddAIChatService();

var app = builder.Build();

app.Services.GetRequiredService<Bm25Retriever>();

if (app.Configuration.GetSection(DemoOptions.SectionName).Get<DemoOptions>()?.ResetDatabaseOnStartup == true)
{
    using var scope = app.Services.CreateScope();
    await DemoDatabase.ResetAsync(scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>());
    await scope.ServiceProvider.GetRequiredService<UserService>().EnsureTestAccountsAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseRequestLocalization(new RequestLocalizationOptions()
    .SetDefaultCulture("pl-PL")
    .AddSupportedCultures("pl-PL")
    .AddSupportedUICultures("pl-PL"));

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapAuthEndpoints();
app.MapEvaluationPdfEndpoints();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

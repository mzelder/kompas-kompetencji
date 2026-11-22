using KompasKompetencji.Web.Auth;

namespace KompasKompetencji.Web.Evaluations.Pdf;

public static class EvaluationPdfEndpoints
{
    private const string SheetPath = "/przelozony/oceny/{evaluationId:int}/arkusz.pdf";

    public static string SheetUrl(int evaluationId) => $"/przelozony/oceny/{evaluationId}/arkusz.pdf";

    public static IEndpointRouteBuilder MapEvaluationPdfEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(SheetPath, async (int evaluationId, HttpContext http, UserService users, EvaluationService evaluations, CancellationToken ct) =>
        {
            var userId = http.User.GetUserId();
            var user = userId is null ? null : await users.GetByIdAsync(userId.Value, ct);
            if (user?.EmployeeId is not int supervisorId)
            {
                return Results.NotFound();
            }

            var sheet = await evaluations.GetEvaluationSheetAsync(supervisorId, evaluationId, ct);
            return sheet is null
                ? Results.NotFound()
                : Results.File(new EvaluationSheetDocument(sheet).Generate(), "application/pdf", sheet.FileName);
        }).RequireAuthorization(policy => policy.RequireRole(AppRoles.Supervisor));

        return app;
    }
}

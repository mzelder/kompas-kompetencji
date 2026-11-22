namespace KompasKompetencji.Web.Materials;

public static class MaterialEndpoints
{
    public const string BasePath = "/materialy";
    public const string OutputFolder = "Materials";

    public static void MapMaterialEndpoints(this WebApplication app)
    {
        app.MapGet($"{BasePath}/{{fileName}}", (string fileName) =>
        {
            if (MaterialCatalog.Find(fileName) is not Material material)
            {
                return Results.NotFound();
            }

            var path = Path.Combine(AppContext.BaseDirectory, OutputFolder, material.FileName);
            if (!File.Exists(path))
            {
                return Results.NotFound();
            }

            return material.Extension switch
            {
                "PDF" => Results.File(path, "application/pdf"),
                "DOCX" => Results.File(path, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", material.FileName),
                "XLSX" => Results.File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", material.FileName),
                _ => Results.File(path, "application/octet-stream", material.FileName),
            };
        }).RequireAuthorization();
    }
}

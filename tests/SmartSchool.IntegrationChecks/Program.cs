using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SmartSchool.Modules.Admissions.Persistence;
using SmartSchool.Modules.Documents.Persistence;
using SmartSchool.Modules.Examinations.Persistence;
using SmartSchool.Modules.HR.Persistence;
using SmartSchool.Modules.Learning.Persistence;
using SmartSchool.Modules.Organization.Persistence;
using SmartSchool.Modules.Students.Persistence;
using SmartSchool.Modules.Transport.Persistence;

if (!args.Contains("--export-model")) { await WorkflowChecks.RunAsync(); return; }

var contexts = new[] { typeof(OrganizationDbContext), typeof(StudentsDbContext), typeof(AdmissionsDbContext),
    typeof(HRDbContext), typeof(DocumentsDbContext), typeof(LearningDbContext), typeof(ExaminationsDbContext), typeof(TransportDbContext) };
var mappings = new List<object>();
foreach (var type in contexts)
{
    var builder = (DbContextOptionsBuilder)Activator.CreateInstance(typeof(DbContextOptionsBuilder<>).MakeGenericType(type))!;
    builder.UseNpgsql("Host=127.0.0.1;Port=5433;Database=postgres;Username=postgres;Password=postgres;SSL Mode=Disable;Pooling=false");
    using var context = (DbContext)Activator.CreateInstance(type, builder.Options)!;
    foreach (var entity in context.Model.GetEntityTypes())
    {
        var table = entity.GetTableName();
        if (table is null) continue;
        var schema = entity.GetSchema() ?? "public";
        var store = StoreObjectIdentifier.Table(table, schema);
        mappings.Add(new { Context = type.Name, Entity = entity.Name, Schema = schema, Table = table,
            Columns = entity.GetProperties().Select(property => new { Name = property.GetColumnName(store),
                Type = property.GetRelationalTypeMapping().StoreType, Nullable = property.IsNullable,
                PrimaryKey = property.IsPrimaryKey(), Property = property.Name }).ToList() });
    }
}
Console.WriteLine(JsonSerializer.Serialize(mappings, new JsonSerializerOptions { WriteIndented = true }));

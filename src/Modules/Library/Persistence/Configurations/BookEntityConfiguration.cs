using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence.Configurations;

/// <summary>
/// Defines relational persistence rules for <see cref="BookEntity"/>.
/// </summary>
public sealed class BookEntityConfiguration
    : IEntityTypeConfiguration<BookEntity>
{
    public void Configure(EntityTypeBuilder<BookEntity> builder)
    {
        builder.ToTable("book", schema: "library");
        builder.HasKey(entity => entity.BookId);

        builder
            .Property(entity => entity.TenantId)
            .IsRequired();

        builder
            .Property(entity => entity.IsActive)
            .IsRequired();

        builder.HasIndex(entity => entity.TenantId);

        builder.Property(entity => entity.CreatedAt).IsRequired();
        builder.Property(entity => entity.UpdatedAt);
        builder.Property(entity => entity.RowVersion).IsRequired().IsConcurrencyToken();

        builder
            .Property(entity => entity.Code)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .HasIndex(entity => new { entity.TenantId, entity.Code })
            .IsUnique();

        builder
            .Property(entity => entity.Name)
            .HasMaxLength(250)
            .IsRequired();


        // Canonical database mapping generated from SmartSchoolComplete.sql.
        builder.Property(entity => entity.Code).HasColumnName("code");
        builder.Property(entity => entity.Name).HasColumnName("name");
        builder.Property(entity => entity.MetadataJson).HasColumnName("metadata_json").HasColumnType("jsonb");
        builder.Property(entity => entity.BookId).HasColumnName("book_id");
        builder.Property(entity => entity.TenantId).HasColumnName("tenant_id");
        builder.Property(entity => entity.IsActive).HasColumnName("is_active");
        builder.Property(entity => entity.CreatedAt).HasColumnName("created_at");
        builder.Property(entity => entity.UpdatedAt).HasColumnName("updated_at");
        builder.Property(entity => entity.RowVersion).HasColumnName("row_version");

        // Database columns synchronized from SmartSchoolComplete.sql.
        builder.Property(entity => entity.Isbn).HasColumnName("isbn");
        builder.Property(entity => entity.Title).HasColumnName("title");
        builder.Property(entity => entity.AuthorText).HasColumnName("author_text");
        builder.Property(entity => entity.PublisherText).HasColumnName("publisher_text");
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SmartSchool.Modules.Identity.Persistence.Identity;

public sealed class SmartSchoolIdentityDbContext(
    DbContextOptions<SmartSchoolIdentityDbContext> options)
    : IdentityDbContext<SmartSchoolUser, SmartSchoolRole, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("identity");

        builder.Entity<SmartSchoolUser>(entity =>
        {
            entity.ToTable("Users", "identity");
            entity.Property(x => x.FirstName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.LastName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.DisplayName).HasMaxLength(300);
            entity.Property(x => x.AccountType).HasMaxLength(50);
            entity.HasIndex(x => new { x.TenantId, x.BusinessEntityId, x.AccountType });
            entity.HasIndex(x => new { x.TenantId, x.NormalizedEmail });
        });

        builder.Entity<SmartSchoolRole>().ToTable("Roles", "identity");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<Guid>>().ToTable("UserRoles", "identity");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>>().ToTable("UserClaims", "identity");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<Guid>>().ToTable("UserLogins", "identity");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<Guid>>().ToTable("RoleClaims", "identity");
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<Guid>>().ToTable("UserTokens", "identity");
    }
}

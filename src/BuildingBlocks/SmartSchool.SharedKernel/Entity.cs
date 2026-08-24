namespace SmartSchool.SharedKernel;

/// <summary>
/// Provides lifecycle, auditing, and optimistic concurrency state shared by domain entities.
/// </summary>
public abstract class AggregateRootEntity
{
    /// <summary>Gets a value indicating whether the entity is active.</summary>
    public bool IsActive { get; protected set; } = true;

<<<<<<< HEAD
    /// <summary>Gets the UTC creation date and time.</summary>
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
=======
	/// <summary>Gets the tenant that owns the entity.</summary>
	public Guid TenantId { get; protected set; } 
>>>>>>> c40f31f829a59dcdb7fd9fe0046a26e6e366eca0

    /// <summary>Gets the UTC last-updated date and time.</summary>
    public DateTimeOffset? UpdatedAt { get; protected set; }

    /// <summary>Gets the optimistic concurrency token.</summary>
    public byte[] RowVersion { get; protected set; } = [];

    /// <summary>Activates the entity.</summary>
    public void Activate()
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        MarkAsUpdated();
    }

    /// <summary>Deactivates the entity.</summary>
    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        MarkAsUpdated();
    }

    /// <summary>Marks the entity as updated.</summary>
    protected void MarkAsUpdated()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}

/// <summary>Provides tenant ownership for tenant-scoped domain entities.</summary>
public abstract class Entity : AggregateRootEntity
{
    /// <summary>Gets the tenant that owns the entity.</summary>
    public Guid TenantId { get; protected set; }
}

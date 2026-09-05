using SmartSchool.Modules.Inventory.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Inventory.Models;

namespace SmartSchool.Modules.Inventory.Features.PurchaseOrder;

/// <summary>
/// Executes database writes for <see cref="PurchaseOrderEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PurchaseOrderCommand(IInventoryDbContext dbContext) : IPurchaseOrderCommand
{
    public async Task AddAsync(
        PurchaseOrderEntity entity,
        CancellationToken cancellationToken)
    {
        await dbContext.PurchaseOrders
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        PurchaseOrderEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.PurchaseOrders
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        PurchaseOrderEntity entity,
        CancellationToken cancellationToken)
    {
        dbContext.PurchaseOrders
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

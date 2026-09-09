using SmartSchool.SharedKernel;
namespace SmartSchool.Modules.Finance.Models;
public sealed class FeeStructureEntity : Entity
{
    public Guid FeeStructureId { get; private set; }=Guid.NewGuid();
    public Guid DepartmentId { get; private set; }
    public Guid GradeLevelId { get; private set; }
    public Guid FeeTypeId { get; private set; }
    public Guid? AcademicYearId { get; private set; }
    public decimal Amount { get; private set; }
    public string Frequency { get; private set; }="Monthly";
    public DateOnly? EffectiveFrom { get; private set; }
    public DateOnly? EffectiveTo { get; private set; }
    public string Code { get; private set; }=string.Empty;
    public string Name { get; private set; }=string.Empty;
    public string? MetadataJson { get; private set; }
    private FeeStructureEntity(){}
    public static FeeStructureEntity Create(Guid tenantId, Guid departmentId, string code, Guid gradeLevelId,Guid feeTypeId,decimal amount,string frequency,Guid? academicYearId=null,DateOnly? effectiveFrom=null,DateOnly? effectiveTo=null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        return new FeeStructureEntity{TenantId=tenantId,DepartmentId=departmentId,GradeLevelId=gradeLevelId,FeeTypeId=feeTypeId,Amount=amount,Frequency=frequency,AcademicYearId=academicYearId,EffectiveFrom=effectiveFrom,EffectiveTo=effectiveTo,Code=code,Name=code};
    }
    public void Update(decimal amount,string frequency,DateOnly? effectiveFrom,DateOnly? effectiveTo,bool isActive){Amount=amount;Frequency=frequency;EffectiveFrom=effectiveFrom;EffectiveTo=effectiveTo;if(isActive)Activate();else Deactivate();MarkAsUpdated();}
}

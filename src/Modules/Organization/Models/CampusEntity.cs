using SmartSchool.SharedKernel;
using SmartSchool.Modules.Organization.Enums;

namespace SmartSchool.Modules.Organization.Models;

/// <summary>
/// Represents a physical branch or campus belonging to a school.
/// </summary>
public sealed class CampusEntity : Entity
{
	private readonly List<AcademicYearEntity> _academicYears = [];
	//private readonly List<TermEntity> _academicTerms = [];
	//private readonly List<CampusBrandingEntity> _brandings = [];
	private readonly List<DepartmentEntity> _departments = [];
	private readonly List<TimetableEntity> _timetables = [];

	private CampusEntity()
	{
	}

	public Guid CampusId { get; private set; } = Guid.NewGuid();

	public Guid SchoolId
	{
		get; private set;
	}

	public string Code { get; private set; } = string.Empty;

	public string Name { get; private set; } = string.Empty;

	public BranchType BranchType { get; private set; } = BranchType.RegionalBranch;

	public Guid BranchGenderTypeId
	{
		get; private set;
	}

	public Guid? AcademicSystemId
	{
		get; private set;
	}

	public string? Address
	{
		get; private set;
	}

	public string? City
	{
		get; private set;
	}

	public string? Province
	{
		get; private set;
	}

	public string? Country
	{
		get; private set;
	}

	public string? Phone
	{
		get; private set;
	}

	public string? Fax
	{
		get; private set;
	}

	public string? Mobile
	{
		get; private set;
	}

	public string? Email
	{
		get; private set;
	}

	public string? LogoUrl
	{
		get; private set;
	}

	public string? MetadataJson
	{
		get; private set;
	}

	public IReadOnlyCollection<AcademicYearEntity> AcademicYears =>
		_academicYears.AsReadOnly();

	//public IReadOnlyCollection<TermEntity> AcademicTerms =>
	//	_academicTerms.AsReadOnly();

	//public IReadOnlyCollection<CampusBrandingEntity> Brandings =>
	//	_brandings.AsReadOnly();

	public IReadOnlyCollection<DepartmentEntity> Departments =>
		_departments.AsReadOnly();

	public IReadOnlyCollection<TimetableEntity> Timetables =>
		_timetables.AsReadOnly();

	public static CampusEntity Create(
		Guid tenantId,
		Guid schoolId,
		string code,
		string name,
		BranchType branchType,
		Guid branchGenderTypeId,
		Guid? academicSystemId,
		string? address,
		string? city,
		string? province,
		string? country,
		string? phone,
		string? fax,
		string? mobile,
		string? email,
		string? logoUrl,
		string? metadataJson = null)
	{
		if (tenantId == Guid.Empty)
		{
			throw new ArgumentException(
				"Tenant is required.",
				nameof(tenantId));
		}

		if (schoolId == Guid.Empty)
		{
			throw new ArgumentException(
				"School is required.",
				nameof(schoolId));
		}

		if (branchGenderTypeId == Guid.Empty)
		{
			throw new ArgumentException(
				"Branch gender type is required.",
				nameof(branchGenderTypeId));
		}

		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);
	
		return new CampusEntity
		{
			TenantId = tenantId,
			SchoolId = schoolId,
			Code = code.Trim(),
			Name = name.Trim(),
			BranchType = branchType,
			BranchGenderTypeId = branchGenderTypeId,
			AcademicSystemId = academicSystemId,
			Address = Clean(address),
			City = Clean(city),
			Province = Clean(province),
			Country = Clean(country),
			Phone = Clean(phone),
			Fax = Clean(fax),
			Mobile = Clean(mobile),
			Email = Clean(email),
			LogoUrl = Clean(logoUrl),
			MetadataJson = Clean(metadataJson)
		};
	}

	public void UpdateDetails(
		string code,
		string name,
		BranchType branchType,
		Guid branchGenderTypeId,
		Guid? academicSystemId,
		string? address,
		string? city,
		string? province,
		string? country,
		string? phone,
		string? fax,
		string? mobile,
		string? email,
		string? logoUrl,
		string? metadataJson = null)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(code);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);
		

		if (branchGenderTypeId == Guid.Empty)
		{
			throw new ArgumentException(
				"Branch gender type is required.",
				nameof(branchGenderTypeId));
		}

		Code = code.Trim();
		Name = name.Trim();
		BranchType = branchType;
		BranchGenderTypeId = branchGenderTypeId;
		AcademicSystemId = academicSystemId;
		Address = Clean(address);
		City = Clean(city);
		Province = Clean(province);
		Country = Clean(country);
		Phone = Clean(phone);
		Fax = Clean(fax);
		Mobile = Clean(mobile);
		Email = Clean(email);
		LogoUrl = Clean(logoUrl);
		MetadataJson = Clean(metadataJson);

		MarkAsUpdated();
	}

	public void UpdateDetails(
		string code,
		string name)
	{
		UpdateDetails(
			code,
			name,
			BranchType,
			BranchGenderTypeId,
			AcademicSystemId,
			Address,
			City,
			Province,
			Country,
			Phone,
			Fax,
			Mobile,
			Email,
			LogoUrl,
			MetadataJson);
	}

	public void AddAcademicYear(AcademicYearEntity academicYear)
	{
		ArgumentNullException.ThrowIfNull(academicYear);

		if (_academicYears.Any(
				x => x.AcademicYearId == academicYear.AcademicYearId))
		{
			throw new InvalidOperationException(
				"Academic year already belongs to this campus.");
		}

		_academicYears.Add(academicYear);

		MarkAsUpdated();
	}

	public void RemoveAcademicYear(Guid academicYearId)
	{
		AcademicYearEntity academicYear = GetRequired(
			_academicYears,
			x => x.AcademicYearId == academicYearId,
			"Academic year");

		_academicYears.Remove(academicYear);

		MarkAsUpdated();
	}

	//public void AddAcademicTerm(TermEntity term)
	//{
	//	ArgumentNullException.ThrowIfNull(term);

	//	if (_academicTerms.Any(x => x.TermId == term.TermId))
	//	{
	//		throw new InvalidOperationException(
	//			"Academic term already belongs to this campus.");
	//	}

	//	_academicTerms.Add(term);

	//	MarkAsUpdated();
	//}

	//public void RemoveAcademicTerm(Guid termId)
	//{
	//	TermEntity term = GetRequired(
	//		_academicTerms,
	//		x => x.TermId == termId,
	//		"Academic term");

	//	_academicTerms.Remove(term);

	//	MarkAsUpdated();
	//}

	//public void AddBranding(CampusBrandingEntity branding)
	//{
	//	ArgumentNullException.ThrowIfNull(branding);

	//	if (_brandings.Any(
	//			x => x.CampusBrandingId == branding.CampusBrandingId))
	//	{
	//		throw new InvalidOperationException(
	//			"Branding already belongs to this campus.");
	//	}

	//	_brandings.Add(branding);

	//	MarkAsUpdated();
	//}

	//public void RemoveBranding(Guid campusBrandingId)
	//{
	//	CampusBrandingEntity branding = GetRequired(
	//		_brandings,
	//		x => x.CampusBrandingId == campusBrandingId,
	//		"Campus branding");

	//	_brandings.Remove(branding);

	//	MarkAsUpdated();
	//}

	public void AddDepartment(DepartmentEntity department)
	{
		ArgumentNullException.ThrowIfNull(department);

		if (_departments.Any(
				x => x.DepartmentId == department.DepartmentId))
		{
			throw new InvalidOperationException(
				"Department already belongs to this campus.");
		}

		_departments.Add(department);

		MarkAsUpdated();
	}

	public void RemoveDepartment(Guid departmentId)
	{
		DepartmentEntity department = GetRequired(
			_departments,
			x => x.DepartmentId == departmentId,
			"Department");

		_departments.Remove(department);

		MarkAsUpdated();
	}

	public void AddTimetable(TimetableEntity timetable)
	{
		ArgumentNullException.ThrowIfNull(timetable);

		if (_timetables.Any(
				x => x.TimetableId == timetable.TimetableId))
		{
			throw new InvalidOperationException(
				"Timetable already belongs to this campus.");
		}

		_timetables.Add(timetable);

		MarkAsUpdated();
	}

	public void RemoveTimetable(Guid timetableId)
	{
		TimetableEntity timetable = GetRequired(
			_timetables,
			x => x.TimetableId == timetableId,
			"Timetable");

		_timetables.Remove(timetable);

		MarkAsUpdated();
	}

	private static TEntity GetRequired<TEntity>(
		IEnumerable<TEntity> entities,
		Func<TEntity, bool> predicate,
		string entityName)
	{
		TEntity? entity = entities.FirstOrDefault(predicate);

		return entity ?? throw new InvalidOperationException(
			$"{entityName} does not belong to this campus.");
	}

	private static string? Clean(string? value)
	{
		return string.IsNullOrWhiteSpace(value)
			? null
			: value.Trim();
	}
}

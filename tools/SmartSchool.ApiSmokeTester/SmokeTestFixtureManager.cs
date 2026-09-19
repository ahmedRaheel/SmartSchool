using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SmartSchool.ApiSmokeTester;

internal sealed class SmokeTestFixtureManager(
    HttpClient httpClient,
    SmokeTestOptions options)
{
    private readonly string _runId =
        $"smoke-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}";

    private string? _bootstrapToken;
    private Guid? _disposableSuperAdminId;
    private SmokeTestFixture? _activeFixture;

    public async Task<SmokeTestFixture> SetupAsync(
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(options.BootstrapSuperAdminEmail)
            || string.IsNullOrWhiteSpace(options.BootstrapSuperAdminPassword))
        {
            throw new InvalidOperationException(
                "Self-contained smoke testing requires the Development bootstrap "
                + "SuperAdmin configuration. The runner reads it automatically from "
                + "SmartSchool.Identity.Api/appsettings.Development.json.");
        }

        Console.WriteLine("Creating disposable smoke-test fixture ...");

        _bootstrapToken = await LoginAsync(
            options.BootstrapSuperAdminEmail,
            options.BootstrapSuperAdminPassword,
            cancellationToken);

        var superAdminEmail =
            $"{_runId}.superadmin@smartschool.test";
        var superAdminPassword = CreatePassword("Super");

        var (userId, _) = await CreateIdentityUserAsync(
            _bootstrapToken,
            tenantId: null,
            schoolId: null,
            branchId: null,
            businessEntityId: null,
            superAdminEmail,
            superAdminPassword,
            "Smoke",
            "SuperAdmin",
            "SuperAdmin",
            ["SuperAdmin"],
            cancellationToken);

        _disposableSuperAdminId = userId;

        var superAdminToken = await LoginAsync(
            superAdminEmail,
            superAdminPassword,
            cancellationToken);

        await ValidateTokenAgainstApiAsync(
            "SuperAdmin",
            superAdminToken,
            cancellationToken);

        var tenantId = Guid.NewGuid();
        var fixture = new SmokeTestFixture
        {
            RunId = _runId,
            TenantId = tenantId
        };
        _activeFixture = fixture;

        fixture.KnownValues["tenantId"] = tenantId.ToString();

        fixture.Actors[SmokeActorKind.SuperAdmin] = new SmokeActor(
            SmokeActorKind.SuperAdmin,
            userId,
            null,
            superAdminEmail,
            superAdminPassword,
            superAdminToken,
            null,
            null,
            null);

        await CreateCoreBusinessFixtureAsync(
            fixture,
            superAdminToken,
            cancellationToken);

        await CreateTenantActorsAsync(
            fixture,
            superAdminToken,
            cancellationToken);

        foreach (var item in fixture.KnownValues)
        {
            options.KnownValues[item.Key] = item.Value;
        }

        Console.WriteLine(
            $"Fixture ready. RunId={fixture.RunId}, Tenant={fixture.TenantId}, "
            + $"School={fixture.SchoolId}, Campus={fixture.CampusId}, "
            + $"Actors={fixture.Actors.Count}");
        Console.WriteLine();

        return fixture;
    }

    public async Task CleanupAsync(
        SmokeTestFixture? fixture,
        CancellationToken cancellationToken)
    {
        fixture ??= _activeFixture;

        if (fixture is null)
        {
            await PurgeDisposableSuperAdminAsync(cancellationToken);
            return;
        }

        if (!options.CleanupFixture)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                $"Fixture cleanup disabled. Tenant {fixture.TenantId} remains for inspection.");
            Console.ResetColor();
            return;
        }

        Console.WriteLine();
        Console.WriteLine(
            $"Cleaning smoke-test fixture {fixture.RunId} ...");

        if (!string.IsNullOrWhiteSpace(_bootstrapToken))
        {
            try
            {
                using var businessCleanup = CreateRequest(
                    HttpMethod.Delete,
                    $"{options.BaseUrl}/api/testing/fixtures/{fixture.TenantId}"
                    + $"?runId={Uri.EscapeDataString(fixture.RunId)}",
                    _bootstrapToken);
                businessCleanup.Headers.TryAddWithoutValidation(
                    "X-Smoke-Test-Run",
                    fixture.RunId);

                using var response = await httpClient.SendAsync(
                    businessCleanup,
                    cancellationToken);

                var body = await response.Content.ReadAsStringAsync(
                    cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(
                        $"Business fixture cleanup returned HTTP {(int)response.StatusCode}: "
                        + Truncate(body, 800));
                    Console.ResetColor();
                }
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(
                    $"Business fixture cleanup failed: {exception.Message}");
                Console.ResetColor();
            }

            try
            {
                using var deleteTenantUsers = CreateRequest(
                    HttpMethod.Delete,
                    $"{options.IdentityBaseUrl}/api/identity/users/tenant/{fixture.TenantId}",
                    _bootstrapToken);

                using var response = await httpClient.SendAsync(
                    deleteTenantUsers,
                    cancellationToken);

                if (!response.IsSuccessStatusCode
                    && response.StatusCode != HttpStatusCode.NotFound)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(
                        $"Tenant-user cleanup returned HTTP {(int)response.StatusCode}.");
                    Console.ResetColor();
                }
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(
                    $"Identity tenant cleanup failed: {exception.Message}");
                Console.ResetColor();
            }
        }

        await PurgeDisposableSuperAdminAsync(cancellationToken);

        Console.WriteLine("Smoke-test fixture cleanup completed.");
    }

    private async Task PurgeDisposableSuperAdminAsync(
        CancellationToken cancellationToken)
    {
        if (!_disposableSuperAdminId.HasValue
            || string.IsNullOrWhiteSpace(_bootstrapToken))
        {
            return;
        }

        try
        {
            using var request = CreateRequest(
                HttpMethod.Delete,
                $"{options.IdentityBaseUrl}/api/identity/users/"
                + $"{_disposableSuperAdminId.Value}/purge",
                _bootstrapToken);

            using var response = await httpClient.SendAsync(
                request,
                cancellationToken);

            if (!response.IsSuccessStatusCode
                && response.StatusCode != HttpStatusCode.NotFound)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(
                    $"Disposable SuperAdmin cleanup returned HTTP {(int)response.StatusCode}.");
                Console.ResetColor();
            }
        }
        catch (Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                $"Disposable SuperAdmin cleanup failed: {exception.Message}");
            Console.ResetColor();
        }
        finally
        {
            _disposableSuperAdminId = null;
        }
    }

    private async Task CreateCoreBusinessFixtureAsync(
        SmokeTestFixture fixture,
        string superAdminToken,
        CancellationToken cancellationToken)
    {
        var academicSystem = await PostApiValueAsync(
            "/api/academics/academic-system",
            superAdminToken,
            new
            {
                tenantId = fixture.TenantId,
                name = "Matric",
                metadataJson =
                    JsonSerializer.Serialize(
                        new
                        {
                            source = "api-smoke",
                            runId = fixture.RunId
                        })
            },
            cancellationToken);

        fixture.AcademicSystemId =
            ReadGuid(academicSystem, "id");
        SetKnown(
            fixture,
            "academicSystemId",
            fixture.AcademicSystemId);

        var school = await PostApiValueAsync(
            "/api/organization/school",
            superAdminToken,
            new
            {
                tenantId = fixture.TenantId,
                name = $"Smoke School {fixture.RunId[^8..]}",
                registrationNumber = $"SMOKE-{fixture.RunId[^8..]}",
                email = $"{fixture.RunId}.school@smartschool.test",
                phone = "+923001111111",
                fax = (string?)null,
                website = "https://example.test",
                address = "Smoke Test Address",
                city = "Karachi",
                province = "Sindh",
                country = "Pakistan",
                logoUrl = (string?)null
            },
            cancellationToken);

        fixture.SchoolId = ReadGuid(school, "id");
        SetKnown(fixture, "schoolId", fixture.SchoolId);

        var genderTypes = await GetApiValueAsync(
            "/api/organization/lookups/branch-gender-types",
            superAdminToken,
            cancellationToken);
        var genderTypeId = ReadGuidByCode(
            genderTypes,
            "CO_EDUCATION",
            "id",
            "code");
        fixture.KnownValues["branchGenderTypeId"] =
            genderTypeId.ToString();

        var educationLevels = await GetApiValueAsync(
            "/api/organization/lookups/education-levels",
            superAdminToken,
            cancellationToken);
        var educationLevelIds =
            ReadGuidArray(educationLevels, "id");

        if (educationLevelIds.Count > 0)
        {
            fixture.KnownValues["educationLevelId"] =
                educationLevelIds[0].ToString();
        }

        var campus = await PostApiValueAsync(
            "/api/organization/campus",
            superAdminToken,
            new
            {
                tenantId = fixture.TenantId,
                schoolId = fixture.SchoolId,
                name = "Smoke Main Campus",
                branchType = "HeadOffice",
                branchGenderTypeId = genderTypeId,
                academicSystemId = fixture.AcademicSystemId,
                educationLevelIds,
                address = "Smoke Campus Address",
                city = "Karachi",
                province = "Sindh",
                country = "Pakistan",
                phone = "+923002222222",
                fax = (string?)null,
                mobile = "+923003333333",
                email = $"{fixture.RunId}.campus@smartschool.test",
                logoUrl = (string?)null
            },
            cancellationToken);

        fixture.CampusId = ReadGuid(campus, "id");
        SetKnown(fixture, "campusId", fixture.CampusId);
        SetKnown(fixture, "branchId", fixture.CampusId);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var startYear =
            today.Month >= 7 ? today.Year : today.Year - 1;
        var startDate = new DateOnly(startYear, 7, 1);
        var endDate = new DateOnly(startYear + 1, 6, 30);

        var academicYear = await PostApiValueAsync(
            "/api/academics/academic-year",
            superAdminToken,
            new
            {
                tenantId = fixture.TenantId,
                campusId = fixture.CampusId,
                name = $"{startYear}-{startYear + 1}",
                startDate = startDate.ToString("yyyy-MM-dd"),
                endDate = endDate.ToString("yyyy-MM-dd"),
                isCurrent = true
            },
            cancellationToken);

        fixture.AcademicYearId =
            ReadGuid(academicYear, "id");
        SetKnown(
            fixture,
            "academicYearId",
            fixture.AcademicYearId);

        if (educationLevelIds.Count == 0)
        {
            throw new InvalidOperationException(
                "No active education levels are available for the smoke fixture.");
        }

        // Create one explicit academic class/grade with the campus education level.
        // This keeps Student admission validation satisfied in the current domain
        // model while the rest of the automatically generated campus grades remain
        // available to the general endpoint suite.
        var gradeLevel = await PostApiValueAsync(
            "/api/academics/grade-level",
            superAdminToken,
            new
            {
                tenantId = fixture.TenantId,
                campusId = fixture.CampusId,
                academicSystemId = fixture.AcademicSystemId,
                name = "Grade I Smoke",
                sortOrder = 1,
                educationLevelId = educationLevelIds[0],
                academicYearId = fixture.AcademicYearId,
                sections = (object?)null
            },
            cancellationToken);

        var gradeLevelId = ReadGuid(gradeLevel, "id");
        fixture.GradeLevelId = gradeLevelId;
        fixture.KnownValues["gradeLevelId"] =
            gradeLevelId.ToString();
        // Several legacy routes still call the academic grade/class identifier classId.
        fixture.KnownValues["classId"] =
            gradeLevelId.ToString();

        var classSection = await PostApiValueAsync(
            "/api/academics/class-section",
            superAdminToken,
            new
            {
                tenantId = fixture.TenantId,
                campusId = fixture.CampusId,
                academicYearId = fixture.AcademicYearId,
                gradeLevelId,
                name = "A",
                capacity = 30,
                roomNo = "SMOKE-101"
            },
            cancellationToken);

        fixture.ClassSectionId =
            ReadGuid(classSection, "id");
        SetKnown(
            fixture,
            "classSectionId",
            fixture.ClassSectionId);
    }

    private async Task CreateTenantActorsAsync(
        SmokeTestFixture fixture,
        string superAdminToken,
        CancellationToken cancellationToken)
    {
        // Owner/Tenant is an identity-level actor rather than an HR employee.
        await CreateManualActorAsync(
            fixture,
            superAdminToken,
            SmokeActorKind.Owner,
            role: "Tenant",
            accountType: "Owner",
            businessEntityId: null,
            cancellationToken);

        await CreateStudentAndParentActorsAsync(
            fixture,
            superAdminToken,
            cancellationToken);

        var staffDefinitions = new[]
        {
            new StaffActorDefinition(
                SmokeActorKind.Admin,
                "AdminOfficer"),
            new StaffActorDefinition(
                SmokeActorKind.Principal,
                "Principal"),
            new StaffActorDefinition(
                SmokeActorKind.Teacher,
                "Teacher"),
            new StaffActorDefinition(
                SmokeActorKind.Driver,
                "Driver"),
            new StaffActorDefinition(
                SmokeActorKind.Accountant,
                "Accountant"),
            new StaffActorDefinition(
                SmokeActorKind.HRManager,
                "HrManager"),
            new StaffActorDefinition(
                SmokeActorKind.Librarian,
                "Librarian"),
            new StaffActorDefinition(
                SmokeActorKind.Examiner,
                "Examiner")
        };

        var staffIndex = 1;
        foreach (var definition in staffDefinitions)
        {
            try
            {
                await CreateEmployeeActorAsync(
                    fixture,
                    superAdminToken,
                    definition,
                    staffIndex++,
                    cancellationToken);
            }
            catch (Exception exception)
            {
                // Keep the smoke suite operational even if employee provisioning
                // itself is one of the defects being investigated. The fallback
                // still gives the role a real Identity user/token; its business
                // id remains deterministic for request generation.
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(
                    $"Employee-backed {definition.Kind} fixture failed; "
                    + $"falling back to identity-only actor. {exception.Message}");
                Console.ResetColor();

                fixture.ProvisioningWarnings.Add(
                    $"{definition.Kind}: employee-backed provisioning failed; "
                    + "the fallback Identity actor has no fabricated business ID. "
                    + exception.Message);

                await CreateManualActorAsync(
                    fixture,
                    superAdminToken,
                    definition.Kind,
                    RoleFor(definition.Kind),
                    definition.Kind.ToString(),
                    businessEntityId: null,
                    cancellationToken);
            }
        }
    }

    private async Task CreateStudentAndParentActorsAsync(
        SmokeTestFixture fixture,
        string superAdminToken,
        CancellationToken cancellationToken)
    {
        if (!fixture.SchoolId.HasValue
            || !fixture.CampusId.HasValue
            || !fixture.AcademicYearId.HasValue
            || !fixture.ClassSectionId.HasValue)
        {
            throw new InvalidOperationException(
                "The core school fixture must exist before actor provisioning.");
        }

        var student = await PostApiValueAsync(
            "/api/students/student",
            superAdminToken,
            new
            {
                tenantId = fixture.TenantId,
                schoolId = fixture.SchoolId,
                branchId = fixture.CampusId,
                academicYearId = fixture.AcademicYearId,
                classSectionId = fixture.ClassSectionId,
                userId = (Guid?)null,
                firstName = "Smoke",
                lastName = "Student",
                dateOfBirth = "2015-01-15",
                gender = "Male",
                photo = (byte[]?)null,
                photoContentType = (string?)null,
                photoFileName = (string?)null,
                admissionDate =
                    DateOnly.FromDateTime(DateTime.UtcNow)
                        .ToString("yyyy-MM-dd")
            },
            cancellationToken);

        var studentId = ReadGuid(student, "id");
        fixture.KnownValues["studentId"] = studentId.ToString();

        var guardian = await PostApiValueAsync(
            "/api/students/guardian",
            superAdminToken,
            new
            {
                tenantId = fixture.TenantId,
                userId = (Guid?)null,
                fullName = "Smoke Parent",
                cnicNumber = CreateCnic(90),
                email = $"{_runId}.parent.profile@smartschool.test",
                phone = "+923004444444"
            },
            cancellationToken);

        var guardianId = ReadGuid(guardian, "id");
        fixture.KnownValues["guardianId"] = guardianId.ToString();
        fixture.KnownValues["parentId"] = guardianId.ToString();

        await PostApiValueAsync(
            "/api/students/student-guardian/link",
            superAdminToken,
            new
            {
                tenantId = fixture.TenantId,
                studentId,
                guardianId,
                relationship = "Father",
                isPrimary = true
            },
            cancellationToken);

        await CreateManualActorAsync(
            fixture,
            superAdminToken,
            SmokeActorKind.Student,
            role: "Student",
            accountType: "Student",
            businessEntityId: studentId,
            cancellationToken);

        await CreateManualActorAsync(
            fixture,
            superAdminToken,
            SmokeActorKind.Parent,
            role: "Parent",
            accountType: "Parent",
            businessEntityId: guardianId,
            cancellationToken);
    }

    private async Task CreateEmployeeActorAsync(
        SmokeTestFixture fixture,
        string superAdminToken,
        StaffActorDefinition definition,
        int index,
        CancellationToken cancellationToken)
    {
        if (!fixture.SchoolId.HasValue
            || !fixture.CampusId.HasValue)
        {
            throw new InvalidOperationException(
                "School and campus are required before employee actor provisioning.");
        }

        var email =
            $"{_runId}.{definition.Kind.ToString().ToLowerInvariant()}"
            + "@smartschool.test";

        var employee = await PostApiValueAsync(
            "/api/hr/employee",
            superAdminToken,
            new
            {
                tenantId = fixture.TenantId,
                schoolId = fixture.SchoolId,
                branchId = fixture.CampusId,
                departmentId = (Guid?)null,
                userId = (Guid?)null,
                firstName = "Smoke",
                lastName = definition.Kind.ToString(),
                cnicNumber = CreateCnic(index),
                dateOfBirth = "1990-01-01",
                gender = "Male",
                jobTitle = definition.Kind.ToString(),
                photo = (byte[]?)null,
                photoContentType = (string?)null,
                photoFileName = (string?)null,
                email,
                phone = $"+92310{index:0000000}",
                alternatePhone = (string?)null,
                address = "Smoke Test Address",
                emergencyContactName = "Smoke Emergency",
                emergencyContactPhone = "+923009999999",
                hireDate =
                    DateOnly.FromDateTime(DateTime.UtcNow)
                        .ToString("yyyy-MM-dd"),
                employmentTypeCode = "FULL_TIME",
                staffType = definition.Designation,
                sourceCandidateId = (Guid?)null
            },
            cancellationToken);

        var employeeId = ReadGuid(employee, "id");
        var loginAccount =
            GetPropertyIgnoreCase(employee, "loginAccount");
        var userId = ReadGuid(loginAccount, "userId");
        var accountEmail =
            ReadString(loginAccount, "email", "Email") ?? email;
        var temporaryPassword =
            ReadString(
                loginAccount,
                "temporaryPassword",
                "TemporaryPassword")
            ?? string.Empty;

        var actorToken = await StartImpersonationAsync(
            superAdminToken,
            userId,
            cancellationToken);

        var actor = new SmokeActor(
            definition.Kind,
            userId,
            employeeId,
            accountEmail,
            temporaryPassword,
            actorToken,
            fixture.TenantId,
            fixture.SchoolId,
            fixture.CampusId);

        fixture.Actors[definition.Kind] = actor;
        AddActorKnownValues(fixture, actor);

        if (definition.Kind == SmokeActorKind.Teacher)
        {
            fixture.KnownValues["teacherId"] =
                employeeId.ToString();
            fixture.KnownValues["employeeId"] =
                employeeId.ToString();
        }
        else if (definition.Kind == SmokeActorKind.Driver)
        {
            fixture.KnownValues["driverId"] =
                employeeId.ToString();
        }
        else if (definition.Kind == SmokeActorKind.Examiner)
        {
            fixture.KnownValues["examinerId"] =
                employeeId.ToString();
        }
    }

    private async Task CreateManualActorAsync(
        SmokeTestFixture fixture,
        string superAdminToken,
        SmokeActorKind kind,
        string role,
        string accountType,
        Guid? businessEntityId,
        CancellationToken cancellationToken)
    {
        var email =
            $"{_runId}.{kind.ToString().ToLowerInvariant()}"
            + "@smartschool.test";
        var password = CreatePassword(kind.ToString());

        var (actorUserId , _) = await CreateIdentityUserAsync(
            superAdminToken,
            fixture.TenantId,
            fixture.SchoolId,
            fixture.CampusId,
            businessEntityId,
            email,
            password,
            "Smoke",
            kind.ToString(),
            accountType,
            [role],
            cancellationToken);

        var actorToken = await StartImpersonationAsync(
            superAdminToken,
             actorUserId ,
            cancellationToken);

        var actor = new SmokeActor(
            kind,
            actorUserId,
            businessEntityId,
            email,
            password,
            actorToken,
            fixture.TenantId,
            fixture.SchoolId,
            fixture.CampusId);

        fixture.Actors[kind] = actor;
        AddActorKnownValues(fixture, actor);
    }

    private static void AddActorKnownValues(
        SmokeTestFixture fixture,
        SmokeActor actor)
    {
        var actorName = actor.Kind.ToString();
        fixture.KnownValues[
            $"{char.ToLowerInvariant(actorName[0])}{actorName[1..]}UserId"] =
            actor.UserId.ToString();

        if (!actor.BusinessEntityId.HasValue)
        {
            return;
        }

        var businessId = actor.BusinessEntityId.Value.ToString();

        switch (actor.Kind)
        {
            case SmokeActorKind.Student:
                fixture.KnownValues["studentId"] = businessId;
                break;
            case SmokeActorKind.Parent:
                fixture.KnownValues["guardianId"] = businessId;
                fixture.KnownValues["parentId"] = businessId;
                break;
            case SmokeActorKind.Teacher:
                fixture.KnownValues["teacherId"] = businessId;
                fixture.KnownValues["employeeId"] = businessId;
                break;
            case SmokeActorKind.Driver:
                fixture.KnownValues["driverId"] = businessId;
                break;
            case SmokeActorKind.Examiner:
                fixture.KnownValues["examinerId"] = businessId;
                break;
            case SmokeActorKind.SuperAdmin:
                break;
            case SmokeActorKind.Owner:
                break;
            case SmokeActorKind.Admin:
                break;
            case SmokeActorKind.Principal:
                break;
            case SmokeActorKind.Accountant:
                break;
            case SmokeActorKind.HRManager:
                break;
            case SmokeActorKind.Librarian:
                break;
            default:
                break;
        }
    }

    private static string RoleFor(SmokeActorKind kind) =>
        kind switch
        {
            SmokeActorKind.Owner => "Tenant",
            SmokeActorKind.Admin => "Admin",
            SmokeActorKind.Principal => "Principal",
            SmokeActorKind.Teacher => "Teacher",
            SmokeActorKind.Student => "Student",
            SmokeActorKind.Parent => "Parent",
            SmokeActorKind.Driver => "Driver",
            SmokeActorKind.Accountant => "Accountant",
            SmokeActorKind.HRManager => "HRManager",
            SmokeActorKind.Librarian => "Librarian",
            SmokeActorKind.Examiner => "Examiner",
            SmokeActorKind.SuperAdmin => "SuperAdmin",
            _ => throw new ArgumentOutOfRangeException(
                nameof(kind),
                kind,
                "Unsupported smoke actor.")
        };

    private static string CreateCnic(int index) =>
        $"42101{index:00000000}";

    private async Task<(Guid UserId, string Password)> CreateIdentityUserAsync(
        string bearerToken,
        Guid? tenantId,
        Guid? schoolId,
        Guid? branchId,
        Guid? businessEntityId,
        string email,
        string password,
        string firstName,
        string lastName,
        string accountType,
        string[] roles,
        CancellationToken cancellationToken)
    {
        using var request = CreateRequest(
            HttpMethod.Post,
            $"{options.IdentityBaseUrl}/api/identity/users",
            bearerToken);

        request.Content = JsonContent.Create(
            new
            {
                tenantId,
                schoolId,
                branchId,
                businessEntityId,
                email,
                password,
                firstName,
                lastName,
                accountType,
                roles
            });

        using var response = await httpClient.SendAsync(
            request,
            cancellationToken);
        var body = await response.Content.ReadAsStringAsync(
            cancellationToken);

        EnsureSuccess(
            response,
            body,
            $"create identity user {email}");

        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;
        var user = GetPropertyIgnoreCase(root, "user");

        return (
            ReadGuid(user, "id"),
            password);
    }

    private async Task<string> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(options.LoginClientSecret))
        {
            throw new InvalidOperationException(
                "LoginApiClient:ClientSecret is required for self-contained smoke testing.");
        }

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{options.IdentityBaseUrl}/connect/token")
        {
            Content = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["grant_type"] = "password",
                    ["client_id"] = options.LoginClientId,
                    ["client_secret"] = options.LoginClientSecret,
                    ["username"] = email,
                    ["password"] = password,
                    // Deliberately omit offline_access so the disposable fixture
                    // creates no refresh-token grants that need later cleanup.
                    ["scope"] = "openid profile email smartschool.api offline_access"
                })
        };

        using var response = await httpClient.SendAsync(
            request,
            cancellationToken);

        var body = await response.Content.ReadAsStringAsync(
            cancellationToken);

        EnsureSuccess(response, body, $"login {email}");

        using var document = JsonDocument.Parse(body);
        var token = ReadString(
            document.RootElement,
            "access_token",
            "accessToken",
            "AccessToken");

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException(
                $"Token response for {email} did not contain an access token.");
        }

        return token;
    }

    private async Task<string> StartImpersonationAsync(
        string superAdminToken,
        Guid targetUserId,
        CancellationToken cancellationToken)
    {
        using var request = CreateRequest(
            HttpMethod.Post,
            $"{options.IdentityBaseUrl}/api/identity/users/impersonation/start",
            superAdminToken);

        request.Content = JsonContent.Create(
            new
            {
                targetUserId,
                reason = $"Automated API smoke test {_runId}"
            });

        using var response = await httpClient.SendAsync(
            request,
            cancellationToken);
        var body = await response.Content.ReadAsStringAsync(
            cancellationToken);

        EnsureSuccess(
            response,
            body,
            $"impersonate smoke actor {targetUserId}");

        using var document = JsonDocument.Parse(body);
        var token = ReadString(
            document.RootElement,
            "accessToken",
            "AccessToken");

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException(
                $"Impersonation response for {targetUserId} did not contain an access token.");
        }

        await ValidateTokenAgainstApiAsync(
            $"impersonated user {targetUserId}",
            token,
            cancellationToken);

        return token;
    }

    private async Task ValidateTokenAgainstApiAsync(
        string actorName,
        string token,
        CancellationToken cancellationToken)
    {
        using var request = CreateRequest(
            HttpMethod.Get,
            $"{options.BaseUrl}/api/testing/auth-probe",
            token);
        request.Headers.TryAddWithoutValidation(
            "X-Smoke-Test-Run",
            _runId);

        using var response = await httpClient.SendAsync(
            request,
            cancellationToken);
        var body = await response.Content.ReadAsStringAsync(
            cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var authenticate = response.Headers.WwwAuthenticate.Count > 0
            ? string.Join(", ", response.Headers.WwwAuthenticate.Select(value => value.ToString()))
            : "<none>";

        throw new InvalidOperationException(
            $"Smoke-test authentication preflight failed for {actorName}. "
            + $"SmartSchool.Api returned HTTP {(int)response.StatusCode}. "
            + $"WWW-Authenticate={authenticate}. "
            + $"Token={DescribeToken(token)}. "
            + $"Response={Truncate(body, 1200)}. "
            + "The endpoint scan was not started because every result would be misleading 401/403 noise.");
    }

    private static string DescribeToken(string token)
    {
        try
        {
            var parts = token.Split('.');
            if (parts.Length < 2)
            {
                return "opaque-or-malformed";
            }

            var payload = parts[1]
                .Replace('-', '+')
                .Replace('_', '/');
            payload = payload.PadRight(
                payload.Length + ((4 - payload.Length % 4) % 4),
                '=');

            using var document = JsonDocument.Parse(
                Convert.FromBase64String(payload));
            var root = document.RootElement;

            static string Read(JsonElement element, string name)
            {
                if (!element.TryGetProperty(name, out var value))
                {
                    return "<missing>";
                }

                return value.ValueKind switch
                {
                    JsonValueKind.Array =>
                        string.Join(",", value.EnumerateArray().Select(item => item.ToString())),
                    _ => value.ToString()
                };
            }

            return $"iss={Read(root, "iss")}; aud={Read(root, "aud")}; "
                + $"scope={Read(root, "scope")}; sub={Read(root, "sub")}; "
                + $"exp={Read(root, "exp")}";
        }
        catch (Exception exception)
        {
            return $"unable-to-decode ({exception.GetType().Name})";
        }
    }

    private async Task<JsonElement> PostApiValueAsync(
        string path,
        string token,
        object body,
        CancellationToken cancellationToken)
    {
        using var request = CreateRequest(
            HttpMethod.Post,
            options.BaseUrl + path,
            token);
        request.Headers.TryAddWithoutValidation(
            "X-Smoke-Test-Run",
            _runId);
        request.Content = JsonContent.Create(body);

        using var response = await httpClient.SendAsync(
            request,
            cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(
            cancellationToken);

        EnsureSuccess(
            response,
            responseBody,
            $"create fixture through {path}");

        using var document = JsonDocument.Parse(responseBody);
        var value = TryGetPropertyIgnoreCase(
            document.RootElement,
            "value",
            out var wrapped)
            ? wrapped
            : document.RootElement;

        return value.Clone();
    }

    private async Task<JsonElement> GetApiValueAsync(
        string path,
        string token,
        CancellationToken cancellationToken)
    {
        using var request = CreateRequest(
            HttpMethod.Get,
            options.BaseUrl + path,
            token);
        request.Headers.TryAddWithoutValidation(
            "X-Smoke-Test-Run",
            _runId);

        using var response = await httpClient.SendAsync(
            request,
            cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(
            cancellationToken);

        EnsureSuccess(
            response,
            responseBody,
            $"read fixture data through {path}");

        using var document = JsonDocument.Parse(responseBody);
        var value = TryGetPropertyIgnoreCase(
            document.RootElement,
            "value",
            out var wrapped)
            ? wrapped
            : document.RootElement;

        return value.Clone();
    }

    private static HttpRequestMessage CreateRequest(
        HttpMethod method,
        string url,
        string token)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        request.Headers.TryAddWithoutValidation(
            "X-Smoke-Test",
            "true");
        return request;
    }

    private static void EnsureSuccess(
        HttpResponseMessage response,
        string body,
        string operation)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        throw new InvalidOperationException(
            $"Unable to {operation}. HTTP {(int)response.StatusCode}. "
            + Truncate(body, 1500));
    }

    private static JsonElement GetPropertyIgnoreCase(
        JsonElement element,
        string name)
    {
        if (TryGetPropertyIgnoreCase(element, name, out var value))
        {
            return value;
        }

        throw new InvalidOperationException(
            $"JSON property '{name}' was not found.");
    }

    private static bool TryGetPropertyIgnoreCase(
        JsonElement element,
        string name,
        out JsonElement value)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                if (property.Name.Equals(
                        name,
                        StringComparison.OrdinalIgnoreCase))
                {
                    value = property.Value;
                    return true;
                }
            }
        }

        value = default;
        return false;
    }

    private static string? ReadString(
        JsonElement element,
        params string[] names)
    {
        foreach (var name in names)
        {
            if (TryGetPropertyIgnoreCase(
                    element,
                    name,
                    out var value)
                && value.ValueKind == JsonValueKind.String)
            {
                return value.GetString();
            }
        }

        return null;
    }

    private static Guid ReadGuid(
        JsonElement element,
        string name)
    {
        var value = GetPropertyIgnoreCase(element, name);
        if (value.ValueKind == JsonValueKind.String
            && Guid.TryParse(value.GetString(), out var guid))
        {
            return guid;
        }

        throw new InvalidOperationException(
            $"JSON property '{name}' is not a GUID.");
    }

    private static Guid ReadGuidByCode(
        JsonElement array,
        string code,
        string idPropertyName,
        string codePropertyName)
    {
        if (array.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException(
                "Expected an array in fixture lookup response.");
        }

        foreach (var item in array.EnumerateArray())
        {
            var itemCode =
                ReadString(item, codePropertyName, "Code");

            if (string.Equals(
                    itemCode,
                    code,
                    StringComparison.OrdinalIgnoreCase))
            {
                return ReadGuid(item, idPropertyName);
            }
        }

        return ReadFirstGuid(array, idPropertyName);
    }

    private static Guid ReadFirstGuid(
        JsonElement array,
        string propertyName)
    {
        if (array.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException(
                "Expected an array in fixture lookup response.");
        }

        foreach (var item in array.EnumerateArray())
        {
            return ReadGuid(item, propertyName);
        }

        throw new InvalidOperationException(
            "Fixture lookup returned no values.");
    }

    private static IReadOnlyList<Guid> ReadGuidArray(
        JsonElement array,
        string propertyName)
    {
        if (array.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        var result = new List<Guid>();
        foreach (var item in array.EnumerateArray())
        {
            result.Add(ReadGuid(item, propertyName));
        }

        return result;
    }

    private static void SetKnown(
        SmokeTestFixture fixture,
        string name,
        Guid? value)
    {
        if (value.HasValue)
        {
            fixture.KnownValues[name] = value.Value.ToString();
        }
    }

    private static string CreatePassword(string prefix) =>
        $"Ss!{prefix}{Guid.NewGuid():N}9aA";

    private static string Truncate(
        string value,
        int maximumLength) =>
        value.Length <= maximumLength
            ? value
            : value[..maximumLength] + "... [truncated]";

    private sealed record StaffActorDefinition(
        SmokeActorKind Kind,
        string Designation);
}

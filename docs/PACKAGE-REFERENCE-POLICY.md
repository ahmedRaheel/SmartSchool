# Package reference policy

SmartSchool uses Central Package Management.

- `Directory.Packages.props` owns package versions.
- Individual `.csproj` files declare each required `PackageReference` exactly once.
- Do not duplicate package references in the same project.
- Do not put `Version` attributes on project-level `PackageReference` items.
- Shared packages should not be injected again by project-generation scripts if already present.

The v15 cleanup removed duplicate package references and verified that no project
contains the same `PackageReference Include` more than once.

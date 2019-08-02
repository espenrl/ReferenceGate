# Reference Gate
### - for MSBuild and Common Project System (CPS) based projects

> A reference not approved should end in a build error.

Ever discovered a reference that really shouldn't be in the project. It is there because some code needs a single method call in another assembly - and it really isn't neccesary. Tools like ReSharper makes it far to easy to add an external reference by accident. Reference Gate will make the build fail in cases like this.

> You as a developer will need to maintain a whitelist.

[NuGet package](https://www.nuget.org/packages/refGate)

## Examples

##### Assembly references

Guarding the `Reference` variable.

``` xml
<AssemblyReferenceWhitelist Include="System.Linq" />
<AssemblyReferenceBlacklist Include="System.Configuration" />
```

##### Package references

Guarding the `PackageReference` variable.

``` xml
<PackageReferenceWhitelist Include="NUnit" />
<PackageReferenceBlacklist Include="Newtonsoft.Json" />
```

##### Project references

Guarding the `ProjectReference` variable.

``` xml
<ProjectReferenceWhitelist Include="..\Newtonsoft.Json\Newtonsoft.Json.csproj" />
<ProjectReferenceBlacklist Include="..\Newtonsoft.Json.Tests\Newtonsoft.Json.Tests.csproj" />
```

## Limitations
* Both blacklisting and whitelisting at the same time is not supported. One method has to be chosen. This applies per project, per type\*.
* The comparison of references is based on case sensitive string compare. This may pose limits for file paths in assembly references and project references.

\* Type being either assembly, package or project reference.
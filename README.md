# Device Metadata Tools
> Maintained at <https://github.com/sensics/DeviceMetadataTools>

Assemblies and command-line tools to interact with Microsoft "Device Metadata" packages. The assemblies and tools are written in C# and currently target the .NET 4 Client Profile, with the projects/solution created in Visual Studio 2015.

Includes assemblies for loading files from `.cab` files (which is secretly what those device metadata packages are internally), using one of two different backends:

- `Shell32` - shell functions p/invoked from Windows, used by default
- `DTF` - more featureful in theory, but not used by default due to licensing: uses the Microsoft/OuterCurve/WiX "[Deployment Tools Foundation][dtf-history-post]" which is provided under the [Microsoft Reciprocal License (MS-RL)][wix-ms-rl]

## License

This project: Licensed under the Apache License, Version 2.0.

**Note:** The `Sensics.CabTools.DTFCabFile` assembly (not built by default) uses the `Microsoft.Deployment.Compression` and `Microsoft.Deployment.Compression.Cab` assemblies, which are part of MS DTF, part of the WiX toolkit, and licensed under the [MS-RL][wix-ms-rl]. Building/using that assembly may impose additional conditions beyond those of the Apache License, Version 2.0, which are your responsibility to comply with - consult a lawyer if you have questions.

[dtf-history-post]: http://blogs.msdn.com/b/jasongin/archive/2008/05/16/a-brief-history-of-the-deployment-tools-foundation-project.aspx
[wix-ms-rl]: http://wixtoolset.org/about/license/
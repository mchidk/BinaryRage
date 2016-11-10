// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"

open System
open System.IO
open Fake

RestorePackages()

// Directories
let buildDir  = @".\build\"
let testDir   = @".\test\"
let deployDir = @".\deploy\"

// Properties
let buildNumber = getBuildParamOrDefault "buildNumber" "0"
let releaseNotes = 
    ReadFile "ReleaseNotes.md"
    |> ReleaseNotesHelper.parseReleaseNotes
let projectName = "BinaryRage-RaySearch"
let version = releaseNotes.AssemblyVersion + "." + buildNumber

// Targets
Target "Clean" (fun _ ->
    trace " --- Cleaning directories --- "

    CleanDirs [buildDir; testDir; deployDir]
)

open Fake.AssemblyInfoFile

Target "SetAssemblyInfo" (fun _ ->
    trace " --- Setting assembly version information --- "

    CreateCSharpAssemblyInfo "./Properties/AssemblyVersionInfo.cs"
        [Attribute.Version version
         Attribute.InformationalVersion version
         Attribute.FileVersion version]
)

Target "BuildBinaryRage" (fun _ ->
    trace " --- Building BinaryRage --- "

    !! @".\BinaryRage.csproj"
        |> MSBuildRelease buildDir "Build"
        |> Log "AppBuild-Output: "

    trace " --- Fininshed building BinaryRage --- "
)

Target "BuildTests" (fun _ ->
    !! @".\BinaryRage.UnitTests\BinaryRage.FunctionalTests.csproj"
        |> MSBuildDebug testDir "Build"
        |> Log "TestBuild-Output: "
)

Target "RunTests" (fun _ ->
    !! (testDir + @"\BinaryRage.UnitTests.dll")
      |> NUnit (fun p ->
                 {p with
                   DisableShadowCopy = true;
                   OutputFile = testDir + @"TestResults.xml"})
)

Target "Deploy" (fun _ ->
    CopyDir deployDir buildDir allFiles
)

Target "CreateNugetPackage" (fun _ ->
    NuGet (fun p ->
        {p with
            Project = projectName
            Version = version
            OutputPath = deployDir
            WorkingDir = buildDir
            NoPackageAnalysis = false
            Publish = false
            ToolPath = @".\tools\nuget\nuget.exe"
        })
        (@".\BinaryRage.nuspec")
)

Target "Default" DoNothing

// Dependencies
"Clean" 
  ==> "SetAssemblyInfo"
  ==> "BuildBinaryRage"
  ==> "BuildTests"
  ==> "RunTests"
  ==> "Deploy"
  ==> "CreateNugetPackage"
  ==> "Default"

// start build
RunTargetOrDefault "Default"

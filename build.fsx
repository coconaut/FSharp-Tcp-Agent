#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open System
open System.IO
open System.Collections.Generic

trace "About to build!!!"

// ___Config___
let buildDir = "./_build"

// ___Targets___
Target "Build" (fun _->
    trace "starting TcpAgentRunner"
    !! "src/TcpAgentRunner/*.fsproj"
        |> MSBuildRelease buildDir "Build"
        |> Log "TcpAgentRunner-Build-Output: "
    
)

Target "Deploy" (fun _ ->
    trace "deploy..."
)


// ___Deps chain___
"Build"
    ==> "Deploy"


// ___Run___
RunTargetOrDefault "Deploy"


Console.ReadKey() |> ignore
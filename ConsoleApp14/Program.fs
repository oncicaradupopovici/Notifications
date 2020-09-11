// Learn more about F# at http://fsharp.org

open System
open Core
open NBB.Core.Effects.FSharp


let main' = 
    
    let angajat = Sql.from "table" "column" "string" |> cache |> trace
    let marca = Rest.from "www.google.com" "POST" "id" "int" |> cache |> trace
    
    let template = "Salut {angajat}, Bine ai venit in companie, marca ta este {marca}."
    //let templateFn = parse template
    let templateFn = sprintf "Salut %s, Bine ai venit in companie, marca ta este %i."
    let liftedTemplateFn = lift2 templateFn
    let resultToken = liftedTemplateFn angajat marca

    let ctx = Ctx 1666
    resultToken ctx

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    let interpreter = Interpreter.createInterpreter()
    let x = 
        main' 
        |> Effect.interpret interpreter
        |> Async.RunSynchronously
    printfn "%s" x
    0 // return an integer exit code

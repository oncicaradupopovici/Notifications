module Core

open NBB.Core.Effects.FSharp

type Token<'a> = Ctx -> Effect<'a>
    and Ctx = Ctx of int

[<AutoOpen>]
module Token =
    let map (func: 'a -> 'b) (eff: Token<'a>) : Token<'b> = 
        fun ctx ->
            eff ctx |> Effect.map func

    let bind (func: 'a -> Token<'b>) (eff: Token<'a>): Token<'b> = 
        fun ctx ->
            eff ctx |> Effect.bind (fun a -> func a ctx)

    let apply (func:Token<'a->'b>) (eff:Token<'a>) = 
        bind (fun a -> func |> map (fun fn -> fn a)) eff

    let return' (x:'a): Token<'a> =
        fun _ctx -> Effect.return' x

    let composeK f g x = bind g (f x)
    
    let lift2 f = map f >> apply
    
    let flatten eff = bind id eff

module Sql = 
    let from<'a> table column type' = 
        fun ctx -> effect { return "Radu Popovici" }

module Rest = 
    let from<'a> url method parameter type' = 
        fun ctx -> effect { return 253 }

let cache token  = token

let trace token = 
    fun (Ctx id) -> 
    effect{
        printfn "some tracing"
        return! token (Ctx id)
    }







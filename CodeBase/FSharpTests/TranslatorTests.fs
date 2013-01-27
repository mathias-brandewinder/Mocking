namespace FSharpTests

open CodeBase
open NUnit.Framework
open FsUnit

[<TestFixture>]
type ``Translator tests``() = 

    [<Test>]
    member test.``Translate should return successful service response`` () =
        let service = {
            new IService with
                member this.Translate(input) =
                    match input with
                    | "Hello" -> "Kitty"
                    | _       -> failwith "ooops" }

        let logger = {
            new ILogger with
                member this.Log(_) = ignore () }

        let translator = Translator(logger, service)
        let input, output = "Hello", "Kitty"
        translator.Translate(input) |> should equal output

    [<Test>]
    member test.``When service fails Translate should return error message`` () =
        
        let service = {
            new IService with
                member this.Translate(_) = failwith "ooops" }

        let logger = {
            new ILogger with
                member this.Log(_) = ignore () }

        let translator = Translator(logger, service)
        
        translator.Translate("Hello") |> should equal Translator.ErrorMessage

    [<Test>]
    member test.``When service fails exception should be logged`` () =
        
        let error = System.Exception()
        let service = {
            new IService with
                member this.Translate(_) = raise error }
        
        let logged = ref false
        let logger = {
            new ILogger with
                member this.Log(e) = 
                    match e with
                    | error -> 
                        logged := true 
                        ignore ()
                    | _ -> ignore () }

        let translator = Translator(logger, service)
        translator.Translate("Hello") |> ignore

        logged.Value |> should equal true
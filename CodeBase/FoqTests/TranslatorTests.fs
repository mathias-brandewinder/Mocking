namespace FoqTests

open System
open CodeBase
open NUnit.Framework
open FsUnit
open Foq

[<TestFixture>]
type ``Translator tests``() = 

    [<Test>]
    member test.``Translate should return successful service response`` () = 

        let logger = Mock<ILogger>().Create()

        let input, output = "Hello", "Kitty"
        let service = Mock<IService>()
                         .Setup(fun me -> <@ me.Translate(input) @>).Returns(output)
                         .Create()

        let translator = Translator(logger, service)

        translator.Translate(input) |> should equal output

    [<Test>]
    member test.``When service fails Translate should return error message`` () =
        
        let logger = Mock<ILogger>().Create()

        let error = Exception()
        let service = Mock<IService>()
                         .Setup(fun me -> <@ me.Translate(any()) @> ).Raises(error)
                         .Create()

        let translator = Translator(logger, service)
        
        translator.Translate("Hello") |> should equal Translator.ErrorMessage

    [<Test>]
    member test.``When service fails exception should be logged`` () = 

        let error = Exception()
        let logged = ref false
        let logger = Mock<ILogger>()
                        .Setup(fun log -> <@ log.Log(error) @>).Calls<Exception>(fun (_) -> logged := true)
                        .Create()

        let service = Mock<IService>()
                         .Setup(fun me -> <@ me.Translate(any()) @> ).Raises(error)
                         .Create()

        let translator = Translator(logger, service)

        translator.Translate("Hello") |> ignore

        logged.Value |> should equal true
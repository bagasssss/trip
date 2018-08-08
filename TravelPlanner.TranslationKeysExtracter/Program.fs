// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.IO
open System.Text
open System.Text.RegularExpressions
open System.Web.Script.Serialization
open System.Collections.Generic

[<EntryPoint>]
let main argv = 
    let appFolderPath = @"C:\Users\nii\Documents\Visual Studio 2017\Projects\TravelPlanner\TravelPlanner.Web\ClientApp\app\"
    let translationFile = @"C:\Users\nii\Documents\Visual Studio 2017\Projects\TravelPlanner\TravelPlanner.Web\wwwroot\en.json";
    let localizeRegexPattern = @"(?<=\blocalize="")[^""]*"

    let getTranslationsFromFile = 
        translationFile
        |> File.ReadAllText
        |> JavaScriptSerializer().Deserialize<Dictionary<string, string>>

    let addTranslationKeys (translations: Dictionary<string, string>) keys = 
        keys
        |> Array.iter(fun k -> if not (translations.ContainsKey k) then translations.Add(k, ""))
        translations

    let writeTranslationsToFile translations  = 
        File.WriteAllText(translationFile, JavaScriptSerializer().Serialize translations)

    let getHtmlFiles = Directory.GetFiles (appFolderPath, "*.html", SearchOption.AllDirectories)

    let regexMatches pattern (input: string)  =
        Regex.Matches(input, pattern)
        |> Seq.cast<Match>
        |> Seq.map(fun (m: Match) -> m.Value)
        |> Seq.toArray
    
    let execute = 
        getHtmlFiles
        |> Array.iter(fun f -> 
            f
            |> File.ReadAllText
            |> regexMatches localizeRegexPattern
            |> addTranslationKeys getTranslationsFromFile
            |> writeTranslationsToFile)
    
    0 // return an integer exit code

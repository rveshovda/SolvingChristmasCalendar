﻿module Calendar04

//PROBLEM
//Desember kan ofte være litt kjølig. Filen Data/kilma_data_blindern.txt viser klimadata målt på Blindern hver dag siden 1. januar 1950 og frem til 1. januar 2014.
//I denne julenøtten ønsker vi å finne hvilken dato den laveste temperaturen i en desember måned ble målt på Blindern. Om den laveste temperaturen ble målt på flere datoer er det den tidligste vi er ute etter.
//Svaret skal oppgis på følgende form dd.mm.åååå. Eksempelvis: 24.12.2014
//Julegavetips: Det er i kolonnen med overskriften TAN dere finner den laveste målte temperaturen for et døgn.

open Common
open FSharp.Collections.ParallelSeq

let correct = "14.12.1981"
let expectedRuntimeInMs = 150L

open System
open System.IO
open System.Globalization

let parseLine (line:string) =
    let splits = line.Split(' ') |> Seq.filter (fun elem -> elem.Length > 0) |> Seq.toArray
    let date = DateTime.ParseExact(splits.[1], "dd.MM.yyyy", CultureInfo.InvariantCulture)
     
    let value = splits.[3]
    let fValue = Double.Parse value
    (date,fValue)

//TODO: Improve parsing of file and sections
let readFile filename=
    use fs = File.OpenText(filename)
    let text = fs.ReadToEnd()
    let (date, temp) =
        text.Split ('\n')
        |> Seq.skip 24
        |> Seq.take 23377
        |> PSeq.map parseLine
        |> PSeq.filter (fun (date:DateTime, value) -> date.Month = 12)
        |> PSeq.sortBy (fun (_,temp) -> temp)
        |> PSeq.head

    (date, temp)

let get_solution =
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let filename = "..\\..\\..\\Data\\kilma_data_blindern.txt"
    //let filename = "/Users/royveshovda/src/SolvingChristmasCalendar/source/Data/kilma_data_blindern.txt"
    let (date, temp) = readFile filename
    stopWatch.Stop()
    let value = date.ToString("dd.MM.yyyy")
    {
        ExpectedValue=correct;
        ActualValue=value;
        ExpectedRuntimeInMs=expectedRuntimeInMs;
        ActualRuntimeInMs=stopWatch.ElapsedMilliseconds
    }
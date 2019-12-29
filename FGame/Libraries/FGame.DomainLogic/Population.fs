module FGame.DomainLogic.Population

open FGame.DomainLogic.Simulator
open FGame.Models.World
open FGame.Models.GeneticModels


let defaultGenerationNumber = 20

let simulateGeneration states actors =
    actors 
    |> Seq.map (fun b -> simulate b states) 
    |> Seq.sortByDescending (fun r -> r.TotalScore)

let buildInitialPopulation random =
    Seq.init<ActorChromosome> defaultGenerationNumber (fun _ -> getRandomChromosome random)

let simulateFirstGeneration states random =
    buildInitialPopulation random
    |> simulateGeneration states

let mutateBrains (random: System.Random, brains: seq<ActorChromosome>) =
    let brainsList = brains |> Seq.toList
    let numBrains = brainsList.Length
    let survivors = [ brainsList.[0]; brainsList.[1]; ]
    let randos = Seq.init (numBrains - 4) (fun _ -> getRandomChromosome random)
                 |> Seq.toList

    let children = [ 
        createChild(random, survivors.[0].Genes, survivors.[1].Genes, 0.25);
        createChild(random, survivors.[0].Genes, survivors.[1].Genes, 0.5);
    ]

    List.append children randos
    |> List.append survivors

let mutateAndSimulateGeneration (random: System.Random, worlds: seq<World>, results: seq<SimulationResult>) =
    let brains = Seq.map (fun b -> b.Brain) results
                 |> Seq.toList

    mutateBrains (random, brains)
    |> simulateGeneration worlds

let mutateAndSimulateMultiple (random: System.Random, worlds: seq<World>, generations: int, results: seq<SimulationResult>) =
    let mutable currentResults = results
    for _ in 1..generations do
        let brains = Seq.map (fun b -> b.Brain) currentResults

        currentResults <- mutateBrains (random, brains)
                          |> simulateGeneration worlds
                          |> Seq.toList

    currentResults

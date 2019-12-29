module FGame.DomainLogic.Population

open FGame.DomainLogic.Simulator
open FGame.Models.World
open FGame.Models.GeneticModels


let simulateGeneration states actors =
    actors 
    |> Seq.map (fun b -> simulate b states) 
    |> Seq.sortByDescending (fun r -> r.TotalScore)

let buildInitialPopulation random =
    Seq.init<ActorChromosome> 20 (fun _ -> getRandomChromosome random)

let simulateFirstGeneration states random =
    buildInitialPopulation random
    |> simulateGeneration states

let mutateBrains (random: System.Random, brains: list<ActorChromosome>) =
    let numBrains = brains.Length
    let survivors = [ brains.[0]; brains.[1]; ]
    let randos = Seq.init (numBrains - 4) (fun _ -> getRandomChromosome random)
                 |> Seq.toList

    let children = [ 
        createChild(random, survivors.[0].Genes, survivors.[1].Genes, 0.25);
        createChild(random, survivors.[0].Genes, survivors.[1].Genes, 0.5);
    ]

    List.append children randos
    |> List.append survivors

let mutateAndSimulateGeneration (random: System.Random, worlds: list<World>, results: list<SimulationResult>) =
    let brains = Seq.map (fun b -> b.Brain) results
                 |> Seq.toList

    mutateBrains(random, brains)
    |> simulateGeneration worlds

let mutateAndSimulateMultiple (random: System.Random, worlds: list<World>, generations: int, results: list<SimulationResult>) =
    let mutable currentResults = results
    for _ = 1 to generations do    
        let brains = Seq.map (fun b -> b.Brain) currentResults
                     |> Seq.toList

        currentResults <- mutateBrains(random, brains)
                          |> simulateGeneration worlds
                          |> Seq.toList

    currentResults

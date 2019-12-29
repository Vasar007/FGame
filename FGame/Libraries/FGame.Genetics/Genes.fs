﻿module FGame.Genetics.Genes

open GeneticSharp.Domain.Chromosomes
open GeneticSharp.Domain.Fitnesses


type SquirrelChromosome() =
    inherit ChromosomeBase(5) // Number of genes in the chromosome.
    
    override this.CreateNew() : IChromosome =
        new SquirrelChromosome() :> IChromosome
    
    override this.GenerateGene(index: int): Gene =
        new Gene(42)

type SquirrelFitness() =
    interface IFitness with
        member this.Evaluate(chromosome: IChromosome) : double =
            let domainChromosome = chromosome :?> SquirrelChromosome
            42.0

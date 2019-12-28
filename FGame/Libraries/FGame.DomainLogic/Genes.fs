module FGame.DomainLogic.Genes

open FGame.DomainLogic.WorldPos
open FGame.DomainLogic.Actors
open FGame.DomainLogic.World

type ActorChromosome = {
    DogImportance: double
    AcornImportance: double
    RabbitImportance: double
    TreeImportance: double
    SquirrelImportance: double
    RandomImportance: double
}

let getRandomGene (random: System.Random) = (random.NextDouble() * 2.0) - 1.0

let getRandomChromosome (random: System.Random) = 
    {
        DogImportance = getRandomGene random;
        AcornImportance = getRandomGene random;
        RabbitImportance = getRandomGene random;
        TreeImportance = getRandomGene random;
        SquirrelImportance = getRandomGene random;
        RandomImportance = getRandomGene random;
    }

let evaluateProximity (actor: Actor, pos:WorldPos, weight: float) =
    if actor.IsActive then
        getDistance(actor.Pos, pos) * weight
    else
        0.0

let evaluateTile (brain: ActorChromosome, world: World, pos: WorldPos, random: System.Random) =
    evaluateProximity(world.Squirrel, pos, brain.SquirrelImportance) +
        evaluateProximity(world.Rabbit, pos, brain.RabbitImportance) +
        evaluateProximity(world.Doggo, pos, brain.DogImportance) +
        evaluateProximity(world.Acorn, pos, brain.AcornImportance) +
        evaluateProximity(world.Tree, pos, brain.TreeImportance) +
        (random.NextDouble() * brain.RandomImportance)
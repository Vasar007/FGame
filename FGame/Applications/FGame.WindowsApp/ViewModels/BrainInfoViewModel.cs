using FGame.Models;
using FGame.WindowsApp.Domain;
using Prism.Mvvm;

namespace FGame.WindowsApp.ViewModels
{
    internal sealed class BrainInfoViewModel : BindableBase
    {
        public GeneticModels.ActorChromosome Model { get; }

        public double SquirrelPriority => GetGene(GeneticModels.ActorGeneIndex.Squirrel);

        public double DoggoPriority => GetGene(GeneticModels.ActorGeneIndex.Doggo);
        
        public double RabbitPriority => GetGene(GeneticModels.ActorGeneIndex.Rabbit);
        
        public double AcornPriority => GetGene(GeneticModels.ActorGeneIndex.Acorn);
        
        public double TreePriority => GetGene(GeneticModels.ActorGeneIndex.Tree);
        
        public double NextToDoggoPriority => GetGene(GeneticModels.ActorGeneIndex.NextToDoggo);
        
        public double NextToRabbitPriority => GetGene(GeneticModels.ActorGeneIndex.NextToRabbit);


        public BrainInfoViewModel(GeneticModels.ActorChromosome brain)
        {
            Model = brain.ThrowIfNull(nameof(brain));
        }

        private double GetGene(GeneticModels.ActorGeneIndex actor)
        {
            return GeneticModels.getGene(actor, Model.Genes);
        }
    }
}

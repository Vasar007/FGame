using FGame.DomainLogic;
using FGame.WindowsApp.Domain;
using Prism.Mvvm;

namespace FGame.WindowsApp.ViewModels
{
    internal sealed class BrainInfoViewModel : BindableBase
    {
        public Genes.ActorChromosome Model { get; }

        public double SquirrelPriority => GetGene(Genes.ActorGeneIndex.Squirrel);

        public double DoggoPriority => GetGene(Genes.ActorGeneIndex.Doggo);
        
        public double RabbitPriority => GetGene(Genes.ActorGeneIndex.Rabbit);
        
        public double AcornPriority => GetGene(Genes.ActorGeneIndex.Acorn);
        
        public double TreePriority => GetGene(Genes.ActorGeneIndex.Tree);
        
        public double NextToDoggoPriority => GetGene(Genes.ActorGeneIndex.NextToDoggo);
        
        public double NextToRabbitPriority => GetGene(Genes.ActorGeneIndex.NextToRabbit);


        public BrainInfoViewModel(Genes.ActorChromosome brain)
        {
            Model = brain.ThrowIfNull(nameof(brain));
        }

        private double GetGene(Genes.ActorGeneIndex actor)
        {
            return Genes.getGene(actor, Model.Genes);
        }
    }
}

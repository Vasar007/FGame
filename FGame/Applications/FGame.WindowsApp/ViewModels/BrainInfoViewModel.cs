using FGame.DomainLogic;
using FGame.WindowsApp.Domain;
using Prism.Mvvm;

namespace FGame.WindowsApp.ViewModels
{
    internal sealed class BrainInfoViewModel : BindableBase
    {
        public Genes.ActorChromosome Model { get; }

        public double SquirrelPriority => Model.SquirrelImportance;

        public double DoggoPriority => Model.DogImportance;
        
        public double RabbitPriority => Model.RabbitImportance;
       
        public double AcornPriority => Model.AcornImportance;
        
        public double TreePriority => Model.TreeImportance;
       
        public double RandomPriority => Model.RandomImportance;


        public BrainInfoViewModel(Genes.ActorChromosome brain)
        {
            Model = brain.ThrowIfNull(nameof(brain));
        }
    }
}

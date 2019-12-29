using Prism.Events;
using FGame.WindowsApp.ViewModels;

namespace FGame.WindowsApp.Domain.Messages
{
    internal sealed class UpdateBrainMessage : PubSubEvent<SimulationResultViewModel>
    {
        public UpdateBrainMessage()
        {
        }
    }
}

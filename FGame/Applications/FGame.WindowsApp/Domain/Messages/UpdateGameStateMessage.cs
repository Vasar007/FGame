using Prism.Events;
using FGame.DomainLogic;

namespace FGame.WindowsApp.Domain.Messages
{
    internal sealed class UpdateGameStateMessage : PubSubEvent<States.GameState>
    {
        public UpdateGameStateMessage()
        {
        }
    }
}

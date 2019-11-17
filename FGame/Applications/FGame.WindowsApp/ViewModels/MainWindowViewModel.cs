using System.Text;
using FGame.DomainLogic;

namespace FGame.WindowsApp.ViewModels
{
    internal sealed class MainWindowViewModel
    {
        private readonly World.World _world;

        public string TextGrid => BuildAsccGrid();

        public MainWindowViewModel()
        {
            _world = WorldGeneration.makeTestWorld(false);
        }

        private string BuildAsccGrid()
        {
            var sb = new StringBuilder();
            for (int y = 1; y < _world.MaxY; ++y)
            {
                for (int x = 1; x < _world.MaxX; ++x)
                {
                    sb.Append(World.getCharacterAtCell(x, y, _world));
                }

                // Advance to the next line.
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}

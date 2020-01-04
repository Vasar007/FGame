using System.Windows.Controls;
using Acolyte.Assertions;
using Prism.Mvvm;

namespace FGame.WindowsApp.Views
{
    /// <summary>
    /// Interaction logic for ArtificialIntelligenceView.xaml
    /// </summary>
    public partial class ArtificialIntelligenceView : UserControl
    {
        public ArtificialIntelligenceView(BindableBase dataContext)
        {
            InitializeComponent();

            DataContext = dataContext.ThrowIfNull(nameof(dataContext));
        }
    }
}

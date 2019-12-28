using System.Windows.Controls;
using Prism.Mvvm;
using FGame.WindowsApp.Domain;

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

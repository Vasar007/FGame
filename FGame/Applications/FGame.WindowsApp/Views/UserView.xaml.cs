using System.Windows.Controls;
using Acolyte.Assertions;
using Prism.Mvvm;

namespace FGame.WindowsApp.Views
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : UserControl
    {
        public UserView(BindableBase dataContext)
        {
            InitializeComponent();

            DataContext = dataContext.ThrowIfNull(nameof(dataContext));
        }
    }
}

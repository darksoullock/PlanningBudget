using PlanningBudget.Core;
using Windows.UI.Xaml.Controls;

namespace PlanningBudget.Windows.Views
{
    public class BindablePage : Page
    {
        protected override void OnNavigatedTo(global::Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            (this.DataContext as INavigable).Activate(e.Parameter);
        }
    }
}

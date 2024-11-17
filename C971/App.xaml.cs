using C971.Views;

namespace C971
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var termListPage = new TermList();
            var navPage = new NavigationPage(termListPage);

            MainPage = navPage;
        }
    }
}

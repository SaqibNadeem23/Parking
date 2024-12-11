using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Parking
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Login());
        }
        public static string Databaselocation = string.Empty;
        public App(string databaselocation)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Login()); 
            Databaselocation = databaselocation;
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

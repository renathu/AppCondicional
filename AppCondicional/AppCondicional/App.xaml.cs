using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCondicional
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new Views.CondicionaisPage());
            MainPage = new AppShell();
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

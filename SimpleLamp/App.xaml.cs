using System;

using SimpleLamp.Views;
using SimpleLamp.LampAPI;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleLamp
{
    public partial class App : Application
    {
        public static MatrixStream MajorMatrixStream;

        public App()
        {
            this.InitializeComponent();

            this.MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            App.MajorMatrixStream = new MatrixStream();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

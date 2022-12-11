using System;
using System.Diagnostics;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace SimpleLamp.ViewModels
{
    public class MainViewModel : ContentPage
    {
        public ICommand OpenWebCommand { get; }

        public MainViewModel()
        {
            this.Title = "Главная";
            this.OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }
    }
}
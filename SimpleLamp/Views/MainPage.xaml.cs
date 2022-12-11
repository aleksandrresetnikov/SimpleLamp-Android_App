using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using SimpleLamp.ListViews.LampEffects;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleLamp.Views
{
    public partial class MainPage : ContentPage
    {
        public ListViewEffectsGroup EffectsList;
        public EffectItem LastEffectItem;
        public EffectItem SelectEffectItem;

        public MainPage()
        {
            this.BindingContext = this;

            this.EffectsList = new ListViewEffectsGroup
            {
                new EffectItem("Боковая радуга", "Радуга по горизонтали и вертикали", 0),
                new EffectItem("Змейка", "Цветная змейка", 1),
                new EffectItem("Градиент", "Боковой цветовой градиент", 2),
                new EffectItem("Плавный свет", "Плавная градиентная смена цвета", 3),
                new EffectItem("Классическая радуга", "Горизонтальная радуга", 4),
                new EffectItem("Мячики", "Отскакивающие мячики", 5),
                new EffectItem("Камин", "Анимированный камин", 6),
                new EffectItem("Вспышки", "Цветные вспышки", 7),
                new EffectItem("Пули", "Пули разного направления", 8),
                new EffectItem("Цветной дождь", "Падающие цветные капли", 9),
                new EffectItem("Кляксы", "Случайные кляксы", 10),
                new EffectItem("Змейка-2", "Короткая змейка", 11),
                new EffectItem("Точки", "Быстрые точки", 12),
                new EffectItem("Плазма", "Горизонтальная плазма", 13),
                new EffectItem("Конфетти", "Медленные плавные точки", 14),
                new EffectItem("Плазма-2", "(необходима высокая яркость) Полная плазма", 15),
                new EffectItem("Круг", "Вращающаяся окружность", 16),
                new EffectItem("Плазма-3", "Медленная плазма", 17),
                new EffectItem("Линии", "Цветные линии", 18),
                new EffectItem("Дым", "Движущийся дым", 19),
                new EffectItem("Снег", "Падающий снег вниз", 20),
                new EffectItem("Снег-2", "Падающий снег наискосок", 21),
            };

            this.InitializeComponent();

            this.ComboBox.ItemsSource = this.EffectsList;
            this.ComboBox.SelectedItem = this.EffectsList.First();
        }

        private async void OnConnectButtonClicked(object sender, EventArgs e)
        {
            this.ConnectToMatrix();
        }

        private async void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.ComboBox.SelectedItem = this.EffectsList.First();
        }

        private async void ComboBox_SelectChanged(object sender, SelectedItemChangedEventArgs e)
        {
            this.SelectEffectItem = e.SelectedItem as EffectItem;
            this.UpdateMatrixSelectMode();
        }

        private async void BrightnessSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue) return;
        }

        private async void UpdateMatrixSelectMode()
        {
            if (!App.MajorMatrixStream.GetConnectionState()) return;
            if (!this.EffectsList.ContainsName(this.SelectEffectItem.Name)) return;

            //await DisplayAlert("Смена режима", this.SelectEffectItem.Name, "OK");

            int ModeIndex = this.EffectsList.GetEffectItemIndex(this.SelectEffectItem.Name);
            if (ModeIndex == -1) return;
            App.MajorMatrixStream.SetMode((byte)(ModeIndex+1));
        }

        private async void UpdateMatrixBrightnessValue(byte value)
        {
            if (!App.MajorMatrixStream.GetConnectionState()) return;
            if (value >= 255) /*return;*/ value = 254;

            //await DisplayAlert("Смена режима", this.SelectEffectItem.Name, "OK");

            App.MajorMatrixStream.SetBrightness(value);
        }

        private async void ConnectToMatrix()
        {
            if (this.ConnectButton.Text == "Подключиться")
            {
                try
                {
                    App.MajorMatrixStream.Connect();

                    this.ComboBox.SelectedItem = this.EffectsList.First();
                    this.BrightnessSlider.Value = 50;

                    if (App.MajorMatrixStream != null && App.MajorMatrixStream.GetConnectionState())
                    {
                        this.ConnectButton.Text = "Отключиться";
                        this.ConnectButton.ImageSource = "connected4";
                    }
                }
                catch (Exception ex)
                {
                    bool response = await DisplayAlert("Проблема", "Не удалось подключиться к лампе, или к основному серверу лампы. Проверьте наличие подключения к WiFi лампы.",
                        "OK", "Подробнее");
                    if (!response)
                        await DisplayAlert("Трессировка стека исключения", ex.Message + ":\n\n" + ex.StackTrace, "OK");
                }
            }
            else if (this.ConnectButton.Text == "Отключиться")
            {
                try
                {
                    App.MajorMatrixStream.Disconnect();

                    this.ConnectButton.Text = "Подключиться";
                    this.ConnectButton.ImageSource = "not_connected4";
                }
                catch (Exception ex)
                {
                    bool response = await DisplayAlert("Проблема", "Не удалось отключиться от лампы. Возможно это программная ошибка. Перезагрузите приложение.",
                        "OK", "Подробнее");
                    if (!response)
                        await DisplayAlert("Трессировка стека исключения", ex.Message + ":\n\n" + ex.StackTrace, "OK");
                }
            }
            else
            {
                await DisplayAlert("Проблема", "Произошла непредвидимая ошибка в пользовательском интерфейсе. Перезагрузите приложение.", "OK");
            }

            GC.Collect();
        }
    }  
}
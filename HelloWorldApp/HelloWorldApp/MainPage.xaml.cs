using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using UIKit;
using System.Net.Http;
using HelloWorldApp.Models;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace HelloWorldApp
{
    public partial class MainPage : ContentPage
    {
        public int Count { get; set; }

        private List<object> _counterButtons = new List<object>();
        private HttpClient _httpClient;
        private List<string> _todoList;

        private string _todoFilePath = "todo.txt";

        public MainPage()
        {
            Count = 0;
            InitializeComponent();

            Battery.BatteryInfoChanged += Battery_BatteryInfoChanged;
            Battery_BatteryInfoChanged(null, null);
            _httpClient = new HttpClient();

            _todoList = new List<string>();

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _todoFilePath = Path.Combine(documents, _todoFilePath);

            if (!File.Exists(_todoFilePath))
            {
                File.WriteAllText(_todoFilePath, "");
            }
            else
            {
                _todoList = File.ReadAllLines(_todoFilePath).ToList();

                foreach (var todo in _todoList)
                {
                    Add_TodoButton(todo);
                }
            }
        }

        private async void Get_Location(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    ((Button)sender).Text = $"Lat: {location.Latitude}, Long: {location.Longitude}";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                ((Button)sender).Text = fnsEx.Message;
            }
            catch (FeatureNotEnabledException fneEx)
            {
                ((Button)sender).Text = fneEx.Message;
            }
            catch (PermissionException pEx)
            {
                ((Button)sender).Text = pEx.Message;
            }
            catch (Exception ex)
            {
                ((Button)sender).Text = ex.Message;
            }
        }

        private async void Get_A_Card(object sender, EventArgs e)
        {
            try
            {
                var response = await _httpClient.GetAsync("https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1");
                var content = await response.Content.ReadAsStringAsync();
                var deck = JsonConvert.DeserializeObject<Deck>(content);
                if (deck == null)
                {
                    ((Button)sender).Text = "No Deck";
                    return;
                }
                response = await _httpClient.GetAsync($"https://deckofcardsapi.com/api/deck/{deck.deck_id}/draw/?count=1");
                content = await response.Content.ReadAsStringAsync();
                var cards = JsonConvert.DeserializeObject<Cards>(content);
                var card = cards?.cards?.FirstOrDefault();
                if (card == null)
                {
                    ((Button)sender).Text = "No Card";
                    return;
                }
                ((Button)sender).Text = $"{card.value} of {card.suit}";
            }
            catch (Exception)
            {
                ((Button)sender).Text = "Error";
            }
        }

        private void Battery_BatteryInfoChanged(object sender, EventArgs e)
        {
            var batteryLevel = Math.Floor(Battery.ChargeLevel * 100);
            BatteryBtn.Text = $"Your battery is at {batteryLevel}% charge";
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!_counterButtons.Contains(sender))
            {
                _counterButtons.Add(sender);
            }

            Count++;

            foreach (var button in _counterButtons)
            {
                if (button is Button)
                {
                    Update_Button_Text(button);
                }
            }
        }

        private void Update_Button_Text(object sender)
        {
            var s = Count == 1 ? "" : "s";
            ((Button)sender).Text = $"You clicked {Count} time{s}!";
        }

        private async void Toggle_Flashlight(object sender, EventArgs e)
        {
            try
            {
                await Flashlight.TurnOffAsync();
                Thread.Sleep(20);
                await Flashlight.TurnOnAsync();
                Thread.Sleep(20);
                await Flashlight.TurnOffAsync();
                Thread.Sleep(20);
                await Flashlight.TurnOnAsync();
                Thread.Sleep(20);
                await Flashlight.TurnOffAsync();
                Thread.Sleep(20);
                await Flashlight.TurnOnAsync();
                Thread.Sleep(20);
                await Flashlight.TurnOffAsync();
            }
            catch (Exception ex)
            {
                ((Button)sender).Text = ex.Message;
            }
        }

        private void Entry_TextChanged(object sender, EventArgs e)
        {
            if (sender is Entry && !string.IsNullOrWhiteSpace(((Entry)sender).Text))
            {
                Add_TodoButton(((Entry)sender).Text);
                _todoList.Add(((Entry)sender).Text);
                ((Entry)sender).Text = "";
                Write_todos_to_file();
            }
        }

        private void Add_TodoButton(string text)
        {
            var todoBtn = new Button();
            todoBtn.Clicked += Remove_self;
            todoBtn.Text = text;
            todoBtn.TextColor = Color.White;
            todoBtn.BackgroundColor = Color.DarkBlue;
            TodoList.Children.Add(todoBtn);
        }

        private void Remove_self(object sender, EventArgs e)
        {
            TodoList.Children.Remove((Button)sender);
            _todoList.Remove(_todoList.Find(todo => todo == ((Button)sender).Text));
            Write_todos_to_file();
        }

        private void Write_todos_to_file()
        {
            File.WriteAllText(_todoFilePath, String.Join("\n", _todoList));
        }
    }
}

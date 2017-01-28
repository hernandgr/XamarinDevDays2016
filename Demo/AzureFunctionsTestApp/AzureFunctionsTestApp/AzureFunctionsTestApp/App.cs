using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AzureFunctionsTestApp
{
    public class App : Application
    {
        static ListView lstVehicles = new ListView();
        static Label titleLabel = new Label();

        public App()
        {
            MainPage = GetMainPage();
        }

        public static ContentPage GetMainPage()
        {
            titleLabel.HorizontalTextAlignment = TextAlignment.Center;
            titleLabel.Text = "Testing Azure Functions from Xamarin!";

            Button newButn = new Button()
            {
                Text = "Connect to Azure Function",
            };

            newButn.Clicked += newButn_Clicked;
            lstVehicles.ItemTemplate = new DataTemplate(typeof(TextCell));
            lstVehicles.ItemTemplate.SetBinding(TextCell.TextProperty, "Model");

            return new ContentPage
            {
                Content = new StackLayout()
                {
                    Children = {
                        titleLabel,
                        newButn,
                        lstVehicles
                    }
                }
            };
        }

        static async void newButn_Clicked(object sender, EventArgs e)
        {
            string previousTextValue = titleLabel.Text;

            titleLabel.Text = "*******Executing the Azure Function!********";

            VehiclesAzureFunction vehiclesAzureFunction = new VehiclesAzureFunction();

            //Consume the Azure Function with hardcoded method easier to see as an example
            var vehiclesList = await vehiclesAzureFunction.GetVehiclesHardCodedAsync();

            //Consume the Azure Function with HttpClient reusable code within the Function/Service Agent

            //var vehiclesList = await vehiclesAzureFunction.GetVehiclesAsync(“Chevrolet”);
            lstVehicles.ItemsSource = vehiclesList;
            titleLabel.Text = previousTextValue;
        }

    }

    public class Vehicle
    {
        public string Make { get; set; }
        public string Model { get; set; }
    }

    public class VehiclesAzureFunction
    {
        public VehiclesAzureFunction()
        {
        }

        public async Task<List<Vehicle>> GetVehiclesHardCodedAsync()
        {
            var client = new System.Net.Http.HttpClient();
            string url = $"https://xamarin-devdays.azurewebsites.net/api/MyTestFunction?code=moSQop6bkmY5VxqgoJU3ovSwffQBhP41TahPJHma6AZFGywOKKyFmg==";

            var response = await client.GetAsync(url);

            var vehiclesJson = response.Content.ReadAsStringAsync().Result;

            List<Vehicle> listOfVehicles = JsonConvert.DeserializeObject<List<Vehicle>>(vehiclesJson);

            return listOfVehicles;
        }
    }
}

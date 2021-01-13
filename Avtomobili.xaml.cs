using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Avtosalon
{
    /// <summary>
    /// Interaction logic for Avtomobili.xaml
    /// </summary>
    public partial class Avtomobili : Window
    {
        RestClient client;

        public Avtomobili()
        {
            InitializeComponent();

            client = new RestClient("http://localhost:62610/AvtosalonService.svc");
            prikaziAvte_Click(null, null);
        }

        private void zapriAvto_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void prikaziAvto_Click(object sender, RoutedEventArgs e)
        {
            Avto avto = (Avto)listView.SelectedItem;

            var request = new RestRequest("avto/{id}", Method.GET);
            request.AddUrlSegment("id", avto.Id);
            
            IRestResponse response = client.Execute(request);
            string content = response.Content;
            Avto avto2 = JsonConvert.DeserializeObject<Avto>(content);

            listView.Items.Clear();
            listView.Items.Add(avto);
        }

        private void prikaziAvte_Click(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest("avti", Method.GET);
            IRestResponse response = client.Execute(request);
            string content = response.Content;
            Avto[] avti = JsonConvert.DeserializeObject<Avto[]>(content);

            listView.Items.Clear();
            foreach (Avto avto in avti)
            {
                listView.Items.Add(avto);
            }
        }

        private void dodajAvto_Click(object sender, RoutedEventArgs e)
        {
            Avto avto = new Avto()
            {
                Id = IdTextBox.Text,
                Ime = ImeTextBox.Text,
                Letnik = int.Parse(LetnikTextBox.Text),
                Tip = IdTextBox.Text,
                StPrevozenihKm = double.Parse(SteviloKmTextBox.Text),
                Gorivo = GorivoTextBox.Text,
                Poraba = float.Parse(PorabaTextBox.Text),
                Barva = BarvaTextBox.Text
            };

            var request = new RestRequest("avto", Method.POST);

            string jsonToSend = JsonConvert.SerializeObject(avto);

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = RestSharp.DataFormat.Json;

            IRestResponse response = client.Execute(request);
            string content = response.Content;
            Avto avto2 = JsonConvert.DeserializeObject<Avto>(content);

            listView.Items.Clear();
            prikaziAvte_Click(null, null);
        }

        private void posodobiAvto_Click(object sender, RoutedEventArgs e)
        {
            Avto avto = new Avto()
            {
                Id = IdTextBox.Text,
                Ime = ImeTextBox.Text,
                Letnik = int.Parse(LetnikTextBox.Text),
                Tip = TipTextBox.Text,
                StPrevozenihKm = double.Parse(SteviloKmTextBox.Text),
                Gorivo = GorivoTextBox.Text,
                Poraba = float.Parse(PorabaTextBox.Text),
                Barva = BarvaTextBox.Text
            };

            var request = new RestRequest("avto/{id}", Method.PUT);
            request.AddUrlSegment("id", avto.Id);

            string jsonToSend = JsonConvert.SerializeObject(avto);

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = RestSharp.DataFormat.Json;


            IRestResponse response = client.Execute(request);
            string content = response.Content;
            Avto avto2 = JsonConvert.DeserializeObject<Avto>(content);

            listView.Items.Clear();
            prikaziAvte_Click(null, null);
        }

        private void odstraniAvto_Click(object sender, RoutedEventArgs e)
        {
            Avto avto = (Avto)listView.SelectedItem;

            var request = new RestRequest("avto/{id}", Method.DELETE);
            request.AddUrlSegment("id", avto.Id);

            IRestResponse response = client.Execute(request);
            string content = response.Content;
            Avto avto2 = JsonConvert.DeserializeObject<Avto>(content);

            listView.Items.Clear();
            prikaziAvte_Click(null, null);
        }

        private void listView_Selected(object sender, RoutedEventArgs e)
        {
            Avto avto = (Avto)listView.SelectedItem;

            if (avto != null)
            {
                IdTextBox.Text = avto.Id;
                ImeTextBox.Text = avto.Ime;
                LetnikTextBox.Text = avto.Letnik + "";
                TipTextBox.Text = avto.Tip;
                SteviloKmTextBox.Text = avto.StPrevozenihKm + "";
                GorivoTextBox.Text = avto.Gorivo;
                PorabaTextBox.Text = avto.Poraba + "";
                BarvaTextBox.Text = avto.Barva;
            }
        }
    }
}

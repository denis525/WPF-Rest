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
    /// Interaction logic for Narocniki.xaml
    /// </summary>
    public partial class Narocniki : Window
    {
        RestClient client;

        public Narocniki()
        {
            InitializeComponent();

            client = new RestClient("http://localhost:62610/AvtosalonService.svc");
            prikaziNarocnike_Click(null, null);
        }

        private void zapriNaro_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void prikaziNarocnika_Click(object sender, RoutedEventArgs e)
        {
            Narocnik izposoja = (Narocnik)listView.SelectedItem;

            var request = new RestRequest("narocnik/{id}", Method.GET);
            request.AddUrlSegment("id", izposoja.Id);

            IRestResponse response = client.Execute(request);
            string content = response.Content;
            Narocnik izposoja2 = JsonConvert.DeserializeObject<Narocnik>(content);

            listView.Items.Clear();
            listView.Items.Add(izposoja2);
        }

        private void prikaziNarocnike_Click(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest("narocniki", Method.GET);
            IRestResponse response = client.Execute(request);
            string content = response.Content;
            Narocnik[] izposoje = JsonConvert.DeserializeObject<Narocnik[]>(content);

            listView.Items.Clear();
            foreach (Narocnik izposoja in izposoje)
            {
                listView.Items.Add(izposoja);
            }
        }

        private void dodajNarocnika_Click(object sender, RoutedEventArgs e)
        {
            Narocnik avto = new Narocnik()
            {
                Id = int.Parse(IdTextBox.Text),
                Ime = ImeTextBox.Text,
                Priimek = PriimekTextBox.Text,
                Kraj = KrajTextBox.Text,
                Naslov = NaslovTextBox.Text,
            };

            var request = new RestRequest("narocnik", Method.POST);

            string jsonToSend = JsonConvert.SerializeObject(avto);

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = RestSharp.DataFormat.Json;

            IRestResponse response = client.Execute(request);
            string content = response.Content;

            listView.Items.Clear();
            prikaziNarocnike_Click(null, null);
        }

        private void posodobiNarocnika_Click(object sender, RoutedEventArgs e)
        {
            Narocnik avto = new Narocnik()
            {
                Id = int.Parse(IdTextBox.Text),
                Ime = ImeTextBox.Text,
                Priimek = PriimekTextBox.Text,
                Kraj = KrajTextBox.Text,
                Naslov = NaslovTextBox.Text,
            };

            var request = new RestRequest("narocnik/{id}", Method.PUT);
            request.AddUrlSegment("id", avto.Id);

            string jsonToSend = JsonConvert.SerializeObject(avto);

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = RestSharp.DataFormat.Json;


            IRestResponse response = client.Execute(request);
            string content = response.Content;

            listView.Items.Clear();
            prikaziNarocnike_Click(null, null);
        }

        private void odstraniNarocnika_Click(object sender, RoutedEventArgs e)
        {
            Narocnik izposoja = (Narocnik)listView.SelectedItem;

            var request = new RestRequest("narocnik/{id}", Method.DELETE);
            request.AddUrlSegment("id", izposoja.Id);

            IRestResponse response = client.Execute(request);
            string content = response.Content;

            listView.Items.Clear();
            prikaziNarocnike_Click(null, null);
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Narocnik narocnik = (Narocnik)listView.SelectedItem;

            if (narocnik != null)
            {
                IdTextBox.Text = narocnik.Id + "";
                ImeTextBox.Text = narocnik.Ime;
                PriimekTextBox.Text = narocnik.Priimek;
                KrajTextBox.Text = narocnik.Kraj;
                NaslovTextBox.Text = narocnik.Naslov;
            }

            
        }
    }
}

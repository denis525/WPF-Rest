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
    /// Interaction logic for Izposoje.xaml
    /// </summary>
    public partial class Izposoje : Window
    {
        RestClient client;

        public Izposoje()
        {
            InitializeComponent();

            client = new RestClient("http://localhost:62610/AvtosalonService.svc");
            prikaziIzposoje_Click(null, null);
        }

        private void zapriIzpo_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void prikaziIzposojo_Click(object sender, RoutedEventArgs e)
        {
            Izposoja izposoja = (Izposoja)listView.SelectedItem;

            var request = new RestRequest("izposoja/{id}", Method.GET);
            request.AddUrlSegment("id", izposoja.Id);

            IRestResponse response = client.Execute(request);
            string content = response.Content;
            Izposoja izposoja2 = JsonConvert.DeserializeObject<Izposoja>(content);

            listView.Items.Clear();
            listView.Items.Add(izposoja2);
        }

        private void prikaziIzposoje_Click(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest("izposoje", Method.GET);
            IRestResponse response = client.Execute(request);
            string content = response.Content;
            Izposoja[] izposoje = JsonConvert.DeserializeObject<Izposoja[]>(content);

            listView.Items.Clear();
            foreach (Izposoja izposoja in izposoje)
            {
                listView.Items.Add(izposoja);
            }
        }

        private void dodajIzposojo_Click(object sender, RoutedEventArgs e)
        {
            Izposoja izposoja = new Izposoja()
            {
                Id = int.Parse(IdTextBox.Text),
                StPrevozenihNarocnika = double.Parse(StPrevozenihNarocnikaTextBox.Text),
                DatumIzposoje = Convert.ToDateTime(DatumIzposojeTextBox.Text), // .ToString("yyyy-MM-dd"))
                DatumVrnitve = Convert.ToDateTime(DatumVrnitveTextBox.Text),
                Cena = float.Parse(CenaTextBox.Text),
                AvtoId = int.Parse(AvtoIdTextBox.Text),
                NarocnikId = int.Parse(NarocnikIdTextBox.Text)
            };

            var request = new RestRequest("izposoja", Method.POST);

            string jsonToSend = JsonConvert.SerializeObject(izposoja);

            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            request.RequestFormat = RestSharp.DataFormat.Json;

            IRestResponse response = client.Execute(request);
            string content = response.Content;

            listView.Items.Clear();
            prikaziIzposoje_Click(null, null);
        }

        private void odstraniIzposojo_Click(object sender, RoutedEventArgs e)
        {
            Izposoja izposoja = (Izposoja)listView.SelectedItem;

            var request = new RestRequest("izposoja/{id}", Method.DELETE);
            request.AddUrlSegment("id", izposoja.Id);

            IRestResponse response = client.Execute(request);
            string content = response.Content;            

            listView.Items.Clear();
            prikaziIzposoje_Click(null, null);
        }
    }
}

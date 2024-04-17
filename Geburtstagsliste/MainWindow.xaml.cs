using Geburtstagsliste;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Geburtstagsliste
{
    public partial class MainWindow : Window
    {
        ObservableCollection<Geburtstag> geburtstagsliste = new ObservableCollection<Geburtstag>();

        public MainWindow()
        {
            InitializeComponent();

            // Fülle die ComboBox mit den Jahren von 1900 bis zum aktuellen Jahr
            ObservableCollection<string> Years = new ObservableCollection<string>();

            int aktuell = DateTime.Now.Year;
            for (int year = aktuell; year >= 1900; year--)
            {
                Years.Add(year.ToString());
            }

            comboBoxGeburtsjahr.ItemsSource = Years;

            //Laden der Geburtstage vom Server
            getGeburtstageFromServer();
        }

        


        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            labelName.Visibility= Visibility.Visible;
            textBoxName.Visibility = Visibility.Visible;
            buttonName.Visibility = Visibility.Visible;
            labelGeburtsjahr.Visibility = Visibility.Visible;
            checkBoxGeburtsjahr.Visibility = Visibility.Visible;

            DateTime selectedDate = Calendar.SelectedDate.GetValueOrDefault();
            string datum = selectedDate.ToShortDateString();

            //Datum splitten
            string[] arrDatum = datum.Split(".");
            string day = arrDatum[0];
            string month = arrDatum[1];

            textBlockTag.Text = $"Geburtstag am {day}.{month} hinzufügen:";
        }

        //Geburtstag speichern
        private void buttonName_Click(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate = Calendar.SelectedDate.GetValueOrDefault();
            string datum = selectedDate.ToShortDateString();

            //Datum splitten
            string[] arrDatum = datum.Split(".");
            string day = arrDatum[0];
            string month = arrDatum[1];

            string name = textBoxName.Text;

            if(name.Length == 0)
            {
                MessageBox.Show("Name der Person eingeben!");
            }
            else
            {
                //Geburtsjahr wird auch gespeichert
                if (checkBoxGeburtsjahr.IsChecked == true)
                {
                    if (comboBoxGeburtsjahr.SelectedItem is not null)
                    {
                        string year = comboBoxGeburtsjahr.SelectedValue.ToString();
                        Geburtstag geb = new Geburtstag(null, name, day, month, year);
                        addGeburtstagToServer(geb);

                        MessageBox.Show($"<{geb.ToString()}> wurde erfolgreich hinzugefügt.");

                        //Aktualisieren
                        getGeburtstageFromServer();
                    }
                    else
                    {
                        MessageBox.Show("Geburtsjahr der Person auswählen!");
                    }
                }
                //Geburtsjahr wird nicht gespeichert
                else
                {
                    Geburtstag geb = new Geburtstag(null, name, day, month);
                    addGeburtstagToServer(geb);

                    MessageBox.Show($"<{geb.ToString()}> wurde erfolgreich hinzugefügt.");

                    //Aktualisieren
                    getGeburtstageFromServer();
                }
            }

            List<String> strings = listToString();
            listView.ItemsSource = strings;
            textBoxName.Text = null;
        }

        private void checkBoxGeburtsjahr_Checked(object sender, RoutedEventArgs e)
        {
            comboBoxGeburtsjahr.Visibility = Visibility.Visible;
        }

        private void checkBoxGeburtsjahr_Unchecked(object sender, RoutedEventArgs e)
        {
            comboBoxGeburtsjahr.Visibility = Visibility.Hidden;
        }

        //Erstellt Liste an Strings für die Ausgabe
        public List<String> listToString()
        {
            List<String> strings = new List<String>();
            geburtstagsliste = SortByDate(geburtstagsliste);

            foreach(Geburtstag geb in geburtstagsliste)
            {
                strings.Add(geb.ToString());
            }

            return strings;
        }

        //Sortiert die Geburtstage
        private static ObservableCollection<Geburtstag> SortByDate(ObservableCollection<Geburtstag> geburtstagsliste)
        {
            return new ObservableCollection<Geburtstag>(geburtstagsliste.OrderBy(g => g.Month).ThenBy(g => g.Day));
        }

        //Zum löschen eines Geburtstages
        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Popup anzeigen
            int index = listView.SelectedIndex;

            if (index >= 0 && index < geburtstagsliste.Count)
            {
                Trace.WriteLine($"List View DoubleClick -> {geburtstagsliste[index].Name}");
                var result = MessageBox.Show($"Do you want to delete {geburtstagsliste[index].Name}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Element löschen
                    geburtstagsliste.Remove(geburtstagsliste[index]);
                    listView.ItemsSource = geburtstagsliste;
                }
            }            
        }

        //Laden der Daten vom Server
        public async void getGeburtstageFromServer()
        {
            Trace.WriteLine("Laden vom Server");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8081/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("geb-liste/geburtstage");
                if (response.IsSuccessStatusCode)
                {
                    Trace.WriteLine("Success");
                    string jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var geburtstageFromServer = JsonSerializer.Deserialize<ObservableCollection<Geburtstag>>(jsonString, options);

                    geburtstagsliste.Clear();
                    foreach (var geburtstag in geburtstageFromServer)
                    {
                        geburtstagsliste.Add(geburtstag);
                    }

                    //Laden in Ausgabe
                    Trace.WriteLine("Ausgabe wird aktualisiert");
                    List<String> strings = listToString();
                    listView.ItemsSource = strings;   
                }
                else
                {
                    MessageBox.Show($"Server returned {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }

        //Geburtstag zum Server senden
        private async Task<bool> addGeburtstagToServer(Geburtstag geb)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:8081/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string jsonString = JsonSerializer.Serialize(geb, options);
                    var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("geb-liste/geburtstag", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"Server returned {response.StatusCode} - {response.ReasonPhrase}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }

        }
    }
}
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

            loadData();


            // Fülle die ComboBox mit den Jahren von 1900 bis zum aktuellen Jahr
            ObservableCollection<string> Years = new ObservableCollection<string>();
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear; year >= 1900; year--)
            {
                Years.Add(year.ToString());
            }
            comboBoxGeburtsjahr.ItemsSource = Years;
        }

        //Daten vom Server laden
        public async Task loadData()
        {
            try
            {
                // HTTP-Client erstellen
                using (var httpClient = new HttpClient())
                {
                    string serverUrl = "http://localhost:8080/geburtstage";
                    HttpResponseMessage response = await httpClient.GetAsync(serverUrl);
                    response.EnsureSuccessStatusCode();
                    string json = await response.Content.ReadAsStringAsync();

                    // JSON-String deserialisieren und in eine ObservableCollection einlesen
                    ObservableCollection<Geburtstag> geburtstage = JsonSerializer.Deserialize<ObservableCollection<Geburtstag>>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Abrufen der Daten: {ex.Message}");
                MessageBox.Show($"Fehler beim Abrufen der Daten: {ex.Message}");
            }

            List<String> strings = listToString();
            listView.ItemsSource = strings;
        }

        //Daten an den Server schicken
        static async Task SendGeburtstageAsync(ObservableCollection<Geburtstag> geburtstage)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string serverUrl = "http://localhost:8080/geburtstage";
                    string json = JsonSerializer.Serialize(geburtstage);
                    var content = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(serverUrl, content);

                    response.EnsureSuccessStatusCode();

                    Trace.WriteLine("Daten erfolgreich an den Server gesendet.");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Fehler beim Senden der Daten an den Server: {ex.Message}");
                MessageBox.Show($"Fehler beim Senden der Daten an den Server: {ex.Message}");
            }
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
                        Geburtstag geb = new Geburtstag(name, day, month, year);
                        geburtstagsliste.Add(geb);
                        MessageBox.Show($"<{geb.ToString()}> wurde erfolgreich hinzugefügt.");
                    }
                    else
                    {
                        MessageBox.Show("Geburtsjahr der Person auswählen!");
                    }
                }
                //Geburtsjahr wird nicht gespeichert
                else
                {
                    Geburtstag geb = new Geburtstag(name, day, month);
                    geburtstagsliste.Add(geb);
                    MessageBox.Show($"<{geb.ToString()}> wurde erfolgreich hinzugefügt.");
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

        private static ObservableCollection<Geburtstag> SortByDate(ObservableCollection<Geburtstag> geburtstagsliste)
        {
            return new ObservableCollection<Geburtstag>(geburtstagsliste.OrderBy(g => g.month).ThenBy(g => g.day));
        }

        private void buttonLaden_Click(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        private void buttonSpeichern_Click(object sender, RoutedEventArgs e)
        {
            SendGeburtstageAsync(geburtstagsliste);
        }
    }
}

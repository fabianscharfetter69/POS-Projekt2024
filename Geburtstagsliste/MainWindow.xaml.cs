using Geburtstagsliste;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        private async void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
                    string id = geburtstagsliste[index].Id;
                    string url = $"http://localhost:8081/geb-liste/geburtstag/{id}";

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            HttpResponseMessage response = await client.DeleteAsync(url);

                            //Laden in Ausgabe
                            getGeburtstageFromServer();

                            if (response.IsSuccessStatusCode)
                            {
                                Trace.WriteLine($"Geburtstag mit ID {id} erfolgreich gelöscht.");
                            }
                            else
                            {
                                Trace.WriteLine($"Fehler beim Löschen des Geburtstags mit ID {id}. Statuscode: {response.StatusCode}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine($"Fehler beim Löschen des Geburtstags: {ex.Message}");
                        }
                    }
                }
            }            
        }

        //Laden der Daten vom Server
        public async void getGeburtstageFromServer()
        {
            try
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
                        erstelleAusgabe();
                    }
                    else
                    {
                        MessageBox.Show($"Server returned {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}");
            }
        }



        //Geburtstag zum Server senden
        private async Task addGeburtstagToServer(Geburtstag geb)
        {
            using (HttpClient client = new HttpClient())
            {
                // Url zum posten
                string url = "http://localhost:8081/geb-liste/geburtstag";

                //Json erstellen
                string data = "{";
                data += $"  \"name\": \"{geb.Name}\",     \"day\": \"{geb.Day}\",     \"month\": \"{geb.Month}\",     \"year\": \"{geb.Year}\"  ";
                data += "}";
                Trace.WriteLine(data);

                try
                {
                    var content = new StringContent(data, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    response.EnsureSuccessStatusCode();

                    Console.WriteLine("Daten erfolgreich gesendet.");
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Fehler beim Senden der Daten: {e.Message}");
                }
            }
        }
        private void erstelleAusgabe()
        {
            if(checkBox.IsChecked == true)
            {
                string selectedDate = Calendar.DisplayDate.ToShortDateString();
                Trace.WriteLine(selectedDate);

                //Datum splitten
                string[] arrDatum = selectedDate.Split(".");
                string month = arrDatum[1];
                Trace.WriteLine(month);


                //Ausgabe aktualisieren
                string monat = getMonatString(month);
                labelGeburtstage.Content = $"Geburtstage im {monat}:";

                List<String> temp = new List<String>();
                foreach(Geburtstag geb in geburtstagsliste)
                {
                    if(geb.Month == month)
                    {
                        temp.Add(geb.ToString());
                    }
                }
                listView.ItemsSource = temp;
            }
            else
            {
                labelGeburtstage.Content = "Alle Geburtstage:";

                //Ausgabe aktualisieren
                Trace.WriteLine("Ausgabe wird aktualisiert");
                List<String> strings = listToString();
                listView.ItemsSource = strings;
            }
        }

        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            erstelleAusgabe();
        }

        private void Calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            erstelleAusgabe();
        }

        private String getMonatString(string s)
        {
            string monat = "";
            switch (s)
            {
                case "01":
                    monat = "Januar";
                    break;
                case "02":
                    monat = "Februar";
                    break;
                case "03":
                    monat = "März";
                    break;
                case "04":
                    monat = "April";
                    break;
                case "05":
                    monat = "Mai";
                    break;
                case "06":
                    monat = "Juni";
                    break;
                case "07":
                    monat = "Juli";
                    break;
                case "08":
                    monat = "August";
                    break;
                case "09":
                    monat = "September";
                    break;
                case "10":
                    monat = "Oktober";
                    break;
                case "11":
                    monat = "November";
                    break;
                case "12":
                    monat = "Dezember";
                    break;
            }
            return monat;
        }
    }
}
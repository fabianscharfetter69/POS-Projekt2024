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
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear; year >= 1900; year--)
            {
                Years.Add(year.ToString());
            }
            comboBoxGeburtsjahr.ItemsSource = Years;
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

                        SendGeburtstagToServerAsync(geb);
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

                    SendGeburtstagToServerAsync(geb);
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
            GetGeburtstageFromServerAsync().Wait();
        }

        private void listView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Popup anzeigen
            int index = listView.SelectedIndex;

            if (index >= 0 && index < geburtstagsliste.Count)
            {
                Trace.WriteLine($"List View DoubleClick -> {geburtstagsliste[index].name}");
                var result = MessageBox.Show($"Do you want to delete {geburtstagsliste[index].name}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Element löschen
                    geburtstagsliste.Remove(geburtstagsliste[index]);
                    listView.ItemsSource = geburtstagsliste;
                }
            }            
        }





        private async Task<List<Geburtstag>> GetGeburtstageFromServerAsync()
        {
            List<Geburtstag> geburtstage = new List<Geburtstag>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8081"); 
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("/geb-list/geburtstage");

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    geburtstage = JsonSerializer.Deserialize<List<Geburtstag>>(jsonString);
                }
                else
                {
                    MessageBox.Show("Fehler");
                }
            }

            return geburtstage;
        }

        private async Task<bool> SendGeburtstagToServerAsync(Geburtstag geburtstag)
        {
            bool success = false;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8081"); // Setzen Sie die Basisadresse Ihrer Web-API ein
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonString = JsonSerializer.Serialize(geburtstag);
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/geb-list/geburtstag", content);

                if (response.IsSuccessStatusCode)
                {
                    success = true;
                }
                else
                {
                    MessageBox.Show("Fehler");
                }
            }

            return success;
        }


        private async Task<bool> DeleteGeburtstagFromServerAsync(string id)
        {
            bool success = false;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8081"); // Setzen Sie die Basisadresse Ihrer Web-API ein

                HttpResponseMessage response = await client.DeleteAsync($"/api/geburtstag/{id}"); // Stellen Sie sicher, dass die Endpunkt-URL Ihrer Web-API korrekt ist

                if (response.IsSuccessStatusCode)
                {
                    success = true;
                }
                else
                {
                    MessageBox.Show("Fehler");
                }
            }

            return success;
        }

    }
}
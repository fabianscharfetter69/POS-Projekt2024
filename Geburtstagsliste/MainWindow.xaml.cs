using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            ObservableCollection<string> Years = new ObservableCollection<string>();

            // Fülle die ComboBox mit den Jahren von 1900 bis zum aktuellen Jahr
            int currentYear = DateTime.Now.Year;
            for (int year = 1900; year <= currentYear; year++)
            {
                Years.Add(year.ToString());
            }

            // Setze die Datenquelle für die ComboBox
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
        }

        private void checkBoxGeburtsjahr_Checked(object sender, RoutedEventArgs e)
        {
            comboBoxGeburtsjahr.Visibility = Visibility.Visible;
        }

        private void checkBoxGeburtsjahr_Unchecked(object sender, RoutedEventArgs e)
        {
            comboBoxGeburtsjahr.Visibility = Visibility.Hidden;
        }

        public string listToString()
        {
            string s = "Januar:\n";
            //Jänner hinzufügen
            string month = "01";
            foreach(Geburtstag geb in geburtstagsliste)
            {
                if(geb.month == month)
                {

                }
            }

            return s;
        }
    }
}

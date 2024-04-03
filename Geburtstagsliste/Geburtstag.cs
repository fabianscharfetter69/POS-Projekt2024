using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Geburtstagsliste
{
    internal class Geburtstag
    {
        private string? id;
        private string name;
        private string day;
        private string month;
        private string year;
        public Geburtstag()
        {
        }


        //Konstruktor mit ID
        public Geburtstag(string? id, string name, string day, string month)
        {
            this.id = id;
            this.name = name;
            this.day = day;
            this.month = month;
        }

        public Geburtstag(string? id, string name, string day, string month, string year)
        {
            this.id = id;
            this.name = name;
            this.day = day;
            this.month = month;
            this.year = year;
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Day
        {
            get { return day; }
            set { day = value; }
        }

        public string Month
        {
            get { return month; }
            set { month = value; }
        }

        public string Year
        {
            get { return year; }
            set { year = value; }
        }

        public override string ToString()
        {
            if (year == null)
            {
                return day + "." + month + ".\t" + name;
            }
            else
            {
                return day + "." + month + ".\t" + name + ", " + year;
            }
        }
    }
}

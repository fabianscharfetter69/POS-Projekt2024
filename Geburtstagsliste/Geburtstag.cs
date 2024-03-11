using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geburtstagsliste
{
    internal class Geburtstag
    {
        public string name { get; set; }
        public string day { get; set; }
        public string month { get; set; }
        public string year { get; set; }

        public Geburtstag(string name, string day, string month) 
        { 
            this.name = name;   
            this.day = day;
            this.month = month;
        }

        public Geburtstag(string name, string day, string month, string year)
        {
            this.name = name;
            this.day = day;
            this.month = month;
            this.year = year;
        }

        public String ToString()
        {
           
            if(year is null)
            {
                return $"{day}.{month}.\t{name}";

            }
            else
            {
                return $"{day}.{month}.\t{name}, {year}";
            }
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t2
{
    public interface IPrototypeManager
    {

    }
    class PhoneBook
    {
        public DataPerson[] book { get; set; }  
        public PhoneBook DeepCopy()
        {
            PhoneBook clone = (PhoneBook)this.MemberwiseClone();
            for (int i = 0; i < book.Length; i++)
                clone.book[i] = book[i].DeepCopy();
            return clone;
        }
    }
    class DataPerson
    {
        public string name { get; set; }
        public Address address { get; set; }
        public DataPerson()
        {
            name = "";
            address = new Address();
        }
        // ShallowCopy vs DeepCopy
        public DataPerson ShallowCopy()
        {
            return (DataPerson)this.MemberwiseClone();
        }

        public DataPerson DeepCopy()
        {
            DataPerson clone = (DataPerson)this.MemberwiseClone(); // create shallowcopy of current object
            clone.name = string.Copy(name); // create new instance with the same value
            clone.address = new Address(address.city, address.state);

            return clone;
        }

        // init clones ShallowCopy vs DeepCopy
        public DataPerson ShallowCopy(string _name)
        {
            name = _name;
            return (DataPerson)this.MemberwiseClone();
        }
        public DataPerson DeepCopy(string name)
        {
            DataPerson clone = (DataPerson)this.MemberwiseClone();
            clone.name = name;
            clone.address = new Address(address.city, address.state);
            return clone;
        }


        public DataPerson(DataPerson ps)
        {
            this.name = ps.name;
            this.address = ps.address;
        }
        public void show()
        {
            Console.Write($"Name: {name}");
            Console.WriteLine($"; Address: city: {address.city}, state: {address.state}"); 
        }
    }

    class Address
    {
        public string city { get; set; }
        public string state { get; set; }
        public Address()
        {
            city = "";
            state = "";
        }
        public Address(string _city, string _state)
        {
            city = _city;
            state = _state;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            DataPerson personOrigin = new DataPerson();
            personOrigin.name = "Jeremy";
            personOrigin.address = new Address("Los Angeles", "California");

            DataPerson person_ShallowCopy = personOrigin.ShallowCopy();
            DataPerson person_DeepCopy = personOrigin.DeepCopy();
            DataPerson person_CopyCons = new DataPerson();
            person_CopyCons = personOrigin;


            // changing
            personOrigin.address.city = "Boston";
            personOrigin.address.state = "Massachusetts";

            // after change personOrigin
            personOrigin.show(); // Name: Jeremy; Address: city: Boston, state: Massachusetts
            // reference values change
            person_ShallowCopy.show(); // Name: Jeremy; Address: city: Boston, state: Massachusetts
            // keep the original data
            person_DeepCopy.show(); // Name: Jeremy; Address: city: Los Angeles, state: California
            // copy cons - same as ShallowCopy
            person_CopyCons.show(); // Name: Jeremy; Address: city: Boston, state: Massachusetts

            // init clone:
            DataPerson person_ShallowCopy2 = personOrigin.ShallowCopy("Kata");
            DataPerson person_DeepCopy2 = personOrigin.DeepCopy("Kata");
            personOrigin.address.city = "No name";
            personOrigin.address.state = "No name";
            person_ShallowCopy2.show(); // Name: Kata; Address: city: No name, state: No name
            person_DeepCopy2.show();    // Name: Kata; Address: city: Boston, state: Massachusetts



        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon
{
    public class Skills : ICloneable
    {
        public string Normal { get; set; }
        public string Special { get; set; }
        public string Ultimate { get; set; }
        public string Description { get; set; }

        public Skills(string _Normal, string _Special, string _Ultimate, string _Description)
        {
            Normal = _Normal;
            Special = _Special;
            Ultimate = _Ultimate;
            Description = _Description;
        }
        object ICloneable.Clone()
        {
            // Shallow copy
            return this.MemberwiseClone() as Skills;
        }
        public override string ToString()
        {
            return "Normal skill: " + Normal + " Special skill: " + Special + " Ultimate skill: " + Ultimate + "\nSkill Description: " + Description;
        }

    }
    public class PokeProperty : ICloneable
    {
        public Skills Skillset { get; set; }
        public string Description { get; set; }
        public PokeProperty(Skills s, string des)
        {
            Skillset = s;
            Description = des;
        }
        
        public void AddDescription()
        {
            Description = "Adding description for pokemon property";
        }
        object ICloneable.Clone()
        {
            // Deep copy
            var clone = (PokeProperty)this.MemberwiseClone(); 
            clone.Description = string.Copy(Description);
            clone.Skillset = (Skills)(clone.Skillset as ICloneable).Clone();

            return clone;
        }
        public override string ToString()
        {
            return Skillset.ToString() + "\nPokeProperty's Description: " + Description;
        }
        public void show()
        {
            Console.WriteLine(this.ToString());
        }
    }
    public class PokemonProfile : ICloneable
    {
        public string Name { get; set; }
        public PokeProperty Ability { get; set; }
        public PokemonProfile()
        {
            Name = "";
            Ability = null;
        }
        public PokemonProfile(PokeProperty ab)
        {
            Ability =  ab;
        }
        public PokemonProfile(string n, PokeProperty ab)
        {
            Name = n;
            Ability = ab;
        }
        object ICloneable.Clone()
        {
            var clone = (PokemonProfile)this.MemberwiseClone();
            clone.Name = string.Copy(Name);
            clone.Ability = (PokeProperty)(clone.Ability as ICloneable).Clone();


            return clone;
        }
        public PokemonProfile InitClone_Name(string name)
        {
            var clone = (PokemonProfile)this.MemberwiseClone();
            clone.Name = name;
            clone.Ability = (PokeProperty)(clone.Ability as ICloneable).Clone();


            return clone;
        }
        public override string ToString()
        {
            return "Name: " + Name + "\nPokeProperty: " + Ability.ToString();
        }
        public void show()
        {
            Console.WriteLine(this.ToString());
        }
    }
    public interface IPokePrototypeManager
    {
        void AddPrototype<T>(T prototype) where T : ICloneable;
        T GetPrototype<T>() where T : ICloneable;
    }

    public class PokePrototypeManager : IPokePrototypeManager
    {
        private readonly IDictionary<Type, ICloneable> prototypes = new Dictionary<Type, ICloneable>();
        public void AddPrototype<T>(T prototype) where T : ICloneable
        {
            var key = typeof(T); // get data type
            if (!prototypes.ContainsKey(key))
            {
                prototypes.Add(key, prototype);
            }


        }
        public T GetPrototype<T>() where T : ICloneable
        {
            var key = typeof (T);
            if (!prototypes.ContainsKey(key))
                throw new ArgumentOutOfRangeException(string.Format("Prototype for {0} does not exist!", key.Name));

            return (T)prototypes[key].Clone();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Skills s1 = new Skills("Fire", "Area Burning", "Global Rocket", "nothing");
            PokeProperty p1 = new PokeProperty(s1, "nothing");
            PokemonProfile poke1 = new PokemonProfile("Caterpod", p1);

            IPokePrototypeManager pm = new PokePrototypeManager();
            pm.AddPrototype<PokeProperty>(p1);
            pm.AddPrototype<PokemonProfile>(poke1);


            var clonePokeProperty = pm.GetPrototype<PokeProperty>();
            var clonePokeProfile = pm.GetPrototype<PokemonProfile>();



            p1.Description = "Change the description"; // nothing change for clonePokeProperty
            Console.WriteLine("---------------------------------------------------------------------------");
            clonePokeProperty.Description = "new description for clone";
            clonePokeProperty.show();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("---------------------------------------------------------------------------");
            clonePokeProfile.Name = "New name pokemon";
            clonePokeProfile.show();
            Console.WriteLine("---------------------------------------------------------------------------");


            //PokeProperty clone_p1 = p1.Clone();
            s1.Normal = "No skill";
            p1.Skillset = s1;
            PokemonProfile clone_poke1 = poke1.InitClone_Name("new name from clone_poke1");
            IPokePrototypeManager pm2 = new PokePrototypeManager();
            pm2.AddPrototype<PokemonProfile>(clone_poke1);
            var clone_profile_poke1 = pm2.GetPrototype<PokemonProfile>();


            Console.WriteLine("---------------------------------------------------------------------------");
            clone_profile_poke1.show();

            Console.WriteLine("---------------------------------------------------------------------------");



        }
    }
}

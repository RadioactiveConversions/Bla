using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro2033
{
    public abstract class Levelbar
    {
        private string name;
        private uint value;

        public Levelbar(string name)
        {
            this.name = name;
            this.value = 0;
        }

        public Levelbar(string name, uint value)
        {
            this.name = name;
            this.value = value;
        }

        public void Leveln()
        {
            this.value++;
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        public uint Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
            }
        }
    }

    public class Skill : Levelbar
    {

        public Skill(string name) : base(name)
        {

        }

        public Skill(string name, uint value)
            : base(name, value)
        {

        }

    }

    public class Attribut : Levelbar
    {

        public Attribut(string name) : base(name)
        {

        }

        public Attribut(string name, uint value)
            : base(name, value)
        {

        }
    }
}

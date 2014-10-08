using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro2033
{
    public class Character
    {
        private string name;
        private uint karma;
        private Attribut[] attribute;
        private Skill[] skills;
        private uint lp;
        private uint pp;

        public Character(string name)
        {
            this.name = name;
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

        public uint Karma
        {
            get
            {
                return this.karma;
            }

            set
            {
                this.karma = value;
            }
        }

        public Attribut[] Attribute
        {
            get
            {
                return this.attribute;
            }

            set
            {
                this.attribute = value;
            }
        }

        public Skill[] Skills
        {
            get
            {
                return this.skills;
            }

            set
            {
                this.skills = value;
            }
        }

        public uint Lp
        {
            get
            {
                return this.lp;
            }

            set
            {
                this.lp = value;
            }
        }


        public uint Pp
        {
            get
            {
                return this.pp;
            }
            set
            {
                this.pp = value;
            }
        }

        public void AddKarma(uint value)
        {
            this.karma += value;
        }
    }

    public class PlayerCharacter : Character
    {

        public PlayerCharacter(string name)
            : base(name)
        {

        }
    }

    public class NonPlayerCharacter : Character
    {

        public NonPlayerCharacter(string name)
            : base(name)
        {

        }
    }
}

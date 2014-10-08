using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro2033
{
    public class ShadowrunDice
    {
        private Random random;
        private uint[] dicethrow;
        private uint success;
        private uint slip;

        public ShadowrunDice()
        {
            this.random = new Random();
            this.ThrowDice(1);
        }

        public uint[] DiceThrow
        {
            get
            {
                return this.dicethrow;
            }
        }

        public uint Success
        {
            get
            {
                return this.success;
            }
        }

        public uint Slip
        {
            get
            {
                return this.slip;
            }
        }

        public void ThrowDice(uint value)
        {
            dicethrow = new uint[value];
            this.success = 0;
            this.slip = 0;
            for (uint i = 0; i < value; i++)
            {
                dicethrow[i] = (uint)(random.Next(1, 7));
                if (dicethrow[i] > 4)
                    success++;
                if (dicethrow[i] < 2)
                    slip++;
            }
        }

        override public string ToString()
        {
            string local = "";
            for (int i = 0; i < dicethrow.Length; i++)
            {
                if (i != dicethrow.Length - 1)
                    local += dicethrow[i] + ",";
                else
                    local += dicethrow[i] + " -> ";
            }
            if (success == 1)
                local += success + " Erfolg";
            else
                local += success + " Erfolge";

            return local;

        }
    }
}

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

        /// <summary>
        /// Methode würfelt "value" d6 würfel und speichert sie im "dicethrow" Array. Die davon gelungenen Würfel werden in "success" und gepatze Würfel in "slip" gepeichert.
        /// </summary>
        /// <param name="value"> ist die anzahl der geworfenen würfel</param>
        public void ThrowDice(uint value)
        {
            this.dicethrow = new uint[value];
            this.success = 0;
            this.slip = 0;
            for (uint i = 0; i < value; i++)
            {
                this.dicethrow[i] = (uint)(random.Next(1, 7));
                if (this.dicethrow[i] > 4)
                    this.success++;
                if (this.dicethrow[i] < 2)
                    this.slip++;
            }
        }

        override public string ToString()
        {
            string local = "";
            for (int i = 0; i < this.dicethrow.Length; i++)
            {
                if (i != this.dicethrow.Length - 1)
                    local += this.dicethrow[i] + ",";
                else
                    local += this.dicethrow[i] + " -> ";
            }
            if (this.success == 1)
                local += this.success + " Erfolg!";
            else
                local += this.success + " Erfolge!";

            if (this.slip >= (this.dicethrow.Length / 2))
                if (this.success == 0)
                    local += " KRITISCHER Patzer!";
                else
                    local += " Patzer!";

            return local;

        }
    }
}

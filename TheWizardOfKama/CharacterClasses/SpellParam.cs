using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWizardOfKama
{
    class SpellParam
    {
        int spellCost;
        public int SpellCost
        {
            get { return spellCost; }
            set { spellCost = value; }
        }
        int spellPower;
        public int SpellPower
        {
            get { return spellPower; }
            set { spellPower = value; }
        }

        public SpellParam(int spellCost, int spellPower)
        {
            this.spellCost = spellCost;
            this.spellPower = spellPower;
        }
    }
}

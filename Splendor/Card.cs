using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splendor
{
    /// <summary>
    /// class Card : attributes and methods to deal with a card
    /// </summary>
    class Card
    {
        public Ressources ress { get; set; }
        public int prestigePt { get; set; }
		public int level { get; set; }

		//tableau : l'index correspond à l'énumération, la valeur à la ressource requise
		public int[] cost { get; set; } = new int[4];


        public Card(int level, int prestigePt, int[] cost, int ress)
        {
            this.level = level;
            this.prestigePt = prestigePt;
            this.cost = cost;
            this.ress = (Ressources)ress;
        }        

        /// <summary>
        /// displays information about the card
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string res = "";

            res = Enum.GetName(typeof(Ressources), ress);
            //Be careful, those \t enables to split the string when clicking on a card
            res += "\t";

            if (prestigePt != 0)
            {
                res += prestigePt;
            }
            res += "\t";
            res += "\r\n\r\n";
            int boucle = 0;

            foreach (int i in cost)
            {

                string ressource = "";

                if (i != 0)
                {
                    ressource = "    ";
                    ressource += Enum.GetName(typeof(Ressources), boucle) + " ";
                    ressource += i + "\r\n";
                }

                res += ressource;
                boucle++;

            }
            return res;
        }
    }
}

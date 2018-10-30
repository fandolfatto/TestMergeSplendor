using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splendor
{

    /// <summary>
    /// class Player : attributes and methods to deal with a player
    /// </summary>
    class Player
    {
        public string name { get; set; }
        public int id { get; set; }
        public int[] ressources { get; set; }
        public int[] coins { get; set; }

		public Player(int id, string name, int[] ressources = null)
		{
			this.id = id;
			this.name = name;

			if (ressources == null)
			{
				ressources = new int[] { 0, 0, 0, 0, 0 };
			}

			this.ressources = ressources;
		}  	
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Splendor
{
    /// <summary>
    /// contains methods and attributes to connect and deal with the database
    /// TO DO : le modèle de données n'est pas super, à revoir!!!!
    /// </summary>
    class ConnectionDB
    {
        //connection to the database
        private SQLiteConnection m_dbConnection;
        private List<Card> allCards = new List<Card>();

        /// <summary>
        /// constructor : creates the connection to the database SQLite
        /// </summary>
        public ConnectionDB()
        {

            SQLiteConnection.CreateFile("Splendor.sqlite");

            m_dbConnection = new SQLiteConnection("Data Source=Splendor.sqlite;Version=3;");
            m_dbConnection.Open();

            //create and insert players
            CreateInsertPlayer();
            //Create and insert cards
            //TO DO
            CreateInsertCards();
            //Create and insert ressources
            //TO DO
            CreateInsertRessources();

			ImportCardCsv();

		}


        /// <summary>
        /// get the list of cards according to the level
        /// </summary>
        /// <returns>cards stack</returns>
        public List<Card> GetListCardAccordingToLevel(int level)
        {
            var sql = "SELECT id,fkRessource,nbPtPrestige,level FROM card where level = " + level;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            List<Card> listCard = new List<Card>();

            while (reader.Read())
            {
                sql = "select cost.nbRessource FROM cost where cost.fkCard = " + reader["id"].ToString();
                SQLiteCommand costCommand = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader costReader = costCommand.ExecuteReader();

                var cost = new List<int>();

                while (costReader.Read())
                {
                    cost.Add(int.Parse(costReader[0].ToString()));
                }

                listCard.Add(new Card(int.Parse(reader["level"].ToString()), int.Parse(reader["nbPtPrestige"].ToString()), cost.ToArray(), int.Parse(reader["fkRessource"].ToString())));

            }

            return listCard;
        }


        /// <summary>
        /// create the "player" table and insert data
        /// </summary>
        private void CreateInsertPlayer()
        {
            string sql = "CREATE TABLE player (id INT PRIMARY KEY, pseudo VARCHAR(20))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "insert into player (id, pseudo) values (0, 'Fred')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "insert into player (id, pseudo) values (1, 'Harry')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "insert into player (id, pseudo) values (2, 'Sam')";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }


        /// <summary>
        /// get the name of the player according to his id
        /// </summary>
        /// <param name="id">id of the player</param>
        /// <returns></returns>
        public string GetPlayerName(int id)
        {
            string sql = "select pseudo from player where id = " + id;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            string name = "";
            while (reader.Read())
            {
                name = reader["pseudo"].ToString();
            }
            return name;
        }

        /// <summary>
        /// create the table "ressources" and insert data
        /// </summary>
        private void CreateInsertRessources()
        {
            string sql = "CREATE TABLE ressource (id INTEGER PRIMARY KEY, name VARCHAR(20))";
            var command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "INSERT INTO ressource (id,name) values(" + (int)Ressources.Rubis + ",\"rubis\")";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "INSERT INTO ressource (id,name) values(" + (int)Ressources.Emeraude + ",\"emeraude\")";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "INSERT INTO ressource (id,name) values(" + (int)Ressources.Onyx + ",\"onyx\")";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "INSERT INTO ressource (id,name) values(" + (int)Ressources.Saphir + ",\"saphir\")";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "INSERT INTO ressource (id,name) values(" + (int)Ressources.Diamand + ",\"diamand\")";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();


        }


        /// <summary>
        ///  create tables "cards", "cost" and insert data
        /// </summary>
        private void CreateInsertCards()
        {
            string sql = "CREATE TABLE card (id INTEGER PRIMARY KEY, fkRessource INT, level INT, nbPtPrestige INT, fkPlayer INT)";
            var command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "CREATE TABLE cost (id INTEGER PRIMARY KEY, fkCard INT, fkRessource INT, nbRessource INT)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

        }



        /// <summary>
        /// Add card in the database
        /// </summary>
        /// <param name="level"></param>
        /// <param name="ressource"></param>
        /// <param name="prestige"></param>
        /// <param name="cost"></param>
        /// <param name="player"></param>
        public void AddCard(int level, int ressource, int prestige, int[] cost, int player = 0)
        {
            var sql = "INSERT INTO card(fkRessource,level,nbPtPrestige) values(" + ressource + ", " + level + "," + prestige + ")";
            var command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "select last_insert_rowid() as last";
            command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader read = command.ExecuteReader();

            while (read.Read())
            {

                for (int i = 0; i < cost.Length; i++)
                {
                    sql = "INSERT INTO cost(fkCard,fkRessource,nbRessource) values(" + read["last"] + "," + (i) + "," + cost[i] + ")";
                    command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
            }

        }

        /// <summary>
        /// Import card from csv to put in the database
        /// </summary>
        public void ImportCardCsv(string csv = "./cards.csv")
        {
            using (var reader = new StreamReader(csv))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    var newValues = new int[8];

                    for (int x = 0; x < values.Length; x++)
                    {
                        newValues[x] = int.Parse(values[x].ToString() == "" ? "0" : values[x].ToString());
                    }

                    this.AddCard(newValues[0], newValues[1], newValues[2], new int[] { newValues[3], newValues[4], newValues[5], newValues[6], newValues[7] });
                }
            }
        }

    }
}

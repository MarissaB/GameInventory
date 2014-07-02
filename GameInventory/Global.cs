using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GameInventory
{
    public partial class Global
    {
        public static List<string> Platforms = new List<string>(new string[] 
        {   "Gameboy", "Gameboy Color", "Gameboy Advance", 
            "Gamecube",
            "Nintendo 3DS", "Nintendo DS", 
            "Nintendo 64",
            "PC", 
            "Wii", "Wii U",
            "Xbox", "Xbox 360"});

        public static List<string> Languages = new List<string>(new string[] 
        {   "English", 
            "Japanese" });

        public static Game SelectedGame = new Game();


        public static string QueryBuilder(string search)
        {
            string[] searchterms = search.Split(' ');
            string query = "SELECT * FROM Games ";

            if (searchterms.Length > 1)
            {
                foreach (string x in searchterms)
                {
                    if (x.Equals(searchterms.First()))
                    {
                        query = query + "WHERE Name LIKE '%" + x + "%' AND ";
                    }
                    if (x.Equals(searchterms.Last()))
                    {
                        query = query + "Name LIKE '%" + x + "%' ";
                    }
                    else
                    {
                        query = query + "Name LIKE '%" + x + "%' AND ";
                    }
                }
            }

            else
            {
                query = query + "WHERE Name LIKE '%" + search + "%' ";
            }
           // query = query + "ORDER BY CASE WHEN Name LIKE '" + search + "%' THEN 0 ELSE 1 END ASC, Name ASC";
            return query;  
        }

        public static string AdvancedQueryBuilder(string columntitle, string columndata)
        {
            string advancedquery = columntitle + " = '" + columndata + "'";
            return advancedquery;
        }

        public static string AdvancedBooleanQueryBuilder(string columntitle, int selection)
        {
            string advancedbooleanquery = "";
            if (selection == 1)
            {
                advancedbooleanquery = columntitle + " = 'True'";
            }

            if (selection == 2)
            {
                advancedbooleanquery = columntitle + " = 'False'";
            }

            return advancedbooleanquery;
        }

        public static DataTable Search(string query)
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["Local"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("SearchResults");
                sda.Fill(dt);
                return dt;
            }
        }

       public static void AddGame(Game tetris)
        {
            GamingEntities context = new GamingEntities();
            context.Games.Add(tetris);
            context.SaveChanges();
        }

        public static void DeleteGame(int id)
       {
           GameRepository gr = new GameRepository();
           Game frogger = gr.GetById(id);
           gr.Delete(frogger);
           gr.SaveChanges();
           Global.SelectedGame = new Game();
       }

        public static void EditGame(Game tetris)
        {
            GameRepository gr = new GameRepository();
            gr.Update(tetris);
            gr.SaveChanges();
            Global.SelectedGame = new Game();
        }
    }
}

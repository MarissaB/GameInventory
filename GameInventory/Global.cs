using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInventory
{
    public partial class Global
    {
        public static string programmer = "Marissa Berresford";

        public static string QueryBuilder(string search)
        {
            string[] searchterms = search.Split(' ');
            string query = "SELECT * FROM Games ";
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
            query = query + "ORDER BY CASE WHEN Name LIKE '" + search + "%' THEN 0 ELSE 1 END ASC, Name ASC";
            return query;  
        }
    }
}

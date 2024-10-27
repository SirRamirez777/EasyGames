using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyGames.Models
{
    public class Clients
    {
        [Key]
            public int ClientID { get; set; }
            public string Name { get; set; }
        public string Surname { get; set; }
        public decimal ClientBalance { get; set; }

        // Navigation property for related transactions
        public ICollection<Transaction> Transactions { get; set; }




    }
}
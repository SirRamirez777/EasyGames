using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using static EasyGames.Models.Clients;

namespace EasyGames.Models
{
    public class Transaction
    {
        [Key]
            public int TransactionID { get; set; }
            public int ClientID { get; set; }
            public decimal Amount { get; set; }
            //public DateTime TransactionDate { get; set; }
            public string Comment { get; set; }
            public virtual Clients Client { get; set; }
            public int TransactionTypeID { get; set; }

    }
}
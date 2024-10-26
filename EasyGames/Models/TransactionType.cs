using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EasyGames.Models
{
    public class TransactionType
    {
        [Key]
        public int TransactionTypeID { get; set; }
        public string TransactionTypeName { get; set; }
    }
}
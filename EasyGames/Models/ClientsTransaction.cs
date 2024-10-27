using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EasyGames.Models
{
    public class ClientsTransaction
    {
        public class Client
        {
            [Key]
            public int ClientID { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public decimal ClientBalance { get; set; }

            // Navigation property for related transactions
            public ICollection<Transaction> Transactions { get; set; }
        }

        public class Transaction
        {
            [Key]
            public int TransactionID { get; set; }

            // Foreign key for Client
            [ForeignKey("Client")]
            public int ClientID { get; set; }

            // Foreign key for TransactionType
            [ForeignKey("TransactionType")]
            public int TransactionTypeID { get; set; }

            public decimal Amount { get; set; }
            public string Comment { get; set; }

            // Navigation properties
            public Client Client { get; set; }
            public TransactionType TransactionType { get; set; }
        }
        public class TransactionType
        {
            [Key]
            public int TransactionTypeID { get; set; }

            [Required]
            public string TransactionTypeName { get; set; } // "Debit" or "Credit"

            // Navigation property for related transactions
            public ICollection<Transaction> Transactions { get; set; }
        }

    }
}
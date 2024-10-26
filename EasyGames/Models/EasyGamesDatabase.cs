using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EasyGames.Models
{
    public class EasyGamesDatabase:DbContext
    {
        public EasyGamesDatabase( ): base("EasyGames"){ }

        public DbSet <Clients> Clients { get; set; }
        public DbSet <Transaction> Transactions { get; set; }

        public System.Data.Entity.DbSet<EasyGames.Models.TransactionType> TransactionTypes { get; set; }
    }
}
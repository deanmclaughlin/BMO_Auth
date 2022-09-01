using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BMO_Auth.Data;

namespace BMO_Auth.Models
{
    [Table("account")]
    [Index("AccountTypeId", Name = "AccountTypeID")]
    [Index("ClientId", Name = "ClientID")]
    public partial class Account
    {
        public Account()
        {
        }
        public Account(decimal balance)
        {
            Balance = balance;
        }

        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column("ClientID", TypeName = "int(11)")]
        public int ClientId { get; set; }
        [Column("AccountTypeID", TypeName = "int(11)")]
        public int AccountTypeId { get; set; }
        [Precision(9, 2)]
        public decimal Balance { get; set; }
        // [DataType(DataType.Date)]
        public DateOnly InterestAppliedDate { get; set; }

        [NotMapped]
        public string FormattedAcctNumber
        {
            get
            {
                return (this.Id.ToString("0000000"));
            }
        }

        [NotMapped]
        public string FormattedBalance
        {
            get
            {
                return Balance.ToString("C2");
            }
        }

        [NotMapped]
        public string StrIntDate
        {
            get
            {
                // return (InterestAppliedDate.ToString("yyyy, dd MMMM"));
                return (InterestAppliedDate.ToString("yyyy-MM-dd"));

            }
        }

        /*
        private decimal IntRate
        {
            get
            {
                decimal rate = 0.50m;
                using (DatabaseContext dbcntxt = new DatabaseContext())
                {
                    foreach (Accounttype type in dbcntxt.Accounttypes)
                    {
                        if (type.Id == this.AccountTypeId)
                        {
                            rate = type.InterestRate;
                        }
                    }
                }
                return rate;
            }
        }
        */

        /*
        public decimal ApplyInterest()
        {
            bool vip = this.Client.VIPClient;
            decimal apply_rate = this.AccountType.InterestRate;          
            if (vip) { apply_rate = apply_rate + 1.00m; }
            Balance = Decimal.Round(Balance * (1.00m + apply_rate / 100.00m), 2);
            return Balance;
        }

        public decimal Deposit(decimal amount)
        {
            if (amount <=0)
            {
                throw new InvalidOperationException("\n\n*** DEPOSIT AMOUNT MUST BE POSITIVE ***\n\n");
            }
            else
            {
                Balance = Decimal.Round(Balance + amount, 2);
            }
            return Balance;
        }

        public decimal Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("\n\n*** WITHDRAWAL AMOUNT MUST BE POSITIVE ***\n\n");
            }
            else if (amount > Balance)
            {
                throw new InvalidOperationException("\n\n*** INSUFFICIENT FUNDS ***\n\n");
            }
            else
            {
                Balance = Decimal.Round(Balance - amount, 2);
            }
            return Balance;
        }
        */

        [ForeignKey("AccountTypeId")]
        [InverseProperty("Accounts")]
        public virtual Accounttype AccountType { get; set; } = null!;
        [ForeignKey("ClientId")]
        [InverseProperty("Accounts")]
        public virtual Client Client { get; set; } = null!;
    }
}

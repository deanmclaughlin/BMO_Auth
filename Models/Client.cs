using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using BMO_Auth.Data;

namespace BMO_Auth.Models
{
    [Table("client")]
    public partial class Client
    {
        public Client()
        {
            Accounts = new HashSet<Account>();
        }

        [Key]
        [Column("ID", TypeName = "int(11)")]
        public int Id { get; set; }
        [StringLength(20)]
        public string FirstName { get; set; } = null!;
        [StringLength(20)]
        public string LastName { get; set; } = null!;
        // [DataType(DataType.Date)]
        public DateOnly BirthDate { get; set; }
        [StringLength(50)]
        public string HomeAddress { get; set; }

        [NotMapped]
        public string FormattedClientId
        {
            get
            {
                return (Id.ToString("000-000-0000"));
            }
        }

        [NotMapped]
        public string NameAndId
        {
            get
            {
                return (FormattedClientId + ": " + LastName + ",  " + FirstName);
            }
        }

        [NotMapped]
        public string StrBday
        {
            get
            {
                // return (BirthDate.ToString("yyyy, dd MMMM"));
                return (BirthDate.ToString("yyyy-MM-dd"));

            }
        }

        [NotMapped]
        public decimal TotalDeposits
        {
            get
            {
                decimal total = 0.00m;
                using (ApplicationDbContext _client_dbcntxt = new ApplicationDbContext())
                {
                    foreach (Account account in _client_dbcntxt.Accounts.ToList())
                    {
                        if (account.ClientId == this.Id)
                        {
                            total += account.Balance;
                        }
                    }
                }
                return total;
            }
        }

        [NotMapped]
        public string StrTotalDep
        {
            get
            {
                return (TotalDeposits.ToString("C2"));
            }
        }

        [NotMapped]
        public bool VIPClient
        {
            get
            {
                return this.TotalDeposits >= 100000.00m;
            }
        }

        [InverseProperty("Client")]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}

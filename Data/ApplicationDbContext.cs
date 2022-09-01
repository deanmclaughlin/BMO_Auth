using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BMO_Auth.Models;

namespace BMO_Auth.Data
{
    public partial class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Accounttype> Accounttypes { get; set; } = null!;
        public virtual DbSet<Client> Clients { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;database=bmo_auth",
                                        new MySqlServerVersion(new Version(10, 4, 24)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseCollation("utf8_general_ci")
               .HasCharSet("utf8");
            
            modelBuilder.Entity<Account>(entity =>
            {
                base.OnModelCreating(modelBuilder);

                entity.HasOne(d => d.AccountType)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.AccountTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_ibfk_2");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_ibfk_1");
            });

            modelBuilder.Entity<Accounttype>()
              .HasData(
                new Accounttype
                {
                    Id = 1,
                    Name = "Chequing",
                    InterestRate = 0.25m
                },
                new Accounttype
                {
                    Id = 2,
                    Name = "Standard Savings",
                    InterestRate = 0.75m
                },
                new Accounttype
                {
                    Id = 3,
                    Name = "TFSA",
                    InterestRate = 1.95m
                });

            modelBuilder.Entity<Client>()
              .HasData(
                new Client
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Smith-Jones",
                    BirthDate = new DateOnly(1970, 12, 25),
                    HomeAddress = "123 Doe Street"
                },
                new Client
                {
                    Id = 2,
                    FirstName = "Jacob",
                    LastName = "Jingleheimer",
                    BirthDate = new DateOnly(1987, 8, 22),
                    HomeAddress = "3765 Main Crescent"
                },
                new Client
                {
                    Id = 3,
                    FirstName = "Mary",
                    LastName = "Donovan",
                    BirthDate = new DateOnly(1958, 4, 4),
                    HomeAddress = "3975 Lark Close"
                },
                new Client
                {
                    Id = 4,
                    FirstName = "Peg",
                    LastName = "Bundy",
                    BirthDate = new DateOnly(1994, 6, 30),
                    HomeAddress = "14 Pinecone Way"
                });

            modelBuilder.Entity<Account>()
              .HasData(
                new Account()
                {
                    Id = 1,
                    ClientId = 1,
                    AccountTypeId = 1,
                    Balance = 9035.62m,
                    InterestAppliedDate = new DateOnly(2022, 7, 31)
                },
                new Account()
                {
                    Id = 2,
                    ClientId = 1,
                    AccountTypeId = 2,
                    Balance = 510.37m,
                    InterestAppliedDate = new DateOnly(2022, 7, 31)
                },
                new Account()
                {
                    Id = 3,
                    ClientId = 2,
                    AccountTypeId = 3,
                    Balance = 19300.00m,
                    InterestAppliedDate = new DateOnly(2022, 7, 31)
                },
                new Account()
                {
                    Id = 4,
                    ClientId = 3,
                    AccountTypeId = 2,
                    Balance = 5495.53m,
                    InterestAppliedDate = new DateOnly(2022, 7, 31)
                },
                new Account()
                {
                    Id = 5,
                    ClientId = 3,
                    AccountTypeId = 1,
                    Balance = 39450.78m,
                    InterestAppliedDate = new DateOnly(2022, 7, 31)
                },
                new Account()
                {
                    Id = 6,
                    ClientId = 4,
                    AccountTypeId = 2,
                    Balance = 23.96m,
                    InterestAppliedDate = new DateOnly(2022, 7, 31)
                },
                new Account()
                {
                    Id = 7,
                    ClientId = 2,
                    AccountTypeId = 3,
                    Balance = 98000.00m,
                    InterestAppliedDate = new DateOnly(2022, 7, 31)
                });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
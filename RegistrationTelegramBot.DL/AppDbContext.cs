using RegistrationTelegramBot.Core;
using RegistrationTelegramBot.DL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationTelegramBot.DL
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CategoryType> CategoryType { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Currency> Currency { get; set; }

        public DbSet<Client> Client { get; set; }

        private string _connectionString;
        public AppDbContext(IOptions<AppSettings> appSettings)
        {
            _connectionString = appSettings.Value.CONNECTION_STRING;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var serverVersion = MySqlServerVersion.AutoDetect(_connectionString);
            optionsBuilder.UseMySql(_connectionString, serverVersion);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка первичных ключей и отношений
            modelBuilder.Entity<User>()
                .HasMany(u => u.Transactions)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Categories)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Accounts)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Transactions)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.CategoryType)
                .WithMany(ct => ct.Categories)
                .HasForeignKey(c => c.TypeId);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.CurrencyInfo)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.Currency);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId);

            modelBuilder.Entity<Client>()
                .Property(c => c.Check)
                .HasColumnType("varbinary(max)");
        }
    }

}

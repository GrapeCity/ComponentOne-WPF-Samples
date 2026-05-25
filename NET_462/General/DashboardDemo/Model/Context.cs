using System;
using System.Collections.Generic;
using System.Text;
#if WINDOWS_UWP
using Windows.Storage;
using System.IO;
using Microsoft.EntityFrameworkCore;
#elif !XF
using System.Data.Entity;
#endif

namespace DashboardModel
{
#if !XF
    public class DashboardContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<LeadState> LeadStates { get; set; }
        public DbSet<LeadHistoryItem> LeadHistory { get; set; }
        public DbSet<BudgetItem> Budget { get; set; }
        public DbSet<RegionWiseSale> RegionWiseSales { get; set; }
        public DbSet<OpportunityItem> Opportunities { get; set; }
        public DbSet<ProfitAndSale> ProfitAndSales { get; set; }
        public DbSet<RegionSalesGoal> RegionSales { get; set; }

        public void Seed(Model model)
        {
            if (model.Categories != null)
                Categories.AddRange(model.Categories);
            if (model.Products != null)
                Products.AddRange(model.Products);
            if (model.Regions != null)
                Regions.AddRange(model.Regions);
            if (model.Shops != null)
                Shops.AddRange(model.Shops);
            if (model.Customers != null)
                Customers.AddRange(model.Customers);
            if (model.Campaigns != null)
                Campaigns.AddRange(model.Campaigns);
            if (model.Sales != null)
                Sales.AddRange(model.Sales);
            if (model.LeadStates != null)
                LeadStates.AddRange(model.LeadStates);
            if (model.LeadHistory != null)
                LeadHistory.AddRange(model.LeadHistory);
            if (model.Budget != null)
                Budget.AddRange(model.Budget);
            if (model.RegionWiseSales != null)
                RegionWiseSales.AddRange(model.RegionWiseSales);
            if (model.Opportunities != null)
                Opportunities.AddRange(model.Opportunities);
            if (model.ProfitAndSales != null)
                ProfitAndSales.AddRange(model.ProfitAndSales);
            if (model.RegionSales != null)
                RegionSales.AddRange(model.RegionSales);
            SaveChanges();
        }

#if WINDOWS_UWP

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Dashboard.db");
        }
#endif
    }
#endif
        }

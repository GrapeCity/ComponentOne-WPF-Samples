using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
#if !XF
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endif
using System.Linq;
using System.Threading.Tasks;

namespace DashboardModel
{

    public class Product
    {
#if !XF
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
#endif
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public virtual int CategoryId { get; set; }
        [XmlIgnore]
        public virtual Category Category { get; set; }
        public byte[] Image { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class Region
    {
#if !XF
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
#endif
        public int Id { get; set; }
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        [XmlIgnore]
        public virtual List<Shop> Shops { get; set; }
        [XmlIgnore]
        public virtual List<Customer> Customers { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class Shop
    {
#if !XF
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
#endif
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual int RegionId { get; set; }
        [XmlIgnore]
        public virtual Region Region { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class Category
    {
#if !XF
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
#endif
        public int Id { get; set; }
        public string Name { get; set; }
        [XmlIgnore]
        public virtual List<Product> Products { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class Customer
    {
#if !XF
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
#endif
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual int RegionId { get; set; }
        [XmlIgnore]
        public virtual Region Region { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class Campaign
    {
#if !XF
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
#endif
        public int Id { get; set; }
        public string Name { get; set; }
        [XmlElement(DataType = "date")]
        public DateTime Start { get; set; }
        [XmlElement(DataType = "date")]
        public DateTime Finish { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class Sale
    {
#if !XF
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
#endif
        public int Id { get; set; }
        [XmlElement(DataType = "date")]
        public DateTime Date { get; set; }
        public virtual int ProductId { get; set; }
        [XmlIgnore]
        public Product Product { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }
        public double Summ { get; set; }
        public virtual int CustomerId { get; set; }
        [XmlIgnore]
        public virtual Customer Customer { get; set; }
        public virtual int CampaignId { get; set; }
        [XmlIgnore]
        public virtual Campaign Campaign { get; set; }
    }

    public class LeadState
    {
#if !XF
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
#endif
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class LeadHistoryItem
    {
#if !XF
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
#endif
        public int Id { get; set; }
        public virtual int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        [XmlElement(DataType = "date")]
        public virtual DateTime Date { get; set; }
        public virtual int LeadStateId { get; set; }
        public virtual LeadState LeadState { get; set; }
        public string Comment { get; set; }
        public override string ToString()
        {
            return string.Format("{0} - {1} - {2}", Customer, Date, LeadState);
        }
    }

    public class BudgetItem
    {
#if !XF
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
#endif
        public int Id { get; set; }
        [XmlElement(DataType = "date")]
        public virtual DateTime Date { get; set; }
        public virtual int CategoryId { get; set; }
        [XmlIgnore]
        public virtual Category Category { get; set; }
        public double Goal { get; set; }
        public double Expences { get; set; }
        public double Sales { get; set; }
        public double Profit { get; set; }
    }

    public class RegionWiseSale
    {
#if !XF
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
#endif
        public int Id { get; set; }
        public int RegionId { get; set; }
        public string Region { get; set; }
        public string Customer { get; set; }
        public string Product { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Sales { get; set; }
        public double Profit { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime Date { get; set; }
    }

    public class OpportunityItem
    {
#if !XF
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
#endif
        public int Id { get; set; }
        public double Sales { get; set; }

        public int LevelId { get; set; }
        public string Level { get; set; }
        public string Customer { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime Date { get; set; }
    }

    public class ProfitAndSale
    {
#if !XF
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
#endif
        public int Id { get; set; }
        public double Sales { get; set; }
        public double Profit { get; set; }
        public double Quantity { get; set; }

        public string Product { get; set; }
        public string Customer { get; set; }
        public string Region { get; set; }
        public string Category { get; set; }
        public string Campaign { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime Date { get; set; }
    }

    public class RegionSalesGoal
    {
#if !XF
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
#endif
        public int Id { get; set; }

        public double Sales { get; set; }
        public double Profit { get; set; }
        public double Goal { get; set; }

        [XmlElement(DataType = "date")]
        public virtual DateTime Date { get; set; }

        public virtual int RegionId { get; set; }
        [XmlIgnore]
        public virtual Region Region { get; set; }
        [XmlIgnore]
        public string Name { get { return Region.ToString(); } }
    }

    public class Model
    {
        public readonly DateTime StartDate = new DateTime(2016, 1, 1);
        public readonly DateTime EndDate = new DateTime(2017, 8, 1);

        public Category[] Categories { get; set; }
        public Product[] Products { get; set; }
        public Region[] Regions { get; set; }
        public Shop[] Shops { get; set; }
        public Customer[] Customers { get; set; }
        public Campaign[] Campaigns { get; set; }
        public Sale[] Sales { get; set; }
        public LeadState[] LeadStates { get; set; }
        public LeadHistoryItem[] LeadHistory { get; set; }
        public BudgetItem[] Budget { get; set; }
        public RegionWiseSale[] RegionWiseSales { get; set; }
        public OpportunityItem[] Opportunities { get; set; }
        public ProfitAndSale[] ProfitAndSales { get; set; }
        public RegionSalesGoal[] RegionSales { get; set; }

#if WINDOWS_UWP||XF
        public static Model GetPopulated(Stream stream)
#else
        public static Model GetPopulated(string initialPath)
#endif
        {
#if !WINDOWS_UWP && !XF
            using (Stream stream = File.OpenRead(initialPath))
            {
#endif
                var model = (Model)new XmlSerializer(typeof(Model)).Deserialize(stream);
#if XF
            foreach (var product in model.Products)
                    product.Category = model.Categories[product.CategoryId];
                foreach (var customer in model.Customers)
                    customer.Region = model.Regions[customer.RegionId];
                foreach (var sale in model.Sales)
                {
                    sale.Campaign = model.Campaigns[sale.CampaignId];
                    sale.Customer = model.Customers[sale.CustomerId];
                    sale.Product = model.Products[sale.ProductId];
                }
                foreach (var budgetItem in model.Budget)
                    budgetItem.Category = model.Categories[budgetItem.CategoryId];
                foreach (var regionSaleGoal in model.RegionSales)
                    regionSaleGoal.Region = model.Regions[regionSaleGoal.RegionId];
#endif
                return model;
#if !WINDOWS_UWP && !XF
            }
#endif
        }

#if WINDOWS_UWP || XF
        public void Unload(Stream stream)
#else
        public void Unload(string filePath)
#endif
        {
#if !WINDOWS_UWP && !XF
            using (StreamWriter stream = File.CreateText(filePath))
            {
#endif
            new XmlSerializer(typeof(Model)).Serialize(stream, this);
#if !WINDOWS_UWP && !XF
            }
#endif
        }
    }
}

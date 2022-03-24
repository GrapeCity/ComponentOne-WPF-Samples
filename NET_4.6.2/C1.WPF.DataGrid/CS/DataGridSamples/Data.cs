using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Windows.Media;
using System.ComponentModel.DataAnnotations;

namespace DataGridSamples
{
    public class Data
    {
        static XDocument productsDoc, tennisDoc;

        static Data()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string productsResourcestring = assembly.GetManifestResourceNames().FirstOrDefault((resourceName) => resourceName.Contains(".Resources.products.xml"));
            if (productsResourcestring != null)
                productsDoc = XDocument.Load(new StreamReader(assembly.GetManifestResourceStream(productsResourcestring)));
            string tennisResourcestring = assembly.GetManifestResourceNames().FirstOrDefault((resourceName) => resourceName.Contains(".tennis.xml"));
            if (tennisResourcestring != null)
                tennisDoc = XDocument.Load(new StreamReader(assembly.GetManifestResourceStream(tennisResourcestring)));
        }

        #region ** products

        public static IEnumerable<Product> GetProducts()
        {
            return GetProducts(null);
        }
        public static IEnumerable<Product> GetProducts(Predicate<XElement> predicate)
        {
            var products = (from product in productsDoc.Descendants("Product")
                            where predicate == null || predicate(product)//product.Element("Image") != null && product.Element("Image").Value != "no_image_available_small.jpg"
                            select new Product
                            {
                                ProductNumber = product.Element("ProductNumber").Value,
                                Name = product.Element("Name").Value,
                                StandardCost = double.Parse(product.Element("StandardCost").Value, CultureInfo.InvariantCulture.NumberFormat),
                                ProductModelID = !string.IsNullOrEmpty(product.Element("ProductModelID").Value) ? int.Parse(product.Element("ProductModelID").Value) : 0,
                                ProductSubcategoryID = !string.IsNullOrEmpty(product.Element("ProductSubcategoryID").Value) ? uint.Parse(product.Element("ProductSubcategoryID").Value) : 0,
                                Description = product.Element("Description") != null ? product.Element("Description").Value : "",
                                Available = product.Element("Available") != null ? bool.Parse(product.Element("Available").Value) : (bool?)null,
                                ImageUrl = product.Element("Image") != null ? product.Element("Image").Value : null,
                                ExpirationDate = product.Element("ExpirationDate") != null ? !string.IsNullOrEmpty(product.Element("ExpirationDate").Value) ? (DateTime?)DateTime.Parse(product.Element("ExpirationDate").Value) : (DateTime?)null : (DateTime?)null,
                            });
            List<Product> result = new List<Product>(products);
            int i = 0;
            foreach (var product in result)
            {
                product.ID = i++;
                product.ExpirationDate = GetDate(product.StandardCost);
                product.Available = GetAvailable(product.Name);
            }
            return result;
        }

        private static bool? GetAvailable(string seed)
        {
            return (seed.Length % 3 == 0 ? (bool?)true : (seed.Length % 3 == 1 ? (bool?)false : (bool?)null));
        }

        private static DateTime? GetDate(double seed)
        {
            return new DateTime(DateTime.Now.Ticks - ((long)seed * 10000000000));
        }

        public static IEnumerable<Subcategory> GetSubCategories(Predicate<XElement> predicate)
        {
            var subCategories = (from subCategory in productsDoc.Descendants("Subcategory")
                                 where predicate == null || predicate(subCategory)
                                 select new Subcategory
                                 {
                                     ProductSubcategoryID = int.Parse(subCategory.Element("ProductSubcategoryID").Value),
                                     Name = subCategory.Element("Name").Value
                                 });
            return subCategories;
        }

        public static IEnumerable<Category> GetCategories(Predicate<XElement> predicate)
        {
            var categories = (from category in productsDoc.Descendants("Category")
                              where predicate == null || predicate(category)
                              select new Category
                              {
                                  ProductCategoryID = int.Parse(category.Element("ProductCategoryID").Value),
                                  Name = category.Element("Name").Value
                              });
            return categories;
        }

        public static IEnumerable<Model> GetModels()
        {
            var models = from model in productsDoc.Descendants("Model")
                         select new Model
                         {
                             ProductModelID = int.Parse(model.Element("ProductModelID").Value),
                             Name = model.Element("Name").Value
                         };
            return models;
        }

        #endregion

        #region ** tennis players

        public static IEnumerable<Player> GetPlayers(Predicate<XElement> predicate)
        {
            CultureInfo englishCulture = new CultureInfo("en-US");
            return (from player in tennisDoc.Descendants("player")
                    where predicate == null || predicate(player)
                    select new Player
                    {
                        Name = player.Attribute("Name").Value,
                        // the next string fails if user works in non-English culture <<IP>>
                        //     Birthday = DateTime.ParseExact(player.Attribute("Birthday").Value, "dd/MM/yyyy", CultureInfo.CurrentCulture.DateTimeFormat),
                        Birthday = DateTime.ParseExact(player.Attribute("Birthday").Value, "dd/MM/yyyy", englishCulture),
                        Birthplace = player.Attribute("Birthplace").Value,
                        Residence = player.Attribute("Residence").Value,
                        Height = player.Attribute("Height").Value,
                        Weight = player.Attribute("Weight").Value,
                        Plays = player.Attribute("Plays").Value,
                        TurnedPro = player.Attribute("TurnedPro").Value,
                        Coach = player.Attribute("Coach").Value,
                        Website = player.Attribute("Website").Value,
                        Photo = player.Attribute("Photo").Value
                    });
        }
        #endregion
    }

    public class Model
    {
        public int ProductModelID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Category : INotifyPropertyChanged
    {
        private int _productSubcategoryID;
        private string _name;

        public int ProductCategoryID
        {
            get { return _productSubcategoryID; }
            set { _productSubcategoryID = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ProductSubcategoryID")); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Name")); }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public class Subcategory : INotifyPropertyChanged
    {
        private int _productSubcategoryID;
        private string _name;

        public int ProductSubcategoryID
        {
            get { return _productSubcategoryID; }
            set { _productSubcategoryID = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ProductSubcategoryID")); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Name")); }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    [CustomValidation(typeof(Product), "ValidateAvailability")]
    public class Product : INotifyPropertyChanged, IEditableObject, IDataErrorInfo
    {
        private int _id;
        private double _standardCost;
        private string _name;
        private bool? _available;
        private string _productNumber;
        private Color? _color;
        private static Random rand = new Random();
        private Product _backup;

        [Display(AutoGenerateField = false)]
        public int ID { get { return _id; } set { _id = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ID")); } }

        [Required]
        [Display(Name = "Product Number")]
        public string ProductNumber
        {
            get
            {
                return _productNumber;
            }
            set
            {
                //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "ProductNumber" });
                _productNumber = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ProductNumber"));
            }
        }

        [Required]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "Name" });
                _name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        [Required]
        [Range(0, 10000)]
        [Display(Name = "Standard Cost")]
        [DisplayFormat(DataFormatString = "C")]
        public double StandardCost
        {
            get
            {
                return _standardCost;
            }
            set
            {
                //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "StandardCost" });
                _standardCost = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("StandardCost"));
            }
        }

        [Display(Name = "Model")]
        public int ProductModelID { get; set; }

        [Display(AutoGenerateField = false)]
        public long ProductSubcategoryID { get; set; }

        [Display(AutoGenerateField=false)]
        public string Description { get; set; }

        public bool? Available { get { return _available; } set { _available = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Available")); } }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Expiration Date")]
        [DisplayFormat(DataFormatString = "D")]
        public DateTime? ExpirationDate { get; set; }

        [Display(AutoGenerateField = false)]
        public Color Color
        {
            get
            {
                if (_color == null)
                {
                    byte[] color = new byte[4];
                    rand.NextBytes(color);
                    Color = Color.FromArgb((byte)(0xA0 | color[0]), color[1], color[2], color[3]);
                }
                return _color.Value;
            }
            set
            {
                _color = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Color"));
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public static ValidationResult ValidateAvailability(Product product)
        {
            return (product.Available.HasValue &&
                product.Available.Value &&
                (product.ExpirationDate < DateTime.Today)) ?
                new ValidationResult("The product shouldn't be available if it has expired.", new string[2] { "Available", "ExpirationDate" }) : ValidationResult.Success;
        }

        #region IEditableObject Members

        public void BeginEdit()
        {
            _backup = this.MemberwiseClone() as Product;
        }

        public void CancelEdit()
        {
            if (_backup != null)
            {
                try
                {
                    ID = _backup.ID;
                    ProductNumber = _backup.ProductNumber;
                    Name = _backup.Name;
                    StandardCost = _backup.StandardCost;
                    ProductModelID = _backup.ProductModelID;
                    ProductSubcategoryID = _backup.ProductSubcategoryID;
                    Description = _backup.Description;
                    Available = _backup.Available;
                    ImageUrl = _backup.ImageUrl;
                    ExpirationDate = _backup.ExpirationDate;
                }
                catch
                {
                }
                finally
                {
                    EndEdit();
                }
            }
        }

        public void EndEdit()
        {
            _backup = null;
        }

        #endregion

        string IDataErrorInfo.Error
        {
            get
            {
                return "";
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "ProductNumber":
                        if (string.IsNullOrEmpty(ProductNumber))
                            return "Product number required.";
                        break;
                    case "Name":
                        if (string.IsNullOrEmpty(Name))
                            return "Name required.";
                        break;
                    case "StandardCost":
                        if (StandardCost < 0)
                            return "Cost must be greater than zero";
                        break;
                    case "ProductModelID":
                        if (ProductModelID <= 0)
                            return "You must select a valid model.";
                        break;
                }
                return "";
            }
        }
    }

    public class ProductsCollection : List<Product> { }

    public class Player
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Birthplace { get; set; }
        public string Residence { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Plays { get; set; }
        public string TurnedPro { get; set; }
        public string Coach { get; set; }
        public string Website { get; set; }
        public string Photo { get; set; }
    }

    public class PersonInfo
    {
        public string Name { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }

        public static Dictionary<string, string> Regions
        {
            get
            {
                return All.Where(i => i.Region != null).GroupBy(i => i.Region.Trim()).ToDictionary(x => x.Key, x => x.Key);
            }
        }

        public static Dictionary<string, string> Countries
        {
            get
            {
                return All.Where(i => i.Country != null).GroupBy(i => i.Country.Trim()).ToDictionary(x => x.Key, x => x.Key);
            }
        }

        static List<PersonInfo> _all = null;
        public static List<PersonInfo> All
        {
            get
            {
                if (_all == null)
                {
                    _all = new List<PersonInfo>();
                    _all.Add(new PersonInfo() { Name = "John", Country = "US", Region = "North America" });
                    _all.Add(new PersonInfo() { Name = "Mark", Country = "Mexico", Region = "North America" });
                    _all.Add(new PersonInfo() { Name = "Cross", Country = "US", Region = "North America" });
                    _all.Add(new PersonInfo() { Name = "Smith", Country = "Canada", Region = "North America" });
                    _all.Add(new PersonInfo() { Name = "Bell", Country = "Canada", Region = "North America" });
                    _all.Add(new PersonInfo() { Name = "Graham", Country = "Canada", Region = "North America" });
                    _all.Add(new PersonInfo() { Name = "Emma", Country = "Brazil", Region = "South America" });
                    _all.Add(new PersonInfo() { Name = "Petra", Country = "Brazil", Region = "South America" });
                    _all.Add(new PersonInfo() { Name = "Volga", Country = "Argentina", Region = "South America" });
                    _all.Add(new PersonInfo() { Name = "Daisy", Country = "Argentina", Region = "South America" });
                    _all.Add(new PersonInfo() { Name = "Disy", Country = "Peru", Region = "South America" });
                    _all.Add(new PersonInfo() { Name = "Glenn", Country = "UK", Region = "Europe" });
                    _all.Add(new PersonInfo() { Name = "Johnny", Country = "UK", Region = "Europe" });
                    _all.Add(new PersonInfo() { Name = "Sharp", Country = "UK", Region = "Europe" });
                    _all.Add(new PersonInfo() { Name = "Nicholas", Country = "France", Region = "Europe" });
                    _all.Add(new PersonInfo() { Name = "Carla", Country = "France", Region = "Europe" });
                    _all.Add(new PersonInfo() { Name = "Chan", Country = "China", Region = "Asia" });
                    _all.Add(new PersonInfo() { Name = "Lisa", Country = "Taiwan", Region = "Asia" });
                    _all.Add(new PersonInfo() { Name = "Lina", Country = "Taiwan", Region = "Asia" });
                    _all.Add(new PersonInfo() { Name = "Lena", Country = "India", Region = "Asia" });
                    _all.Add(new PersonInfo() { Name = "Khan", Country = "India", Region = "Asia" });
                    _all.Add(new PersonInfo() { Name = "Monica", Country = "Australia", Region = "Australia" });
                }
                return _all;
            }
        }
    }
}

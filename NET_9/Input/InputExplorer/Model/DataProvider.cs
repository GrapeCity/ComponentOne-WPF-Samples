using System.Collections.ObjectModel;

namespace InputExplorer
{
    public class DataProvider
    {
        static DataProvider() { }

        public static ObservableCollection<Customer> GetCustomers()
        {
            ObservableCollection<Customer> data = new ObservableCollection<Customer>()
            {
                new Customer{ Name="Name 1", Age=22, Country="Argentina", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 2", Age=18, Country="Armenia", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 3", Age=15, Country="Australia", IsLicensed=false, IsMarried = false},
                new Customer{ Name="Name 4", Age=45, Country="Austria", IsLicensed=true, IsMarried = true},
                new Customer{ Name="Name 5", Age=44, Country="Belgium", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 6", Age=33, Country="Brazil", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 7", Age=30, Country="Cameroon", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 8", Age=18, Country="Canada", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 9", Age=19, Country="Chile", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 10", Age=22, Country="Colombia", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 11", Age=27, Country="China", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 12", Age=28, Country="Czech", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 13", Age=56, Country="Denmark", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 14", Age=75, Country="Egypt", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 15", Age=71, Country="France", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 16", Age=35, Country="Germany", IsLicensed=true, IsMarried = true},
                new Customer{ Name="Name 17", Age=55, Country="Greece", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 18", Age=42, Country="Guam", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 19", Age=32, Country="Iceland", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 20", Age=17, Country="India", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 21", Age=33, Country="Iran", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 22", Age=52, Country="Iraq", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 23", Age=51, Country="Ireland", IsLicensed=true, IsMarried = true},
                new Customer{ Name="Name 24", Age=38, Country="Italy", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 25", Age=28, Country="Japan", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 26", Age=58, Country="Korea", IsLicensed=false, IsMarried = false},
                new Customer{ Name="Name 27", Age=46, Country="Malaysia", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 28", Age=44, Country="Mexico", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 29", Age=31, Country="Monaco", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 30", Age=48, Country="Myanmar", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 31", Age=29, Country="Netherlands", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 32", Age=20, Country="New Zealand", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 33", Age=30, Country="Norway", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 34", Age=66, Country="Poland", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 35", Age=40, Country="Portugal", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 36", Age=59, Country="Russia", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 37", Age=19, Country="Romania", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 38", Age=18, Country="Singapore", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 39", Age=43, Country="South Africa", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 40", Age=37, Country="Spain", IsLicensed=true, IsMarried = true},
                new Customer{ Name="Name 41", Age=41, Country="Switzerland", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 42", Age=34, Country="Sweden", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 43", Age=52, Country="Thailand", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 44", Age=19, Country="Turkey", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 45", Age=21, Country="Ukraine", IsLicensed=true, IsMarried = false},
                new Customer{ Name="Name 46", Age=25, Country="United States", IsLicensed=true, IsMarried = true},
                new Customer{ Name="Name 47", Age=24, Country="Uruguay", IsLicensed=true, IsMarried = false},               
            };
            return data;
        }
    }
}

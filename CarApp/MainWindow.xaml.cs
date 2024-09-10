using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

using System.Threading.Tasks;
using System.Globalization;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Intrinsics.Arm;


namespace CarApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Vytvoření finálního objektu
        public class FinalCarReport
        {
            public string? modelName { get; set; }
            public string? priceWithoutTax { get; set; }
            public string? price { get; set; }
         
        }


        
        private void loadButton_Click(object sender, RoutedEventArgs e)
            
        {
            Button btn = (Button)sender;

            double ConvertToDouble(string s)
            {
                char systemSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
                double result = 0;
                try
                {
                    if (s != null)
                        if (!s.Contains(","))
                            result = double.Parse(s, CultureInfo.InvariantCulture);
                        else
                            result = Convert.ToDouble(s.Replace(".", systemSeparator.ToString()).Replace(",", systemSeparator.ToString()));
                }
                catch (Exception e)
                {
                    try
                    {
                        result = Convert.ToDouble(s);
                    }
                    catch
                    {
                        try
                        {
                            result = Convert.ToDouble(s.Replace(",", ";").Replace(".", ",").Replace(";", "."));
                        }
                        catch
                        {
                            throw new Exception("Wrong string-to-double format");
                        }
                    }
                }
                return result;
            }




            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = "xml";
            dlg.Filter = "XML Files (*.xml)|*.xml";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            

            // Get the selected file name and display in a TextBox 
            
            
                // Open document 
                string filename = dlg.FileName;
                string filepath = $"{filename}";
            


            DataSet dataSet = new DataSet();

            dataSet.ReadXml(@"" + filepath);


            DataTable dt = dataSet.Tables[0];





            int k = 0;


            foreach (DataRow row in dt.Rows)
            {
                k++;

            }

            tableButton.Visibility = Visibility.Collapsed;
       

            List<string> forCarList = new List<string>();

            List<string> forDateList = new List<string>();

            List<string> weekendDateIndexList = new List<string>();

            string[] forCars;

            string[] forDates;

            string[] weekends;

            int w = 0;

            //Smyčka pro zapsání indexů řádků, které mají víkendové datum a spočítání, kolik těch řádků je
            for (int i = 0; i <= k -1; i++) 
                
            {
              


                string forSaleDate = "";
                forSaleDate = dt.Rows[i]["datum_prodeje"].ToString();

                string evaluatedDate = "";

                evaluatedDate = (DateTime.Parse(((forSaleDate).ToString()))).DayOfWeek.ToString();


                if (evaluatedDate == "Saturday" ||  evaluatedDate == "Sunday")
                {
                    string index = i.ToString();

                    weekendDateIndexList.Add(index);
                    
                    w++ ;
                };
                
            }

       
            forDates = forDateList.ToArray();

            weekends = weekendDateIndexList.ToArray();

            string indexes = "";

         
            //Smyčka pro sečtení všech víkendových cen

            for (int i = 0; i <= w - 1; i++)
            {
                indexes = weekends[i];

                dt.Rows[i]["cena"].ToString();

                string stringedPrice = "";

                stringedPrice = dt.Rows[int.Parse(indexes)]["cena"].ToString();

            }



         

            List<string> weekendCarList = new List<string>();



            //Funkce pro sečtení všech víkendových jmen aut

            for (int i = 0; i <= w - 1; i++)
            {
                
                indexes = weekends[i];

             
                string stringedNames = "";

                stringedNames = dt.Rows[int.Parse(indexes)]["nazev_modelu"].ToString();


                weekendCarList.Add(stringedNames);

            }



            forCars = forCarList.ToArray();


            string repeatedNames = "";

            string[] loadedCars = ["Škoda Fabia", "Škoda Oktávia", "Škoda Felicia", "Škoda Forman", "Škoda Favorit"];

            string[] orderedArrayOfCars;

         
            string formRecord = "";



            List<string> orderedListOfCars = new List<string>();


            List<string> finalCarList = new List<string>();

            List<string> finalPriceList = new List<string>();


            List<string> finalSoldPriceList = new List<string>();



            List<double> finalDoublePriceList = new List<double>();
            List<double> finalWithoutTaxPriceList = new List<double>();

            List<double> totalDoublePriceList = new List<double>();
            List<double> totalWithoutTaxPriceList = new List<double>();

            List<string> stringedTotalDoublePriceList = new List<string>();


            //Funkce pro uspořádání opakujících se jmen

            void repeatedNameFunction(int indexOfModel)

            {

                for (int i = 0; i <= w - 1; i++)
                {

                    indexes = weekends[i];

                    string stringedNames = "";

                    string ifStringedNames = "";

                 

                    stringedNames = dt.Rows[int.Parse(indexes)]["nazev_modelu"].ToString();

                  

                    if (loadedCars[indexOfModel] == stringedNames)
                    {
                        
                        orderedListOfCars.Add(dt.Rows[int.Parse(indexes)]["nazev_modelu"].ToString());
                        

                        finalDoublePriceList.Add(ConvertToDouble(dt.Rows[int.Parse(indexes)]["cena"].ToString())); /*ConvertToDouble(dt.Rows[int.Parse(indexes)]["SOLDCARS"].ToString()*/
                        finalWithoutTaxPriceList.Add(priceWithoutTaxCalculator(ConvertToDouble(dt.Rows[int.Parse(indexes)]["cena"].ToString()), ConvertToDouble(dt.Rows[int.Parse(indexes)]["dph"].ToString())));


                        
                        repeatedNames = repeatedNames + dt.Rows[int.Parse(indexes)]["nazev_modelu"].ToString();

                        formRecord = ifStringedNames + repeatedNames;

                        finalCarList.Add(formRecord);

                    }

                   
                }
                 
            }

            

            //Funkce pro sečtení všech prodaných kusů z "víkendového záznamu" daného modelu dle jeho indexu

            void soldModelsCalculation(int indexOfModel)

            {  


                string loadedModelOfCar = "";
               
                double totalDoublePrice = 0;
                double totalWithoutTaxPrice = 0;


                loadedModelOfCar = loadedCars[indexOfModel];

                for (int i = 0; i <= w - 1; i++)
                {
                    
                    loadedModelOfCar = loadedCars[indexOfModel];


                    if (loadedModelOfCar == orderedArrayOfCars[i])
                    {
                      
                        totalDoublePrice = totalDoublePrice + finalDoublePriceList[i];
                        totalWithoutTaxPrice = totalWithoutTaxPrice + finalWithoutTaxPriceList[i];
                     
                    }
                }
                
                totalDoublePriceList.Add(totalDoublePrice * 1000);
                totalWithoutTaxPriceList.Add(totalWithoutTaxPrice * 1000);

              
            }


            //Funkce pro výpočet ceny bez daně

            double priceWithoutTaxCalculator(double priceWithTax, double taxRate)
            {
                double result = 0;
                double taxCoeficient = (taxRate + 100) / 100;

                result = priceWithTax - (priceWithTax - (priceWithTax / taxCoeficient));

                return result;
            }



            //Smyčka pro aplikování funkce na všechny nahrané modely

            for (int i = 0; i <= loadedCars.Length - 1; i++)

            {
                 repeatedNameFunction(i);
                

            }




            orderedArrayOfCars = orderedListOfCars.ToArray();

        
            //Smyčka pro aplikování funkce na všechny nahrané modely

            for (int i = 0; i <= loadedCars.Length - 1; i++)

            {
                soldModelsCalculation(i);
            }


            //Pro korektní formátování výsledné ceny

            NumberFormatInfo num = new CultureInfo("de-DE", false).NumberFormat;


            //Zaznamenání výsledných dat do objektů:

            FinalCarReport skodaFabia = new FinalCarReport();

                           skodaFabia.modelName = loadedCars[0];
                           skodaFabia.priceWithoutTax = decimal.Parse((totalWithoutTaxPriceList[0]).ToString(), num).ToString("#,##0.00", num);
                           skodaFabia.price = decimal.Parse((totalDoublePriceList[0]).ToString(), num).ToString("#,##0.00", num);


            FinalCarReport skodaOktavia = new FinalCarReport();

                           skodaOktavia.modelName = loadedCars[1];
                           skodaOktavia.priceWithoutTax = decimal.Parse((totalWithoutTaxPriceList[1]).ToString(), num).ToString("#,##0.00", num);
                           skodaOktavia.price = decimal.Parse((totalDoublePriceList[1]).ToString(), num).ToString("#,##0.00", num);


            FinalCarReport skodaFelicia = new FinalCarReport();

                           skodaFelicia.modelName = loadedCars[2];
                           skodaFelicia.priceWithoutTax = decimal.Parse((totalWithoutTaxPriceList[2]).ToString(), num).ToString("#,##0.00", num);
                           skodaFelicia.price = decimal.Parse((totalDoublePriceList[2]).ToString(), num).ToString("#,##0.00", num);

            FinalCarReport skodaForman = new FinalCarReport();

            skodaForman.modelName = loadedCars[3];
            skodaForman.priceWithoutTax = decimal.Parse((totalWithoutTaxPriceList[3]).ToString(), num).ToString("#,##0.00", num);
            skodaForman.price = decimal.Parse((totalDoublePriceList[3]).ToString(), num).ToString("#,##0.00", num);

            FinalCarReport skodaFavorit = new FinalCarReport();

            skodaFavorit.modelName = loadedCars[4];
            skodaFavorit.priceWithoutTax = decimal.Parse((totalWithoutTaxPriceList[4]).ToString(), num).ToString("#,##0.00", num);
            skodaFavorit.price = decimal.Parse((totalDoublePriceList[4]).ToString(), num).ToString("#,##0.00", num);



            //Zaznamenání výsledných dat do formy:
          
            formFabiaName.Text = skodaFabia.modelName;
            formFabiaPriceNoTax.Text = skodaFabia.priceWithoutTax;
            formFabiaPrice.Text = skodaFabia.price;

            formOktaviaName.Text = skodaOktavia.modelName;
            formOktaviaPriceNoTax.Text = skodaOktavia.priceWithoutTax;
            formOktaviaPrice.Text = skodaOktavia.price;

            formFeliciaName.Text = skodaFelicia.modelName;
            formFeliciaPriceNoTax.Text = skodaFelicia.priceWithoutTax;
            formFeliciaPrice.Text = skodaFelicia.price;

            formFormanName.Text = skodaForman.modelName;
            formFormanPriceNoTax.Text = skodaForman.priceWithoutTax;
            formFormanPrice.Text = skodaForman.price;


            formFavoritName.Text = skodaFavorit.modelName;
            formFavoritPriceNoTax.Text = skodaFavorit.priceWithoutTax;
            formFavoritPrice.Text = skodaFavorit.price;

        }

        //Vytvoření tabulky s načtenými daty
        public DataTable ReadXML(string file)
        {
            //create the DataTable that will hold the data
            DataTable table = new DataTable("XmlData");
            try
            {
                //open the file using a Stream
                using (Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    //create the table with the appropriate column names
                    table.Columns.Add("nazev_modelu", typeof(string));
                    table.Columns.Add("datum_prodeje", typeof(DateTime));
                    table.Columns.Add("cena", typeof(double));
                    table.Columns.Add("dph", typeof(double));


                    
                    return table;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return table;
            }
        }
    }
}
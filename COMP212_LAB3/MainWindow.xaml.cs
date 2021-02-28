using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Numerics;

namespace COMP212_LAB3
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

        private async void calculateFactorial(object sender, RoutedEventArgs e)
        {

            if (inputTextBox.Text != "") 
            {
                string number = inputTextBox.Text;
                var result = Task<BigInteger>.Run(() => Factorial(number));
                await result;
                resultTextBox.Text = result.Result.ToString();
            }
        }

        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            progressLabel.Visibility = Visibility.Visible;
            progressLabel.Content = "Searching...";
            string searchFilter = symbolSearchInput.Text;
            dataGrid.ItemsSource = await GetStocks(searchFilter);
            dataGrid.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Date", System.ComponentModel.ListSortDirection.Ascending));
            if (dataGrid.ItemsSource == null)
            {
                progressLabel.Content = "No matching Items";
             
            }
            else
            {
                progressLabel.Content = "Search done";

            }
        }


        private void stockRearchReset_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
            dataGrid.Items.Refresh();
            progressLabel.Visibility = Visibility.Hidden;
        }
        private BigInteger Factorial(string input)
        {
            BigInteger number = BigInteger.Parse(input);
            BigInteger total = 1;
            for (BigInteger i = 1; i <= number; i++)
            {
                total = total * i;
            }
            return total;
        }
        private async Task<IEnumerable<Stock>> GetStocks(string symbol)
        {
            var list = new List<string>();
            using (var csvreader = new StreamReader("stockData.csv"))
            {
                
                Regex minusPattern = new Regex("[\\(\\)]");
                while (!csvreader.EndOfStream) 
                {
                    string line = await csvreader.ReadLineAsync();
                    if (!minusPattern.IsMatch(line) && line.Contains(symbol.ToUpper()))
                    {
                        list.Add(line);
                    }
                }
            }
            var lines = list.ToArray();

            //string[] lines = File.ReadAllLines(System.IO.Path.ChangeExtension(fileName, ".csv")).Skip(1).ToArray();
            return lines.Select(line =>
            {
               Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
               string[] data = CSVParser.Split(line);
                Regex pattern = new Regex("[\"\\$\\(\\)]");
               return new Stock(data[0], data[1], double.Parse(pattern.Replace(data[2], "")), double.Parse(pattern.Replace(data[3], "")), double.Parse(pattern.Replace(data[4], "")), double.Parse(pattern.Replace(data[5], "")));
            });

        }

        private void calculationResetButton_Click(object sender, RoutedEventArgs e)
        {
            resultTextBox.Text = "";
        }
     
    }
}

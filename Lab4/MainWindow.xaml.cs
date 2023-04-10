using C__Lab3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Car> myCars = new List<Car>(){
            new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
            new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
            new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
            new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
            new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
            new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
            new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
            new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
            new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
        };
        List<Car> seachedCars;

        BindingSource carBindingSource;
        CarBindingList myCarsBindingList;

        void showMessageBox(Car c) { System.Windows.MessageBox.Show(c.ToString()); }
        public MainWindow()
        {
            // Zad 1

            var elements =
                from a in myCars.Where(car => car.model == "A6").
                    Select(a => new
                    {
                        engineType = (a.motor.model == "TDI" ? "disel" : "petrol"),
                        hppl = a.motor.horsePower / a.motor.displacement
                    })
                group a by new { a.engineType } into g
                select new
                {
                    g.Key.engineType,
                    avgHPPL = g.Sum(s => s.hppl) / g.Count()
                };

            foreach (var e in elements) Trace.WriteLine(e.engineType + ": " + e.avgHPPL);


            // Zad 2
            Func<Car, Car, int> arg1 = (c1, c2) => (c2.motor.horsePower.CompareTo(c1.motor.horsePower));
            Predicate<Car> arg2 = car => (car.motor.model != "TDI");
            Action<Car> arg3 = showMessageBox;

            myCars.Sort(new Comparison<Car>(arg1));
            myCars.FindAll(arg2).ForEach(arg3);



            InitializeComponent();

            myCarsBindingList = new CarBindingList(myCars);
            carBindingSource = new BindingSource();
            UpdateGrid();

            BindingList<string> list = new BindingList<string>();
            list.Add("model");  //0
            list.Add("year");  //1
            list.Add("motor");//2
            comboBox.ItemsSource = list;
            comboBox.SelectedIndex = 0;


            
            

        }
        
        void UpdateGrid()
        {
            carBindingSource.DataSource = myCarsBindingList;
            //Drag a DataGridView control from the Toolbox to the form.
            dataGridView1.ItemsSource = carBindingSource;
        }
        private void Del_Row_Click(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    Car car = (Car)row.Item;
                    myCarsBindingList.Remove(car);
                    myCars.Remove(car);
                    UpdateGrid();
                    break;
                }
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == "")
            {
                myCarsBindingList = new CarBindingList(myCars);
                UpdateGrid();
                return;
            }
            myCarsBindingList = new CarBindingList(myCars);
            myCarsBindingList = myCarsBindingList.Find(searchTextBox.Text, comboBox.SelectedIndex);
            UpdateGrid();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var newElementWindow = new AddElement();
            newElementWindow.ShowDialog();
            myCars.Add(newElementWindow.newElement);
            myCarsBindingList = new CarBindingList(myCars);
            carBindingSource = new BindingSource();
            UpdateGrid();
        }
        private void SortColumn_Event(object sender, RoutedEventArgs e)
        {
            var columnHeader = sender as DataGridColumnHeader;
            string columnName = columnHeader.ToString().Split(' ')[1].ToLower();
            if (columnName == "")
                return;

            myCarsBindingList.Sort(columnName);

            myCarsBindingList = new CarBindingList(myCars);
            carBindingSource = new BindingSource();
            UpdateGrid();
        }
    }
}

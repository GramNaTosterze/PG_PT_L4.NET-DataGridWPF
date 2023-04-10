using C__Lab3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Lab4
{
    /// <summary>
    /// Logika interakcji dla klasy AddElement.xaml
    /// </summary>
    public partial class AddElement : Window
    {
        public Car newElement;
        public AddElement()
        {
            InitializeComponent();

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            newElement = new Car(model.Text, new Engine(double.Parse(motorD.Text), int.Parse(motorHP.Text), motorModel.Text), int.Parse(year.Text));
            Close();
        }
    }
}
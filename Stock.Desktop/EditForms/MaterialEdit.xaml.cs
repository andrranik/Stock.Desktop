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
using System.Windows.Shapes;
using Stock.Desktop.Helpers;
using Stock.Desktop.Models;

namespace Stock.Desktop.EditForms
{
    /// <summary>
    /// Interaction logic for MaterialEdit.xaml
    /// </summary>
    public partial class MaterialEdit : Window
    {

        public Material MaterialItem { get; set; }

        public MaterialEdit()
        {
            InitializeComponent();
            MaterialItem = new Material();
            DataContext = this;
        }

        private async void MaterialSave_Click(object sender, RoutedEventArgs e)
        {
            using (var dbh = new DataBaseHelper())
            {
                var rowAdded = MaterialItem.Id == 0
                    ? await dbh.Create(MaterialItem, Tables.MaterialTable, Tables.MaterialFields)
                    : await dbh.Update(MaterialItem, Tables.MaterialTable, Tables.MaterialFields);
                Close();
            }
        }

        private void MaterialCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

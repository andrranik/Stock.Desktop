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
    public partial class DocTypeEdit : Window
    {

        public DocType DocTypeItem { get; set; }

        public DocTypeEdit()
        {
            InitializeComponent();
            DocTypeItem = new DocType();
            DataContext = this;
        }

        private async void DocTypeSave_Click(object sender, RoutedEventArgs e)
        {
            using (var dbh = new DataBaseHelper())
            {
                var rowAdded = DocTypeItem.Id == 0
                    ? await dbh.Create(DocTypeItem, Tables.DocTypeTable, Tables.DocTypeFields)
                    : await dbh.Update(DocTypeItem, Tables.DocTypeTable, Tables.DocTypeFields);
                Close();
            }
        }

        private void DocTypeCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

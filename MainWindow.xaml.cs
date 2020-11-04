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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration; //selber hinzugefügt

namespace LinqToSQLDeutsch
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LinqToSqlDataClassesDataContext dataContext;    //unsere Kontet für daten setzen...?

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["LinqToSQLDeutsch.Properties.Settings.AR_CSHARP_DB_1_ConnectionString"].ConnectionString;

            /// CONNECTION STRING: AR_CSHARP_DB_1_ConnectionString
            /// ich hatte es falschlich gespeichert, jetzt geht es!! :D
            /// Student hilfe
            /// string sqlConnection = Properties.Settings.Default.NameDesConnectionStrings;
            dataContext = new LinqToSqlDataClassesDataContext(connectionString);
        }
    }
}

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

            //mit Unis füllen
            insertUniversities();
            insertStudents();
        }


        public void insertUniversities()
        {
            // AUFPASSEN, JEDE NEUE INSERT HIER WIRD WIEDERHOLUNGEN ERFOLGEN!!
            // da die alten löschen wir noch nicht.
            // hiermit löschen wir immer wieder die ganze tabelle uns schreiben sie erneut
            dataContext.ExecuteCommand("DELETE FROM University");
            // die Gelöschten Ids werden aber nicht ersetzt, sondern weiter hochgezählt


            University yale = new University();             // neue Uni erstellt
            yale.Name = "Yale";                             // Name eingegeben
            dataContext.University.InsertOnSubmit(yale);    // zu Datacontext bzw. DB hinzufügen

            University beijingTech = new University();             // neue Uni erstellt
            beijingTech.Name = "Beijing Tech";                             // Name eingegeben
            dataContext.University.InsertOnSubmit(beijingTech);    // zu Datacontext bzw. DB hinzufügen

            University madrid = new University();
            madrid.Name = "Madrid";
            dataContext.University.InsertOnSubmit(madrid); 
            
            University valladolid = new University();
            valladolid.Name = "Valladolid";
            dataContext.University.InsertOnSubmit(valladolid);

            // AUFPASSEN, JEDE NEUE INSERT HIER WIRD WIEDERHOLUNGEN ERFOLGEN!!
            // da die alten löschen wir noch nicht.[siehe oben]

            dataContext.SubmitChanges();                    //update the changes to the DB
            MainDataGrid.ItemsSource = dataContext.University;  // die Data im Grid wird von Universities geholt (erstmal ganz unformatiert.
        }

        //Herausforderung Studenten Hinzufügen 
        public void insertStudents()
        {
            // setzen die Universitäten um die benutzen zu können
            // lambda expression, in den datacontext uni, gib mir die erste deren Name ist Yale und speichere in yale
            // bzw wir haben die ganze yale Uni in unsere University Tabelle.
            // genauso gleich für die anderen unis
            University yale = dataContext.University.First(un => un.Name.Equals("Yale"));
            University beijingTech = dataContext.University.First(un => un.Name.Equals("Beijing Tech"));
            University madrid = dataContext.University.First(un => un.Name.Equals("Madrid"));
            University valladolid = dataContext.University.First(un => un.Name.Equals("Valladolid"));

            //wir schreiben eine Liste von studenten... und setzen die Ids von der unis ohne die genaue Zahlen kennen zu müssen
            List<Student> students = new List<Student>();
            students.Add(new Student { Name = "Carla", Gender = "female", UniversityId = yale.Id });
            students.Add(new Student { Name = "Pedro", Gender = "male", UniversityId = madrid.Id });
            students.Add(new Student { Name = "Xing Mi Huang", Gender = "female", UniversityId = beijingTech.Id });
            students.Add(new Student { Name = "Jose Luisete", Gender = "male", UniversityId = valladolid.Id });

            // die Liste in die Tabelle ubertragen

            dataContext.Student.InsertAllOnSubmit(students);    // alle zusammen submitten  //diese Methode hat er sich aus dem A**** gezogen mal wieder!!
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Student;

            // bisher zeigt es dann bei University "LinqToSQLDeutsch.University", an braucht noch was korrigieren... was und wo?
            // wird extra hinzugefügt... wieso?
            // VERDAMMT NOCHMAL ER KORRIGIERT ES GAR NICHT!!!!

            // Meine eigene doofe Versuch, war doof, braucht zuerst die Unis zu kennen!!!
            ////dataContext.ExecuteCommand("DELETE FROM Student");
            ////Student marie = new Student();
            ////marie.Name = "Marie";
            ////marie.Gender = "Divers";
            ////////marie.UniversityId = 13;    //diese direkt gehen gar nicht!!
            ////////marie.University.Id = 13;
            ////dataContext.Student.InsertOnSubmit(marie);

            ////dataContext.SubmitChanges();                    //update the changes to the DB

            ////MainDataGrid.ItemsSource = dataContext.Student;  // die Data im Grid wird von Universities geholt (erstmal ganz unformatiert.
        }

    }
}

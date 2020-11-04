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

            ////mit Unis füllen
            insertUniversities();
            insertStudents();
            //insertLectures();
            //insertStudenLectureAssociations();
            //GetUniversityOfJoseluisete();       //zeigt die Uni Name von Joseluisete
            //GetLectureOfJoseluisete();
            //GetAllLecturesFromBeijingTech();
            UpdateCarla();
            DeleteJames();
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
            students.Add(new Student { Name = "Jose Luisete", Gender = "male", UniversityId = beijingTech.Id });
            students.Add(new Student { Name = "James", Gender = "male", UniversityId = beijingTech.Id });

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


        //Herausforderung Insertlectures selber hinzukriegen
        public void insertLectures()
        {
            dataContext.ExecuteCommand("DELETE FROM Lecture");

            Lecture mathe = new Lecture();
            mathe.Name = "Mathematik";
            dataContext.Lecture.InsertOnSubmit(mathe);

            Lecture geschichte = new Lecture();
            geschichte.Name = "Geschichte";
            dataContext.Lecture.InsertOnSubmit(geschichte);

            //LEHRER SCHNELLERE VERSION, anstatt die 3 Linie alles zusammen:
            dataContext.Lecture.InsertOnSubmit(new Lecture { Name="Fisik"});


            dataContext.SubmitChanges();                    //update the changes to the DB
            MainDataGrid.ItemsSource = dataContext.Lecture;
            // das klappt aber die anzeige ist einfach doof, zumindest erstmals.
        }

        //WEITERMACHEN Verbindungen!!!
        public void insertStudenLectureAssociations()
        {
            //erst die Studenten Holen
            Student carla = dataContext.Student.First(st => st.Name.Equals("Carla"));
            Student pedro = dataContext.Student.First(st => st.Name.Equals("Pedro"));
            Student xing = dataContext.Student.First(st => st.Name.Equals("Xing Mi Huang"));
            Student joseluisete = dataContext.Student.First(st => st.Name.Equals("Jose Luisete"));
            Student james = dataContext.Student.First(st => st.Name.Equals("James"));       //gebraucht um die ganzen verbindungen zu haben!!

            //dann die Fächer
            Lecture mathe = dataContext.Lecture.First(Lecture => Lecture.Name.Equals("Mathematik"));
            Lecture geschichte = dataContext.Lecture.First(Lecture => Lecture.Name.Equals("Geschichte"));
            Lecture fisik = dataContext.Lecture.First(Lecture => Lecture.Name.Equals("Fisik"));


            //wir schreiben die studenten-Leture Verbindungen!!
            //LEHRER SCHNELLERE VERSION, anstatt die 3 Linie alles zusammen:
            dataContext.StudentLecture.InsertOnSubmit(new StudentLecture { Student = carla, Lecture = mathe });
            dataContext.StudentLecture.InsertOnSubmit(new StudentLecture { Student = pedro, Lecture = mathe });

            // Längere Version auch von Lehrer
            StudentLecture slXing = new StudentLecture();
            slXing.StudentId = xing.Id;
            slXing.LectureId = geschichte.Id;
            dataContext.StudentLecture.InsertOnSubmit(slXing);


            dataContext.StudentLecture.InsertOnSubmit(new StudentLecture { Student = joseluisete, Lecture = fisik });
            dataContext.StudentLecture.InsertOnSubmit(new StudentLecture { Student = joseluisete, Lecture = geschichte });
            dataContext.StudentLecture.InsertOnSubmit(new StudentLecture { Student = joseluisete, Lecture = mathe });

            dataContext.SubmitChanges();    //update the changes to the DB
            MainDataGrid.ItemsSource = dataContext.StudentLecture;

            // Student und Lecture werden auch fälschlich angezeigt da wir das ganze Objekt übergeben... das ist nicht so ganz ok

        }

        public void GetUniversityOfJoseluisete()
        {
            Student joseluisete = dataContext.Student.First(st => st.Name.Equals("Jose Luisete"));
            University joseluisetesUni = joseluisete.University;

            // wir dürfen nicht eine  uni zur MainDataGrid.ItemsSource übergeben, da es  eine IEnumerable erwartet.
            // deswegen bauen wir hier einfach eine liste mit diese uni...
            List<University> universities = new List<University>();
            universities.Add(joseluisetesUni);

            MainDataGrid.ItemsSource = universities;
        }

        //Herausforderung, hol die Lecture von Tony
        public void GetLectureOfJoseluisete()
        {
            Student joseluisete = dataContext.Student.First(st => st.Name.Equals("Jose Luisete"));
            var joseluisetesLectures = from sl in joseluisete.StudentLecture select sl.Lecture;
            // wir holen alle fächer die in Joseluisete zu finden sind!! so zu sagen wir suchen von  joseluisete seite alle einträge in StudentLecture.
            // natürlich geht es so , es war aber nicht erklärt das es wieder mit From select 

            MainDataGrid.ItemsSource = joseluisetesLectures;
        }


        /// Verbindung zwischen Uni und alle seine fächer Lectures da
        /// in StudentLecture, verbunden mit Student tabelle vial studenten+Ids
        /// und dann dort welche sind aus diese uni. dann daraus die Fächer Lectures auswählen...
        ///  SUPER EINFACH ALLES WAS SOLL DAS HIER!!!!!!!!!!!!!!
        public void GetAllLecturesFromBeijingTech()
        {
            var LecturesFromBeijingTech = from sl in dataContext.StudentLecture
                                          join student in dataContext.Student on sl.StudentId equals student.Id
                                          where student.University.Name == "Beijing Tech"
                                          select sl.Lecture;
            // Visualstudio ist schlau genug und brauch nicht die zwischenspets von UniversityId und LectureIds...
            //es klappt aber die verdoppelten einträge werden doch wiederholt gezeigt.werden ...

            //SELBER UPDATE die Liste um doppelten zu entfernen :)
            LecturesFromBeijingTech = LecturesFromBeijingTech.Distinct();

            MainDataGrid.ItemsSource = LecturesFromBeijingTech;
        }

        //Löschen und aktualisieren..... letzter video ENDLICH
        public void UpdateCarla()
        {
            Student carla = dataContext.Student.FirstOrDefault(st => st.Name.Equals("Carla"));
            carla.Name = "Carlota María del Rosario";
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Student;
        }

        public  void DeleteJames()
        {
            Student james = dataContext.Student.FirstOrDefault(st => st.Name.Equals("James"));
            dataContext.Student.DeleteOnSubmit(james);
            
            // ES WIRD TATSÄCHLICH GELÖSCHT, ABERR BEI MIR IRGENDWIE IMMER NOCH ANGEZEIGT KEIN PLAN WIESO UND IST MIR ENDLICH EGAL!
            // SCHLUSS DAMIT!
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Student;

        }
    }
}

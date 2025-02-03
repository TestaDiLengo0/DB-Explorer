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
using System.Data;
using System.Configuration;
using Npgsql;

namespace DB_Explorer_v0._2
{
    /// <summary>
    /// Логика взаимодействия для WorkPage.xaml
    /// </summary> 
    ///Класс для создания строки к таблице преподавателей
    public class Teacher
    {
        private int id;
        private string name = "";
        private int departmentId; 

        public int Id
        {
            set { id = value; }
            get { return id; }
        }
        
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int DepartmentId
        {
            set { departmentId = value; }
            get { return departmentId; }
        }

        public Teacher(int  id, string name, int departmentId)
        {
            Id = id;
            Name = name;
            DepartmentId = departmentId;
        }
    }

    public class Makers
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["university_DB"].ToString();
        public DataTable makeTeachersTable(string Querry)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = new NpgsqlConnection(connectionString);
            command.Connection.Open();
            if (Querry != null | Querry != "") command.CommandText = Querry;
            else command.CommandText = "Select * From teachers";
            NpgsqlDataReader dataReader = command.ExecuteReader();
            command.Connection.Close();
            command.Connection.Dispose();
            command.Dispose();

            //Инициализация таблицы DataTable для хранения данных и последующей вставки в DataGrid
            DataTable table = new DataTable("teachers");
            DataColumn column;
            DataRow row;

            //Инициализация и настройка столбца ID
            column = new DataColumn();
            column.DataType = typeof(int);
            column.ColumnName = "ID";
            column.ReadOnly = true;
            table.Columns.Add(column);

            //Инициализация и настройка столбца Name
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "Имя";
            table.Columns.Add(column);

            //Инициализация и настройка столбца department_id
            column = new DataColumn();
            column.DataType = typeof(int);
            column.ColumnName = "ID кафедры";
            table.Columns.Add(column);

            //Вставка данных в таблицу
            while(dataReader.Read())
            {
                table.Rows.Add(dataReader.Read());
            }
            dataReader.Close();
            dataReader.Dispose();
            return table;
        }
    }

    public partial class WorkPage : Page
    {

        static private string connectionString = ConfigurationManager.ConnectionStrings["university_DB"].ToString();

        public WorkPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

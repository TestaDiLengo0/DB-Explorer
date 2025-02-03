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

        static public DataTable makeTableByQuerry(string Querry)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = Querry;

            NpgsqlDataReader dataReader = command.ExecuteReader();
            DataTable dt = new DataTable();

            if(dataReader.HasRows)
            {
                dt.Load(dataReader);
            }

            return dt;
        } 

        static public DataTable makeTeachersTable(string Querry)
        {
            //Создание таблицы
            DataTable table = makeTableByQuerry(Querry);

            //Локализация имен столбцов
            table.Columns[0].ColumnName = "ID";
            table.Columns[1].ColumnName = "Имя";
            table.Columns[2].ColumnName = "ID кафедры";
            return table;
        }

        static public DataTable makeMaterialsTable(string Querry)
        {
            DataTable table = makeTableByQuerry(Querry);

            table.Columns[0].ColumnName = "ID";
            table.Columns[1].ColumnName = "Название";
            table.Columns[2].ColumnName = "ID Автора";
            table.Columns[3].ColumnName = "Год выхода";
            table.Columns[4].ColumnName = "Число страниц";
            table.Columns[5].ColumnName = "ID типа издания";
            table.Columns[6].ColumnName = "ID дисциплины";

            return table;
        }

        static public DataTable makeDisciplinesTable(string Querry) 
        {
            DataTable table = makeTableByQuerry(Querry);

            table.Columns[0].ColumnName = "ID";
            table.Columns[1].ColumnName = "Название дисциплины";

            return table;
        }

        static public DataTable makeDepartmentsTable(string Querry)
        {
            DataTable table = makeTableByQuerry(Querry);

            table.Columns[0].ColumnName = "ID";
            table.Columns[1].ColumnName = "Название кафедры";

            return table;
        }

        static public DataTable makeTypesOfMaterialsTable(string Querry)
        {
            DataTable table = makeTableByQuerry(Querry);

            table.Columns[0].ColumnName = "ID";
            table.Columns[1].ColumnName = "Название типа";

            return table;
        }
    }

    public partial class WorkPage : Page
    {
        enum Tables
        {
            teachers,
            guides_types,
            guides,
            disciplines,
            departments
        }

        static private string connectionString = ConfigurationManager.ConnectionStrings["university_DB"].ToString();
        static private Tables currentTable;

        public WorkPage()
        {
            InitializeComponent();


            SetDataGrid(Makers.makeTeachersTable("SELECT * FROM teachers"));
            currentTable = Tables.teachers;
        }

        private void SetDataGrid(DataTable table)
        {
            DBDataGrid.ItemsSource = table.DefaultView;
        }

        private void TablesMenuTeachers_Click(object sender, RoutedEventArgs e)
        {
            //Косметичские изменения при нажатии
            Teachers.Foreground = new SolidColorBrush(Colors.Purple);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            string Querry = "SELECT * FROM teachers";
            SetDataGrid(Makers.makeTeachersTable(Querry));
        }

        private void TablesMenuMaterials_Click(object sender, RoutedEventArgs e)
        {
            //Косметичские изменения при нажатии
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Purple);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            string Querry = "SELECT * FROM guides";
            SetDataGrid(Makers.makeMaterialsTable(Querry));
        }

        private void TablesMenuDisciplines_Click(object sender, RoutedEventArgs e)
        {
            //Косметичские изменения при нажатии
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Purple);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            string Querry = "SELECT * FROM disciplines";
            SetDataGrid(Makers.makeDisciplinesTable(Querry));
        }

        private void TablesMenuDepartments_Click(object sender, RoutedEventArgs e)
        {
            //Косметичские изменения при нажатии
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Purple);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            string Querry = "SELECT * FROM departments";
            SetDataGrid(Makers.makeDepartmentsTable(Querry));
        }

        private void TablesMenuTypesOfMaterials_Click(object sender, RoutedEventArgs e)
        {
            //Косметичские изменения при нажатии
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Purple);

            string Querry = "SELECT * FROM guides_types";
            SetDataGrid(Makers.makeTypesOfMaterialsTable(Querry));
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

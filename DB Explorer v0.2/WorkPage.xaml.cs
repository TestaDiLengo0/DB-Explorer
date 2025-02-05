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
using System.Windows.Documents.DocumentStructures;
using DB_Explorer_v0._2.RedcWins;

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
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["university_DB"].ToString();

        //
        //Method that makes the SQL-Querry by some arguments
        //

        static public String MakeQuerry(string tableName, string targetColumn, string[] args)
        {
            string querry = $"SELECT * FROM {tableName}";
            if (args.Length != 0 | args != null)
            {
                querry += $" WHERE LOWER({targetColumn}) LIKE \'%{args[0]}%\'";
                for (int i = 1; i < args.Length; i++)
                {
                    querry += $" OR LOWER({targetColumn}) LIKE \'%{args[i]}%\'";
                }
            }
            return querry;
        }

        //
        //Методы для создания таблиц для наполнения Грида
        //

        static public DataTable MakeTableByQuerry(string Querry)
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

        static public DataTable MakeTeachersTable(string Querry)
        {
            //Создание таблицы
            DataTable table = MakeTableByQuerry(Querry);

            //Локализация имен столбцов
            if (table.Rows.Count > 0)
            {
                table.Columns[0].ColumnName = "ID";
                table.Columns[1].ColumnName = "Имя";
                table.Columns[2].ColumnName = "ID кафедры";
            }
            else
            {
                table.Columns.Add("Вывод");
                table.Rows.Add("По этому запросу ничего не найдено");
            }
            return table;
        }

        static public DataTable MakeMaterialsTable(string Querry)
        {
            DataTable table = MakeTableByQuerry(Querry);

            if (table.Rows.Count > 0)
            {
                table.Columns[0].ColumnName = "ID";
                table.Columns[1].ColumnName = "Название";
                table.Columns[2].ColumnName = "ID Автора";
                table.Columns[3].ColumnName = "Год выхода";
                table.Columns[4].ColumnName = "Число страниц";
                table.Columns[5].ColumnName = "ID типа издания";
                table.Columns[6].ColumnName = "ID дисциплины";
            }
            else
            {
                table.Columns.Add("Вывод");
                table.Rows.Add("По этому запросу ничего не найдено");
            }
            return table;
        }

        static public DataTable MakeDisciplinesTable(string Querry) 
        {
            DataTable table = MakeTableByQuerry(Querry);

            if (table.Rows.Count > 0)
            {
                table.Columns[0].ColumnName = "ID";
                table.Columns[1].ColumnName = "Название дисциплины";
            }
            else
            {
                table.Columns.Add("Вывод");
                table.Rows.Add("По этому запросу ничего не найдено");
            }
            return table;
        }

        static public DataTable MakeDepartmentsTable(string Querry)
        {
            DataTable table = MakeTableByQuerry(Querry);

            if (table.Rows.Count > 0)
            {
                table.Columns[0].ColumnName = "ID";
                table.Columns[1].ColumnName = "Название кафедры";
            }
            else
            {
                table.Columns.Add("Вывод");
                table.Rows.Add("По этому запросу ничего не найдено");
            }
            return table;
        }

        static public DataTable MakeTypesOfMaterialsTable(string Querry)
        {
            DataTable table = MakeTableByQuerry(Querry);

            if (table.Rows.Count > 0)
            {
                table.Columns[0].ColumnName = "ID";
                table.Columns[1].ColumnName = "Название типа";
            }
            else
            {
                table.Columns.Add("Вывод");
                table.Rows.Add("По этому запросу ничего не найдено");
            }
            return table;
        }
        //////////////////////////////////////////////////////////////
        ///

        //
        //Методы для создания окон редактирования и вставки данных
        // Нужно будет ещё это обдумать
        //

    }

    public partial class WorkPage : Page
    {
        public enum Tables
        {
            teachers,
            guides_types,
            guides,
            disciplines,
            departments
        }

        static public Tables currentTable;


        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["university_DB"].ToString();

        private string teachersEnter = string.Empty;
        private string materialsEnter = string.Empty;
        private string disciplinesEnter = string.Empty;
        private string departmentsEnter = string.Empty;
        private string typesEnter = string.Empty;

        public WorkPage()
        {
            InitializeComponent();


            SetDataGrid(Makers.MakeTeachersTable("SELECT * FROM teachers"));
            currentTable = Tables.teachers;
        }

        public void SetDataGrid(DataTable table)
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

            if (teachersEnter == string.Empty)
            {
                string Querry = "SELECT * FROM teachers";
                SetDataGrid(Makers.MakeTeachersTable(Querry));
            }
            currentTable = Tables.teachers;

            SearchBox.Text = teachersEnter;
            MaterialDesignThemes.Wpf.HintAssist.SetHelperText(SearchBox, "Поиск по имени преподавателя");
        }

        private void TablesMenuMaterials_Click(object sender, RoutedEventArgs e)
        {
            //Косметичские изменения при нажатии
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Purple);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            if (materialsEnter == string.Empty)
            {
                string Querry = "SELECT * FROM guides";
                SetDataGrid(Makers.MakeMaterialsTable(Querry));
            }
            currentTable = Tables.guides;

            SearchBox.Text = materialsEnter;
            MaterialDesignThemes.Wpf.HintAssist.SetHelperText(SearchBox, "Поиск по названию издания");
        }

        private void TablesMenuDisciplines_Click(object sender, RoutedEventArgs e)
        {
            //Косметичские изменения при нажатии
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Purple);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            if (disciplinesEnter == string.Empty)
            {
                string Querry = "SELECT * FROM disciplines";
                SetDataGrid(Makers.MakeDisciplinesTable(Querry));
            }
            currentTable = Tables.disciplines;

            SearchBox.Text = disciplinesEnter;
            MaterialDesignThemes.Wpf.HintAssist.SetHelperText(SearchBox, "Поиск по названию дисциплины");
        }

        private void TablesMenuDepartments_Click(object sender, RoutedEventArgs e)
        {
            //Косметичские изменения при нажатии
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Purple);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            if (departmentsEnter == string.Empty)
            {
                string Querry = "SELECT * FROM departments";
                SetDataGrid(Makers.MakeDepartmentsTable(Querry));
            }
            currentTable = Tables.departments;

            SearchBox.Text = departmentsEnter;
            MaterialDesignThemes.Wpf.HintAssist.SetHelperText(SearchBox, "Поиск по названию кафедры");
        }

        private void TablesMenuTypesOfMaterials_Click(object sender, RoutedEventArgs e)
        {
            //Косметичские изменения при нажатии
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Purple);

            if (typesEnter == string.Empty)
            {
                string Querry = "SELECT * FROM guides_types";
                SetDataGrid(Makers.MakeTypesOfMaterialsTable(Querry));
            }
            currentTable = Tables.guides_types;

            SearchBox.Text = typesEnter;
            MaterialDesignThemes.Wpf.HintAssist.SetHelperText(SearchBox, "Поиск по типам");
        }

        public DataTable CreateTableWithEnters()
        {
            string args;
            string targetColumn;
            DataTable dt = new DataTable();
            switch (currentTable)
            {
                case Tables.teachers:
                    teachersEnter = SearchBox.Text;
                    args = teachersEnter;
                    targetColumn = "teacher_name";

                    dt = Makers.MakeTeachersTable(Makers.MakeQuerry(currentTable.ToString(), targetColumn, args.ToLower().Split(", ")));
                    break;
                case Tables.guides:
                    materialsEnter = SearchBox.Text;
                    args = materialsEnter;
                    targetColumn = "guide_name";

                    dt = Makers.MakeMaterialsTable(Makers.MakeQuerry(currentTable.ToString(), targetColumn, args.ToLower().Split(", ")));
                    break;
                case Tables.disciplines:
                    disciplinesEnter = SearchBox.Text;
                    args = disciplinesEnter;
                    targetColumn = "discipline_name";

                    dt = Makers.MakeDisciplinesTable(Makers.MakeQuerry(currentTable.ToString(), targetColumn, args.ToLower().Split(", ")));
                    break;
                case Tables.departments:
                    departmentsEnter = SearchBox.Text;
                    args = departmentsEnter;
                    targetColumn = "department_name";

                    dt = Makers.MakeDepartmentsTable(Makers.MakeQuerry(currentTable.ToString(), targetColumn, args.ToLower().Split(", ")));
                    break;
                case Tables.guides_types:
                    typesEnter = SearchBox.Text;
                    args = typesEnter;
                    targetColumn = "guide_tye_name";

                    dt = Makers.MakeTypesOfMaterialsTable(Makers.MakeQuerry(currentTable.ToString(), targetColumn, args.ToLower().Split(", ")));
                    break;
            }
            return dt;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetDataGrid(CreateTableWithEnters());
        }

        private void ContextDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Это действие удалит также все связанные записи!\nХотите продолжить?", "Вопрос жизни и смерти", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                int id = (int)((DataRowView)DBDataGrid.SelectedItem)["ID"];
                NpgsqlConnection connection = new(connectionString);
                connection.Open();

                NpgsqlCommand command = new();
                command.Connection = connection;
                command.CommandText = $"DELETE FROM {currentTable.ToString()} WHERE {currentTable.ToString()}_id = {id}";
                command.ExecuteNonQuery();

                SetDataGrid(CreateTableWithEnters());
            }
        }

        private void ContextUpdate_Click(object sender, RoutedEventArgs e)
        {
            string[] args = new string[DBDataGrid.SelectedCells.Count];
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = Convert.ToString(((DataRowView)DBDataGrid.SelectedItem)[i]);
            }

            switch (currentTable)
            {
                case Tables.teachers:
                    TeachersWin win = new TeachersWin(this, "UPDATE", args)
                    {
                        Owner = Application.Current.MainWindow
                    };
                    win.Show();
                    break;

                case Tables.guides:

                    break;
                case Tables.disciplines:

                    break;
                case Tables.departments:

                    break;
                case Tables.guides_types:

                    break;
            }
        }

        private void AddNewRow_Click(object sender, RoutedEventArgs e)
        {
            switch(currentTable)
            {
                case Tables.teachers:
                    TeachersWin win = new TeachersWin(this, "INSERT", new string[0])
                    {
                        Owner = Application.Current.MainWindow
                    };
                    win.Show();
                    break;

                case Tables.guides:

                    break;
                case Tables.disciplines:

                    break;
                case Tables.departments:

                    break;
                case Tables.guides_types:

                    break;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }
    }
}

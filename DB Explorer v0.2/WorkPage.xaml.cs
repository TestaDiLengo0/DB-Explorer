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
using System.Windows.Controls.Primitives;

namespace DB_Explorer_v0._2
{
    /// <summary>
    /// Логика взаимодействия для WorkPage.xaml
    /// </summary> 
    ///Класс для создания строки к таблице преподавателей

    public class Makers
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["university_DB"].ToString();

        //
        //Method that makes the SQL-Querry by some arguments
        //

        static public String MakeQuerry(WorkPage.Tables tableName, string targetColumn, string[] args)
        {
            string querry = "SELECT * FROM ";
            switch (tableName)
            {
                case WorkPage.Tables.teachers:
                case WorkPage.Tables.guides:
                    querry += $"select_all_{tableName.ToString()}()";
                    break;
                default:
                    querry += tableName.ToString();
                    break;
            }
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
            dataReader.Close();
            command.Dispose();
            connection.Close();
            connection.Dispose();

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
                table.Columns[2].ColumnName = "Кафедра";
            }
            else
            {
                table.Columns.Add("Вывод");
                table.Rows.Add("По этому запросу ничего не найдено :(");
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
                table.Columns[2].ColumnName = "Имя автора";
                table.Columns[3].ColumnName = "Год выхода";
                table.Columns[4].ColumnName = "Число страниц";
                table.Columns[5].ColumnName = "Тип издания";
                table.Columns[6].ColumnName = "Дисциплина";
            }
            else
            {
                table.Columns.Add("Вывод");
                table.Rows.Add("По этому запросу ничего не найдено :(");
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
                table.Rows.Add("По этому запросу ничего не найдено :(");
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
                table.Rows.Add("По этому запросу ничего не найдено :(");
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
                table.Rows.Add("По этому запросу ничего не найдено :(");
            }
            return table;
        }
        
        ///Таблицы под готовые запросы

        static public DataTable MakeReady1Table()
        {
            DataTable table = MakeTableByQuerry("SELECT * FROM get_authors_departments_publications();");

            if(table.Rows.Count > 0)
            {
                table.Columns[0].ColumnName = "Имя автора";
                table.Columns[1].ColumnName = "Кафедра";
                table.Columns[2].ColumnName = "Название издания";
                table.Columns[3].ColumnName = "Год издания";
            }
            else
            {
                table.Columns.Add("Вывод");
                table.Rows.Add("По этому запросу ничего не найдено :(");
            }
            return table;
        }

        static public DataTable MakeReady2Table()
        {
            DataTable table = MakeTableByQuerry("SELECT * FROM get_publications_grouped_by_departments_and_disciplines();");

            if (table.Rows.Count > 0)
            {
                table.Columns[0].ColumnName = "Кафедра";
                table.Columns[1].ColumnName = "Дисциплина";
                table.Columns[2].ColumnName = "Название издания";
                table.Columns[3].ColumnName = "Год издания";
            }
            else
            {
                table.Columns.Add("Вывод");
                table.Rows.Add("По этому запросу ничего не найдено :(");
            }
            return table;
        }

        static public DataTable MakeReady3Table()
        {
            DataTable table = MakeTableByQuerry("SELECT * FROM get_publication_counts_by_departments_last_5_years();");

            if (table.Rows.Count > 0)
            {
                table.Columns[0].ColumnName = "Кафедра";
                table.Columns[1].ColumnName = "Кол-во изданий";
            }
            else
            {
                table.Columns.Add("Вывод");
                table.Rows.Add("По этому запросу ничего не найдено :(");
            }
            return table;
        }

        static public DataTable MakeReady4Table()
        {
            DataTable table = MakeTableByQuerry("SELECT * FROM get_publication_counts_by_departments_last_5_years();");

            if (table.Rows.Count > 0)
            {
                table.Columns[0].ColumnName = "Тип издания";
                table.Columns[1].ColumnName = "Кол-во изданий";
            }
            else
            {
                table.Columns.Add("Вывод");
                table.Rows.Add("По этому запросу ничего не найдено :(");
            }
            return table;
        }

        static public DataTable MakeReady5Table(string[] args)
        {
            string Querry = "SELECT * FROM get_publication_counts_and_type_by_author_and_period() ";
            if (args.Length > 0)
            {
                Querry += $"WHERE LOWER(author_name) LIKE \'%{args[0]}%\'";
                for(int i = 1; i < args.Length; i++)
                {
                    Querry += $" LOWER(author_name) LIKE \'{args[i]}\' ";
                }
            }
                DataTable table = MakeTableByQuerry(Querry);

                if (table.Rows.Count > 0)
                {
                    table.Columns[0].ColumnName = "Имя автора";
                    table.Columns[1].ColumnName = "Тип издания";
                    table.Columns[2].ColumnName = "Кол-во изданий";
                }
                else
                {
                    table.Columns.Add("Вывод");
                    table.Rows.Add("По этому запросу ничего не найдено :(");
                }
            return table;
        }

    }


    //////////////////////////////////

    public partial class WorkPage : Page
    {
        public enum Tables
        {
            teachers,
            guides_types,
            guides,
            disciplines,
            departments,
            readyQuerries,
            readyQuerries5
        }

        static public Tables currentTable;


        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["university_DB"].ToString();

        private string teachersEnter = string.Empty;
        private string materialsEnter = string.Empty;
        private string disciplinesEnter = string.Empty;
        private string departmentsEnter = string.Empty;
        private string typesEnter = string.Empty;
        private string readiesEnter = string.Empty;

        public WorkPage()
        {
            InitializeComponent();


            SetDataGrid(Makers.MakeTeachersTable("SELECT * FROM select_all_teachers();"));
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

            SearchBox.IsEnabled = true;
            AddNewRow.IsEnabled = true;
            ContextDelete.IsEnabled = true;
            ContextUpdate.IsEnabled = true;

            if (teachersEnter == string.Empty)
            {
                string Querry = "SELECT * FROM select_all_teachers();";
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

            SearchBox.IsEnabled = true;
            AddNewRow.IsEnabled = true;
            ContextDelete.IsEnabled = true;
            ContextUpdate.IsEnabled = true;

            if (materialsEnter == string.Empty)
            {
                string Querry = "SELECT * FROM select_all_guides();";
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

            SearchBox.IsEnabled = true;
            AddNewRow.IsEnabled = true;
            ContextDelete.IsEnabled = true;
            ContextUpdate.IsEnabled = true;

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

            SearchBox.IsEnabled = true;
            AddNewRow.IsEnabled = true;
            ContextDelete.IsEnabled = true;
            ContextUpdate.IsEnabled = true;

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

            SearchBox.IsEnabled = true;
            AddNewRow.IsEnabled = true;
            ContextDelete.IsEnabled = true;
            ContextUpdate.IsEnabled = true;

            if (typesEnter == string.Empty)
            {
                string Querry = "SELECT * FROM guides_types";
                SetDataGrid(Makers.MakeTypesOfMaterialsTable(Querry));
            }
            currentTable = Tables.guides_types;

            SearchBox.Text = typesEnter;
            MaterialDesignThemes.Wpf.HintAssist.SetHelperText(SearchBox, "Поиск по типам");
        }

        private void ReadyQuerries1_Click(object sender, RoutedEventArgs e)
        {
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            SearchBox.IsEnabled = false;
            AddNewRow.IsEnabled = false;
            ContextDelete.IsEnabled = false;
            ContextUpdate .IsEnabled = false;

            SearchBox.Text = string.Empty;

            SetDataGrid(Makers.MakeReady1Table());

            currentTable = Tables.readyQuerries;
        }

        private void ReadyQuerries2_Click(object sender, RoutedEventArgs e)
        {
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            SearchBox.IsEnabled = false;
            AddNewRow.IsEnabled = false;
            ContextDelete.IsEnabled = false;
            ContextUpdate.IsEnabled = false;

            SearchBox.Text = string.Empty;

            SetDataGrid(Makers.MakeReady2Table());

            currentTable = Tables.readyQuerries;
        }

        private void ReadyQuerries3_Click(object sender, RoutedEventArgs e)
        {
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            SearchBox.IsEnabled = false;
            AddNewRow.IsEnabled = false;
            ContextDelete.IsEnabled = false;
            ContextUpdate.IsEnabled = false;

            SearchBox.Text = string.Empty;

            SetDataGrid(Makers.MakeReady3Table());

            currentTable = Tables.readyQuerries;
        }

        private void ReadyQuerries4_Click(object sender, RoutedEventArgs e)
        {
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            SearchBox.IsEnabled = false;
            AddNewRow.IsEnabled = false;
            ContextDelete.IsEnabled = false;
            ContextUpdate.IsEnabled = false;

            SearchBox.Text = string.Empty;

            SetDataGrid(Makers.MakeReady4Table());

            currentTable = Tables.readyQuerries;
        }

        private void ReadyQuerries5_Click(object sender, RoutedEventArgs e)
        {
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            SearchBox.IsEnabled = true;
            AddNewRow.IsEnabled = false;
            ContextDelete.IsEnabled = false;
            ContextUpdate.IsEnabled = false;

            SearchBox.Text = readiesEnter;

            if(readiesEnter == string.Empty)
            {
                SetDataGrid(Makers.MakeReady5Table(Array.Empty<string>()));
            }

            MaterialDesignThemes.Wpf.HintAssist.SetHelperText(SearchBox, "Поиск по имени автора");
            currentTable = Tables.readyQuerries5;
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

                    dt = Makers.MakeTeachersTable(Makers.MakeQuerry(currentTable, targetColumn, args.ToLower().Split(", ")));
                    break;
                case Tables.guides:
                    materialsEnter = SearchBox.Text;
                    args = materialsEnter;
                    targetColumn = "guide_name";

                    dt = Makers.MakeMaterialsTable(Makers.MakeQuerry(currentTable, targetColumn, args.ToLower().Split(", ")));
                    break;
                case Tables.disciplines:
                    disciplinesEnter = SearchBox.Text;
                    args = disciplinesEnter;
                    targetColumn = "discipline_name";

                    dt = Makers.MakeDisciplinesTable(Makers.MakeQuerry(currentTable, targetColumn, args.ToLower().Split(", ")));
                    break;
                case Tables.departments:
                    departmentsEnter = SearchBox.Text;
                    args = departmentsEnter;
                    targetColumn = "department_name";

                    dt = Makers.MakeDepartmentsTable(Makers.MakeQuerry(currentTable, targetColumn, args.ToLower().Split(", ")));
                    break;
                case Tables.guides_types:
                    typesEnter = SearchBox.Text;
                    args = typesEnter;
                    targetColumn = "guide_tye_name";

                    dt = Makers.MakeTypesOfMaterialsTable(Makers.MakeQuerry(currentTable, targetColumn, args.ToLower().Split(", ")));
                    break;

                case Tables.readyQuerries5:
                    readiesEnter = SearchBox.Text;
                    args = readiesEnter;

                    dt = Makers.MakeReady5Table(args.ToLower().Split(", ")); 
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

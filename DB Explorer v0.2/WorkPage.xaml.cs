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
            querry += $"ORDER BY {tableName}_id;";
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

        static public DataTable MakeReady1Table(string[] args)
        {
            string Querry = "SELECT * FROM get_authors_departments_publications() ";
            if (args.Length > 0)
            {
                Querry += $"WHERE LOWER(author_name) LIKE \'%{args[0]}%\'";
                for (int i = 1; i < args.Length; i++)
                {
                    Querry += $"OR LOWER(author_name) LIKE \'{args[i]}\' ";
                }
            }
            DataTable table = MakeTableByQuerry(Querry);

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

        static public DataTable MakeReady2Table(string[] args)
        {
            string Querry = "SELECT * FROM get_publications_grouped_by_departments_and_disciplines() ";
            if (args.Length > 0)
            {
                Querry += $"WHERE LOWER(department_name) LIKE \'%{args[0]}%\'";
                for (int i = 1; i < args.Length; i++)
                {
                    Querry += $"OR LOWER(department_name) LIKE \'{args[i]}\' ";
                }
            }
            DataTable table = MakeTableByQuerry(Querry);

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

        static public DataTable MakeReady3Table(string[] args)
        {
            string Querry = "SELECT * FROM get_publication_counts_by_departments_last_5_years() ";
            if (args.Length > 0)
            {
                Querry += $"WHERE LOWER(department_name) LIKE \'%{args[0]}%\'";
                for (int i = 1; i < args.Length; i++)
                {
                    Querry += $" OR LOWER(department_name) LIKE \'%{args[i]}%\' ";
                }
            }
            DataTable table = MakeTableByQuerry(Querry);

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

        static public DataTable MakeReady4Table(string[] args)
        {
            string Querry = "SELECT * FROM get_publication_type_counts_by_period() ";
            if (args.Length > 0)
            {
                Querry += $"WHERE LOWER(guide_type_name) LIKE \'%{args[0]}%\'";
                for (int i = 1; i < args.Length; i++)
                {
                    Querry += $"OR LOWER(guide_type_name) LIKE \'%{args[i]}%\' ";
                }
            }
            DataTable table = MakeTableByQuerry(Querry);

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
                    Querry += $"OR LOWER(author_name) LIKE \'%{args[i]}%\' ";
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

            readyQuerries1,
            readyQuerries2,
            readyQuerries3,
            readyQuerries4,
            readyQuerries5
        }

        static public Tables currentTable;


        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["university_DB"].ToString();

        private string teachersEnter = string.Empty;
        private string materialsEnter = string.Empty;
        private string disciplinesEnter = string.Empty;
        private string departmentsEnter = string.Empty;
        private string typesEnter = string.Empty;

        private string[] readiesEnter = new string[5];

        public WorkPage()
        {
            for (int i = 0; i < readiesEnter.Length; i++)
            {
                readiesEnter[i] = string.Empty;
            }

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

            AddNewRow.IsEnabled = true;
            ContextDelete.IsEnabled = true;
            ContextUpdate.IsEnabled = true;

            if (disciplinesEnter == string.Empty)
            {
                string Querry = "SELECT * FROM disciplines ORDER BY disciplines_id";
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

            AddNewRow.IsEnabled = true;
            ContextDelete.IsEnabled = true;
            ContextUpdate.IsEnabled = true;

            if (departmentsEnter == string.Empty)
            {
                string Querry = "SELECT * FROM departments ORDER BY departments_id";
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

            AddNewRow.IsEnabled = true;
            ContextDelete.IsEnabled = true;
            ContextUpdate.IsEnabled = true;

            if (typesEnter == string.Empty)
            {
                string Querry = "SELECT * FROM guides_types ORDER BY guides_types_id";
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

            AddNewRow.IsEnabled = false;
            ContextDelete.IsEnabled = false;
            ContextUpdate .IsEnabled = false;

            SearchBox.Text = readiesEnter[0];

            if (readiesEnter[0] == string.Empty)
            {
                SetDataGrid(Makers.MakeReady1Table(Array.Empty<string>()));
            }

            MaterialDesignThemes.Wpf.HintAssist.SetHelperText(SearchBox, "Поиск по автору");

            currentTable = Tables.readyQuerries1;
        }

        private void ReadyQuerries2_Click(object sender, RoutedEventArgs e)
        {
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            AddNewRow.IsEnabled = false;
            ContextDelete.IsEnabled = false;
            ContextUpdate.IsEnabled = false;

            SearchBox.Text = readiesEnter[1];

            if (readiesEnter[1] == string.Empty)
            {
                SetDataGrid(Makers.MakeReady2Table(Array.Empty<string>()));
            }

            MaterialDesignThemes.Wpf.HintAssist.SetHelperText(SearchBox, "Поиск по кафедре");

            currentTable = Tables.readyQuerries2;
        }

        private void ReadyQuerries3_Click(object sender, RoutedEventArgs e)
        {
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            AddNewRow.IsEnabled = false;
            ContextDelete.IsEnabled = false;
            ContextUpdate.IsEnabled = false;

            SearchBox.Text = readiesEnter[2];

            if (readiesEnter[2] == string.Empty)
            {
                SetDataGrid(Makers.MakeReady3Table(Array.Empty<string>()));
            }

            MaterialDesignThemes.Wpf.HintAssist.SetHelperText(SearchBox, "Поиск по кафедре");

            currentTable = Tables.readyQuerries3;
        }

        private void ReadyQuerries4_Click(object sender, RoutedEventArgs e)
        {
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            AddNewRow.IsEnabled = false;
            ContextDelete.IsEnabled = false;
            ContextUpdate.IsEnabled = false;

            SearchBox.Text = readiesEnter[3];

            if (readiesEnter[3] == string.Empty)
            {
                SetDataGrid(Makers.MakeReady4Table(Array.Empty<string>()));
            }

            MaterialDesignThemes.Wpf.HintAssist.SetHelperText(SearchBox, "Поиск по типу издания");
            currentTable = Tables.readyQuerries4;
        }

        private void ReadyQuerries5_Click(object sender, RoutedEventArgs e)
        {
            Teachers.Foreground = new SolidColorBrush(Colors.Black);
            Materials.Foreground = new SolidColorBrush(Colors.Black);
            Disciplines.Foreground = new SolidColorBrush(Colors.Black);
            Departments.Foreground = new SolidColorBrush(Colors.Black);
            TypesOfMaterials.Foreground = new SolidColorBrush(Colors.Black);

            AddNewRow.IsEnabled = false;
            ContextDelete.IsEnabled = false;
            ContextUpdate.IsEnabled = false;

            SearchBox.Text = readiesEnter[4];

            if (readiesEnter[4] == string.Empty)
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
                    targetColumn = "guide_type_name";

                    dt = Makers.MakeTypesOfMaterialsTable(Makers.MakeQuerry(currentTable, targetColumn, args.ToLower().Split(", ")));
                    break;

                case Tables.readyQuerries1:
                    readiesEnter[0] = SearchBox.Text;
                    args = readiesEnter[0];

                    dt = Makers.MakeReady1Table(args.ToLower().Split(", "));
                    break;

                case Tables.readyQuerries2:
                    readiesEnter[1] = SearchBox.Text;
                    args = readiesEnter[1];

                    dt = Makers.MakeReady2Table(args.ToLower().Split(", "));
                    break;

                case Tables.readyQuerries3:
                    readiesEnter[2] = SearchBox.Text;
                    args = readiesEnter[2];

                    dt = Makers.MakeReady3Table(args.ToLower().Split(", "));
                    break;

                case Tables.readyQuerries4:
                    readiesEnter[3] = SearchBox.Text;
                    args = readiesEnter[3];

                    dt = Makers.MakeReady4Table(args.ToLower().Split(", "));
                    break;

                case Tables.readyQuerries5:
                    readiesEnter[4] = SearchBox.Text;
                    args = readiesEnter[4];

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
            if (args.Length > 0)
            {
                switch (currentTable)
                {
                    case Tables.teachers:
                        TeachersWin Twin = new TeachersWin(this, "UPDATE", args)
                        {
                            Owner = Application.Current.MainWindow
                        };
                        Twin.Show();
                        break;

                    case Tables.guides:
                        GuidesWin Gwin = new GuidesWin(this, "UPDATE", args)
                        {
                            Owner = Application.Current.MainWindow
                        };
                        Gwin.Show();
                        break;
                    case Tables.disciplines:
                        DisciplinesWin Dwin = new DisciplinesWin(this, "UPDATE", args)
                        {
                            Owner = Application.Current.MainWindow
                        };
                        Dwin.Show();
                        break;
                    case Tables.departments:
                        DepartmentsWin Dpwin = new DepartmentsWin(this, "UPDATE", args)
                        {
                            Owner = Application.Current.MainWindow
                        };
                        Dpwin.Show();
                        break;
                    case Tables.guides_types:
                        GuidesTypesWin Gtwin = new GuidesTypesWin(this, "UPDATE", args)
                        {
                            Owner = Application.Current.MainWindow
                        };
                        Gtwin.Show();
                        break;
                }
            }
        }

        private void AddNewRow_Click(object sender, RoutedEventArgs e)
        {
            switch(currentTable)
            {
                case Tables.teachers:
                    TeachersWin win = new TeachersWin(this, "INSERT", Array.Empty<string>())
                    {
                        Owner = Application.Current.MainWindow
                    };
                    win.Show();
                    break;

                case Tables.guides:
                    GuidesWin Gwin = new GuidesWin(this, "INSERT", Array.Empty<string>())
                    {
                        Owner = Application.Current.MainWindow
                    };
                    Gwin.Show();
                    break;
                case Tables.disciplines:
                    DisciplinesWin Dwin = new DisciplinesWin(this, "INSERT,", Array.Empty<string>())
                    {
                        Owner = Application.Current.MainWindow
                    };
                    Dwin.Show();
                    break;
                case Tables.departments:
                    DepartmentsWin Dpwin = new DepartmentsWin(this, "INSERT", Array.Empty<string>())
                    {
                        Owner = Application.Current.MainWindow
                    };
                    Dpwin.Show();
                    break;
                case Tables.guides_types:
                    GuidesTypesWin Gtwin = new GuidesTypesWin(this, "INSERT", Array.Empty<string>())
                    {
                        Owner = Application.Current.MainWindow
                    };
                    Gtwin.Show();
                    break;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }
    }
}

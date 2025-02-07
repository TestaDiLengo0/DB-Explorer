using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace DB_Explorer_v0._2.RedcWins
{
    /// <summary>
    /// Логика взаимодействия для GuidesWin.xaml
    /// </summary>
    public partial class GuidesWin : Window
    {
        WorkPage parentPage;

        private string connectionString = ConfigurationManager.ConnectionStrings["university_DB"].ToString();

        private string commandType = string.Empty;
        private string[] args = Array.Empty<string>();

        private void SetStartBoxes()
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();


            //Заполнение списка авторов
            NpgsqlCommand command = new NpgsqlCommand()
            {
                Connection = connection,
                CommandText = "SELECT teachers_id, teacher_name FROM teachers ORDER BY teachers_id"
            };
            NpgsqlDataReader dataReader = command.ExecuteReader();

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    authorNameCombo.Items.Add(dataReader.GetInt32(0) + ", " + dataReader.GetString(1));
                }
            }
            dataReader.Close();
            command.Dispose();

            //Заполнение списка типов изданий
            command = new NpgsqlCommand()
            {
                Connection = connection,
                CommandText = "SELECT * FROM guides_types ORDER BY guides_types_id"
            };
            dataReader = command.ExecuteReader();

            if (dataReader.HasRows) 
            {
                while (dataReader.Read())
                {
                    typeCombo.Items.Add(dataReader.GetInt32(0) + ", " + dataReader.GetString(1));
                }
            }
            dataReader.Close();
            command.Dispose();

            //Заполнение списка дисциплин
            command = new NpgsqlCommand()
            {
                Connection = connection,
                CommandText = "SELECT * FROM disciplines ORDER BY disciplines_id"
            };
            dataReader = command.ExecuteReader();

            if (dataReader.HasRows) 
            {
                while(dataReader.Read())
                {
                    disciplineCombo.Items.Add(dataReader.GetInt32(0) + ", " + dataReader.GetString(1));
                }
            }
            dataReader.Close();
            command.Dispose();
            connection.Close();
            connection.Dispose();


            switch (commandType)
            {
                case "INSERT":
                    authorNameCombo.SelectedIndex = 0;
                    typeCombo.SelectedIndex = 0;
                    disciplineCombo.SelectedIndex = 0;
                    break;
                case "UPDATE":
                    nameBox.Text = args[1];

                    int authorNameIndex = 0;
                    for (; authorNameIndex < authorNameCombo.Items.Count; authorNameIndex++)
                    {
                        if (args[2] == authorNameCombo.Items[authorNameIndex].ToString().Split(", ")[1]) break;
                    }
                    authorNameCombo.SelectedIndex = authorNameIndex;

                    pubYearBox.Text = args[3];
                    numberPagesBox.Text = args[4];

                    int typeIndex = 0;
                    for(;typeIndex < typeCombo.Items.Count; typeIndex++)
                    {
                        if (args[5] == typeCombo.Items[typeIndex].ToString().Split(", ")[1]) break;
                    }
                    typeCombo.SelectedIndex = typeIndex;

                    int disciplineIndex = 0;
                    for(; disciplineIndex < disciplineCombo.Items.Count; disciplineIndex++)
                    {
                        if (args[6] == disciplineCombo.Items[disciplineIndex].ToString().Split(", ")[1]) break;
                    }
                    disciplineCombo.SelectedIndex = disciplineIndex;
                    break;
            }
        }

        public GuidesWin(WorkPage parentPage, string commandType, string[] args)
        {
            this.parentPage = parentPage;
            this.parentPage.IsEnabled = false;
            this.commandType = commandType;
            this.args = args;

            InitializeComponent();

            SetStartBoxes();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();

            try
            {
                parentPage.IsEnabled = true;
                switch (commandType)
                {
                    case "INSERT":
                        NpgsqlCommand com = new NpgsqlCommand();
                        com.Connection = connection;
                        com.CommandText = $"INSERT INTO guides(guide_name, guide_author_id, guide_pub_year, guide_number_pages, guide_type, guide_discipline) " +
                            $"VALUES (\'{nameBox.Text}\', {authorNameCombo.SelectedItem.ToString().Split(", ")[0]}, {pubYearBox.Text}, {numberPagesBox.Text}, {typeCombo.SelectedItem.ToString().Split(", ")[0]}, {disciplineCombo.SelectedItem.ToString().Split(", ")[0]});";
                        com.ExecuteReader();
                        com.Dispose();

                        parentPage.SetDataGrid(parentPage.CreateTableWithEnters());
                        break;

                    case "UPDATE":
                        NpgsqlCommand command = new NpgsqlCommand();
                        command.Connection = connection;
                        command.CommandText = $"UPDATE guides SET guide_name = \'{nameBox.Text}\', guide_author_id = {authorNameCombo.SelectedItem.ToString().Split(", ")[0]}, " +
                            $"guide_pub_year = {pubYearBox.Text}, guide_number_pages = {numberPagesBox.Text}, guide_type = {typeCombo.SelectedItem.ToString().Split(", ")[0]}, " +
                            $"guide_discipline = {disciplineCombo.SelectedItem.ToString().Split(", ")[0]} WHERE guides_id = {args[0]};";
                        command.ExecuteReader();
                        command.Dispose();

                        parentPage.SetDataGrid(parentPage.CreateTableWithEnters());
                        break;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Данные введены некорректно!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            parentPage.IsEnabled = true;
            this.Close();
        }
    }
}
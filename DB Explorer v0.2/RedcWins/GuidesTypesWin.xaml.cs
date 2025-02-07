using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
    /// Логика взаимодействия для GuidesTypesWin.xaml
    /// </summary>
    public partial class GuidesTypesWin : Window
    {
        private WorkPage parentPage;

        private string connectionString = ConfigurationManager.ConnectionStrings["university_DB"].ToString();

        private string commandType = string.Empty;
        private string[] args = Array.Empty<string>();

        private void SetStartBoxes()
        {
            if (commandType == "UPDATE")
            {
                nameBox.Text = args[1];
            }
        }

        public GuidesTypesWin(WorkPage parentPage, string comandType, string[] args)
        {
            this.parentPage = parentPage;
            this.parentPage.IsEnabled = false;

            this.commandType = comandType;
            this.args = args;

            InitializeComponent();

            SetStartBoxes();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();

            if (!(nameBox.Text == string.Empty | nameBox.Text == null | nameBox.Text == ""))
            {
                parentPage.IsEnabled = true;
                switch (commandType)
                {
                    case "INSERT":
                        NpgsqlCommand com = new NpgsqlCommand();
                        com.Connection = connection;
                        com.CommandText = $"INSERT INTO guides_types(guide_type_name) VALUES (\'{nameBox.Text}\');";
                        com.ExecuteReader();
                        com.Dispose();

                        parentPage.SetDataGrid(parentPage.CreateTableWithEnters());
                        break;

                    case "UPDATE":
                        NpgsqlCommand command = new NpgsqlCommand();
                        command.Connection = connection;
                        command.CommandText = $"UPDATE guides_types SET guide_type_name = \'{nameBox.Text}\' WHERE guides_types_id = {args[0]};";
                        command.ExecuteReader();
                        command.Dispose();

                        parentPage.SetDataGrid(parentPage.CreateTableWithEnters());
                        break;
                }
                this.Close();
            }
            else
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

﻿using Npgsql;
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
    /// Логика взаимодействия для TeachersWin.xaml
    /// </summary>
    public partial class TeachersWin : Window
    {
        private WorkPage parentPage;

        static private string connectionString = ConfigurationManager.ConnectionStrings["university_DB"].ToString();

        private string commandType = "";
        private string[] args = [];

        private void SetDepCombo()
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();

            NpgsqlCommand command = new NpgsqlCommand()
            {
                Connection = connection,
                CommandText = "SELECT * FROM departments ORDER BY departments_id"
            };
            NpgsqlDataReader dataReader = command.ExecuteReader();

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    departmentIDCombo.Items.Add(dataReader.GetInt32(0) + ", " + dataReader.GetString(1));
                }
                dataReader.Dispose();
            }
            switch (commandType)
            {
                case "INSERT":
                    departmentIDCombo.SelectedIndex = 0;
                    break;
                case "UPDATE":
                    nameBox.Text = args[1];
                    int i = 0;
                    for(; i < departmentIDCombo.Items.Count; i++)
                    {
                        if (args[2] == departmentIDCombo.Items[i].ToString().Split(", ")[1]) break;
                    }
                    departmentIDCombo.SelectedIndex = i;
                    break;
            }
        }

        public TeachersWin(WorkPage parentPage, string commandType, string[] args)
        {
            this.parentPage = parentPage;
            this.parentPage.IsEnabled = false;

            this.commandType = commandType;
            this.args = args;

            InitializeComponent();

            SetDepCombo();
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
                        com.CommandText = $"INSERT INTO teachers(teacher_name, teacher_department_id) VALUES (\'{nameBox.Text}\', {departmentIDCombo.SelectedItem.ToString().Split(", ")[0]});";
                        com.ExecuteReader();
                        com.Dispose();

                        parentPage.SetDataGrid(parentPage.CreateTableWithEnters());
                        break;

                    case "UPDATE":
                        NpgsqlCommand command = new NpgsqlCommand();
                        command.Connection = connection;
                        command.CommandText = $"UPDATE teachers SET teacher_name = \'{nameBox.Text}\', teacher_department_id = {departmentIDCombo.SelectedItem.ToString().Split(", ")[0]} WHERE teachers_id = {args[0]};";
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

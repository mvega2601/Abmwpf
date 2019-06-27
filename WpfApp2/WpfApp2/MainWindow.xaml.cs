using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;

namespace WPF_AccessDB
{
    
    public partial class MainWindow : Window
    {
        OleDbConnection con;
        DataTable dt;

        public MainWindow()
        {
            InitializeComponent();

            
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\AbmDB.mdb";
            BindGrid();
        }

        
        private void BindGrid()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from AbmDB";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvData.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lblCount.Visibility = System.Windows.Visibility.Hidden;
                gvData.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lblCount.Visibility = System.Windows.Visibility.Visible;
                gvData.Visibility = System.Windows.Visibility.Hidden;
            }

        }

        
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtEmpId.Text != "")
            {
                if (txtEmpId.IsEnabled == true)
                {
                    if (ddlGender.Text != "Seleccionar Género")
                    {
                        cmd.CommandText = "insert into AbmDB(Id,Nombre,Genero,Numero,Direccion) Values(" + txtEmpId.Text + ",'" + txtEmpName.Text + "','" + ddlGender.Text + "'," + txtContact.Text + ",'" + txtAddress.Text + "')";
                        cmd.ExecuteNonQuery();
                        BindGrid();
                        MessageBox.Show("Empleado cargado con éxito");
                        ClearAll();

                    }
                    else
                    {
                        MessageBox.Show("Por favor Seleccione un genero");
                    }
                }
                else
                {
                    cmd.CommandText = "update AbmDB set Nombre='" + txtEmpName.Text + "',Genero='" + ddlGender.Text + "',Numero=" + txtContact.Text + ",Direccion='" + txtAddress.Text + "' where Id=" + txtEmpId.Text;
                    cmd.ExecuteNonQuery();
                    BindGrid();
                    MessageBox.Show("Empleado actualizado");
                    ClearAll();
                }
            }
            else
            {
                MessageBox.Show("Por favor complete todos los campos");
            }
        }

        
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void ClearAll()
        {
            txtEmpId.Text = "";
            txtEmpName.Text = "";
            ddlGender.SelectedIndex = 0;
            txtContact.Text = "";
            txtAddress.Text = "";
            btnAdd.Content = "Add";
            txtEmpId.IsEnabled = true;
        }

        
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];
                txtEmpId.Text = row["ID"].ToString();
                txtEmpName.Text = row["Nombre"].ToString();
                ddlGender.Text = row["Genero"].ToString();
                txtContact.Text = row["Numero"].ToString();
                txtAddress.Text = row["Direccion"].ToString();
                txtEmpId.IsEnabled = false;
                btnAdd.Content = "Update";
            }
            else
            {
                MessageBox.Show("Por favor seleccione algun item de la lista");
            }
        }

        
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (gvData.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvData.SelectedItems[0];

                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from AbmDB where Id=" + row["Id"].ToString();
                cmd.ExecuteNonQuery();
                BindGrid();
                MessageBox.Show("Empleado borrado con éxito");
                ClearAll();
            }
            else
            {
                MessageBox.Show("Seleccione un item de la lista");
            }
        }

        //salir
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtContact_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
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
using MySqlConnector;
using System.Data;
namespace Konyvtar_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connString = "server=localhost;user=root;password =;database=konyvtar_db;";
        List<string> mufaj2 = new List<string>();
        List<string> mufaj = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            
            mufajFeltolt();
            cb_Mufaj.ItemsSource = mufaj;
            cb_Mufaj.SelectedItem = mufaj[0];
            mufajFeltolt2();
            cb_Mufaj2.ItemsSource = mufaj2;
            cb_Mufaj2.SelectedItem = mufaj2[0];
            Betoltes();

        }
        private void mufajFeltolt()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT DISTINCT mufaj FROM konyv";

                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow sor in dt.Rows)
                    {
                        mufaj.Add(sor["mufaj"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Hiba a műfajok betöltésekor!");
                }
            }
        }
        private void mufajFeltolt2()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT DISTINCT mufaj FROM konyv";

                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow sor in dt.Rows)
                    {
                        mufaj.Add(sor["mufaj"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Hiba a műfajok betöltésekor!");
                }
            }
        }

        private void Betoltes()
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM konyv";

                    MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgKonyv.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Hiba az adatbetöltés során!");
                }
            }
            

        }

        private bool szamE(string szoveg)
        {
            return int.TryParse(szoveg, out int szam);

        }

        private void Hozzaad_Button_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                if (txtKiadas_eve.Text == "" || txtCim.Text == "")
                {
                    MessageBox.Show("A mező kitöltése kötelező!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if (!szamE(txtKiadas_eve.Text) || int.Parse(txtKiadas_eve.Text) < 0 || int.Parse(txtKiadas_eve.Text) > 500)
                {
                    MessageBox.Show("Számadat megadása kötelező ill. nem megfelelő számérték!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                conn.Open();
                string sql = "INSERT INTO konyv (cim, hossz, korhatar, mufaj) VALUES (@c, @k, @sz, @m)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@c", txtCim.Text);
                cmd.Parameters.AddWithValue("@k", txtKiadas_eve.Text);
                cmd.Parameters.AddWithValue("@sz", txtSzerzo.Text);
                cmd.Parameters.AddWithValue("@m", cb_Mufaj.SelectedItem.ToString());
                cmd.ExecuteNonQuery();
            }
            Betoltes();
        }

        private void Modosit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dgKonyv.SelectedItem == null)
            {
                MessageBox.Show("Nem választottál ki elemet!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (txtKiadas_eve.Text == "" || txtCim.Text == "")
            {
                MessageBox.Show("A mező kitöltése kötelező!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (!szamE(txtKiadas_eve.Text) || int.Parse(txtKiadas_eve.Text) < 0 || int.Parse(txtKiadas_eve.Text) > 500)
            {
                MessageBox.Show("Számadat megadása kötelező ill. nem megfelelő számérték!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataRowView drv = (DataRowView)dgKonyv.SelectedItem;


            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "UPDATE konyv SET cim=@c, kiadas_eve=@h, szerzo=@k, mufaj=@m WHERE id=@id";


                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", drv["id"]);
                cmd.Parameters.AddWithValue("@c", txtCim.Text);
                cmd.Parameters.AddWithValue("@k", txtKiadas_eve.Text);
                cmd.Parameters.AddWithValue("@sz", txtSzerzo.Text);
                cmd.Parameters.AddWithValue("@m", cb_Mufaj.SelectedItem.ToString());
                cmd.ExecuteNonQuery();
            }
            Betoltes();
        }

        private void dgKonyv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgKonyv.SelectedItem == null) return;
            DataRowView drv = (DataRowView)dgKonyv.SelectedItem;
            txtCim.Text = drv["cim"].ToString();
            txtKiadas_eve.Text = drv["kiadaseve"].ToString();
            txtSzerzo.Text = drv["szerzo"].ToString();
            cb_Mufaj.SelectedItem = drv["mufaj"].ToString();
            cb_Mufaj2.SelectedItem = drv["mufaj2"].ToString();
        }

        private void Torles_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dgKonyv.SelectedItem == null)
            {
                MessageBox.Show("Nem választottál ki elemet!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataRowView drv = (DataRowView)dgKonyv.SelectedItem;


            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "DELETE FROM konyv WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", drv["id"]);
                cmd.ExecuteNonQuery();
            }
            Betoltes();
        }
        private void UrlapTorles_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dgKonyv.SelectedItem == null)
            {
                MessageBox.Show("Nem választottál ki elemet!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataRowView drv = (DataRowView)dgKonyv.SelectedItem;


            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();
                string sql = "DELETE FROM konyv WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", drv["id"]);
                cmd.ExecuteNonQuery();
            }
            Betoltes();
        }


        private void txtCimkeres_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT* FROM diakok WHERE  nev like @k";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@c", "%" + txtCimkeres.Text + "%");
                    MySqlDataAdapter ad = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    dgKonyv.ItemsSource = dt.DefaultView;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}


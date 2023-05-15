using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        MySqlConnection sqlConnection;
        MySqlCommand sqlCommand;
        MySqlDataAdapter sqlDataAdapter;
        MySqlDataReader sqlDataReader;
        DataTable dtcombobox =new DataTable();
        DataTable dtPlayer = new DataTable();
        DataTable dtManager = new DataTable();
        DataTable dtManager0 = new DataTable();
        string Connection = "server=localhost;uid=root;pwd=;database=premier_league";
        string query = "";
        string managername = "";
        public Form1()
        {
            InitializeComponent();
        }
        private void updateDGVplayer()
        {
            dtPlayer = new DataTable();
            sqlConnection = new MySqlConnection(Connection);
            query = "select * from player;";
            sqlCommand = new MySqlCommand(query, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dtPlayer);
            dataGridView1.DataSource = dtPlayer;
        }
        private void manageridstatus()
        {
            dtManager = new DataTable();
            query = $"SELECT manager_name,nation,birthdate\r\nFROM manager m\r\nJOIN nationality n\r\non m.nationality_id=n.nationality_id\r\nWhere working=0;";
            sqlCommand = new MySqlCommand(query, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dtManager);
            dataGridView3.DataSource = dtManager;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                dtcombobox = new DataTable();
                sqlConnection = new MySqlConnection(Connection);
                query = "SELECT nation N,nationality_id as 'ID' FROM nationality n;";
                sqlCommand = new MySqlCommand(query, sqlConnection);
                sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dtcombobox);
                cmb_nationality.DataSource= dtcombobox;
                cmb_nationality.DisplayMember = "N";
                cmb_nationality.ValueMember = "ID";

                dtcombobox = new DataTable();
                query = "SELECT team_name as 'tname', team_id as 'tid' FROM team t;";
                sqlCommand = new MySqlCommand(query, sqlConnection);
                sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dtcombobox);
                cmb_teamname.DataSource = dtcombobox;
                cmb_teamname.DisplayMember = "tname";
                cmb_teamname.ValueMember = "tid";
                cmb_team.DataSource= dtcombobox;
                cmb_team.DisplayMember = "tname";
                cmb_team.ValueMember = "tid";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            updateDGVplayer();
            manageridstatus();
        }

        private void btn_playertoteam_Click(object sender, EventArgs e)
        {
            string playerid = txtBox_playerid.Text;
            string name = txtBox_name.Text;
            string teamnumber = txtBox_teamnumber.Text;
            string nationality = cmb_nationality.SelectedValue.ToString();
            string pos = txtBox_position.Text;
            string height = txtBox_height.Text;
            string weight = txtBox_weight.Text;
            string birthdate = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
            string teamname = cmb_teamname.SelectedValue.ToString();
            //MessageBox.Show(teamname);
            query = $"INSERT INTO PLAYER VALUES ('{playerid}',{teamnumber},'{name}','{nationality}','{pos}',{height},{weight},'{birthdate}','{teamname}',1,0);";
            try
            {
                sqlConnection.Open();
                sqlCommand= new MySqlCommand(query,sqlConnection);
                sqlDataReader=sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
                updateDGVplayer();
            }
        }

        private void cmb_team_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtManager0 = new DataTable();
            sqlConnection = new MySqlConnection(Connection);
            query = $"SELECT m.manager_name,t.team_name,birthdate,nation\r\nFrom team t\r\nJOIN manager m\r\non t.manager_id=m.manager_id\r\njoin nationality n\r\non m.nationality_id=n.nationality_id\r\nwhere t.team_name='{cmb_team.Text}';";
            sqlCommand = new MySqlCommand(query, sqlConnection);
            sqlDataAdapter = new MySqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dtManager0);
            dataGridView2.DataSource = dtManager0;
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            managername = dataGridView3.CurrentCell.Value.ToString();
            MessageBox.Show(dataGridView3.CurrentCell.Value.ToString());
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string managerdgvatas=dtManager0.Rows[0][0].ToString();
            query = $"UPDATE manager\r\nset working=0\r\nwhere manager_name='{managerdgvatas}'";
        }
    }
    
}

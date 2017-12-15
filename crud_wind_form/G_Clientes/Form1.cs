using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace G_Clientes
{
    public partial class Form1 : Form
    {
        int id_cliente;
        string connString = @"server = .\sqlexpress;Database=crud;integrated security = true;";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(Object sender, EventArgs e)
        {
            disp_data();    
        }

        private void buscar_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM CLIENTE WHERE id LIKE '%" + txtBuscar.Text + 
                         "%' or nome LIKE '%" + txtBuscar.Text + "%' or dt_nascimento LIKE '%" + txtBuscar.Text +"%'"+
                          " or email LIKE '%" + txtBuscar.Text + "%'  or cidade LIKE '%" + txtBuscar.Text + "%'";

            disp_data(sql);
            zerarTxt();

        }

        private void salvar_click(object sender, EventArgs e)
        {
            if (id_cliente < 1)
            {
                string sql = "INSERT INTO CLIENTE (NOME, DT_NASCIMENTO, TELEFONE, EMAIL, CIDADE) "  
                      + "VALUES ('" + txtNome.Text + "', '" + txtDt_nascimento.Text + "', '"
                      + txtTelefone.Text + "', '" + txtEmail.Text
                      + "', '" + txtCidade.Text + "')";
                conectionData(sql, "Resgitro inserido!");
                disp_data();
            }
            else
            {
                string sql = "UPDATE CLIENTE SET NOME='" + txtNome.Text + "', Dt_nascimento = '" + txtDt_nascimento.Text + "', " +
                "telefone='" + txtTelefone.Text + "', Email='" + txtEmail.Text + "',CIDADE = '" + txtCidade.Text + "' WHERE ID=" + id_cliente;
                conectionData(sql, "Upload efetuado!");
                disp_data();
            }           
            
        }

        private void excluir_click(object sender, EventArgs e)
        {
            
            if(MessageBox.Show("Deseja deletar item selecionado ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "DELETE FROM CLIENTE WHERE ID=" + id_cliente;
                conectionData(sql, "Cliente excluido!");
                disp_data();
                zerarTxt();
            }
        }

        private void atualizar_click(object sender, EventArgs e)
        {
            zerarTxt();    
        }

        public void zerarTxt()
        {
            id_cliente = 0;
            atualizar.Enabled = false;
            salvar.Enabled = true;
            excluir.Enabled = false;
            txtNome.Text = "";
            txtDt_nascimento.Text = "";
            txtTelefone.Text = "";
            txtEmail.Text = "";
            txtCidade.Text = "";
            txtNome.Focus();
            id_cliente = 0;
        }

        private void conectionData(String sql, String msg)
        {
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                try
                {
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                        MessageBox.Show(msg);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.ToString());
                }
            }
            catch (SqlException sqle)
            {
                MessageBox.Show("Falha ao efetuar a conexão. Erro: " + sqle);
            }

            finally
            {
                conn.Close();
            }
        }

        public void disp_data(string sql = "select * from cliente")
        {
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    ListaClientes.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao consulta lista de clientes: " + ex.ToString());
                }
            }
            catch (SqlException sqle)
            {
                MessageBox.Show("Falha ao efetuar a conexão. Erro: " + sqle);
            }

            finally
            {
                conn.Close();
            }
            
        }

        private void ListaClientes_MouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            id_cliente = Convert.ToInt32(ListaClientes.SelectedRows[0].Cells[0].Value.ToString());
            txtNome.Text = ListaClientes.SelectedRows[0].Cells[1].Value.ToString();
            txtDt_nascimento.Text = ListaClientes.SelectedRows[0].Cells[2].Value.ToString();
            txtTelefone.Text = ListaClientes.SelectedRows[0].Cells[3].Value.ToString();
            txtEmail.Text = ListaClientes.SelectedRows[0].Cells[4].Value.ToString();
            txtCidade.Text = ListaClientes.SelectedRows[0].Cells[5].Value.ToString();
            atualizar.Enabled = true;
            excluir.Enabled = true;
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}

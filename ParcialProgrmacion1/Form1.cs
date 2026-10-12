using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ControlProductos
{
    public partial class Form1 : Form
    {
        // CAMBIA ESTO SEGUN TU SERVIDOR
        string connectionString = @"Server=DESKTOP-GDAOJOT\SQLEXPRESS;Database=ParcialProgramacion1;Integrated Security=True;";


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConsultarProductos();
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        // VALIDACIONES - Actividad 4
        private bool ValidarCampos()
        {
            if (!(!string.IsNullOrWhiteSpace(txtNombre.Text) && !string.IsNullOrWhiteSpace(txtCategoria.Text)))
            {
                MessageBox.Show("El nombre y la categoría son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!decimal.TryParse(textBox2.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("El precio debe ser numérico y mayor que cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!int.TryParse(textBox4.Text, out int existencias) || existencias < 0)
            {
                MessageBox.Show("Las existencias deben ser un número entero igual o mayor que cero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // 1. GUARDAR
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "INSERT INTO Productos (Nombre, Categoria, Precio, Existencias) VALUES (@Nombre, @Categoria, @Precio, @Existencias)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@Categoria", txtCategoria.Text.Trim());
                        cmd.Parameters.AddWithValue("@Precio", decimal.Parse(textBox2.Text));
                        cmd.Parameters.AddWithValue("@Existencias", int.Parse(textBox4.Text));
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Producto registrado correctamente.");
                ConsultarProductos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }

        // 2. CONSULTAR
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            ConsultarProductos();
        }

        private void ConsultarProductos()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Productos", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvProductos.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar: " + ex.Message);
            }
        }

        // 3. ACTUALIZAR
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow == null) { MessageBox.Show("Seleccione un producto de la tabla."); return; }
            if (!ValidarCampos()) return;
            try
            {
                int id = Convert.ToInt32(dgvProductos.CurrentRow.Cells["IdProducto"].Value);
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "UPDATE Productos SET Nombre=@Nombre, Categoria=@Categoria, Precio=@Precio, Existencias=@Existencias WHERE IdProducto=@Id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@Categoria", txtCategoria.Text.Trim());
                        cmd.Parameters.AddWithValue("@Precio", decimal.Parse(textBox2.Text));
                        cmd.Parameters.AddWithValue("@Existencias", int.Parse(textBox4.Text));
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Producto actualizado correctamente.");
                ConsultarProductos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message);
            }
        }

        // 4. ELIMINAR
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow == null) { MessageBox.Show("Seleccione un producto."); return; }
            if (MessageBox.Show("¿Está seguro de eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                int id = Convert.ToInt32(dgvProductos.CurrentRow.Cells["IdProducto"].Value);
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM Productos WHERE IdProducto=@Id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Producto eliminado correctamente.");
                ConsultarProductos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
        }

        // 5. LIMPIAR
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtCategoria.Clear();
            textBox2.Clear();
            textBox4.Clear();
            txtNombre.Focus();
        }

        // Al hacer clic en la tabla, llena los campos
        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProductos.Rows[e.RowIndex];
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtCategoria.Text = row.Cells["Categoria"].Value.ToString();
                textBox2.Text = row.Cells["Precio"].Value.ToString();
                textBox4.Text = row.Cells["Existencias"].Value.ToString();
            }
        }
    }
}


//Carlos Edenilson Osorio MAgaña 260218, Enzo Fabricio Argueta Alas 261088, Jose Francisco Rivera Vazquez 260346
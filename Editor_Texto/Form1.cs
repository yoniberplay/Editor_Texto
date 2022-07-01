using System.Text.Json;
using System.Text.Json.Serialization;
namespace Editor_Texto
{
    public partial class Form1 : Form
    {
        string file_ext=".txt";
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        Dictionary<string, string> prefs = new Dictionary<string, string>();
        FontConverter var = new FontConverter();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = "Guardar Archivo";
            saveFileDialog.DefaultExt = file_ext;

            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = "Abrir Archivo";
            openFileDialog.DefaultExt = file_ext;
            Load_Preferences();
            Color bg = Color.FromName(prefs["Color de Fondo"]);
            Color fg = Color.FromName(prefs["Color de Texto"]);
            txt_box.Font = var.ConvertFromString(prefs["Fuente"]) as Font;
            this.ForeColor = fg;
            txt_box.ForeColor = fg;
            this.BackColor = bg;
            txt_box.BackColor = bg;
        }
        private void File_Refresh()
        {
            saveFileDialog.Filter = ".txt | Plain Text";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.Title = "";
            saveFileDialog.DefaultExt = file_ext;

            openFileDialog.Filter = ".txt | Plain Text";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = "";
            openFileDialog.DefaultExt = file_ext;
        }
        private void Guardar(string file)
        {
            File.WriteAllText(file, txt_box.Text);
            this.Text = Path.GetFileName(file);

        }
        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.Text.Equals("Nuevo"))
                {
                    if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        Guardar(saveFileDialog.FileName);
                    }
                }
                else
                {
                    Guardar(saveFileDialog.FileName);
                }
                
            }
            catch (Exception Error)
            {
                MessageBox.Show("Error: " + Error.Message, "Ha habido un error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Text = "Nuevo";
            txt_box.Clear();
            saveFileDialog.Reset();
            openFileDialog.Reset();
            File_Refresh();
        }

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog.ShowDialog()==DialogResult.OK)
                {
                    txt_box.Text = File.ReadAllText(openFileDialog.FileName);
                    this.Text =  Path.GetFileName(openFileDialog.FileName);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cambiarFuenteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog font = new FontDialog();
            if (font.ShowDialog()==DialogResult.OK)
            {
                txt_box.Font = font.Font;
                Save_Preferences();
            }
        }
        private Color get_Color()
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                return colorDialog.Color;
            }
            else
            {
                return Color.Empty;
            }
        }
        private void opcionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void colorDeTextoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color forecolor = get_Color();
            if (forecolor!=Color.Empty)
            {
                this.ForeColor = forecolor;
                txt_box.ForeColor = forecolor;
                Save_Preferences();
            }
        }

        private void colorDeFondoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color bgcolor = get_Color();
            if (bgcolor != Color.Empty)
            {
                txt_box.BackColor = bgcolor;
                this.BackColor = bgcolor;
                Save_Preferences();
            }
        }
        private void Save_Preferences()
        {
            prefs.Clear();
            prefs.Add("Color de Texto", txt_box.ForeColor.Name);
            prefs.Add("Color de Fondo", txt_box.BackColor.Name);
            prefs.Add("Fuente", var.ConvertToString(txt_box.Font));
            string textfile =  JsonSerializer.Serialize(prefs); 
            File.WriteAllText("Preferences.json", textfile);
        }
        private void Load_Preferences()
        {
            if (File.Exists("Preferences.json"))
            {
                prefs = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText("Preferences.json"));
            }
            else
            {
                Save_Preferences();
            }
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txt_box.Copy();
        }

        private void copiarTodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txt_box.SelectAll();
            txt_box.Copy();
        }

        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txt_box.Paste();
        }

        private void cortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txt_box.Cut();
        }

    }
}
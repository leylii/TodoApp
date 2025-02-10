using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TodoApp
{
    public partial class TodoList : Form
    {
        private List<string> allTasks = new List<string>();  

        public TodoList()
        {
            InitializeComponent();
        }

        private void TodoList_Load(object sender, EventArgs e)
        {
            LoadTasks();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lstTasks.SelectedIndex != -1)
            {
                string newTask = PromptDialog("Edit Task:", "Edit Task");
                if (!string.IsNullOrWhiteSpace(newTask))
                {
                    lstTasks.Items[lstTasks.SelectedIndex] = newTask;
                    SaveTasks();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTask.Text))
            {
                lstTasks.Items.Add(txtTask.Text);
                txtTask.Clear();
                SaveTasks();  // ذخیره لیست تسک‌ها
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstTasks.SelectedIndex != -1)
            {
                lstTasks.Items.RemoveAt(lstTasks.SelectedIndex);
                SaveTasks();
            }
        }

        private string PromptDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label lblText = new Label() { Left = 10, Top = 20, Text = text };
            TextBox txtInput = new TextBox() { Left = 10, Top = 50, Width = 260 };
            Button btnOk = new Button() { Text = "OK", Left = 100, Width = 100, Top = 80 };

            btnOk.Click += (sender, e) => { prompt.DialogResult = DialogResult.OK; prompt.Close(); };

            prompt.Controls.Add(lblText);
            prompt.Controls.Add(txtInput);
            prompt.Controls.Add(btnOk);
            prompt.AcceptButton = btnOk;

            return prompt.ShowDialog() == DialogResult.OK ? txtInput.Text : "";
        }

        private void SaveTasks()
        {
            allTasks.Clear();
            foreach (var item in lstTasks.Items)
            {
                allTasks.Add(item.ToString());
            }

            string json = JsonConvert.SerializeObject(allTasks, Formatting.Indented);
            File.WriteAllText("tasks.json", json);
        }

        private void LoadTasks()
        {
            if (File.Exists("tasks.json"))
            {
                string json = File.ReadAllText("tasks.json");
                allTasks = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();

                lstTasks.Items.Clear();
                foreach (var task in allTasks)
                {
                    lstTasks.Items.Add(task);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string query = txtSearch.Text.ToLower();
            lstTasks.Items.Clear();

            foreach (var task in allTasks)
            {
                if (task.ToLower().Contains(query))
                {
                    lstTasks.Items.Add(task);
                }
            }
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            lstTasks.Items.Clear();

            foreach (var task in allTasks)
            {
                lstTasks.Items.Add(task);
            }
        }
    }
}

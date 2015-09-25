using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExcelToJsonGenerator;

namespace Xamasoft.JsonClassGenerator.UI
{
    public partial class GetFile : Form
    {
        private frmCSharpClassGeneration formMain;
        public GetFile(frmCSharpClassGeneration main)
        {
            formMain = main;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Entrance.Instance.Init(FilePath.Text.Trim(), index.Text);
            string json = Entrance.Instance.GetJsonString();

            if (string.IsNullOrEmpty(json))
            {
                MessageBox.Show("error , the file is empty or the file formate erorr , please call Fez at RTX  ...");
            }

            formMain.edtJson.Text = Entrance.Instance.GetSchema();
            formMain.edtMainClass.Text = index.Text;
            //formMain.edtJson.Text = Entrance.Instance.GetJsonString();

            System.IO.File.WriteAllText(Path.Combine(edtTargetFolder.Text,string.Format("{0}Config.json",index.Text)), Entrance.Instance.GetJsonString(), Encoding.UTF8);

            MessageBox.Show("Success~");
            this.Close();
        }

        private void btnBrowse_Click_1(object sender, EventArgs e)
        {
            using (var b = new FolderBrowserDialog())
            {
                b.ShowNewFolderButton = true;
                b.SelectedPath = edtTargetFolder.Text;
                b.Description = "Please select a folder where to save the generated files.";
                if (b.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    edtTargetFolder.Text = b.SelectedPath;
                    formMain.edtTargetFolder.Text = b.SelectedPath;
                }

            }
        }


        private void SelectFile_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            using (var b = new OpenFileDialog())
            {
                //while(true)
                //{
                //    if ((path.EndsWith("xls") || path.EndsWith("XLS")))
                //    {
                //        return;
                //    }

                //    b.ShowDialog();
                //    b.AddExtension = true;

                //    if (b.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                //    {
                //        path = b.FileName;
                //        FilePath.Text = b.FileName;
                //    }
                //}


                b.ShowDialog();
                b.AddExtension = true;

                if (b.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    path = b.FileName;
                    FilePath.Text = b.FileName;
                }
            }

        }



    }
}

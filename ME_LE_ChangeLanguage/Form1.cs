using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ME_LE_ChangeLanguage
{
    public partial class Form1 : Form
    {
        BackgroundWorker worker = new BackgroundWorker();

        public Form1()
        {
            InitializeComponent();

            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;

            worker.DoWork += Worker_DoWork;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            string message = "Wymagane jest stworzenie kopii zapasowej plików językowych." + "\nCzy chcesz to zrobić?\n\n" +
                "Upewnij się, że masz wystarczającą ilość miejsca na dysku.\nME1: 21,7 GB"; //\nME2: 7,95 GB
            string title = "Uwaga!";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                button1.BeginInvoke(new Action(() => { button1.Enabled = false; }));
                button2.BeginInvoke(new Action(() => { button2.Enabled = false; }));
                button3.BeginInvoke(new Action(() => { button3.Enabled = false; }));
                groupBox1.BeginInvoke(new Action(() => { groupBox1.Enabled = false; }));
                metroProgressSpinner1.BeginInvoke(new Action(() => { metroProgressSpinner1.Visible = true; }));
                label1.BeginInvoke(new Action(() => { label1.Visible = true; }));

                string lang_folder_me1 = textBox1.Text + @"\Game\ME1\BioGame\Content\Packages\ISACT";
                string lang_folder_me1_pcc = textBox1.Text + @"\Game\ME1\BioGame\CookedPCConsole";
                string lang_folder_me2 = "";

                if (radioButton1.Checked == true)
                {
                    //tutaj stwórz folder z kopiami
                    Directory.CreateDirectory(lang_folder_me1 + @"\kopia_lang");
                    Directory.CreateDirectory(lang_folder_me1_pcc + @"\kopia_lang");


                    //tutaj stwórz kopie
                    string sourcePath = lang_folder_me1;
                    string sourcePath_pcc = lang_folder_me1_pcc;
                    string targetPath = lang_folder_me1 + @"\kopia_lang";
                    string targetPath_pcc = lang_folder_me1_pcc + @"\kopia_lang";

                    foreach (var sourceFilePath in Directory.GetFiles(sourcePath))
                    {
                        string fileName = Path.GetFileName(sourceFilePath);
                        string destinationFilePath = Path.Combine(targetPath, fileName);

                        File.Copy(sourceFilePath, destinationFilePath, true);
                    }
                    foreach (var sourceFilePath_pcc in Directory.GetFiles(sourcePath_pcc))
                    {
                        string fileName = Path.GetFileName(sourceFilePath_pcc);
                        string destinationFilePath = Path.Combine(targetPath_pcc, fileName);

                        File.Copy(sourceFilePath_pcc, destinationFilePath, true);
                    }



                    //tutaj zmien jezyk
                    var extenstions = new List<string> { "isb" };
                    var extenstions_pcc = new List<string> { "pcc" };
                    var files = Directory.EnumerateFiles(lang_folder_me1, "*.*", SearchOption.TopDirectoryOnly).Where(x => extenstions.Contains(Path.GetExtension(x).TrimStart('.').ToLowerInvariant()));
                    var files_pcc = Directory.EnumerateFiles(lang_folder_me1_pcc, "*.*", SearchOption.TopDirectoryOnly).Where(x => extenstions_pcc.Contains(Path.GetExtension(x).TrimStart('.').ToLowerInvariant()));

                    foreach (var file in files)
                    {
                        string file2 = Path.GetFileNameWithoutExtension(file);

                        if (file.Contains("_plpc"))
                        {
                            //string file2_substr = file2.Substring(0, file2.Length - 5);
                            //string filter1_file = lang_folder_me1 + @"\" + file2_substr + ".isb";

                            //if (File.Exists(filter1_file))
                            //{
                            //    string f = Path.GetFileNameWithoutExtension(filter1_file);

                            //    File.Move(filter1_file, filter1_file.Replace(f, f + "+"));
                            //    File.Move(file, file.Replace("_plpc", ""));

                            //    string filter2_file = lang_folder_me1 + @"\" + file2_substr + "+" + ".isb";
                            //    File.Move(filter2_file, filter2_file.Replace("+", "_plpc"));
                            //}

                            File.Move(file, file.Replace("_plpc", "_plkopia"));
                        }
                    }

                    foreach (var file in files_pcc)
                    {
                        string file2 = Path.GetFileNameWithoutExtension(file);

                        if (file.Contains("_PLPC"))
                        {
                            string file2_substr = file2.Substring(0, file2.Length - 5);
                            string filter1_file = lang_folder_me1_pcc + @"\" + file2_substr + ".pcc";

                            if (File.Exists(filter1_file))
                            {
                                string f = Path.GetFileNameWithoutExtension(filter1_file);

                                File.Move(filter1_file, filter1_file.Replace(f, f + "+"));
                                File.Move(file, file.Replace("_PLPC", ""));

                                string filter2_file = lang_folder_me1_pcc + @"\" + file2_substr + "+" + ".pcc";
                                File.Move(filter2_file, filter2_file.Replace("+", "_PLPC"));
                            }
                        }

                        if (file.Contains("_LOC_PLPC"))
                        {
                            string file2_substr = file2.Substring(0, file2.Length - 5);
                            string filter1_file = lang_folder_me1_pcc + @"\" + file2_substr + "_INT" + ".pcc";

                            if (File.Exists(filter1_file))
                            {
                                string f = Path.GetFileNameWithoutExtension(filter1_file);

                                File.Move(filter1_file, filter1_file.Replace(f, f + "+"));
                                File.Move(file, file.Replace("_PLPC", "_INT"));

                                string filter2_file = lang_folder_me1_pcc + @"\" + file2_substr + "_INT+" + ".pcc";
                                File.Move(filter2_file, filter2_file.Replace("_INT+", "_PLPC"));
                            }
                        }
                    }

                    MessageBox.Show("Kopia zapasowa została stworzona. Język zmieniony.");

                    button2.BeginInvoke(new Action(() => { button2.Enabled = true; }));
                    button3.BeginInvoke(new Action(() => { button3.Enabled = true; }));
                    groupBox1.BeginInvoke(new Action(() => { groupBox1.Enabled = true; }));
                    metroProgressSpinner1.BeginInvoke(new Action(() => { metroProgressSpinner1.Visible = false; }));
                    label1.BeginInvoke(new Action(() => { label1.Visible = false; }));
                }

                //if (radioButton2.Checked == true)
                //{

                //}

                button1.BeginInvoke(new Action(() => { button1.Enabled = true; }));
            }

            if (result == DialogResult.No)
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ustaw ścieżkę do folderu\n'Mass Effect Legendary Edition', nie do jego podfolderów!");
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (folderBrowserDialog1.SelectedPath.Contains("Mass Effect Legendary Edition"))
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                groupBox1.Enabled = true;
                textBox1.Enabled = true;
            }
            else
            {
                MessageBox.Show("Podana ścieżka nie zawiera nazwy gry. Spróbuj jeszcze raz.");
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                button3.Enabled = true;

                if (Directory.Exists(textBox1.Text + @"\Game\ME1\BioGame\Content\Packages\ISACT\kopia_lang"))
                {
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //if (radioButton2.Checked == true)
            //{
            //    button3.Enabled = true;

            //    //
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            worker.RunWorkerAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Process.Start(textBox1.Text + @"\Game\ME1\BioGame\Content\Packages\ISACT\kopia_lang");
                Process.Start(textBox1.Text + @"\Game\ME1\BioGame\CookedPCConsole\kopia_lang");
            }

            //if (radioButton2.Checked == true)
            //{
            //    //
            //}
        }

    }
}

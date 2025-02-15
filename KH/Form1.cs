using System;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace KH
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ketasTxtBox_TextChanged(object sender, EventArgs e)
        {
            ketasTxtBox.ScrollBars = ScrollBars.Vertical;
        }

        private void segmentSaveBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Text Files | *.txt",
                DefaultExt = "txt"
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                {
                    sw.Write(segmentTxtBox.Text);
                }
            }
        }

        private void ketasProgram_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            ketasTxtBox.Clear();
            ketasTxtBox.Text += "Toote kood: \r\nKiu pikkus: \r\nKaldenurk: \r\n";

            try
            {
                int kettaLabimoot = int.Parse(AtxtBox.Text);
                int auguridadeArv = int.Parse(BtxtBox.Text);
                int kpArv = int.Parse(CtxtBox.Text);
                int kpDiam = int.Parse(DtxtBox.Text);
                double aVahe = double.Parse(ItxtBox.Text);
                int vRida = int.Parse(JtxtBox.Text);
                int rVahe = int.Parse(KtxtBox.Text);
                int kaugusKP = int.Parse(LtxtBox.Text);
                int poltidestV = int.Parse(MtxtBox.Text);
                int poltidestS = int.Parse(NtxtBox.Text);

                int set = 0;
                int diam = kettaLabimoot - vRida - (rVahe * (auguridadeArv - 1));
                double ak = 0;

                if (diam > kpDiam + poltidestS || diam < kpDiam - poltidestV)
                {
                    pptr(ak, kettaLabimoot, auguridadeArv, kpArv, kpDiam, aVahe, vRida, rVahe, kaugusKP, poltidestV, poltidestS, set, diam);
                }
                else
                {
                    ppsk(ak, kettaLabimoot, auguridadeArv, kpArv, kpDiam, aVahe, vRida, rVahe, kaugusKP, poltidestV, poltidestS, set, diam);
                }
            }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(AtxtBox.Text) || string.IsNullOrWhiteSpace(BtxtBox.Text))
            {
                MessageBox.Show("Täitke vähemalt A: ja B: väljad!");
                return false;
            }
            if ((string.IsNullOrWhiteSpace(CtxtBox.Text) ^ string.IsNullOrWhiteSpace(DtxtBox.Text)))
            {
                MessageBox.Show("C: ja D: välja saab vaid üheaegselt kasutada!");
                return false;
            }
            string[] requiredFields = { ItxtBox.Text, JtxtBox.Text, KtxtBox.Text, LtxtBox.Text, MtxtBox.Text, NtxtBox.Text };
            string[] fieldNames = { "I", "J", "K", "L", "M", "N" };
            for (int i = 0; i < requiredFields.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(requiredFields[i]))
                {
                    MessageBox.Show($"Tekstiväli {fieldNames[i]} on tühi!");
                    return false;
                }
            }
            if (NtxtBox.Text == "1970")
            {
                MessageBox.Show("Houston, we've had a problem here.", "Apollo 13");
                return false;
            }
            return true;
        }

        private void pptr(double ak, int kettaLabimoot, int auguridadeArv, int kpArv, int kpDiam, double aVahe, int vRida, int rVahe, int kaugusKP, int poltidestV, int poltidestS, int set, int diam)
        {
            double dnoh = Math.PI * diam / aVahe;
            int noh = Convert.ToInt32(dnoh);
            double angl = 360.0 / noh;

            ketasTxtBox.Text += $"\r\nSet {set}\r\nDiameter {diam}\r\nStart angle 0\r\nAngle betw.holes {angl:0.0}\r\nNo. of holes {noh}\r\n";
        }

        private void ppsk(double ak, int kettaLabimoot, int auguridadeArv, int kpArv, int kpDiam, double aVahe, int vRida, int rVahe, int kaugusKP, int poltidestV, int poltidestS, int set, int diam)
        {
            double sekNurk = 360.0 / kpArv;
            double ringiPikkus = Math.PI * diam;
            double startd = 360.0 / (ringiPikkus / kaugusKP);

            double soh = (((Math.PI * diam) / (360.0 / sekNurk)) - (2 * kaugusKP)) / aVahe;
            int noh = Convert.ToInt16(soh) + 1;
            double angl = (sekNurk - (360.0 / ((Math.PI * diam) / (2 * kaugusKP)))) / (noh - 1);

            for (int i = 0; i < kpArv; i++)
            {
                ketasTxtBox.Text += $"\r\nSet {set}\r\nDiameter {diam}\r\nStart angle {startd:0.00}\r\nAngle betw.holes {angl:0.00}\r\nNo. of holes {noh}\r\n";
                set++;
                startd += sekNurk;
                ak += noh;
            }
        }
    }
}

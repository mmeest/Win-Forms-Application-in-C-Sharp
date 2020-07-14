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
using Microsoft.SqlServer;
using System.Configuration;
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
        // lisab ketta programmi tekstiväljale scrollbari
        ketasTxtBox.ScrollBars = ScrollBars.Vertical;
    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
    {

    }

    private void textBox4_TextChanged(object sender, EventArgs e)
    {

    }

    private void textBox6_TextChanged(object sender, EventArgs e)
    {

    }

    private void segmentSaveBtn_Click(object sender, EventArgs e)
    {
        // segmendi programmi salvestamine tekstifaili
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        // faili formaadi filtrid
        saveFileDialog1.Filter = "Text Files | *.txt";
        saveFileDialog1.DefaultExt = "txt";
        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        {
            // luuakse uus tekstifail
            using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    // salvestatav tekst võetakse segmendi väljalt
                    sw.Write(segmentTxtBox.Text);
                }
        }
    }

    private void ketasProgram_Click(object sender, EventArgs e)
    {
        // muutujate deklareerimine
        int kpArv = 0, kpDiam = 0, vRida = 0, rVahe = 0, kaugusKP = 0, poltidestV = 0, poltidestS = 0, start = 0, noh = 0;              // muutujate deklareerimine
        double dnoh = 0, angl = 0, aVahe = 0;

        ketasTxtBox.Clear();                // puhastab tekstivälja
        ketasTxtBox.Text += "Toote kood: \r\nKiu pikkus: \r\nKaldenurk: \r\n";  // lisainfo

        // kontrollib kas kettaprogrammi jaoks on nõutud muutujad sisestatud
        // kui ei, siis väljastab vastava teateakna
        if (string.IsNullOrWhiteSpace(AtxtBox.Text) || string.IsNullOrWhiteSpace(BtxtBox.Text))
        {
            MessageBox.Show("Täitke vähemalt A: ja B: väljad!");
            return;
        }
        else if (string.IsNullOrWhiteSpace(CtxtBox.Text) && !string.IsNullOrWhiteSpace(DtxtBox.Text) || !string.IsNullOrWhiteSpace(CtxtBox.Text) && string.IsNullOrWhiteSpace(DtxtBox.Text))
        {
            MessageBox.Show("C: ja D: välja saab vaid üheaegselt kasutada!");
            return;
        }
        else if (string.IsNullOrWhiteSpace(ItxtBox.Text))
        {
             MessageBox.Show("Tekstiväli I on tühi!");
             return;
        }
        else if (string.IsNullOrWhiteSpace(JtxtBox.Text))
        {
              MessageBox.Show("Tekstiväli J on tühi!");
              return;
        }
        else if (string.IsNullOrWhiteSpace(KtxtBox.Text))
            {
                MessageBox.Show("Tekstiväli K on tühi!");
                return;
            }
        else if (string.IsNullOrWhiteSpace(LtxtBox.Text))
            {
                MessageBox.Show("Tekstiväli L on tühi!");
                return;
            }
        else if (string.IsNullOrWhiteSpace(MtxtBox.Text))
            {
                MessageBox.Show("Tekstiväli M on tühi!");
                return;
            }
        else if (string.IsNullOrWhiteSpace(NtxtBox.Text))
            {
                MessageBox.Show("Tekstiväli N on tühi!");
                return;
            }
        else if (NtxtBox.Text == ("1970"))            // easter egg
        {
            MessageBox.Show("Houston, we've had a problem here.", "Apollo 13");
            return;
        }
        else
        {

            /*
            // kui kinnituspoltide väljad(C: ja D:) on tühjad, siis omistatakse neile 0 väärtus
                        if (string.IsNullOrWhiteSpace(CtxtBox.Text))
                            kpArv = 0;
                        if (string.IsNullOrWhiteSpace(CtxtBox.Text))
                            kpDiam = 0; */

            try
            {
                // põhimuutujate väärtuste hankimine tekstiväljadelt

                kpArv = 0;
                int.TryParse(CtxtBox.Text, out kpArv);
                kpDiam = 0;
                int.TryParse(DtxtBox.Text, out kpDiam);


                int kettaLabimoot = int.Parse(AtxtBox.Text);
                int auguridadeArv = int.Parse(BtxtBox.Text);
                kpArv = int.Parse(CtxtBox.Text);
                kpDiam = int.Parse(DtxtBox.Text);

                // valikuliste muutujate väärtuste hankimine

                aVahe = int.Parse(ItxtBox.Text);
                vRida = int.Parse(JtxtBox.Text);
                rVahe = int.Parse(KtxtBox.Text);
                kaugusKP = int.Parse(LtxtBox.Text);
                poltidestV = int.Parse(MtxtBox.Text);
                poltidestS = int.Parse(NtxtBox.Text);

                int set = 0;                                                                // esimese set'i number
                int diam = kettaLabimoot - vRida - (rVahe * (auguridadeArv - 1));           // esimese set'i rea läbimõõt
                int ak = 0;                                                                 // kõik augupaarid kokku

                if (diam > kpDiam + poltidestS || diam < kpDiam - poltidestV)               // päripäeva täisring
                {
                    // liigutakse edasi päripäev täisringi
                    pptr(ketasTxtBox, ak, kettaLabimoot, auguridadeArv, kpArv, kpDiam, aVahe, vRida, rVahe,
                         kaugusKP, poltidestV, poltidestS, set, diam, start, dnoh, noh, angl);
                }
                else                                                                        // päripäeva sektor
                {
                    // liigutakse edasi päripäeva sektorisse
                    ppsk(ketasTxtBox, ak, kettaLabimoot, auguridadeArv, kpArv, kpDiam, aVahe, vRida, rVahe,
                         kaugusKP, poltidestV, poltidestS, set, diam, start, dnoh, noh, angl);
                }
            }

            catch
            {
                MessageBox.Show("ERROR");
            }
        }
    }

    private static void pptr(TextBox ketasTxtBox, double ak, int kettaLabimoot, int auguridadeArv, int kpArv, int kpDiam, double aVahe, int vRida, int rVahe, int kaugusKP, int poltidestV, int poltidestS, int set, int diam, int start, double dnoh, int noh, double angl)    // ketta päripäeva täisring
    {
        // arvutuskäik
        start = 0;
        dnoh = Math.PI * diam / aVahe;
        noh = Convert.ToInt32(dnoh);
        double soh = Convert.ToDouble(noh);
        angl = 360 / soh;

        // seti väärtuste väljastamine
        ketasTxtBox.Text += string.Format(
                                "\r\nSet " + " {0}" +
                                "\r\nDiameter " + " {1}" +
                                "\r\nStart angle " + " {2:0.00}" +
                                "\r\nAngle betw.holes " + " {3:0.0}" +
                                "\r\nNo. of holes " + " {4}\r\n",
                                set, diam, start, angl, noh);

        set = set + 1;      // seti muutujale lisatakse 1
        ak = ak + noh;      // muutuja millega loetakse kõik augupaarid kokku
        if (diam + vRida == kettaLabimoot)
        {
            // kui enam edasi ei liiguta siis väljastatakse agupaaride arv
            ketasTxtBox.Text += "\r\nAugupaare kokku: " + ak + "tk\r\n\r\nMartin_2018";
        }
        else if (diam + rVahe > kpDiam + poltidestS || diam + rVahe < kpDiam - poltidestV)
        {
            // liigutakse edasi vastupäeva täisringi
            diam = diam + rVahe;
            vptr(ketasTxtBox, ak, kettaLabimoot, auguridadeArv, kpArv, kpDiam, aVahe, vRida, rVahe,
                 kaugusKP, poltidestV, poltidestS, set, diam, start, dnoh, noh, angl);
        }
        else
        {
            // liigutakse edasi vastupäeva sektorisse
            diam = diam + rVahe;
            vpsk(ketasTxtBox, ak, kettaLabimoot, auguridadeArv, kpArv, kpDiam, aVahe, vRida, rVahe,
                 kaugusKP, poltidestV, poltidestS, set, diam, start, dnoh, noh, angl);
        }
    }

    private static void ppsk(TextBox ketasTxtBox, double ak, int kettaLabimoot, int auguridadeArv, int kpArv, int kpDiam, double aVahe, int vRida, int rVahe, int kaugusKP, int poltidestV, int poltidestS, int set, int diam, int start, double dnoh, int noh, double angl)    // ketta päripäeva sektor
    {
        // arvutuskäik
        double sekNurk = 360 / kpArv;
        double ringiPikkus = Math.PI * diam;
        double startd = 360 / (ringiPikkus / kaugusKP);

        double soh = (((Math.PI * diam) / (360 / sekNurk)) - (2 * kaugusKP)) / aVahe;
        noh = Convert.ToInt16(soh) + 1;
        dnoh = Convert.ToDouble(noh);
        angl = (sekNurk - (360 / ((Math.PI * diam) / (2 * kaugusKP)))) / (noh - 1);

        for (int i = 0; i < kpArv; i++)
        {
            // seti väärtuste väljastamine
            ketasTxtBox.Text += string.Format(
                                    "\r\nSet " + " {0}" +
                                    "\r\nDiameter " + " {1}" +
                                    "\r\nStart angle " + " {2:0.00}" +
                                    "\r\nAngle betw.holes " + " {3:0.00}" +
                                    "\r\nNo. of holes " + " {4}\r\n",
                                    set, diam, startd, angl, noh);

            set = set + 1;
            startd = startd + sekNurk;
            ak = ak + noh;
        }
        if (diam + vRida == kettaLabimoot)
        {
            ketasTxtBox.Text += "\r\nAugupaare kokku: " + ak + "tk\r\n\r\nMartin_2018";
        }
        else if (diam + rVahe > kpDiam + poltidestS || diam + rVahe < kpDiam - poltidestV)
        {
            diam = diam + rVahe;
            vptr(ketasTxtBox, ak, kettaLabimoot, auguridadeArv, kpArv, kpDiam, aVahe, vRida, rVahe,
                 kaugusKP, poltidestV, poltidestS, set, diam, start, dnoh, noh, angl);
        }
        else
        {
            diam = diam + rVahe;
            vpsk(ketasTxtBox, ak, kettaLabimoot, auguridadeArv, kpArv, kpDiam, aVahe, vRida, rVahe,
                 kaugusKP, poltidestV, poltidestS, set, diam, start, dnoh, noh, angl);
        }
    }

    private static void vptr(TextBox ketasTxtBox, double ak, int kettaLabimoot, int auguridadeArv, int kpArv, int kpDiam, double aVahe, int vRida, int rVahe, int kaugusKP, int poltidestV, int poltidestS, int set, int diam, int start, double dnoh, int noh, double angl)    // ketta vastupäeva täisring
    {
        start = 360;
        dnoh = Math.PI * diam / aVahe;
        noh = Convert.ToInt32(dnoh);
        double soh = Convert.ToDouble(noh);
        angl = 360 / soh;
        ketasTxtBox.Text += string.Format(
                                "\r\nSet " + " {0}" +
                                "\r\nDiameter " + " {1}" +
                                "\r\nStart angle " + " {2:0.00}" +
                                "\r\nAngle betw.holes " + " -{3:0.00}" +
                                "\r\nNo. of holes " + " {4}\r\n",
                                set, diam, start, angl, noh);

        set = set + 1;
        ak = ak + noh;

        if (diam + vRida == kettaLabimoot)
        {
            ketasTxtBox.Text += "\r\nAugupaare kokku: " + ak + "tk\r\n\r\nMartin_2018";
        }
        else if (diam + rVahe > kpDiam + poltidestS || diam + rVahe < kpDiam - poltidestV)
        {
            diam = diam + rVahe;
            pptr(ketasTxtBox, ak, kettaLabimoot, auguridadeArv, kpArv, kpDiam, aVahe, vRida, rVahe,
                 kaugusKP, poltidestV, poltidestS, set, diam, start, dnoh, noh, angl);
        }
        else
        {
            diam = diam + rVahe;
            ppsk(ketasTxtBox, ak, kettaLabimoot, auguridadeArv, kpArv, kpDiam, aVahe, vRida, rVahe,
                 kaugusKP, poltidestV, poltidestS, set, diam, start, dnoh, noh, angl);
        }
    }

    private static void vpsk(TextBox ketasTxtBox, double ak, int kettaLabimoot, int auguridadeArv, int kpArv, int kpDiam, double aVahe, int vRida, int rVahe, int kaugusKP, int poltidestV, int poltidestS, int set, int diam, int start, double dnoh, int noh, double angl)    // ketta vastupäeva sektor
    {

        double sekNurk = 360 / kpArv;
        double ringiPikkus = Math.PI * diam;
        double startd = 360 - (360 / (ringiPikkus / kaugusKP));

        double soh = (((Math.PI * diam) / (360 / sekNurk)) - (2 * kaugusKP)) / aVahe;
        noh = Convert.ToInt16(soh) + 1;
        dnoh = Convert.ToDouble(noh);
        angl = (sekNurk - (360 / ((Math.PI * diam) / (2 * kaugusKP)))) / (noh - 1);

        for (int i = 0; i < kpArv; i++)
        {
            ketasTxtBox.Text += string.Format(
                                    "\r\nSet " + " {0}" +
                                    "\r\nDiameter " + " {1}" +
                                    "\r\nStart angle " + " {2:0.00}" +
                                    "\r\nAngle betw.holes " + " -{3:0.00}" +
                                    "\r\nNo. of holes " + " {4}\r\n",
                                    set, diam, startd, angl, noh);

            startd = startd - sekNurk;
            set = set + 1;
            ak = ak + noh;
        }

        if (diam + vRida == kettaLabimoot)
        {
            ketasTxtBox.Text += "\r\nAugupaare kokku: " + ak + "tk\r\n\r\nMartin_2018";
        }
        else if (diam + rVahe > kpDiam + poltidestS || diam + rVahe < kpDiam - poltidestV)
        {
            diam = diam + rVahe;
            pptr(ketasTxtBox, ak, kettaLabimoot, auguridadeArv, kpArv, kpDiam, aVahe, vRida, rVahe,
                 kaugusKP, poltidestV, poltidestS, set, diam, start, dnoh, noh, angl);
        }
        else
        {
            diam = diam + rVahe;
            ppsk(ketasTxtBox, ak, kettaLabimoot, auguridadeArv, kpArv, kpDiam, aVahe, vRida, rVahe,
                 kaugusKP, poltidestV, poltidestS, set, diam, start, dnoh, noh, angl);
        }
    }

    private void segmentProgram_Click(object sender, EventArgs e)
    {
        // muutujate deklareerimine
        double start = 0, kettaLabimoot = 0, auguridadeArv = 0, aVahe = 0, vRida = 0, rVahe = 0,
               kaugusKP = 0, poltidestV = 0, poltidestS = 0, dnoh = 0, angl = 0;
        int noh = 0;

        segmentTxtBox.Clear();                // puhastab tekstivälja
        segmentTxtBox.Text += "Toote kood: \r\nKiu pikkus: \r\nKaldenurk: \r\n";  // lisainfo

        // kontrollib kas segmendiprogrammi jaoks on nõutud muutujad sisestatud
        // kui ei, siis väljastab vastava teateakna
        if (string.IsNullOrWhiteSpace(AtxtBox.Text) || string.IsNullOrWhiteSpace(BtxtBox.Text) || string.IsNullOrWhiteSpace(DtxtBox.Text) || string.IsNullOrWhiteSpace(EtxtBox.Text) || string.IsNullOrWhiteSpace(FtxtBox.Text) || string.IsNullOrWhiteSpace(GtxtBox.Text) || string.IsNullOrWhiteSpace(HtxtBox.Text))
        {
            MessageBox.Show("Täitke vähemalt A:, B:, D:, E:, F:, G:, ja H: väljad!");
            return;
        }
        else if (NtxtBox.Text == ("1970"))            // easter egg
        {
            MessageBox.Show("This is Houston. Say again please.", "Houston");
            return;
        }
        else
        {
            try
            {
                // põhimuutujate väärtuste hankimine tekstiväljadelt

                double segmendiLabimoot = double.Parse(AtxtBox.Text);
                double segmentRead = double.Parse(BtxtBox.Text);
                double segmendiAlgus = double.Parse(EtxtBox.Text);
                double segmendiLopp = double.Parse(FtxtBox.Text);
                double esimenePolt = double.Parse(GtxtBox.Text);
                double teinePolt = double.Parse(HtxtBox.Text);
                double kpDiam = double.Parse(DtxtBox.Text);

                // valikuliste muutujate hankimine tekstiväljadelt

                aVahe = int.Parse(ItxtBox.Text);
                vRida = int.Parse(JtxtBox.Text);
                rVahe = int.Parse(KtxtBox.Text);
                kaugusKP = int.Parse(LtxtBox.Text);
                poltidestV = int.Parse(MtxtBox.Text);
                poltidestS = int.Parse(NtxtBox.Text);

                int set = 0;   // seti number (esimene on 0)
                double diam = segmendiLabimoot - vRida - (rVahe * (segmentRead - 1));       // esimese set'i rea läbimõõt
                double ak = 0;                                                                 // kõik augupaarid kokku

                if (diam <= kpDiam + poltidestS && aVahe < ((Math.PI * diam) / 360) * (esimenePolt - segmendiAlgus) - (2 * kaugusKP))
                {
                    // liigutakse edasi segment esimene polt päripäeva
                    sgppep(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                           kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
                }
                else if (diam <= kpDiam + poltidestS)
                {
                    // liigutakse edasi segment päripäeva poltide vahe
                    sgpppv(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                           kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
                }
                else                                                                                        // päripäeva täiskaar
                {
                    // liigutakse edasi segment päripäeva
                    sgpp(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                         kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
                }

            }

            catch
            {
                MessageBox.Show("ERROR");
            }
        }
    }

    private static void sgpp(TextBox segmentTxtBox, double ak, double start, double esimenePolt, double teinePolt, double segmendiLabimoot, double segmendiAlgus, double segmendiLopp, double kettaLabimoot, double auguridadeArv, double kpDiam, double aVahe, double vRida, double rVahe, double kaugusKP, double poltidestV, double poltidestS, int set, double diam, double dnoh, double noh, double angl) // segment päripäeva
    {
        // arvutuskäik
        double ringiPikkus = Math.PI * diam;
        double startd = 360 / (ringiPikkus / kaugusKP);
        start = segmendiAlgus + startd;

        double segmendiVahemik = segmendiLopp - segmendiAlgus;
        double soh = (((Math.PI * diam) / (360 / segmendiVahemik)) - (2 * kaugusKP)) / aVahe;
        noh = Convert.ToInt16(soh) + 1;
        dnoh = Convert.ToDouble(noh);
        angl = (segmendiVahemik - (360 / ((Math.PI * diam) / (2 * kaugusKP)))) / (noh - 1);
        // seti väärtuste väljastamine
        segmentTxtBox.Text += string.Format(
                                  "\r\nSet " + " {0}" +
                                  "\r\nDiameter " + " {1}" +
                                  "\r\nStart angle " + " {2:0.00}" +
                                  "\r\nAngle betw.holes " + " {3:0.00}" +
                                  "\r\nNo. of holes " + " {4}\r\n",
                                  set, diam, start, angl, noh);

        set = set + 1;      // seti number suureneb ühe väärtuse võrra
        ak = ak + noh;      // muutuja kõigi augupaaride kokkulugemiseks

        if (diam + vRida < segmendiLabimoot)
        {
            diam = diam + rVahe;
            sgvp(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                 kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
        }
        else
        {
            segmentTxtBox.Text += "\r\nAugupaare kokku: " + ak + "tk\r\n\r\nMartin_2018";
        }
    }

    private static void sgppep(TextBox segmentTxtBox, double ak, double start, double esimenePolt, double teinePolt, double segmendiLabimoot, double segmendiAlgus, double segmendiLopp, double kettaLabimoot, double auguridadeArv, double kpDiam, double aVahe, double vRida, double rVahe, double kaugusKP, double poltidestV, double poltidestS, int set, double diam, double dnoh, double noh, double angl) // segment päripäeva (enne esimest polti)
    {
        double ringiPikkus = Math.PI * diam;
        double startd = 360 / (ringiPikkus / kaugusKP);
        start = segmendiAlgus + startd;

        double segmendiVahemik = esimenePolt - segmendiAlgus;
        double soh = (((Math.PI * diam) / (360 / segmendiVahemik)) - (2 * kaugusKP)) / aVahe;
        noh = Convert.ToInt16(soh) + 1;
        dnoh = Convert.ToDouble(noh);
        //angl = segmendiVahemik / (dnoh - 1);
        angl = (segmendiVahemik - (360 / ((Math.PI * diam) / (2 * kaugusKP)))) / (noh - 1);


        segmentTxtBox.Text += string.Format(
                                  "\r\nSet " + " {0}" +
                                  "\r\nDiameter " + " {1}" +
                                  "\r\nStart angle " + " {2:0.00}" +
                                  "\r\nAngle betw.holes " + " {3:0.00}" +
                                  "\r\nNo. of holes " + " {4}\r\n",
                                  set, diam, start, angl, noh);

        set = set + 1;
        ak = ak + noh;

        sgpppv(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
               kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);

    }

    private static void sgpppv(TextBox segmentTxtBox, double ak, double start, double esimenePolt, double teinePolt, double segmendiLabimoot, double segmendiAlgus, double segmendiLopp, double kettaLabimoot, double auguridadeArv, double kpDiam, double aVahe, double vRida, double rVahe, double kaugusKP, double poltidestV, double poltidestS, int set, double diam, double dnoh, double noh, double angl) // segment päripäeva (poltide vahe)
    {
        double ringiPikkus = Math.PI * diam;
        double startd = 360 / (ringiPikkus / kaugusKP);
        start = esimenePolt + startd;

        double segmendiVahemik = teinePolt - esimenePolt;
        double soh = (((Math.PI * diam) / (360 / segmendiVahemik)) - (2 * kaugusKP)) / aVahe;
        noh = Convert.ToInt16(soh) + 1;
        dnoh = Convert.ToDouble(noh);
        angl = (segmendiVahemik - (360 / ((Math.PI * diam) / (2 * kaugusKP)))) / (noh - 1);

        segmentTxtBox.Text += string.Format(
                                  "\r\nSet " + " {0}" +
                                  "\r\nDiameter " + " {1}" +
                                  "\r\nStart angle " + " {2:0.00}" +
                                  "\r\nAngle betw.holes " + " {3:0.00}" +
                                  "\r\nNo. of holes " + " {4}\r\n",
                                  set, diam, start, angl, noh);

        set = set + 1;
        ak = ak + noh;

        if (aVahe < ((Math.PI * diam) / 360) * (segmendiLopp - teinePolt) - (2 * kaugusKP))
        {
            sgpptp(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                   kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
        }
        else if (diam + rVahe < segmendiLabimoot && diam + rVahe <= kpDiam + poltidestS && diam <= kpDiam + poltidestS && aVahe < ((Math.PI * (diam + rVahe)) / 360) * (segmendiLopp - teinePolt) - (2 * kaugusKP))
        {
            diam = diam + rVahe;
            sgvptp(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                   kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
        }
        else if (diam + rVahe < segmendiLabimoot && diam + rVahe <= kpDiam + poltidestS)
        {
            diam = diam + rVahe;
            sgvppv(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                   kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
        }
        else if (diam + rVahe < segmendiLabimoot && diam + rVahe > kpDiam + poltidestS)
        {
            diam = diam + rVahe;
            sgvp(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                 kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
        }
        else
        {
            segmentTxtBox.Text += "\r\nAugupaare kokku: " + ak + "tk\r\n\r\nMartin_2018";
        }
    }

    private static void sgpptp(TextBox segmentTxtBox, double ak, double start, double esimenePolt, double teinePolt, double segmendiLabimoot, double segmendiAlgus, double segmendiLopp, double kettaLabimoot, double auguridadeArv, double kpDiam, double aVahe, double vRida, double rVahe, double kaugusKP, double poltidestV, double poltidestS, int set, double diam, double dnoh, double noh, double angl) // segment päripäeva (pärast teist polti)
    {
        double ringiPikkus = Math.PI * diam;
        double startd = 360 / (ringiPikkus / kaugusKP);
        start = teinePolt + startd;

        double segmendiVahemik = segmendiLopp - teinePolt;
        double soh = (((Math.PI * diam) / (360 / segmendiVahemik)) - (2 * kaugusKP)) / aVahe;
        noh = Convert.ToInt16(soh) + 1;
        dnoh = Convert.ToDouble(noh);
        angl = (segmendiVahemik - (360 / ((Math.PI * diam) / (2 * kaugusKP)))) / (noh - 1);

        segmentTxtBox.Text += string.Format(
                                  "\r\nSet " + " {0}" +
                                  "\r\nDiameter " + " {1}" +
                                  "\r\nStart angle " + " {2:0.00}" +
                                  "\r\nAngle betw.holes " + " {3:0.00}" +
                                  "\r\nNo. of holes " + " {4}\r\n",
                                  set, diam, start, angl, noh);

        set = set + 1;
        ak = ak + noh;

        if (diam + rVahe < segmendiLabimoot && diam + rVahe <= kpDiam + poltidestS && diam <= kpDiam + poltidestS && aVahe < ((Math.PI * (diam + rVahe)) / 360) * (segmendiLopp - teinePolt) - (2 * kaugusKP))
        {
            diam = diam + rVahe;
            sgvptp(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                   kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
        }
        else if (diam + rVahe < segmendiLabimoot && diam + rVahe <= kpDiam + poltidestS)
        {
            diam = diam + rVahe;
            sgvppv(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                   kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
        }
        else if (diam + rVahe < segmendiLabimoot && diam + rVahe > kpDiam + poltidestS)
        {
            diam = diam + rVahe;
            sgvp(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                 kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
        }
        else
        {
            segmentTxtBox.Text += "\r\nAugupaare kokku: " + ak + "tk\r\n\r\nMartin_2018";
        }
    }

    private static void sgvp(TextBox segmentTxtBox, double ak, double start, double esimenePolt, double teinePolt, double segmendiLabimoot, double segmendiAlgus, double segmendiLopp, double kettaLabimoot, double auguridadeArv, double kpDiam, double aVahe, double vRida, double rVahe, double kaugusKP, double poltidestV, double poltidestS, int set, double diam, double dnoh, double noh, double angl) // segment vastupäeva
    {
        double ringiPikkus = Math.PI * diam;
        double startd = 360 / (ringiPikkus / kaugusKP);
        start = segmendiLopp - startd;

        double segmendiVahemik = segmendiLopp - segmendiAlgus;
        double soh = (((Math.PI * diam) / (360 / segmendiVahemik)) - (2 * kaugusKP)) / aVahe;
        noh = Convert.ToInt16(soh) + 1;
        dnoh = Convert.ToDouble(noh);
        angl = (segmendiVahemik - (360 / ((Math.PI * diam) / (2 * kaugusKP)))) / (noh - 1);

        segmentTxtBox.Text += string.Format(
                                  "\r\nSet " + " {0}" +
                                  "\r\nDiameter " + " {1}" +
                                  "\r\nStart angle " + " {2:0.00}" +
                                  "\r\nAngle betw.holes " + " -{3:0.00}" +
                                  "\r\nNo. of holes " + " {4}\r\n",
                                  set, diam, start, angl, noh);

        set = set + 1;
        ak = ak + noh;
        if (diam + vRida < segmendiLabimoot)
        {
            diam = diam + rVahe;
            sgpp(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                 kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
        }
        else
        {
            segmentTxtBox.Text += "\r\nAugupaare kokku: " + ak + "tk\r\n\r\nMartin_2018";
        }
    }

    private static void sgvptp(TextBox segmentTxtBox, double ak, double start, double esimenePolt, double teinePolt, double segmendiLabimoot, double segmendiAlgus, double segmendiLopp, double kettaLabimoot, double auguridadeArv, double kpDiam, double aVahe, double vRida, double rVahe, double kaugusKP, double poltidestV, double poltidestS, int set, double diam, double dnoh, double noh, double angl) // segment vastupäeva (enne teist polti)
    {
        double ringiPikkus = Math.PI * diam;
        double startd = 360 / (ringiPikkus / kaugusKP);
        start = segmendiLopp - startd;

        double segmendiVahemik = segmendiLopp - teinePolt;
        double soh = (((Math.PI * diam) / (360 / segmendiVahemik)) - (2 * kaugusKP)) / aVahe;
        noh = Convert.ToInt16(soh) + 1;
        dnoh = Convert.ToDouble(noh);
        angl = (segmendiVahemik - (360 / ((Math.PI * diam) / (2 * kaugusKP)))) / (noh - 1);

        segmentTxtBox.Text += string.Format(
                                  "\r\nSet " + " {0}" +
                                  "\r\nDiameter " + " {1}" +
                                  "\r\nStart angle " + " {2:0.00}" +
                                  "\r\nAngle betw.holes " + " -{3:0.00}" +
                                  "\r\nNo. of holes " + " {4}\r\n",
                                  set, diam, start, angl, noh);

        set = set + 1;
        ak = ak + noh;

        sgvppv(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
               kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);

    }

    private static void sgvppv(TextBox segmentTxtBox, double ak, double start, double esimenePolt, double teinePolt, double segmendiLabimoot, double segmendiAlgus, double segmendiLopp, double kettaLabimoot, double auguridadeArv, double kpDiam, double aVahe, double vRida, double rVahe, double kaugusKP, double poltidestV, double poltidestS, int set, double diam, double dnoh, double noh, double angl) // segment vastupäeva (poltide vahe)
    {
        double ringiPikkus = Math.PI * diam;
        double startd = 360 / (ringiPikkus / kaugusKP);
        start = teinePolt - startd;

        double segmendiVahemik = teinePolt - esimenePolt;
        double soh = (((Math.PI * diam) / (360 / segmendiVahemik)) - (2 * kaugusKP)) / aVahe;
        noh = Convert.ToInt16(soh) + 1;
        dnoh = Convert.ToDouble(noh);
        angl = (segmendiVahemik - (360 / ((Math.PI * diam) / (2 * kaugusKP)))) / (noh - 1);

        segmentTxtBox.Text += string.Format(
                                  "\r\nSet " + " {0}" +
                                  "\r\nDiameter " + " {1}" +
                                  "\r\nStart angle " + " {2:0.00}" +
                                  "\r\nAngle betw.holes " + " -{3:0.00}" +
                                  "\r\nNo. of holes " + " {4}\r\n",
                                  set, diam, start, angl, noh);

        set = set + 1;
        ak = ak + noh;
        if (diam + vRida < segmendiLabimoot)

            if (aVahe < ((Math.PI * diam) / 360) * (esimenePolt - segmendiAlgus) - (2 * kaugusKP))
            {
                sgvpep(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                       kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
            }
            else if (diam + rVahe < segmendiLabimoot && diam + rVahe <= kpDiam + poltidestS && aVahe < ((Math.PI * (diam + rVahe)) / 360) * (esimenePolt - segmendiAlgus) - (2 * kaugusKP))
            {
                diam = diam + rVahe;
                sgppep(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                       kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
            }
            else if (diam + rVahe < segmendiLabimoot && diam + rVahe <= kpDiam + poltidestS)
            {
                diam = diam + rVahe;
                sgpppv(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                       kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
            }
            else if (diam + rVahe < segmendiLabimoot && diam + rVahe > kpDiam + poltidestS)
            {
                diam = diam + rVahe;
                sgpp(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                     kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
            }
            else
            {
                segmentTxtBox.Text += "\r\nAugupaare kokku: " + ak + "tk\r\n\r\nMartin_2018";
            }
    }

    private static void sgvpep(TextBox segmentTxtBox, double ak, double start, double esimenePolt, double teinePolt, double segmendiLabimoot, double segmendiAlgus, double segmendiLopp, double kettaLabimoot, double auguridadeArv, double kpDiam, double aVahe, double vRida, double rVahe, double kaugusKP, double poltidestV, double poltidestS, int set, double diam, double dnoh, double noh, double angl) // segment vastupäeva (pärast esimest polt)
    {
        double ringiPikkus = Math.PI * diam;
        double startd = 360 / (ringiPikkus / kaugusKP);
        start = esimenePolt - startd;

        double segmendiVahemik = esimenePolt - segmendiAlgus;
        double soh = (((Math.PI * diam) / (360 / segmendiVahemik)) - (2 * kaugusKP)) / aVahe;
        noh = Convert.ToInt16(soh) + 1;
        dnoh = Convert.ToDouble(noh);
        angl = (segmendiVahemik - (360 / ((Math.PI * diam) / (2 * kaugusKP)))) / (noh - 1);

        segmentTxtBox.Text += string.Format(
                                  "\r\nSet " + " {0}" +
                                  "\r\nDiameter " + " {1}" +
                                  "\r\nStart angle " + " {2:0.00}" +
                                  "\r\nAngle betw.holes " + " -{3:0.00}" +
                                  "\r\nNo. of holes " + " {4}\r\n",
                                  set, diam, start, angl, noh);

        set = set + 1;
        ak = ak + noh;
        double yksAugupaar = 360 / (((diam + rVahe) * Math.PI) / aVahe);

        if (diam + rVahe < segmendiLabimoot && diam + rVahe <= kpDiam + poltidestS && aVahe < ((Math.PI * (diam + rVahe)) / 360) * (esimenePolt - segmendiAlgus) - (2 * kaugusKP))
        {
            diam = diam + rVahe;
            sgppep(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                   kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
        }
        else if (diam + rVahe < segmendiLabimoot && diam + rVahe <= kpDiam + poltidestS)
        {
            diam = diam + rVahe;
            sgpppv(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                   kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
        }
        else if (diam + rVahe < segmendiLabimoot && diam + rVahe > kpDiam + poltidestS)
        {
            diam = diam + rVahe;
            sgpp(segmentTxtBox, ak, start, esimenePolt, teinePolt, segmendiLabimoot, segmendiAlgus, segmendiLopp, kettaLabimoot, auguridadeArv, kpDiam, aVahe, vRida, rVahe,
                 kaugusKP, poltidestV, poltidestS, set, diam, dnoh, noh, angl);
        }
        else
        {
            segmentTxtBox.Text += "\r\nAugupaare kokku: " + ak + "tk\r\n\r\nMartin_2018";
        }

    }

    private void AtxtBox_TextChanged(object sender, EventArgs e)
    {

    }

    private void BtxtBox_TextChanged(object sender, EventArgs e)
    {

    }

    private void CtxtBox_TextChanged(object sender, EventArgs e)
    {

    }

    private void AtxtBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        // lubab tekstiväljadele sisestada vaid numbrilisi väärtuseid
        char ch = e.KeyChar;

        if ((!Char.IsDigit(ch) && ch != 8) && ch > '.')
        {
            e.Handled = true;
        }
    }

    // Salvestab ketta programmi tekstifailina
    private void ketasSaveBtn_Click(object sender, EventArgs e)
    {
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        saveFileDialog1.Filter = "Text Files | *.txt";
        saveFileDialog1.DefaultExt = "txt";
        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        {
            using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(ketasTxtBox.Text);
                }
        }
    }

    // Kopeerib ketta tekstiväljal oleva teksti
    private void ketasCopyBtn_Click(object sender, EventArgs e)
        {
            // kontrollib kas tekstiväljale on sisestatud tekst
            if (!string.IsNullOrWhiteSpace(ketasTxtBox.Text))
            {
                Clipboard.SetText(ketasTxtBox.Text);
            }
            else
            {
                return;
            }
        }

    // Kopeerib segmendi tekstiväljal oleva teksti
    private void segmentCopyBtn_Click(object sender, EventArgs e)
        {
            // kontrollib kas tekstiväljale on sisestatud tekst
            if (!string.IsNullOrWhiteSpace(segmentTxtBox.Text))
            {
                Clipboard.SetText(segmentTxtBox.Text);
            }
            else
            {
                return;
            }
        }

    private void segmentTxtBox_TextChanged(object sender, EventArgs e)
    {
        segmentTxtBox.ScrollBars = ScrollBars.Vertical;
    }



    private void Form1_Load(object sender, EventArgs e)
    {
        MaximizeBox = false;
    }

        // CSV nupp(ketas)
        private void ketasCsvBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reserveeritud!");
            return;

            //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //saveFileDialog1.Filter = "CSV|*.csv";
            //saveFileDialog1.DefaultExt = "csv";
            //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
            //    using (StreamWriter sw = new StreamWriter(s))
            //    {
            //        sw.Write(ketasTxtBox.Text);
            //    }
            //}
        }

        // segment CSV nupp
        private void segmentCsvBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reserveeritud!");
            return;

            //// segmendi programmi salvestamine CSV faili
            //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //// faili formaadi filtrid
            //saveFileDialog1.Filter = "CSV|*.csv";
            //saveFileDialog1.DefaultExt = "txt";
            //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    // luuakse uus tekstifail
            //    using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
            //    using (StreamWriter sw = new StreamWriter(s))
            //    {
            //        // salvestatav tekst võetakse segmendi väljalt
            //        sw.Write(segmentTxtBox.Text);
            //    }
            //}
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        // Laeb andmebaasis olevad lähteandmed programmi tabeli vaatesse
        private void dbLoad_Click(object sender, EventArgs e)
        {
            ////string constring = "datasource=localhost;username=root;password=";
            //string constring = "server=localhost;user=root;database=kh;port=3306;password=;";
            //MySqlConnection conDataBase = new MySqlConnection(constring);
            //MySqlCommand cmdDataBase = new MySqlCommand(" select * from harjad ;", conDataBase);


            string MySQLConnectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=kh";
            MySqlConnection databaseConnection = new MySqlConnection(MySQLConnectionString);
            MySqlCommand commandDatabase = new MySqlCommand(" select * from harjad ;", databaseConnection);


            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = commandDatabase;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bSource = new BindingSource();

                bSource.DataSource = dbdataset;
                dataGridView1.DataSource = bSource;
                sda.Update(dbdataset);
            }
            catch
            {
                MessageBox.Show("Päringu viga: ");
            }


        }

        // Laeb valitud andmed andmebaasist programmi
        private void dbLoadId_Click(object sender, EventArgs e)
        {
            
            // Kontrollib kas ID: väljale on sisestatud väärtus
            if (string.IsNullOrWhiteSpace(dbTxtBox.Text))
            {
                MessageBox.Show("Sisestage ID: väljale soovitud programmi id!");
                return;
            }
            else
            {
                //String source = "server=localhost;user id=root;password=secret;database=kh";

                //MySqlConnection con = new MySqlConnection(source);
                //con.Open();

                // db ühenduse muutujad
                string Server = "localhost";
                string UserName = "root";
                string Password = "";
                string Database = "KH";
                string Port = "3306";

                // andmebaasi ühendusstring
                string config = string.Format("Server = {0}; Port = {1}; Database = {2}; Uid = {3}; Pwd = {4}; pooling = true; Allow Zero Datetime = False; Min Pool Size = 0; Max Pool Size = 200; ", Server, Port, Database, UserName, Password);

                using (var con = new MySqlConnection { ConnectionString = config
                })
                {
                    con.Open();

                    MessageBox.Show("DB Connected");
                    // andmebaasi päring
                    String sqlSelectQuery = "SELECT * FROM harjad WHERE ID = " + int.Parse(dbTxtBox.Text);
                    MySqlCommand cmd = new MySqlCommand(sqlSelectQuery, con);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        // muutujad andmebaasist tekstiväljadele
                        AtxtBox.Text = (dr["Aklm"].ToString());
                        BtxtBox.Text = (dr["Brda"].ToString());
                        CtxtBox.Text = (dr["Ckia"].ToString());
                        DtxtBox.Text = (dr["Dkil"].ToString());
                    }
                    con.Close();

                }

                
                
            }
        }
    }
}

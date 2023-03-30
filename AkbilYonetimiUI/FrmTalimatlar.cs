using AkbilYntmIsKatmani;
using AkbilYntmVeriKatmani;
using System.Collections;

namespace AkbilYonetimiUI;

public partial class FrmTalimatlar : Form
{
    IVeriTabaniIslemleri veriTabaniIslemleri = new SQLVeriTabaniIslemleri();
    public FrmTalimatlar()
    {
        InitializeComponent();
    }

    private void FrmTalimatlar_Load(object sender, EventArgs e)
    {
        lblBekleyenTalimat.Font = new Font("Segoe UI", 25);
        lblBekleyenTalimat.ForeColor = Color.Red;
        ComboBoxaKullanicininAkbilleriniGetir();
        cmbAkbiller.SelectedIndex = -1;
        cmbAkbiller.Text = "Akbil Seçiniz...";
        grpYukleme.Enabled = false;
        //cmbAkbiller.DropDownStyle = ComboBoxStyle.DropDownList;
        dataGridViewTalimatlar.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        TalimatlariDataGrideGetir();
        dataGridViewTalimatlar.ContextMenuStrip = contextMenuStrip1;
        chcTumunuGoster.Checked = false;
        BekleyenTalimatSayisiniGetir();
        tmrBekleyenTalimat.Interval = 1000;
        tmrBekleyenTalimat.Enabled = true;
    }

    private void BekleyenTalimatSayisiniGetir()
    {
        try
        {
            lblBekleyenTalimat.Text = veriTabaniIslemleri.VeriGetir("KullanicininTalimatlari", kosullar: $"KullaniciId = {GenelIslemler.GirisYapanKullaniciId} and YuklendiMi=0").Rows.Count.ToString();
        }
        catch (Exception hata)
        {
            MessageBox.Show("Beklenmedik bir hata oluştu!" + hata.Message);
        }
    }

    private void TalimatlariDataGrideGetir(bool tumunuGoster = false, string akbilno = null)
    {
        try
        {
            string kosullar = $"KullaniciId = {GenelIslemler.GirisYapanKullaniciId}";
            if(cmbAkbiller.SelectedIndex >= 0)
            {
                akbilno = cmbAkbiller.Text;
            }
            if(!string.IsNullOrEmpty(akbilno)) 
            {
                kosullar += $" and Akbil like '%{akbilno}%' ";
            }
            if (tumunuGoster)
            {
                dataGridViewTalimatlar.DataSource = veriTabaniIslemleri.VeriGetir("KullanicininTalimatlari", kosullar:kosullar );
            }
            else
            {
                kosullar += " and YuklendiMi = 0";
                dataGridViewTalimatlar.DataSource = veriTabaniIslemleri.VeriGetir("KullanicininTalimatlari", kosullar:kosullar );
            }
            foreach (DataGridViewColumn item in dataGridViewTalimatlar.Columns)
            {
                item.Width = 200;
            }
            dataGridViewTalimatlar.Columns["Id"].Visible = false;
            dataGridViewTalimatlar.Columns["Akbil"].Width = 300;
            dataGridViewTalimatlar.Columns["YuklendiMi"].HeaderText = "Talimat Yuklendi Mi?";
            dataGridViewTalimatlar.Columns["YuklendiMi"].Width = 150;
            dataGridViewTalimatlar.Columns["KullaniciId"].Visible = false;

        }
        catch (Exception hata)
        {
            MessageBox.Show("Beklenmedik bir hata oluştu!" + hata.Message);
        }
    }

    private void ComboBoxaKullanicininAkbilleriniGetir()
    {
        try
        {
            cmbAkbiller.DataSource = veriTabaniIslemleri.VeriGetir("Akbiller", kosullar: $"AkbilSahibiId = {GenelIslemler.GirisYapanKullaniciId}");
            cmbAkbiller.DisplayMember = "AkbilNo";
            cmbAkbiller.ValueMember = "AkbilNo"; //Genellikle benzersiz bilgi atanır.
        }
        catch (Exception hata)
        {
            MessageBox.Show("Beklenmedik bir hata oluştu!" + hata.Message);
        }
    }

    private void cmbAkbiller_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbAkbiller.SelectedIndex >= 0)
        {
            txtYuklenecekTutar.Clear();
            grpYukleme.Enabled = true;
        }
        else
        {
            txtYuklenecekTutar.Clear();
            grpYukleme.Enabled = false;
        }
        BekleyenTalimatSayisiniGetir();
        TalimatlariDataGrideGetir(chcTumunuGoster.Checked);
    }

    private void btnTalimatıKaydet_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbAkbiller.SelectedIndex < 0)
            {
                MessageBox.Show("Akbil seçmeden talimat kaydedilemez! ");
                return;
            }
            if (string.IsNullOrEmpty(txtYuklenecekTutar.Text))
            {
                MessageBox.Show("Yükleme miktarı girişi zorunludur.");
                return;
            }
            if (!decimal.TryParse(txtYuklenecekTutar.Text.Trim(), out decimal tutar))
            {
                MessageBox.Show("Yükleme miktarı girişi uygun formatta olmalıdır.");
                return;
            }

            Dictionary<string, object> kolonlar = new Dictionary<string, object>();
            kolonlar.Add("OlusturulmaTarihi", $"'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'");
            kolonlar.Add("AkbilId", $"'{cmbAkbiller.SelectedValue}'");
            kolonlar.Add("YuklenecekTutar", txtYuklenecekTutar.Text.Trim().Replace(",", "."));
            kolonlar.Add("YuklendiMi", "0");
            kolonlar.Add("YuklenmeTarih", "null");
            string talimatInsert = veriTabaniIslemleri.VeriEklemeCumlesiOlustur("Talimatlar", kolonlar);
            int sonuc = veriTabaniIslemleri.KomutIsle(talimatInsert);
            if (sonuc > 0)
            {
                MessageBox.Show("Talimat Kaydedildi");
                cmbAkbiller.SelectedIndex = -1;
                cmbAkbiller.Text = "Akbil Seçiniz...";
                grpYukleme.Enabled = false;
                TalimatlariDataGrideGetir(chcTumunuGoster.Checked);
                BekleyenTalimatSayisiniGetir();
            }
            else
            {
                MessageBox.Show("Talimat Kaydeilemedi.");
            }
        }
        catch (Exception hata)
        {
            MessageBox.Show("Beklenmedik bir hata oluştu!" + hata.Message);
        }
    }

    private void chcTumunuGoster_CheckedChanged(object sender, EventArgs e)
    {
        TalimatlariDataGrideGetir(chcTumunuGoster.Checked);
    }

    private void anaMaenuToolStripMenuItem_Click(object sender, EventArgs e)
    {
        FrmAnaSayfa frmA = new FrmAnaSayfa();
        this.Hide();
        frmA.Show();
    }

    private void cikisYapToolStripMenuItem_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Güle güle çıkış yapıldı.");
        GenelIslemler.GirisYapanKullaniciAdSoyad = string.Empty;
        GenelIslemler.GirisYapanKullaniciId = 0;
        foreach (Form item in Application.OpenForms)
        {
            if (item.Name != "FrmGiris")
            {
                item.Hide();
            }
        }
        Application.OpenForms["FrmGiris"].Show();
    }

    private void tmrBekleyenTalimat_Tick(object sender, EventArgs e)
    {
        if (lblBekleyenTalimat.Text != "0")
        {
            if (DateTime.Now.Second % 2 == 0)
            {
                lblBekleyenTalimat.Font = new Font("Segoe UI", 40);
                lblBekleyenTalimat.ForeColor = Color.Green;
            }
            else
            {
                lblBekleyenTalimat.Font = new Font("Segoe UI", 25);
                lblBekleyenTalimat.ForeColor = Color.Red;
            }
        }
    }

    private void talimatiIptalEtToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            int sayac = 0;
            foreach (DataGridViewRow item in dataGridViewTalimatlar.SelectedRows)
            {
                //Yüklenmiş bir talimat iptal edilemez silinemez.
                if ((bool)item.Cells["YuklendiMi"].Value)
                {
                    MessageBox.Show($"DİKKAT! {item.Cells["Akbil"].Value} {item.Cells["YuklenecekTutar"].Value} liralık yüklemesi yapılmıştır. YÜKLENEN TALİMAT İPTAL EDİLEMEZ/SİLİNEMEZ!\nİşlemlerinize devam etmek için tamama basınız.");
                    continue;
                }
                sayac += veriTabaniIslemleri.VeriyiSil("Talimatlar", $"Id = {item.Cells["Id"].Value}");
            }
            MessageBox.Show($"Seçtiğiniz {sayac} adet talimat iptal edilmiştir.");
            TalimatlariDataGrideGetir();
            BekleyenTalimatSayisiniGetir();
        }
        catch (Exception hata)
        {
            MessageBox.Show("Beklenmedik bir hata oluştu!" + hata.Message);
        }
    }

    private void talimatiYukleToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            int sayac = 0;
            foreach (DataGridViewRow item in dataGridViewTalimatlar.SelectedRows)
            {
                if ((bool)item.Cells["YuklendiMi"].Value)
                {
                    continue;
                }
                //talimatlar tablosunu güncellemek
                Hashtable talimatkolonlar = new Hashtable();
                talimatkolonlar.Add("YuklendiMi", 1);
                talimatkolonlar.Add("YuklenmeTarih", $"'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'");
                string talimatGuncelle = veriTabaniIslemleri.VeriGuncellemeCumlesiOlustur("Talimatlar", talimatkolonlar, $"Id={item.Cells["Id"].Value}");
                if (veriTabaniIslemleri.KomutIsle(talimatGuncelle) > 0)
                {
                    //akbilin mevcut bakiyesini öğren
                    decimal bakiye = Convert.ToDecimal(
                    veriTabaniIslemleri.VeriOku("Akbiller", new string[] { "Bakiye" },
                        $"AkbilNo='{item.Cells["Akbil"].Value.ToString()?.Substring(0, 16)}'")["Bakiye"]);

                    //var sonuc = veriTabaniIslemleri.VeriOku("Akbiller", new string[] { "Bakiye" },
                    //    $"AkbilNo='{item.Cells["Akbil"].Value.ToString()?.Substring(0, 15)}'");
                    //decimal bakiye = (decimal)sonuc["Bakiye"];

                    //akbil bakiyesini güncellemek
                    Hashtable akbilkolon = new Hashtable();

                    var sonBakiye = (bakiye + (decimal)item.Cells["YuklenecekTutar"].Value).ToString().Replace(",", ".");

                    akbilkolon.Add("Bakiye", sonBakiye);

                    string akbilGuncelle = veriTabaniIslemleri.VeriGuncellemeCumlesiOlustur("Akbiller", akbilkolon, $"AkbilNo='{item.Cells["Akbil"].Value.ToString()?.Substring(0, 16)}'");

                    sayac += veriTabaniIslemleri.KomutIsle(akbilGuncelle);
                }
            } // foreach bitti.
            MessageBox.Show($"{sayac} adet talimat akbile yüklendi!");
            TalimatlariDataGrideGetir();
            BekleyenTalimatSayisiniGetir();
        }
        catch (Exception hata)
        {
            MessageBox.Show("Beklenmedik bir hata oluştu! " + hata.Message);
        }
    }
}

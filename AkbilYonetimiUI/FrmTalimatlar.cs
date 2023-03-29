using AkbilYntmIsKatmani;
using AkbilYntmVeriKatmani;

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

    }

    private void TalimatlariDataGrideGetir()
    {

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
            Dictionary<string,object> kolonlar = new Dictionary<string,object>();
            kolonlar.Add("OlusturulmaTarihi",$"'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'");
            kolonlar.Add("AkbilId",$"'{cmbAkbiller.SelectedValue}'");
            kolonlar.Add("YuklenecekTutar",tutar);
            kolonlar.Add("YuklendiMi","0");
            kolonlar.Add("YuklenmeTarihi","null");
            string talimatInsert = veriTabaniIslemleri.VeriEklemeCumlesiOlustur("Talimatlar",kolonlar);
            veriTabaniIslemleri.KomutIsle(talimatInsert);
        }
        catch (Exception hata)
        {
            MessageBox.Show("Beklenmedik bir hata oluştu!" + hata.Message);
        }
    }
}

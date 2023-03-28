using AkbilYntmIsKatmani;
using AkbilYntmVeriKatmani;
using System.Data.SqlClient;

namespace AkbilYonetimiUI;

public partial class FrmKayitOl : Form
{
    IVeriTabaniIslemleri veriTabaniIslemleri = new SQLVeriTabaniIslemleri(GenelIslemler.SinifSQLBaglantiCumlesi);
    public FrmKayitOl()
    {
        InitializeComponent();
    }

    private void FrmKayitOl_Load(object sender, EventArgs e)
    {
        #region Ayarlar
        txtSifre.PasswordChar = '*';
        dtpDogumTarihi.MaxDate = new DateTime(2016, 1, 1);
        dtpDogumTarihi.Value = new DateTime(2016, 1, 1);
        dtpDogumTarihi.Format = DateTimePickerFormat.Short;
        #endregion
    }

    private void btnKayıtOl_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (var item in Controls)
            {
                if (item is TextBox txt && string.IsNullOrEmpty(txt.Text))
                {
                    MessageBox.Show("Zorunlu alanları doldurunuz");
                    return;
                }
            }
            Dictionary<string, object> kolonlar = new Dictionary<string, object>
            {
                { "Ad", $"'{txtAd.Text.Trim()}'" },
                { "Soyad", $"'{txtSoyad.Text.Trim()}'" },
                { "Email", $"'{txtEmail.Text.Trim()}'" },
                { "EklenmeTarihi", $"'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'" },
                { "DogumTarihi", $"'{dtpDogumTarihi.Value.ToString("yyyyMMdd")}'" },
                { "Parola", $"'{GenelIslemler.MD5Encryption(txtSifre.Text.Trim())}'" }
            };
            string insertCumle = veriTabaniIslemleri.VeriEklemeCumlesiOlustur("Kullanicilar", kolonlar);
            int sonuc = veriTabaniIslemleri.KomutIsle(insertCumle);
            if (sonuc > 0)
            {
                MessageBox.Show("Kayıt oluşturuldu.");
                DialogResult cevap = MessageBox.Show("Giriş sayfasına gitmek ister misin?","SORU",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if(cevap == DialogResult.Yes)
                {
                    //temizlik
                    //Girişe git.
                    FrmGiris frmg = new FrmGiris();
                    frmg.Email = txtEmail.Text.Trim();
                    foreach (Form item in Application.OpenForms)
                    {
                        item.Hide();
                    }
                    frmg.Show();
                }
            }

            else
            {
                MessageBox.Show("Kayıt EKLENEMEDİ!!!");
            }
        }
        catch (Exception ex)
        {

            //ex log.txt'ye yazaılacak (loglama)
            MessageBox.Show("Beklenmedik bir hata oluştu! Litfen Tekrar deneyiniz.");
        }
    }

    private void GirisFromunaGit()
    {
        FrmGiris frmG = new FrmGiris();
        frmG.Email = txtEmail.Text.Trim();
        this.Hide();
        frmG.Show();
    }

    private void FrmKayitOl_FormClosed(object sender, FormClosedEventArgs e)
    {
        GirisFromunaGit();
    }

    private void btnGiris_Click(object sender, EventArgs e)
    {
        GirisFromunaGit();
    }
}

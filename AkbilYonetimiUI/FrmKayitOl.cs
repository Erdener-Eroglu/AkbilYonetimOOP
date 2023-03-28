using System.Data.SqlClient;

namespace AkbilYonetimiUI;

public partial class FrmKayitOl : Form
{
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
            //1) Emailden kayıtlı biri zaten var mı
            string baglantiCumlesi = @"Server=MONSTER; Database=AkbilDB; Trusted_Connection=True;";
            SqlConnection baglanti = new SqlConnection(); //bağlantı nesnesi
            baglanti.ConnectionString = baglantiCumlesi; //nereye bağlanacak
            SqlCommand komut = new SqlCommand(); //komut nesnesi türettik
            komut.Connection = baglanti; //komutun hangi bağlantı da çalışacağını atadık.
            komut.CommandText = $"select * from Kullanicilar (nolock) where Email='{txtEmail.Text.Trim()}'"; //sql komutu
            baglanti.Open();
            SqlDataReader okuyucu = komut.ExecuteReader(); //çalıştır
            if (okuyucu.HasRows) //satır var mı?
            {
                MessageBox.Show("Bu e-mail zaten sisteme kayıtlıdır.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            baglanti.Close();
            //2) Emaili daha önce kayıylı değilse kaydolacak
            if (string.IsNullOrEmpty(txtAd.Text) || string.IsNullOrEmpty(txtSoyad.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtSifre.Text))
            {
                MessageBox.Show("Bilgileri eksiksiz giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            string insertSQL = $"insert into Kullanicilar (EklenmeTarihi,Email,Parola,Ad,Soyad,DogumTarihi) values " +
                $"('{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{txtEmail.Text.Trim()}','{txtSifre.Text.Trim()}','{txtAd.Text.Trim()}','{txtSoyad.Text.Trim()}','{dtpDogumTarihi.Value.ToString("yyyyMMdd")}')";
            baglanti.ConnectionString = baglantiCumlesi;
            SqlCommand eklemeKomut = new SqlCommand(insertSQL, baglanti);
            baglanti.Open();
            int rowsEffected = eklemeKomut.ExecuteNonQuery(); //Insert Update Delete için kullanılır.
            if (rowsEffected > 0)
            {
                MessageBox.Show("Kayıt Eklendi", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GirisFromunaGit();
            }
            else
            {
                MessageBox.Show("Kayıt Eklenemedi", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
            baglanti.Close();
            //Temizlik gerekli.
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

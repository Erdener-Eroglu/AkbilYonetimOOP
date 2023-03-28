namespace AkbilYonetimiUI;

public partial class FrmAyarlar : Form
{
    public FrmAyarlar()
    {
        InitializeComponent();
    }

    private void FrmAyarlar_Load(object sender, EventArgs e)
    {
        txtSifre.PasswordChar = '*';
        dtpDogumTarihi.MaxDate = new DateTime(2016, 1, 1);
        dtpDogumTarihi.Value = new DateTime(2016, 1, 1);
        dtpDogumTarihi.Format = DateTimePickerFormat.Short;
        KullanicininBilgileriniGetir();
    }

    private void KullanicininBilgileriniGetir()
    {
        try
        {
            //NOT: Giriş yapmış kullanıcının bilgileriyle select sorgusu yazacağız Kullanıcı bilgisini alabilmek için burada 2 yöntem kullanabiliriz.
            //Static bir class açıp içinde static GiriYapmisKullaniciEmail propertysi kullanılabilir.
            //Propertiesden çekebiliriz.

            if (string.IsNullOrEmpty(Properties.Settings1.Default.KullaniciEmail))
            {
                MessageBox.Show("Giriş yapmadan buraya ulaşamazsınız");
                return;
            }
            else
            {
                
            }
        }
        catch (Exception hata)
        {
            MessageBox.Show("Beklenmedik bir hata oluştu." + hata.Message);
        }
    }

    private void btnGuncelle_Click(object sender, EventArgs e)
    {
        try
        {
            
        }
        catch (Exception hata)
        {
            MessageBox.Show("Güncelleme Başarısızdır!" + hata.Message);
        }
    }
}

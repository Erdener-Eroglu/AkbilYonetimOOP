using AkbilYntmIsKatmani;
using AkbilYntmVeriKatmani;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AkbilYonetimiUI
{
    public partial class FrmAkbiller : Form
    {
        public FrmAkbiller()
        {
            InitializeComponent();
        }
        IVeriTabaniIslemleri veriTabaniIslemleri = new SQLVeriTabaniIslemleri();
        private void anaMenüToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbAkbilTipler.SelectedIndex < 0)
                {
                    MessageBox.Show("Lütfen ekleyeceğiniz akbilin türünü seçiniz.");
                    return;
                }
                if(maskedTextBoxAkbilNo.Text.Length < 16)
                {
                    MessageBox.Show("Akbil NO 16 haneli olmak zorundadır");
                    return;
                }
                Dictionary<string,object> yeniAkbilBilgileri = new Dictionary<string,object>();
                yeniAkbilBilgileri.Add("AkbilNo", $"'{maskedTextBoxAkbilNo.Text}'");
                yeniAkbilBilgileri.Add("Bakiye", 0);
                yeniAkbilBilgileri.Add("AkbilTipi", $"'{cmbAkbilTipler.SelectedItem}'");
                yeniAkbilBilgileri.Add("EklenmeTarihi", $"'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'");
                yeniAkbilBilgileri.Add("VizelendigiTarihi", "null");
                yeniAkbilBilgileri.Add("AkbilSahibiId", GenelIslemler.GirisYapanKullaniciId);
                string insertCumle = veriTabaniIslemleri.VeriEklemeCumlesiOlustur("Akbiller",yeniAkbilBilgileri);
                int sonuc = veriTabaniIslemleri.KomutIsle(insertCumle);
                if(sonuc > 0)
                {
                    MessageBox.Show("Akbil eklendi");
                    DataGridViewiDoldur();
                    maskedTextBoxAkbilNo.Clear();
                    cmbAkbilTipler.SelectedIndex = -1;
                    cmbAkbilTipler.Text = "Akbil Tipleri...";
                }
                else
                {
                    MessageBox.Show("Akbil EKLENEMEDİ!!!!");
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Bir hata oluştu" + hata.Message);
            }
        }

        private void FrmAkbiller_Load(object sender, EventArgs e)
        {
            cmbAkbilTipler.Text = "Akbil tipi seçiniz....";
            cmbAkbilTipler.SelectedIndex = -1;
            dataGridViewAkbiller.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGridViewiDoldur();
        }

        private void DataGridViewiDoldur()
        {
            try
            {
                dataGridViewAkbiller.DataSource = veriTabaniIslemleri.VeriGetir("Akbiller", kosullar:$"AkbilSahibiId = {GenelIslemler.GirisYapanKullaniciId}");
                //Bazı kolonlar Gizlensin
                dataGridViewAkbiller.Columns["AkbilSahibiId"].Visible = false;
                dataGridViewAkbiller.Columns["VizelendigiTarihi"].HeaderText = "Vizelendiği Tarih";
                dataGridViewAkbiller.Columns["VizelendigiTarihi"].Width = 200;
            }
            catch (Exception hata)
            {
                MessageBox.Show("Akbilleri listeleyemedim" + hata.Message);
            }
        }
    }
}

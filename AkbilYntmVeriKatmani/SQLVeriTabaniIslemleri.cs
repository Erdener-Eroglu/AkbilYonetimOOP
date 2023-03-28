using AkbilYntmIsKatmani;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace AkbilYntmVeriKatmani;

public class SQLVeriTabaniIslemleri : IVeriTabaniIslemleri
{
    public string BaglantiCumlesi { get; set; }
    private SqlConnection baglanti;
    private SqlCommand komut;
    public SQLVeriTabaniIslemleri()
    {
        BaglantiCumlesi = "";
        baglanti = new SqlConnection(BaglantiCumlesi);
        komut = new SqlCommand();
        komut.Connection = baglanti;
    }

    public SQLVeriTabaniIslemleri(string baglantiCumle)
    {
        BaglantiCumlesi = baglantiCumle;
        baglanti = new SqlConnection(BaglantiCumlesi);
        komut = new SqlCommand();
        komut.Connection = baglanti;
    }
    private void BaglantiyiAc()
    {
        if (baglanti.State != ConnectionState.Open)
        {
            baglanti.Open();
        }
    }
    public int KomutIsle(string eklemeyadaGuncellemeCumlesi)
    {
        try
        {
            using (baglanti)
            {
                komut.CommandType = CommandType.Text;
                komut.CommandText = eklemeyadaGuncellemeCumlesi;
                BaglantiyiAc();
                int etkilenenSatirSayisi = komut.ExecuteNonQuery();
                return etkilenenSatirSayisi;
            }
        }
        catch
        {

            throw;
        }
    }

    public string VeriEklemeCumlesiOlustur(string tabloAdi, Dictionary<string, object> kolonlar)
    {
        try
        {
            //insert into tabloadi (kolonlar) values (degerler)
            string sorgu = string.Empty;
            string sutunlar = string.Empty;
            string degerler = string.Empty;
            foreach (var item in kolonlar)
            {
                sutunlar += $"{item.Key},";
                degerler += $"{item.Value},";
            }
            //en sondaki virgülden kurtulmamız için trim kullanalım.
            sutunlar = sutunlar.TrimEnd(',');
            degerler = degerler.TrimEnd(',');
            sorgu = $"insert into {tabloAdi} ({sutunlar}) values ({degerler})";

            return sorgu;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable VeriGetir(string tabloAdi, string kolonlar = "*", string? kosullar = null)
    {
        try
        {
            using (baglanti)
            {
                string sorgu = $"select {kolonlar} from {tabloAdi} ";
                if (!string.IsNullOrEmpty(kosullar))
                {
                    sorgu += $" where {kosullar}";
                }
                komut.CommandText = sorgu;
                SqlDataAdapter adp = new SqlDataAdapter(komut);
                BaglantiyiAc();
                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string VeriGuncellemeCumlesiOlustur(string tabloAdi, Hashtable kolonlar, string? kosullar = null)
    {
        try
        {
            //update tablo adi set col1 = deger1,... where kosul
            string sorgu = string.Empty, setler = string.Empty;
            foreach (var item in kolonlar.Keys)
            {
                setler += $"{item}={kolonlar[item]}, ";
            }
            setler = setler.Trim().TrimEnd(',');

            sorgu = $"update {tabloAdi} set {setler} ";
            if (string.IsNullOrEmpty(kosullar))
            {
                sorgu += $" where {kosullar}";
            }
            return sorgu;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public Hashtable VeriOku(string tabloAdi, string[] kolonlar, string? kosullar = null)
    {
        try
        {
            Hashtable sonuc = new Hashtable();
            string sutunlar = string.Empty;
            //kolonlara , ekleyeceğiz.

            string sorgu = $"select {sutunlar} from {tabloAdi}";
            if(string.IsNullOrEmpty(kosullar))
            {
                sorgu += $" where {kosullar}";
            }
            return sonuc;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public int VeriyiSil(string tabloAdi, string? kosullar = null)
    {
        try
        {
            using (baglanti)
            {
                string sorgu = $"delete from {tabloAdi}";
                if (string.IsNullOrEmpty(kosullar))
                {
                    sorgu += $" where {kosullar}";
                }
                komut.CommandText = sorgu;
                BaglantiyiAc();
                return komut.ExecuteNonQuery();
            }
        }
        catch
        {
            throw;
        }
    }
}

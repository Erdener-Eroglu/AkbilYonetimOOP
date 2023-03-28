using System.Collections;
using System.Data;

namespace AkbilYntmIsKatmani;

public interface IVeriTabaniIslemleri
{
    string BaglantiCumlesi { get; set; }
    //CRUD: Create Read Update Delete
    DataTable VeriGetir(string tabloAdi, string kolonlar = "*", string? kosullar = null);
    int VeriyiSil(string tabloAdi, string? kosullar = null);

    int KomutIsle(string eklemeyadaGuncellemeCumlesi); //executenonquery

    string VeriEklemeCumlesiOlustur(string tabloAdi, Dictionary<string, object> kolonlar);
    string VeriGuncellemeCumlesiOlustur(string tabloAdi, Hashtable kolonlar, string? kosullar = null);

    Hashtable VeriOku(string tabloAdi, string[] kolonlar, string? kosullar = null);
}

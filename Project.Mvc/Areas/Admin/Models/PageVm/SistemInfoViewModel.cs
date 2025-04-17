namespace Project.MvcUI.Areas.Admin.Models.PageVm
{
    public class SystemInfoViewModel
    {
        public string? ResepsiyonBilgileriSayisi { get; set; }
        public string? BarBilgisayariSayisi { get; set; }
        public string? HavuzBilgisayariSayisi { get; set; }
        public string? YoneticiBilgisayariSayisi { get; set; }

        public string? Sunucu1Bilgisi { get; set; }
        public string? Sunucu2Bilgisi { get; set; }

        public string? DomainDurumu { get; set; }
        public string? WirelessDurumu { get; set; }
        public string? ClientIsletimSistemi { get; set; }

        public string? YedeklemeBilgisi { get; set; }
        public string? GeriYuklemeBilgisi { get; set; }
        public string? EsneklikBilgisi { get; set; }

        public string? Aciklama { get; set; }
    }

}

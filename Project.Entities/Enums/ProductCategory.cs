using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    /// <summary>
    /// Ürün kategorilerini temsil eden sabit değerlerdir.
    /// Bu kategoriler, otelin sabit hizmet alanlarını (örneğin: minibar, restoran, spa) temsil eder.
    /// 
    /// Bu alanları enum olarak sabitledim çünkü bu hizmet türleri dokümantasyonda sabit olarak tanımlanmış.
    /// Yönetici tarafından eklenmesi/silinmesi gibi bir senaryo belirtilmemiştir.
    /// Ancak sistem katmanlı mimari ile geliştirildiğinden dolayı, bu enum yapısı gelecekte kolaylıkla
    /// ayrı bir Entity'ye dönüştürülebilir. Böylece dinamik kategori tanımı, çoklu dil desteği gibi ihtiyaçlara
    /// uyumlu hale gelir.
    /// </summary>
    public enum ProductCategory
    {
        Minibar=0,         // Minibar ürünleri
        RoomService=1,     // Oda servisi ürünleri
        Restaurant=2,      // Restoran menüsü
        Bar=3,             // Bar içecekleri
        Pool=4,            // Havuz çevresi hizmetleri
        Spa=5              // Spa ve masaj hizmetleri
    }

    // 🔄 Genişletilebilirlik Notu:
    // Eğer sistemde ürün kategorileri dinamik hale gelirse, bu enum kaldırılmalı ve
    // yerine ProductCategory adında bir Entity sınıfı oluşturulmalı, Product ile ilişkilendirilmelidir.
}

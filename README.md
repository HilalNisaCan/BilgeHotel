# 🏨 BilgeHotel – Otel Yönetim ve Rezervasyon Sistemi

BilgeHotel, otellerin dijital dönüşümünü hedefleyen kapsamlı bir otomasyon sistemidir. Bu proje, hem otel yöneticilerine hem de müşterilere özel işlevsellikler sunar. Yönetici paneliyle operasyonel süreçlerin tamamı merkezi olarak yönetilirken, müşteri tarafı kolay rezervasyon ve kullanıcı deneyimi sağlar. Proje, kurumsal düzeyde geliştirme disiplinleri ile hayata geçirilmiş, esnek, genişletilebilir ve sektörel ihtiyaçlara karşılık verebilecek niteliktedir.

**BilgeHotel**, gerçek dünyadaki otel operasyonlarını dijital ortama profesyonel olarak aktaran, .NET Core MVC ve WebAPI teknolojileri ile geliştirilmiş kapsamlı bir otomasyon sistemidir. Projede; oda yönetimi, ödeme, kampanya, çalışan vardiyaları, fatura, yorum sistemi ve müşteri etkileşimi gibi tüm modüller detaylı olarak hayata geçirilmiştir.

## 🏗️ Proje Mimarisi

BilgeHotel, **Hybrid N-Tier Architecture** yaklaşımıyla geliştirilmiştir. Tüm katmanlar birbirinden ayrılmış olup bağımlılıkları minimize edecek şekilde tasarlanmıştır. Bu yapı sayesinde hem test edilebilirlik hem de sürdürülebilirlik sağlanmıştır.

**Katmanlar:**

* **Entities Katmanı:** Tüm veritabanı modelleri, enum yapıları
* **DAL (Data Access Layer):** DbContext ve veritabanı erişim katmanı
* **Configuration (Conf):** Entity yapılandırmaları ve ilişkisel haritalamalar (Fluent API)
* **BLL (Business Logic Layer):** DTO’lar, servis sınıfları, iş akışları, TC kimlik doğrulama servisi
* **Common:** Yardımcı servisler (e-posta gönderimi, dosya işlemleri vb.)
* **MVC:** Admin ve müşteri arayüzü (Views, Controllers, ViewModels)
* **WebAPI:** Dış dünyaya açık REST servisleri
* **SeedData:** Bogus kullanılarak oluşturulan sahte test verileri\*\* Bogus kullanılarak oluşturulan sahte test verileri

Bu mimari yapı, projeye hem katmanlı esneklik hem de yeniden kullanılabilirlik kazandırır.

---

## 🚀 Projenin Temel Yetkinlikleri

* MVC + WebAPI birlikte kullanılarak frontend ve backend modüler ayrıldı
* Role-Based Identity yapısı: Admin, HR, Receptionist, Customer
* AutoMapper ile Entity-DTO-ViewModel katmanları ayrıldı
* Generic Repository & Unit of Work ile temiz veri erişimi
* MailKit ile e-posta gönderimi (aktivasyon, fatura, kampanya)
* Bogus ile sahte veri üretimi ve SeederManager ile tek seferlik çalışma
* Mernis SOAP servisi ile T.C. kimlik doğrulama

---

## 🌐 Web Modülü (Müşteri Paneli)

* Web modülümüzde müşteriler sitede istedikleri gibi odaları görselleri ve detaylarıyla görüntüleyebilir ve iletişime bizimle geçebilir.
* Sadece rezervasyon oluşturmak için sistemimize üye olmanız gerekir
* Kayıt ekranımızda sahte üyeleri ve rezervasyonları engellemek için Tc kimlik doğrulama yapılmakta ve E-Postanıza aktivasyon kodu gönderilmektedir.
Mailinize gelen link'e tıklayarak hesabınızı aktif edebilirsiniz.Şifre değişikliğinde ise şifre sıfırlamanız için mailinize link gönderilir ve o linke tıklayarak
şifrenizi sıfırlayabilir yeni şifre oluşturabilirsiniz
* Üye olduktan sonra sistemimizde odalarımıza yorum ve puan verebilirsiniz
* Oluşturduğunuz rezervasyonardan sonra mailinize fatura bilgileriniz gönderilicektir
* Yönetici rezervasyonunuzu onayladığı takdirde profilinizde aktif rezervasyon olarak görebilir dilerseniz iptal edebilirsiniz
* Konaklaması biten ve iptal edilen rezervasyonunuzu geçmiş rezervasyonlarım kısmında görüntüleyebilirsiniz
  


### 👁️ Görüntülenebilir Sayfalar:

* Ana Sayfa

* Hakkımızda

* Odalar ( fiyat, görseller, kapasite, donanımlar, erken rezervasyon indirimi gibi bilgiler listelenir.)
  *Üye olmayan kullanıcılar listelenen sayfada yorum yapamaz,üye olan kullanıcılar için yorum ve puan verme alanı açılır.
  
* İletişim (Gerekli bilgiler girilip gönderildikten sonra yönetici tarafından cevaplanır)

* Giriş / Kayıt Sayfası

* Profilim /Rezervasyon Geçmişim / Aktif Rezervasyonlar


## 🔒 Admin Modülü (Yönetici Paneli)

### 👤 Kullanıcı & Rol Yönetimi (Geliştirilme aşamasındadır)

* Yeni kullanıcı oluşturma ve roller atama (Admin, HR, Receptionist)
* Kullanıcıların yetki seviyelerini kontrol etme
* Aktif müşteri listesi görüntüleme

### 🛏️ Oda & Fiyat Yönetimi


* Fiyat belirleme ve odalara özel fiyatlandırma
* Belirlenen fiyat değişikliği web sitesi ve rezervasyon kısımlarında Api desteğiyle değişir ve güncellenir.
* Manuel Kur girişi sisteme giriliyor geliştirilmesi yapılıcak ve otomatik kur bazlı fiyat değişikleri uygulanacaktır

### 🎯 Kampanya Yönetimi

* Paket türüne özel kampanyaların tanımlanması (Tam Pansiyon, Her Şey Dahil)
* %16, %18, %23 gibi erken rezervasyon indirimi kuralları oluşturma
* Yeni kampanya eklendiğinde sistemde kayıtlı müşterilere otomatik e-posta bildirimi gönderilmesi


### 🗣️ Yorum Onay Süreci

* Müşterilerin bıraktığı yorumları listeleme
* Onaylanan yorumların yalnızca web sayfasında yayınlanması

### 🕒 Çalışan Vardiya Planlaması

* Haftalık vardiya atamaları
* Personel izin günü belirleme ve kontrol
* IT / Elektrik personeline özel sabit vardiya atanması



## 🔒 Admin Modülü (Yönetici Paneli)

  * Oda Yönetimi panelinde odaların müsaitlik,bakım,temizlik durumlarının işaretlenmesi bakım ve temizlik durumunda ilgili personel atamaları
  * Fiyat yönetiminde odaların fiyatlarının güncellenmesi ve bu güncellemenin Api'den çekilerek web sitesi ve rezervasyon formlarında da aynı anda güncellenmesi
  * Kampanya Yönetimi (erken rezervasyon, paket bazlı)  * Yeni kampanya eklendiğinde, **Üye olan kullanıcılara otomatik e-posta bildirimleri gönderilir**.
  * Müşteri yorumlarını onaylama / reddetme ekranı onaylanan yorumların web sitesinde yayınlanması
  * Haftalık vardiya atamaları ve izin kontrolleri,mesai maaş hesaplamaları
  * Rezervasyon Yönetiminde oluşturulan rezervasyon detaylarını görüntüleme ve rezervasyon onaylama iptal etme durumlarının olması onaylanan rezervasyonların sistemde aktif görünmesi
  * Kur Yönetiminde yönetici manuel olarak kur bilgilerini girebiliyor ileride otomatik kur takibi,kur bilgisine göre fiyat değişiklikleri ve de kur ile ödeme sisteme eklenecektir
  * Şikayet/Geri bildirimler yönetiminde web sayfamızdaki iletişim formunda oluşurulan mesajlar bu panelde cevaplanmaktadır.
  * Günlük XAML müşteri raporu alınmakta ve indirilmektedir
  * Müşteri raporları panelinde müşteriye ait geçmiş,gecelek ve konaklama durumlarnı görüntülenmekte aynı zamanda müşteri bilgileri excel ve pdf olarak indirilebilmektedir.
  * Sistemde gerçek Tc kimlik doğrulama kullandık sahte müşteri ve rezervasyonların önüne geçmek için bu yüzden müşteri raporu ve rezervasyon yönetimindeki özel bilgiler güvenlik açısından
  github için gizli şekilde gösterilmiştir
  * Sistem yedeği alma (BackupLog kayıtları)
  * Sistem Yedeklemede sahte veritabanı yedekleme yapılmaktadır
    
---

## 📅 Rezervasyon Modülü 

* Rezervasyon modülü, otel içerisindeki resepsiyonistler için tasarlanmıştır.
* Boş odaların listelenmesi, detaylarının incelenmesi ve rezervasyon oluşturulması mümkündür.
* Otelde oluşturulan rezervasyonlar sistemde faturalandırılır ancak ödemesi fiziksel olarak alınır.
* Yönetici paneline düşen rezervasyonlar onaylandığında konaklama başlatılır ve müşteri raporuna "şu anda konaklıyor" olarak işlenir.
*Check-in ve Check-out işlemleri yapılır, çıkışta ekstra harcamalar hesaplanır.


## 🛠️ Geliştirilmeye Açık Alanlar

> Not: Projede bazı sınıflar şimdilik doğrudan kullanılmamakta ancak yazılım mimarisine dahil edilmiştir. Bu sınıflar, projenin gelecekte genişletilmesi ve yeni özelliklerin entegre edilmesiyle birlikte aktif hale gelecektir.
> Otomatik kur takibi ve kur bazlı ödeme,fiyatlandırma


---

⚠️ Not: Projede kullanılan kişisel veriler (T.C. kimlik numarası, telefon numarası, e-posta vs gibi) veri güvenliği amacıyla ilk birkaç hanesi bırakılarak maskelenmiştir. Gerçek veriler tam haliyle paylaşılmamıştır.

---

## 👩‍💻 Geliştirici Bilgileri

**Ad:** Hilal Nisa Canpolat
**E-posta:** [hilalnisacan92@gmail.com](mailto:hilalnisacan92@gmail.com)
**LinkedIn:** [linkedin.com/in/HilalNisaCanpolat](https://linkedin.com/in/HilalNisaCanpolat)
**GitHub:** [github.com/HilalNisaCan](https://github.com/HilalNisaCan)

> Bu proje, BilgeAdam Akademi .NET eğitim programı kapsamında geliştirilmiş ve kurumsal düzeyde otel otomasyonu mimarisi uygulamalarını temel alarak sektörel ihtiyaçlara yönelik profesyonel bir çözüm sunmaktadır.

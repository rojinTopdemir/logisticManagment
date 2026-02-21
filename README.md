FiberKargo - Lojistik ve Kargo Yönetim Sistemi
FiberKargo, kargo süreçlerini uçtan uca yönetmek için geliştirilmiş web tabanlı bir lojistik yönetim sistemidir. Sistem; admin, personel ve müşteri rollerine yönelik farklı özellikler sunarak kargo takibi, personel yönetimi ve raporlama süreçlerini dijitalleştirmeyi amaçlar.

🚀 Özellikler
🛠 Yönetici (Admin) Paneli
Personel Yönetimi: Yeni personel hesabı oluşturma, düzenleme ve silme işlemleri.

Kargo Denetimi: Sistemdeki tüm kargoların durumunu izleme.

Geri Bildirim Takibi: Müşterilerden gelen geri bildirimleri ve şikayetleri görüntüleme.

Raporlama: Lojistik verimliliğine dair genel raporlar alma.

📦 Personel Paneli
Kargo Kaydı: Yeni kargo gönderisi oluşturma.

Durum Güncelleme: Mevcut kargoların takip bilgilerini düzenleme ve konum güncelleme.

Kargo Takibi: Personelin sorumlu olduğu şubedeki kargoları listeleme.

🌐 Müşteri Özellikleri (Ana Sayfa)
Kargo Takibi: Takip numarası ile kargonun nerede olduğunu sorgulama.

Fiyat Hesaplama: Kargonun ağırlık ve mesafe bilgilerine göre tahmini ücret hesaplama hizmeti.

İletişim ve Geri Bildirim: Şirketle iletişime geçme ve hizmet değerlendirmesi yapma.

💻 Kullanılan Teknolojiler
Framework: ASP.NET MVC (v5.2.9)

Veritabanı Yönetimi: Entity Framework (Code First yaklaşımı)

Frontend: Bootstrap (v5.2.3), jQuery (v3.7.0)

Programlama Dili: C#

Güvenlik: Captcha doğrulama hizmeti

📂 Proje Yapısı
Controllers: İş mantığının yönetildiği ana kontrolcüler (AdminController, PersonelController, HomeController, AccountController).

Models: Veritabanı tablolarının ve sınıfların tanımlandığı katman (Cargo, User, Branch, Feedback).

Services: Fiyat hesaplama ve Captcha gibi yardımcı servisler.

Views: Kullanıcı arayüzünü oluşturan Razor bileşenleri.

🛠 Kurulum
Projeyi GitHub üzerinden klonlayın veya zip olarak indirin.

FiberKargo.sln dosyasını Visual Studio ile açın.

Web.config dosyasındaki connectionStrings bölümünü kendi SQL Server ayarlarınıza göre güncelleyin.

Package Manager Console üzerinden Update-Database komutunu çalıştırarak veritabanı tablolarını oluşturun (veya projenin DbInitializer sınıfı sayesinde ilk çalıştırmada verilerin otomatik eklenmesini bekleyin).

Projeyi derleyin ve çalıştırın.

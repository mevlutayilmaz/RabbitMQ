# RabbitMQ
RabbitMQ mesaj kuyruğu (message queue) sistemidir. Yazdığımız programımız üzerinden yapılacak asenkron (asynchronous) işlemleri sıraya koyup, bunları sırayla kuyruktan çekip gerçekleyerek ilerleyen ölçeklenebilir ve performanslı bir sistemdir.

## 📚 İçindekiler

1. [Message Queue Nedir?](#message-queue-nedir)
2. [Message Broker Nedir?](#message-broker-nedir)
3. [🐇 RabbitMQ’yu Neden Kullanmalıyız?](#rabbitmqyu-neden-kullanmalıyız)
4. [🔄 RabbitMQ’nun İşleyişi Nasıldır?](#rabbitmqnun-i̇şleyişi-nasıldır)
   - [⚡ Exchange Nedir?](#exchange-nedir)
5. [🚀 Basitçe Kuyruğa Mesaj Gönderme ve Okuma](#basitçe-kuyruğa-mesaj-gönderme-ve-okuma)
6. [⚙️ Gelişmiş Kuyruk Mimarisi](#gelişmiş-kuyruk-mimarisi)
7. [🔗 Exchange Kullanımı](#exchange-kullanımı)
8. [📩 Mesaj Tasarımları Nelerdir?](#mesaj-tasarımları-nelerdir)
   - [💡 Yaygın Mesaj Tasarımları Nelerdir?](#yaygın-mesaj-tasarımları-nelerdir)
9. [🚏 Enterprise Service Bus & MassTransit Nedir?](#enterprise-service-bus-masstransit-nedir)
   - [🚋 MassTransit Nedir?](#masstransit-nedir)


## Message Queue Nedir
- Message Queue, yazılım sistemlerinde iletişim için kullanılan bir yapıdır.
- Birbirinden bağımsız sistemler arasında veri alışverişi yapmak için kullanılır.
- Message Queue, gönderilen mesajları kuyrukta saklar ve sonradan bu mesajların işlenmesini sağlar.
- Kuyruğa mesaj gönderene ***Producer(Yayıncı)*** ya da ***Publisher***, kuyruktaki mesajları işleyene ise ***Consumer(Tüketici)*** denir.
- Burada ‘message’dan kastedilen iki sistem arasında iletişim için kullanılan veri birimidir. Yani Producer’ın Consumer tarafından işlenmesini istediği veridir.
- Örnek olarak, bir e-ticaret sisteminde eğer siparişe dair mesaj olarak, ürün bilgilendirmesi veya ödeme bilgileri örnek olarak verilebilir.

## Message Broker Nedir?
İçerisinde Message Queue’yi barındıran ve bu queue üzerinde publisher/producer ile consumer arasındaki iletişimi sağlayan sistemdir. Bir Message Broker içerisinde birden fazla queue bulunabilir.

## RabbitMQ’yu Neden Kullanmalıyız?
Yazılım uygulamalarında ölçeklendirilebilir bir ortam sağlayabilmek istiyorsak eğer RabbitMQ kullanılmalıdır.

Uygulamamızda kullanıcıdan gelen bir istek neticesinde anlık cevap veremiyorsak ya da anlık olmayan/zaman alan işlemleri devreye sokmamız gerekiyorsa kullanıcıyı oyalamak yerine bu tarz bir süreci asenkron bir şekilde işleyip uygulama yoğunluğunu düşürmemiz gerekmektedir. Aksi taktirde kullanıcı gereksiz bir Response Time süresine maruz kalacak ve yazılımımız aleyhine bir durum söz konusu olacaktır.

İşte bu tarz durumlarda asenkron süreci kontrol edecek olan yapı RabbitMQ’dur. RabbitMQ, Response Time zamanı uzun sürebilecek operasyonları uygulamadan bağımsızlaştırarak buradaki sorumluluğu farklı bir uygulamanın üstlenmesini sağlayacak olan bir mekanizma sunmaktadır. Bu mekanizma; uzun sürecek/maliyetli işlemleri RabbitMQ aracılığıyla kuyruğa gönderecektir ve bu kuyruktaki işlemler farklı bir uygulama tarafından işlenerek sonuç asenkron bir şekilde ana uygulamadan bağımsız elde edilecek, böylece ana uygulamada ki yoğunluk mümkün mertebe minimize edilmiş olacaktır.

## RabbitMQ’nun İşleyişi Nasıldır?

![1_3ejQwljyw2TQVvWqz0sL_g](https://github.com/user-attachments/assets/54e747c1-26af-4851-bed4-5abadd8551ec)

Şemada da görüldüğü üzere RabbitMQ temelinde *Publisher* ve *Consumer* olmak üzere iki aktör ile birlikte *Exchange* ve *Queue* enstrümanları mevcuttur.

Burada *Publisher* RabbitMQ mesaj kuyruğuna mesaj gönderen yani bir başka deyişle mesaj üreten kişidir/uygulamadır. *Publisher* mesajı publish ettikten sonra ilgili mesajı *Exchange* karşılayacaktır. *Exchange*, kendisine belirtilen route ile ilgili mesajı kuyruğa yönlendirir. İlgili mesajın nasıl kuyruğa gideceği *Exchange* içerisindeki routedan öğrenilir. 

Ardından kuyruğa gelen mesajlar *Queue*‘de sıralanır. *Queue* ilk giren ilk çıkar mantığında(FIFO) çalışan bir mekanizmaya sahiptir. Kuyruktaki mesajları tüketen/alan kişi/uygulama ise *Consumer*‘dır. *Consumer* herhangi bir dille geliştirilebilir olduğundan dolayı yazılım dilinden bağımsız bir uygulamadır. 

### Exchange Nedir?

Publisher tarafından gönderilen mesajların nasıl yönetileceğini ve hangi route’lara yönlendirileceğini belirlememiz konusunda kontrol sağlayan/karar veren yapıdır. Route ise mesajların exchange üzerinden kuyruklara nasıl gönderileceğini tanımlayan mekanizmadır. Bu süreçte exchange’de bulunan routing key değeri kullanılır.

- **Binding Nedir?**
    
    Exchange ve Queue arasındaki ilişkiyi ifade eden yapıdır. Exchange ile kuyruk arasında bağlantı oluşturmaya verilen addır.
    
- **Exchange Türleri Nelerdir?**
    - ***Direct Exchange***     
        Publisher’ın göndereceği mesajı istediği consumer(lar)a iletilmesini sağlayan ve böylece hedef tüketiciye veriyi işleten bir işleyişe sahip exchange’tir. Bunun için mesaja bir Routing-Key değeri verilmekte ve bu routing-key değerine sahip tüm kuyruklara mesajın iletilmesi sağlanmaktadır. RabbitMQ’da default exchange türüdür.
        
    - ***Fanout Exchange***      
        Mesajların, bu exhange’e bind olmuş olan tüm kuyruklara gönderilmesini sağlar. Publisher mesajların gönderildiği kuyruk isimlerini dikkate almaz ve mesajları tüm kuyruklara gönderir.
        

    - ***Topic Exchange***      
        Topic Exchange’de atılan mesajların routing key değeri .(nokta) operatörü kullanılarak formatlandırılmakta ve bu formattaki isimlerde yapılan filtrelemelere göre uygun düşen kuyruklara mesajlar gönderilmektedir.
      
    - ***Header Exchange***      
        Routing yerine header(key-value)’ları kullanarak mesajları kuyruklara yönlendirmek için kullanılan exchange’dir.
        

## Basitçe Kuyruğa Mesaj Gönderme ve Okuma
[Basitçe Kuyruğa Mesaj Gönderme ve Okuma için](SimpleMessaging)

## Gelişmiş Kuyruk Mimarisi

Bir kuyruk mimarisinin gelişmiş olabilmesi için mesajların güvenli bir şekilde tutulup, eşit dağılımla tüketilmesi ve mesajın işlendiğine dair sunucunun haberdar edilmesi gerekmektedir.

#### 1. Message Acknowledgement (Mesaj Onaylama)
Tüketiciye gönderilen mesaj başarılı bir şekilde işlenmezse RabbitMQ mesajı tekrar yayınlar. Bu nedenle mesajın başarıyla işlendiği durumlarda, mesajın kuyruktan silinmesi için tüketiciden onay alınmalıdır. Aksi halde mesaj tekrar işlenir ve performans düşer.

- **BasicAck**: Mesaj işlendiğinde, `BasicAck` ile kuyruktan silinir.
- **BasicNack**: Mesaj işlenemediğinde, RabbitMQ'ya bilgi verilerek mesaj yeniden kuyruğa alınır veya reddedilir.
- **BasicReject**: Belirli bir mesajın işlenmesi reddedilir.

#### 2. Message Durability (Mesaj Dayanıklılığı)
RabbitMQ'da mesajlar kalıcı hale getirilmelidir. Bunun için kuyruklar `durable` olarak ayarlanır ve mesajlar fiziksel olarak kaydedilir. Böylece sunucu yeniden başlatılsa dahi mesajlar korunur. Ancak, bu yöntem bile %100 güvenlik sağlamaz.

#### 3. Fair Dispatch (Eşit Dağılım)
Birden fazla tüketici varsa mesajlar adil bir şekilde dağıtılmalıdır. RabbitMQ’da `BasicQos` metodu ile mesajların dağılım sırası ve miktarı belirlenebilir. Bu, daha güçlü donanımlara sahip tüketicilerin haksız yere daha fazla yük altında kalmasını engeller.

## Exchange Kullanımı
[Exchange Kullanımı için](Exchanges)

## Mesaj Tasarımları Nelerdir?
Mesaj tasarımlarından kastedilen, tıpkı Design Pattern’lar da olduğu gibi belli başlı senaryolara karşı gösterilebilecek önceden tanımlanmış, tarif edilmiş ve pratiksel olarak adımları saptanmış davranışlardır. Belirli bir problemi çözmek için kullanılan bu tasarımlar, genel anlamda yapısal davranışı ve iletişim modelini ifade etmektedir.

Yani, iki servis arasında **message broker** ile yapılacak haberleşme sürecinde iletilecek mesajların nasıl iletileceğini, nasıl işleneceğini, ne şekilde yapılandırılacağını ve ne tür bilgiler taşıyacağını belirler. Her tasarım farklı bir uygulama senaryosuna ve gereksinimine göre şekillenmekte ve en iyi sonuçlar alınabilecek şekilde yapılandırmaktır.

### Yaygın Mesaj Tasarımları Nelerdir?
- **P2P(Point-to-Point) Tasarımı**
        
  Bu tasarımda, bir publisher ilgili mesajı direkt bir kuyruğa gönderir ve bu mesaj kuyruğu işleyen bir consumer tarafından tüketilir. Eğer ki senaryo gereği bir mesajın bir tüketici tarafından işlenmesi gerekiyorsa bu yaklaşım kullanılır. P2P tasarımı gerektiren senaryolarda Direct Exchange kullanılmaktadır.
          
  ![p2p](https://github.com/user-attachments/assets/f3e75977-317d-410a-b380-6bbbaffdb011)


- **Publish/Subscribe (Pub/Sub) Tasarımı**
        
  Bu tasarımda publisher mesajı bir exchange’e gönderir ve böylece mesaj bu exchange’e bind edilmiş olan tüm kuyruklara yönlendirilir. Bu tasarım, bir mesajın birçok tüketici tarafından işlenmesi gerektiği durumlarda kullanılır. Publish/Subscribe tasarımı gerektiren senaryolarda genellikle Fanout Exchange kullanılmaktadır.
          
  💡Bu tasarımda `BasicQos` metodu üzerinden bir ölçeklendirme ayarı yapılabilir.
          
  ![pubsub](https://github.com/user-attachments/assets/32ae9a08-1cb3-48a8-b9c3-b430c0d05f01)

        
- **Work Queue (İş Kuyruğu) Tasarımı**
        
  Bu tasarımda, publisher tarafından yayınlanmış bir mesajın birden fazla consumer arasından yalnızca biri tarafından tüketilmesi amaçlanmaktadır. Böylece mesajların işlenmesi sürecinde tüm consumer’lar aynı iş yüküne ve eşit görev dağılımına sahip olacaktırlar. Work Queue tasarımı gerektiren senaryolarda genellikle Direct Exchange kullanılmaktadır.
          
  💡Tüm consumer’ların aynı iş yüküne ve eşit görev dağılımına sahip olabilmeleri için `BasicQos` metodu ile ölçeklendirme çalışması yapılmalı.
          
  ![workqueue](https://github.com/user-attachments/assets/eeda5911-3f48-495d-a069-9b71fb76bb73)

        
- **Request/Response Tasarımı**
        
  Bu tasarımda, publisher bir request yapar bir kuyruğa mesaj gönderir ve bu mesajı tüketen consumer’dan sonuca dair başka kuyruktan bir yanıt response bekler. Bu tarz senaryolar için oldukça uygun bir tasarımıdır. Bu tasarımda Publisher ve Consumer özünde hem publisher hem de consumer işlemlerini aynı anda yürütmektedirler.
          
  ![requestreponse](https://github.com/user-attachments/assets/c6097da6-7e6e-49eb-ab0d-9352a9609606)


## Enterprise Service Bus & MassTransit Nedir?
ESB, servisler arası entegrasyon sağlayan komponentlerin bütünüdür diyebiliriz. ESB, **RabbitMQ** gibi farklı sistemlerin birbirleriyle etkileşime girmesine sağlayan teknolojilerin kullanımını ve yönetilebilirliğini kolaylaştırmakta ve buna bir ortam sağlamaktadır.

ESB, servisler arası etkileşim süreçlerinde aracı uygulamalara karşın yüksek bir absraction(soyutlama) görevi görmekte ve böylece bütünsel olarak sistemin tek bir teknolojiye bağımlı olmasını engellemektir. Bu da, bu gün **RabbitMQ** teknolojisiyle tasarlanan bir sistemin yarın ihtiyaç doğrultusunda Kafka vs. gibi farklı bir message broker a geçişini kolaylaştırmaktadır.

ESB’nin temel amacı, farklı yazılım uygulamalarının/servislerinin/sistemlerinin birbirleriyle kolayca iletişim kurabilmesini sağlamaktır.

### MassTransit Nedir?
    
.NET için geliştirilmiş olan, distributed uygulamaları rahatlıkla yönetmeyi ve çalıştırmayı amaçlayan, ücretsiz open source bir **Enterprise Service BUS** framework’üdür.

Messaging tabanlı, gevşek bağlı(loosely coupled) ve asenkron olarak tasarlanmış dağınık sistemlerde yüksek dereceli kullanılabilirlik, güvenilirlik ve ölçeklenebilirlik sağlayabilmek için servisler oluşturmayı oldukça kolaylaştırmaktadır.
    
[MassTransit kullanımı için](MassTransit)

# EWP API'lerin C# Dilinde Hayata Geçirilmesi
## Echo API, Institutions API


Bu dokümantasyon EWP ağına nasıl bağlanılacağını ve EWP ağındaki üniverstelerle c# dilinde nasıl iletişim kurulacağının uygulamasını içerir. Doküman alt başlıkları şu şekildedir:

- EWP ağına nasıl bağlanılır?
- Registry Service nedir ve nasıl bağlanılır?
- HttpSig nedir ve EWP güvenlik standartları nasıl uygulanır?
- Genel anlamda Response nasıl oluşturulur?
-- API şemalarından C# sınıfları nasıl yaratılır? 
-- EWP hata yönetimi nasıl uygulanır?
- Echo API'nin uygulanması
- Institutions API'nin uygulanması

## Gereksinimler

- Framework 5.0
- Microsoft.AspNet.WebApi.Core Version=5.2.7 
- Microsoft.Extensions.Configuration Version=6.0.1 
- Microsoft.Extensions.Configuration.Binder Version=6.0.0 
- Microsoft.Extensions.Configuration.Json Version=6.0.0 
- Portable.BouncyCastle Version=1.9.0 
- Serilog.AspNetCore Version=4.1.0 
- Serilog.Extensions.Logging.File Version=2.0.0 
- Serilog.Settings.Configuration Version=3.3.0 
- Serilog.Sinks.Console Version=4.0.1 
- Serilog.Sinks.File Version=5.0.0 
- Swashbuckle.AspNetCore Version=6.3.0 
- 

## EWP ağına nasıl bağlanılır?

EWP ağına bağlanmak için öncelikle [şurada](https://wiki.uni-foundation.eu/display/EWP/Joining+via+inhouse+mobility+software) belirtilen işlemlerin tamamlanması gerekir. Bu adımlardan kısaca bahsetmek gerekirse, öncelikle bir işbirliği anlaşmasını imzalamanız bunu EUF'a iletmeniz gerekir, sonrasında sizden mail yoluyla manifesto dosyanızın linki istenir. Bu link üniversiteniz ağa dahil olduğunda [şu adreste](https://dev-registry.erasmuswithoutpaper.eu/status) yayınlanır ve Discovery API catalog dosyasında yer alır.

Bu işlemler sırasında teknik birimlerce oluşturulması gereken döküman manifesto dosyasıdır.

> Manifesto dosyası kurum hakkındaki temel iletişim bilgilerini, API servislerinin kullanacağı güvenlik protokollerini, 
>kurum tarafından oluşturulmuş public Key bilgisini, uygulanmış Api url bilgilerini içeren dosyadır. 

[Şu adresten](https://github.com/erasmus-without-paper/ewp-specs-api-discovery/blob/stable-v5/manifest-example.xml) örnek manifesto dosyasına erişebilirsiniz ya da diğer üniversitelerin manifesto dosyalarını [bu sayfadaki](https://dev-registry.erasmuswithoutpaper.eu/status) details alanlarından inceleyebilirsiniz.


Manifesto dosyası ile ilgili dikkat edilmesi gereken bazı hususlar vardır:
- Public Key [EWP dokümantasyonlarında](https://github.com/erasmus-without-paper/ewp-specs-api-discovery) belirtilen standartlarda oluşuturulmalıdır. Key oluşturmak için online tool da kullanabilirsiniz. Örn: [travistidwell](https://travistidwell.com/jsencrypt/demo/)
- Üniversiteniz, API servislerinden herhangi bir tanesinin url bilgisini değiştirdiğinde veya güvenlik protokollerinde güncelleme yaptığında manifesto dosyasını güncellemelidir. Manifesto dosyasında bir değişiklik olduğunda da [bu sayfadan](https://dev-registry.erasmuswithoutpaper.eu/status) yeniden yükleme yapılmalıdır. Bu işlemin yapılması discovery API'deki catalog dosyasının güncellenmesini sağlayacaktır.



## Registry Service nedir ve nasıl bağlanılır?

Discovery API, EWP için uygulanması gereken ilk API'dir.
Bu API manifesto dosyaları ile, sisteminizin hangi HEI'leri kapsadığını, hangi özellikleri (API'leri) uyguladığınızı ve sizinle iletişime geçecek üniversitelerin EWP Ağından veri alırken hangi kimlik bilgilerinizi kullanacağını duyurmaya hizmet eder. Yukarıda belirtildiği üzere manifest dosyasınızı hazırladığınızda bu API'yi uygulamış olursunuz.

Registery Service, EWP ağının içerisindeki tüm üniversitelerin public key ve uyguladıkları api'lere topluca ulaşabilmenizi sağlayan katalog dosyasını sunar. Kurumlar  katalog dosyasını  [bu  adresten](https://dev-registry.erasmuswithoutpaper.eu/catalogue-v1.xml) indirerek bağlanmak istedikleri diğer üniversitelerin bilgilerine ulaşabilirler.


Aşağıda Registry Service'ten catalog dosyasını çeken kod paylaşılmıştır:

```c
        public void DownloadCatalog()
        {
            string host = @"https://dev-registry.erasmuswithoutpaper.eu/catalogue-v1.xml"; 
            string strRegistryAPICatalog = "";

            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(host);
                Request.Headers.Add(HttpRequestHeader.Accept.ToString(), "application/xml");
                Request.UserAgent = "Registry Client Sample";
                Request.Method = "GET";
                HttpWebResponse resp = (HttpWebResponse)Request.GetResponse();


                Stream stream = resp.GetResponseStream();
                using (StreamReader reader = new StreamReader(stream))
                {
                    string lines = reader.ReadToEnd();
                    if (lines != null)
                    {
                        strRegistryAPICatalog = lines;
                        File.WriteAllText(Constants.CatalogFilePath, strRegistryAPICatalog, Encoding.UTF8);
                    }
                    reader.Close();
                }                
                stream.Close();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
        }
```


Katalog dosyası içerisinde diğer üniversitelerin bilgilerine ulaşmak için XPath sorguları kullanmanız önerilir. Örnek XPath sorguları [EWP registry service sayfasında](https://github.com/erasmus-without-paper/ewp-specs-api-registry/blob/stable-v1/README.md) verilmiştir.

Katalog dosyasından XPath sorgusuna göre veri çeken C# kodu aşağıdadır:

```c
public List<string> SearchForMultipleResult(string xPathExpression)
        {
            List<string> dataList = null;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Constants.Contsant.CatalogFilePath);
                var namespaceManager = new XmlNamespaceManager(doc.NameTable);
                namespaceManager.AddNamespace("r", "https://github.com/erasmus-without-paper/ewp-specs-api-registry/tree/stable-v1");
                XmlNode root = doc.DocumentElement;
                XmlNodeList nodeList = root.SelectNodes(xPathExpression, namespaceManager);

                if (nodeList == null) { }
                else
                {
                    dataList = new List<string>();
                    foreach (XmlNode node in nodeList)
                    {
                        dataList.Add(node.InnerText);
                    }
                }
            }
            catch
            {
            }

            return dataList;
        }
```

## HttpSig nedir ve EWP güvenlik standartları nasıl uygulanır?

Sunucu kendisine bir istek geldiğinde kimlik doğrulama yapmak için Http authentication mekanizmasını kullanır. Gelen bir isteğin hangi kimlik doğrulama protokolünü uyguladığı Authorization headerda belirtilir. Bu headerın ilk alanı bize protokol bilgisini verir.

Bu makalede ve örnek kodlarda kimlik doğrulama protokolü olarak Http Signature anlatılacaktır. HTTP Signature protokolü, istemcilerin HTTP isteklerini imzalamaları ilkesine dayanır. İmzalar ve imzayı oluşturan bileşenler isteğin Authorization veya Signature headerlarında yer alır. EWP, Authorization header bilgisinin mutlaka olması gerektiğini belirtir. Bknz : [httpsig sunucu uygulaması](https://github.com/erasmus-without-paper/ewp-specs-sec-srvauth-httpsig) ve [httpsig istemci uygulaması]( https://github.com/erasmus-without-paper/ewp-specs-sec-cliauth-httpsig); diğer bağlantı yöntemlerine [buradan](https://github.com/erasmus-without-paper?q=sec-&type=all&language=&sort=) ulaşabilirsiniz.

• Aşağıda örnek bir HTTP İmza Üstbilgisi bulunmaktadır:
````
Authorization: Signature
            keyId="MIICfzCCAeigAwIBAgIJ... // truncated",
            algorithm="rsa-sha256",
            headers="(request-target) host date digest",
            signature="GdUqDgy94Z8mSYUjr/rL6qrLX/jmudS... // truncated"
````
- Signature - Kimlik doğrulama protokolü olarak HttpSig uygulamanacağını belirtir, sonrasında virgül yoktur 
- keyId - istemcinin sertifikasına ulaşabilmenizi sağlayan olan anahtardır
- algorithm - İmza oluşturulurken kullanılacak dijital imza algoritmasıdır
- headers - İmzalanacak metni oluşturan başlıkların listesi (imzalanacak metin oluşturulurken bu alanda verilen sıralama kullanılmalıdır)
- signature - Yukarıdaki algoritma ve başlıklar alanından oluşturulan dijital imzadır

EWP ağında Httpsig protokolülü şu şekilde uygulanır: 
- Authorization header olup olmadığı kontrol edilir
-  authorization method Signature olup olmadığı kontrol edilir
-  keyId değeri olup olmadığı kontrol edilir. Eğer varsa keyId değeri EWP registry service'ten indirilen katalog dosyası üzerinde aratılır ve karşılık geldiği public key çekilir
-  algorithm alanının rsa-sha256'ya eşit olup olmadığı kontrol edilir
-  headers alanında yer alan headerların isteğin içerisinde olup olmadığı kontrol edilir ve eğer hepsi varsa belirtilen sıralamada imzalanacak metin oluşturulur
-  imzalanacak metin, public key ve algorithm bilgileri kullanılarak gönderilen signature doğrulanır

Buna ek olarak body kısmının da doğrulunu kontrol etmek gerekir. Bu da Digest header ile yapılır. Digest header içerisinde algoritma-token(hashlenmiş metin) eşlerini tutar. Algoritma isteğin body kısmının hangi yöntemle encode edileceğini belirtir, token ise encode edilmiş body'dir. Bkz: [Digest header standardı](https://tools.ietf.org/id/draft-ietf-httpbis-digest-headers-01.html)

• Aşağıda örnek bir HTTP Digest Üstbilgisi bulunmaktadır:
````
Digest: SHA-256=X48E9qOokqqrv...
````

Projenin içerisinde validatorlar HttpSig protokolünün uygulanmasını sağlar. Protokol için gerekli anahtarlar ve id'ler appsttings.json dosyası içerisinde tanımlanır. Aşağıda bir örnek paylaşılmıştır:
````
"HttpSig": {
    "CatalogFilePath": "EWP Files\\catalog.xml",
    "Servers": [
      {
        "HeiId": "iyte.edu.tr",
        "KeyId": "*** key id in catalog file ***",
        "PublicKey": [
          "**",
          "Insert private key lines as string array",
          "**",
          "**"
        ],
        "PrivateKey": [
          "**",
          "Insert private key lines as string array",
          "**",
          "**"
        ]
      }
    ]
  }
````


Tüm bunlara ek olarak EWP ağına bağlanacak API'lerin kontrol etmesi gereken başka header'lar da vardır. Bunlar hakkında bilgi almak için [şu sayfaya](https://github.com/erasmus-without-paper/ewp-specs-sec-srvauth-httpsig) bakabilirsiniz. 

Header bilgileri Filter içerisinde merkezi olarak kontrol edilir ve bu kısımda oluşan bir hata burada yönetilir.
Filter şu şekilde yazılır: 
```c
public class HttpContextFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {       
            try
            {
                string controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
                string actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;

                RequestValidator validator = new RequestValidatorFactory().GetValidator(controllerName);
                await validator.VerifyHttpSignatureRequest(context.HttpContext.Request);
                Log.Information("Filter worked successfully. Now [" + controllerName + "." + actionName + "] is executing");

                var result = await next();
            }
            catch (EwpSecWebApplicationException ex)
            {
                Log.Error(ex.getMessage(), ex);
                context.HttpContext.Response.StatusCode = (int)ex.getStatus();
                Dictionary<string, List<string>> errorParameters = new Dictionary<string, List<string>>();
                errorParameters.Add(Constants.ErrorParameters.DeveloperMessage.ToString(), new List<string>() { ex.getMessage() });                
                IResponse errorResponse = new ErrorResponseBuilder().Build(context.HttpContext.Request, context.HttpContext.Response, errorParameters);
                await errorResponse.WriteXmlBody(context.HttpContext.Response);
            }

        }
    }
```

Filter'ın çalışabilmesi için DI'ya tanımlanması gerekir:
```
        services.AddControllers(config =>
            {
                config.Filters.Add(new HttpContextFilter());
            });
```


Oluşturduğunuz yapıyı bir Echo API içerisinde test edebilirsiniz. Echo API url query string'de ya da body'de gelen echo parametrelerini ve istemcinin hei_id'lerini istemciye döndüren API servisidir. 
Yazdığınız servisi

- [dev-registry sayfası](https://dev-registry.erasmuswithoutpaper.eu/status) -> üniversitenizin adı -> validate -> gelen sayfada HHTT vb. işaretleyip validate 

tıklayarak test edebilirsiniz.

## Genel anlamda Response nasıl oluşturulur?

EWP ağındaki sunucular ister hata döndürsün ister data nesnesi döndürsün body kısmına xml formatında veri döndürür.

Controller içerisinde data nesnesi döndürme şu şekilde yapılır:
```c
        ResponseBuilder builder = _responseBuilderFactory.Create(ResponseBuilderFactory.ResponseBuilderType.<type>);
        IResponse reponseObject = builder.Build(Request, Response, queryParameters);
        await reponseObject.WriteXmlBody(Response);
        return Ok();
```


#### API şemalarından C# sınıfları nasıl yaratılır?

Yukarıda bir data nesnesinden otomatik olarak nasıl xml çıktısı üretiriz üzerinde durduk. Bu bölümde data nesnesinin nasıl oluşturulacağından bahsedeceğiz.
EWP tüm API'larında kullanacağı data nesnelerini şema(schema.xsd) dosyalarında tanımlıyor. Bu nedenle öncelikle kullanılacak data nesneleri sınıflarının schema.xsd dosyalarından türetilmesi gerekiyor. 
Xsd dosyasından sınıf yaratan bir çok online araç mevcut. Aşağıda birkaç tanesi listelenmiştir:

- https://www.liquid-technologies.com/online-xsd-to-cs-generator
- https://www.minify-beautify.com/convert-xsd-to-c-sharp-class-online
- http://xsd2xml.com/

EWP şema dosyaları, içerisinde başka şema dosyalarını da import ettiği ya da kendi veri yapıları tanımlarını içerdiği için herzaman düzgün şekilde sınıf yaratılamıyor. Bu gibi durumlarda öncelikle Xml dosyasına oradan da C# sınıfına dönüştürmek gerekebilir.

#### EWP hata yönetimi nasıl uygulanır?

EWP ağında hizmet veren API'ler hatalar için de Xml formatında veri döndürmelidir. Hatalarda döndürülecek [response-error tanımına](https://github.com/erasmus-without-paper/ewp-specs-architecture/blob/stable-v1/common-types.xsd) ve hata yönetimine ilişkin detaylı bilgiye [buradan](https://github.com/erasmus-without-paper/ewp-specs-architecture#error-handling) ulaşabilirsiniz. 

Hataları üretmenin yanında, hataların nerede ve nasıl üretildiği de önemlidir. Doğrulama işlemlerine ilişkin hataları ActionFilter'lar üretmek hem sorumlulukları dağıtmak hem de daha okunaklı kod üretmek açısından faydalı olacaktır. 
Aşağıda genel hatlarıyla bir filter örneği paylaşılmıştır:

```c
    public class HttpContextFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try{
                Validate();
                ..
                await next();
            }catch(EwpSecWebApplicationException ex){
                Log.Error(ex.getMessage(), ex);
                context.HttpContext.Response.StatusCode = (int)ex.getStatus();
                Dictionary<string, List<string>> errorParameters = new Dictionary<string, List<string>>();
                errorParameters.Add(Constants.ErrorParameters.DeveloperMessage.ToString(), new List<string>() { ex.getMessage() });                
                IResponse errorResponse = new ErrorResponseBuilder().Build(context.HttpContext.Request, context.HttpContext.Response, errorParameters);
                await errorResponse.WriteXmlBody(context.HttpContext.Response);
            }
        }
    }
```

## Echo API’nin uygulanması

Echo API istemci tarafından request query string'de ya da request body kısmında gönderilen echo parametreleri ile istemci hei_id'lerini dönen API'dır. Detaylı bilgiye [şuradan](https://github.com/erasmus-without-paper/ewp-specs-api-echo) ulaşabilirsiniz. 

Aşağıda ECHO API servisinin Get methodu paylaşılmıştır:
```c
        public IActionResult Get([FromQuery] List<string> echo)
        {
            ...
            ResponseBuilder builder = _responseBuilderFactory.Create(ResponseBuilderFactory.ResponseBuilderType.Echo);
            IResponse reponseObject = builder.Build(Request, Response, queryParameters);
            reponseObject.WriteXmlBody(Response);
            return Ok();

        }
```
Echo Api'ye projede şu şekilde ulaşabilirsiniz: 
```
https://localhost:<port>/api/Echo 
```
Echo Api'nin metodlarının hangi inputları aldığını ve nasıl bir output ürettiğini swagger sayfasında da detaylı bir şekilde bulabilirsiniz.:
```
https://localhost:<port>/swagger 
```

## Institutions API’nin uygulanması

Bir sunucu birden fazla üniversite(hei) için API servisi sunabilir. Institution API sunucunun hizmet verdiği üniversitelerden birinin hei_id bilgisini alarak, bu üniversiteye ait genel bilgileri döner. Detaylı bilgiye [şuradan](https://github.com/erasmus-without-paper/ewp-specs-api-institutions) ulaşabilirsiniz.
```c
        public async Task<IActionResult> Get([FromQuery] List<string> hei_id)
        {
            ...
            ResponseBuilder builder = _responseBuilderFactory.Create(ResponseBuilderFactory.ResponseBuilderType.Institutions);
            IResponse reponseObject = builder.Build(Request, Response, queryParameters);
            await reponseObject.WriteXmlBody(Response);
            return Ok();
        }
```
Institution Api'ye projede şu şekilde ulaşabilirsiniz: 
```
https://localhost:<port>/api/Institutions 
```
Institution Api'nin metodlarının hangi inputları aldığını ve nasıl bir output ürettiğini swagger sayfasında da detaylı bir şekilde bulabilirsiniz.:
```
https://localhost:<port>/swagger 
```

## IIA API’nin uygulanması
Inter Institutions Aggreeement (IIA) API kendi içerisinden 2 ayrı endpoint'a sahiptir.

### IIA Index API
Bu endpoint ilgili hei'ye ait ikili antlaşmaların id listesini döndürür. Bu liste partner ve akademik yıl parametreleriyle daraltılabilir. Detaylı bilgiye [şuradan](https://github.com/erasmus-without-paper/ewp-specs-api-iias/tree/stable-v6/endpoints) ulaşabilirsiniz.
IIA Index Api'ye projede şu şekilde ulaşabilirsiniz: 
```
https://localhost:<port>/api/IIA/Index 
```

### IIA Get API
Bu endpoint id'i ya da kodu verilen antlaşmaya ait detayları döndürür. Detaylı bilgiye [şuradan](https://github.com/erasmus-without-paper/ewp-specs-api-iias/tree/stable-v6/endpoints) ulaşabilirsiniz.
IIA Get Api'ye projede şu şekilde ulaşabilirsiniz: 
```
https://localhost:<port>/api/IIA/Get 
```
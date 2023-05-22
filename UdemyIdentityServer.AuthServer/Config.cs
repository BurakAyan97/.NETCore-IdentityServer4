using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace UdemyIdentityServer.AuthServer
{
    public static class Config
    {
        //Kullanacığımız apileri identityservera tanıtıyoruz.
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                //Scopeslarda hangi apinin hangi yetkilere sahip olacağını tanıtıyoruz.
                new ApiResource("resource_api1")//BasicAuth için ID değeri
                {
                    Scopes={"api1.read","api1.write","api1.update"},
                    ApiSecrets=new[]{new Secret("secretapi1".Sha256())}//BasicAuth için şifre değeri
                },
                new ApiResource("resource_api2")
                {
                    Scopes={"api2.read","api2.write","api2.update"},
                    ApiSecrets=new[]{new Secret("secretapi1".Sha256()) }
                }
            };
        }

        //Apilerde hangi aksiyonlar olacağını tanıtıyoruz.
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>()
            {
                new ApiScope("api1.read","API 1 için okuma izni"),
                new ApiScope("api1.write","API 1 için yazma izni"),
                new ApiScope("api1.update","API 1 için güncelleme izni"),

                new ApiScope("api2.read","API 2 için okuma izni"),
                new ApiScope("api2.write","API 2 için yazma izni"),
                new ApiScope("api2.update","API 2 için güncelleme izni"),
            };
        }

        //Hangi clientlar olacağını identityservera tanıtıyoruz
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                //Id ve secret bir nevi username password gibi çalışıyor clientları identityserver tanıyabilsin diye.
                new Client()
                {
                    ClientId="Client1",
                    ClientName="Client 1 app uygulaması",
                    ClientSecrets=new[] {new Secret("secret".Sha256())},
                    //Kullanıcı bilgisi taşımayacak sadece client-api bağlantısı için izinler olacak. 4 farklı izin tipi vardı. Bu token gerektirmeyen tek çeşidi.
                    AllowedGrantTypes=GrantTypes.ClientCredentials, 
                    //Hangi client hangi apide hangi yetkilere sahip olacak propertysi.
                    AllowedScopes=new[] {"api1.read"},

                },
                 new Client()
                {
                    ClientId="Client2",
                    ClientName="Client 2 app uygulaması",
                    ClientSecrets=new[] {new Secret("secret".Sha256())},
                    //Kullanıcı bilgisi taşımayacak sadece client-api bağlantısı için izinler olacak. 4 farklı izin tipi vardı. Bu token gerektirmeyen tek çeşidi.
                    AllowedGrantTypes=GrantTypes.ClientCredentials, 
                    //Hangi client hangi apide hangi yetkilere sahip olacak propertysi.
                    AllowedScopes=new[] {"api1.read","api1.update","api2.write","api2.update"},
                },
                 new Client()
                 {
                     ClientId="Client1-Mvc",
                     RequirePkce=false,//Client secretı güvenli bir şekilde tutuyoruz şuan.Mobil veya SPA olsa true.
                     ClientName="Client 1 app mvc uygulaması",
                     ClientSecrets=new[] {new Secret("secret".Sha256())},
                     AllowedGrantTypes=GrantTypes.Hybrid,//Code ve id_token istediğimiz için hybrid
                     RedirectUris=new List<string>{ "https://localhost:5006/signin-oidc" },//Token alma görevini gerçekleştiren url.İstek yapınca nereye döneceğini belirler
                     AllowedScopes={IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile}
                 }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            //OAuth 2.0 profile claims yazıp bu başlıkların neleri içerdiğine bakabilirsin.Ezberleme
            return new List<IdentityResource>()
            {
                //Üyelik sistemi varsa <orunludur.Kullanıcının ID'si anlamına gelir.Token kimin içindir bilmemiz lazım.
                new IdentityResources.OpenId(),
                //Kullanıcının hangi bilgilerine erişebilirsin muhabbeti.
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser()
                { SubjectId = "1", Username = "reposer", Password = "password",
                  Claims = new List<Claim>() {
                  new Claim("given_name", "Burak"),
                  new Claim("family_name", "Ayan") },
                },
                new TestUser()
                { SubjectId = "2", Username = "zafer61", Password = "password",
                  Claims = new List<Claim>() {
                  new Claim("given_name", "Zafer"),
                  new Claim("family_name", "Ayan") },
                },
            };
        }
    }
}

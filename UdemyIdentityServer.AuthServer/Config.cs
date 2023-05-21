using IdentityServer4.Models;
using System.Collections;
using System.Collections.Generic;

namespace UdemyIdentityServer.AuthServer
{
    public static class Config
    {
        //Kullanacığımız apileri identityservera tanıtıyoruz.
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("resource_api1")
                {Scopes={"api1.read","api1.write","api1.update"}},
                //Scopeslarda hangi apinin hangi yetkilere sahip olacağını tanıtıyoruz.
                new ApiResource("resource_api2")
                {Scopes={"api2.read","api2.write","api2.update"}},
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
                    AllowedScopes=new[] {"api1.read","api2.write","api2.update"},

                }
            };
        }
    }
}

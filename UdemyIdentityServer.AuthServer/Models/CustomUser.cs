namespace UdemyIdentityServer.AuthServer.Models
{
    //Custom üyelik sistemi konusu anlatırken oluşturuldu.Başlarda böyle bir class yok.
    public class CustomUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
    }
}

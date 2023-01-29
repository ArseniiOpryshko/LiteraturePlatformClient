namespace LiteraturePlatformClient.Models
{
    public class SendModel
    {
        public string title { get; set; }
        public string descr { get; set; }
        public int genreId { get; set; }
        public int userId { get; set; }
        public byte[] imageData { get; set; }
        public string text { get; set; }
    }
}

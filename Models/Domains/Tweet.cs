namespace TwitterClone.Models.Domains
{
    public class Tweet
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Content {  get; set; } = string.Empty;
        public int Likes { get; set; }
    }
}

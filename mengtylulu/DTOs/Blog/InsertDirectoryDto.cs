namespace mengtylulu.DTOs.Blog
{
    public class InsertDirectoryDto
    {
        //public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Introduction { get; set; } = string.Empty;

        public DateTime TimeCreate { get; set; } = DateTime.MinValue;

        public DateTime TimeUpdate { get; set; } = DateTime.MinValue;
    }
}

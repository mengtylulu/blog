namespace mengtylulu.Model.BlogApi
{
    public class BlogDirectory
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// </summary>
        public string title { get; set; } = string.Empty;

        /// <summary>
        /// 简介
        /// </summary>
        public string introduction { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime timecreate { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime time_update { get; set; }
    }
}

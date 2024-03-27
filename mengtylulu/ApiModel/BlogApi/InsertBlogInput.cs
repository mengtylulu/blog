using System.Net;

namespace mengtylulu.ApiModel.BlogApi
{
    public class InsertBlogInput
    {
        ///// <summary>
        ///// 主键
        ///// </summary>
        //public Guid BlogId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddress { get; set; } = string.Empty;

        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime CreateTime { get; set; }

        ///// <summary>
        ///// 更新时间
        ///// </summary>
        //public DateTime UpdateTime { get; set; }

        ///// <summary>
        ///// 类型主键
        ///// </summary>
        //public Guid TypeId { get; set; }
    }
}

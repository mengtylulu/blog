namespace mengtylulu.Data
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 获取所有值
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T?> GetByIdAsync(Guid id);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        Task<T> InsertAsync(T Entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        Task<T> UpdateAsync(T Entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> DeleteAsync(int id);
    }
}

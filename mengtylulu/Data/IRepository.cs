namespace mengtylulu.Data
{
    public interface IRepository<T>
    {
        /// <summary>
        /// 获取所有值
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAll();

        /// <summary>
        /// 通过id 获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetById(int id);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        Task<T> Create(T Entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        Task<T> Update(T Entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> Delete(int id);
    }
}

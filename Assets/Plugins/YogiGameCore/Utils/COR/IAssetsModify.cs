namespace YogiGameCore.Utils.COR
{
    public interface IAssetsModify<in T> where T : IContent
    {
        /// <summary>
        /// 资源入库
        /// </summary>
        /// <param name="content"></param>
        void HandleAssetIn(T content);
        /// <summary>
        /// 资源出库
        /// </summary>
        /// <param name="content"></param>
        void HandleAssetOut(T content);
        // void HandleAssetTransfer(T content);
    }
}
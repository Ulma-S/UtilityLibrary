using Object = UnityEngine.Object;

namespace RitsGameSeminar {
    public interface IResourceProvider {
        /// <summary>
        /// ファイル名とIDを指定して素材を登録する.
        /// </summary>
        /// <param name="id">素材ID</param>
        /// <param name="fileName">ファイル名</param>
        /// <typeparam name="T">素材の型</typeparam>
        void RegisterResource<T>(EResourceID id, string fileName) where T : Object;
        
        /// <summary>
        /// IDを指定して素材を読み込む.
        /// </summary>
        /// <param name="id">素材ID</param>
        /// <typeparam name="T">素材の型</typeparam>
        /// <returns></returns>
        T LoadResource<T>(EResourceID id) where T : Object;
    }
}
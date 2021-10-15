using System.Collections.Generic;
using UnityEngine;

namespace RitsGameSeminar {
    /// <summary>
    /// デバッグ用の動的リソース管理クラス.
    /// </summary>
    public class DebugResourceProvider : IResourceProvider {
        private readonly Dictionary<EResourceID, Object> m_cachedResourceMap = new Dictionary<EResourceID, Object>();

        public void RegisterResource<T>(EResourceID id, string fileName) where T : Object {
            T output = Resources.Load<T>(fileName);
            
            if (m_cachedResourceMap.ContainsKey(id)) {
                Debug.LogWarning(id + "に対応する素材は既に登録されています(上書き登録).");
                m_cachedResourceMap[id] = output;
                return;
            }
            m_cachedResourceMap.Add(id, output);
        }

        public T LoadResource<T>(EResourceID id) where T : Object {
            if (!m_cachedResourceMap.ContainsKey(id)) {
                Debug.LogError(id + "に対応する素材が登録されていません.");
                return null;
            }
            var output = m_cachedResourceMap[id] as T;
            return output;
        }
    }
}
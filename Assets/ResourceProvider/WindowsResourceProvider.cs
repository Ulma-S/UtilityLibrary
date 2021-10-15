using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RitsGameSeminar {
    /// <summary>
    /// Windows向けの動的リソース管理クラス.
    /// </summary>
    public class WindowsResourceProvider : IResourceProvider {
        private readonly Dictionary<string, Object> m_cachedResourceMap = new Dictionary<string, Object>();
        private readonly Dictionary<EResourceID, Object> m_cachedResourceIDMap = new Dictionary<EResourceID, Object>();

        public T LoadResource<T>(string fileName) where T : Object {
            if (m_cachedResourceMap.ContainsKey(fileName)) {
                var output = m_cachedResourceMap[fileName] as T;
                return output;
            }
            var assetName = fileName.ToLower();
            var assetPath = Path.Combine(Application.streamingAssetsPath, "Windows", assetName);
            var assetBundle = AssetBundle.LoadFromFile(assetPath);
            var assetData = assetBundle.LoadAsset<T>(assetName);
            m_cachedResourceMap.Add(fileName, assetData);
            return assetData;
        }
        
        public void RegisterResource<T>(EResourceID id, string fileName) where T : Object {
            var assetName = fileName.ToLower();
            var assetPath = Path.Combine(Application.streamingAssetsPath, "Windows", assetName);
            var assetBundle = AssetBundle.LoadFromFile(assetPath);
            var assetData = assetBundle.LoadAsset<T>(assetName);
            
            if (m_cachedResourceIDMap.ContainsKey(id)) {
                Debug.LogWarning(id + "に対応する素材は既に登録されています(上書き登録).");
                m_cachedResourceIDMap[id] = assetData;
                return;
            }
            m_cachedResourceIDMap.Add(id, assetData);
        }

        public T LoadResource<T>(EResourceID id) where T : Object {
            if (!m_cachedResourceIDMap.ContainsKey(id)) {
                Debug.LogError(id + "に対応する素材が登録されていません.");
                return null;
            }
            var output = m_cachedResourceIDMap[id] as T;
            return output;
        }
    }
}
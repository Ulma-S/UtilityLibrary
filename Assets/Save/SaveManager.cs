using System;
using System.IO;
using RitsGameSeminar.Save;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    /// <summary>
    /// セーブデータの管理機能を提供するクラス.
    /// </summary>
    public class SaveManager : SingletonMonoBehaviour<SaveManager> {
        public SaveData SaveData { get; private set; }
        private string m_saveDataPath = "/savadata.json";

        /// <summary>
        /// アプリケーションの一時停止時に自動セーブするか?
        /// </summary>
        public bool EnableSaveOnPause { get; set; } = true;
        
        /// <summary>
        /// アプリケーションの終了時に自動セーブするか?
        /// </summary>
        public bool EnableSaveOnQuit { get; set; } = true;

        /// <summary>
        /// 暗号化するか?
        /// </summary>
        public bool EnableEncryption { get; set; } = true;
        
        /// <summary>
        /// セーブ終了時のevent.
        /// </summary>
        public event Action OnSaveFinishedHandler;
        
        /// <summary>
        /// ロード終了時のevent.
        /// </summary>
        public event Action OnLoadFinishedHandler;
        
        /// <summary>
        /// セーブデータが作成された時のevent.
        /// </summary>
        public event Action OnSaveDataCreatedHandler;

        protected override void Awake() {
            base.Awake();

            m_saveDataPath = Application.persistentDataPath + m_saveDataPath;
        }

        public void Save(Action onSaveFinishedCallback = null) {
            var jsonData = JsonUtility.ToJson(SaveData);
            using (var streamWriter = new StreamWriter(m_saveDataPath, false)) {
                var result = streamWriter.WriteAsync(jsonData);
                while (true) {
                    //セーブが完了するまで待機.
                    if (result.IsCompleted) {
                        break;
                    }
                }
            }
            onSaveFinishedCallback?.Invoke();
            OnSaveFinishedHandler?.Invoke();        
        }

        public void Load(Action onLoadFinishedCallback = null) {
            using (var streamReader = new StreamReader(m_saveDataPath)) {
                string dataStr;
                var result = streamReader.ReadToEndAsync();
                while (true) {
                    //ロードが完了するまで待機.
                    if (result.IsCompleted) {
                        dataStr = result.Result;
                        break;
                    }
                }
                SaveData = JsonUtility.FromJson<SaveData>(dataStr);
            }
            onLoadFinishedCallback?.Invoke();
            OnLoadFinishedHandler?.Invoke();
        }

        private void OnApplicationPause(bool pauseStatus) {
            if (EnableSaveOnPause) {
                Save();
            }
        }

        private void OnApplicationQuit() {
            if (EnableSaveOnQuit) {
                Save();
            }
        }

        /// <summary>
        /// セーブデータが既に存在するか?
        /// </summary>
        /// <returns></returns>
        public bool ExistSaveData() {
            var result = File.Exists(m_saveDataPath);
            return result;
        }

        /// <summary>
        /// セーブデータを新規作成するメソッド.
        /// </summary>
        public void CreateSaveData(Action onSaveDataCreatedCallback = null) {
            SaveData = new SaveData();
            Save();
            onSaveDataCreatedCallback?.Invoke();
            OnSaveDataCreatedHandler?.Invoke();
        }
    }
}

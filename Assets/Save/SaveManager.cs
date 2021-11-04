using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using RitsGameSeminar.Save;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    /// <summary>
    /// セーブデータの管理機能を提供するクラス.
    /// </summary>
    public class SaveManager : SingletonMonoBehaviour<SaveManager> {
        public SaveData SaveData { get; private set; }
        private const string m_fileName = "/savedata";
        private string m_saveDataPath = string.Empty;
        private RijndaelManaged m_rijndael;
        
        /// <summary>
        /// ソルト値.
        /// 任意の文字列を設定してください.
        /// </summary>
        private const string m_salt = "saltstring";
        
        /// <summary>
        /// 暗号化に使用するパスフレーズ.
        /// </summary>
        private const string m_password = "password";

        private const int m_iterationCount = 1000;

        /// <summary>
        /// アプリケーションの一時停止時に自動セーブするか?
        /// </summary>
        [SerializeField] private bool m_enableSaveOnPause = true;
        
        /// <summary>
        /// アプリケーションの終了時に自動セーブするか?
        /// </summary>
        [SerializeField] private bool m_enableSaveOnQuit = true;

        /// <summary>
        /// 暗号化するか?
        /// </summary>
        [SerializeField] private bool m_enableEncryption = true;
        
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
            
            if (m_enableEncryption) {
                m_saveDataPath = Application.persistentDataPath + m_fileName;
            }
            else {
                m_saveDataPath = Application.persistentDataPath + m_fileName + ".json";
            }
            
            m_rijndael = new RijndaelManaged {
                KeySize = 128,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
            };
            //ソルト値をバイト列に変換.
            var byteSalt = Encoding.UTF8.GetBytes(m_salt);
            var deriveBytes = new Rfc2898DeriveBytes(m_password, byteSalt) {
                IterationCount = m_iterationCount,
            };
            m_rijndael.Key = deriveBytes.GetBytes(m_rijndael.KeySize / 8);
            m_rijndael.IV = deriveBytes.GetBytes(m_rijndael.BlockSize / 8);
        }

        /// <summary>
        /// セーブするメソッド.
        /// </summary>
        /// <param name="onSaveFinishedCallback">セーブ完了時のコールバック.</param>
        public void Save(Action onSaveFinishedCallback = null) {
            if (m_enableEncryption) {
                using (var memoryStream = new MemoryStream()) {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(memoryStream, SaveData);
                    var source = memoryStream.ToArray();
                    var encryptor = m_rijndael.CreateEncryptor();
                    var encrypted = encryptor.TransformFinalBlock(source, 0, source.Length);
                    encryptor.Dispose();
                    
                    using (var fileStream = new FileStream(m_saveDataPath, FileMode.Create, FileAccess.Write)) { 
                        fileStream.Write(encrypted, 0, encrypted.Length);
                    }
                }
            }
            else {
                var jsonData = JsonUtility.ToJson(SaveData);
                using (var streamWriter = new StreamWriter(m_saveDataPath, false)) {
                    streamWriter.Write(jsonData);
                }
            }
            onSaveFinishedCallback?.Invoke();
            OnSaveFinishedHandler?.Invoke();        
        }

        
        /// <summary>
        /// セーブデータをロードするメソッド.
        /// </summary>
        /// <param name="onLoadFinishedCallback">ロード完了時のコールバック.</param>
        public void Load(Action onLoadFinishedCallback = null) {
            if (m_enableEncryption) {
                using (var fileStream = new FileStream(m_saveDataPath, FileMode.Open, FileAccess.Read)) {
                    using (var memoryStream = new MemoryStream()) {
                        const int size = 4096;
                        var buffer = new byte[size];
                        int numBytes;
                        while ((numBytes = fileStream.Read(buffer, 0, size)) > 0) { 
                            memoryStream.Write(buffer, 0, numBytes);
                        } 
                        var source = memoryStream.ToArray(); 
                        var decryptor = m_rijndael.CreateDecryptor(); 
                        var plain = decryptor.TransformFinalBlock(source, 0, source.Length);

                        using (var plainStream = new MemoryStream(plain)) {
                            var formatter = new BinaryFormatter();
                            SaveData = formatter.Deserialize(plainStream) as SaveData;
                        }
                    }
                }
            }
            else {
                using (var streamReader = new StreamReader(m_saveDataPath)) {
                    var dataStr = String.Empty;
                    streamReader.ReadToEnd();
                    SaveData = JsonUtility.FromJson<SaveData>(dataStr);
                }
            }
            onLoadFinishedCallback?.Invoke();
            OnLoadFinishedHandler?.Invoke();
        }

        private void OnApplicationPause(bool pauseStatus) {
            if (m_enableSaveOnPause) {
                Save();
            }
        }

        private void OnApplicationQuit() {
            if (m_enableSaveOnQuit) {
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

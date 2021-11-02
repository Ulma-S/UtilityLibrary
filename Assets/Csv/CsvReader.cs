using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RitsGameSeminar {
    /// <summary>
    /// CSVファイルを読み込むクラス.
    /// </summary>
    public class CsvReader : Singleton<CsvReader> {
        private readonly Dictionary<EResourceID, List<string[]>> m_dictionary = new Dictionary<EResourceID, List<string[]>>();

        /// <summary>
        /// CSVファイルを読み込むメソッド.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public List<string[]> Load(string fileName) {
            var csvFile = Resources.Load<TextAsset>(fileName);
            var output = new List<string[]>();
            var reader = new StringReader(csvFile.text);
            while (reader.Peek() > -1) {
                var line = reader.ReadLine();
                output.Add(line.Split(','));
            }
            
            return output;
        }
        
        /// <summary>
        /// CSVファイルを読み込むメソッド.
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        public List<string[]> Load(EResourceID resourceID) {
            //既に登録されていたらキャッシュから返す.
            if (m_dictionary.ContainsKey(resourceID)) {
                return m_dictionary[resourceID];
            }

            var csvFile = ServiceLocator.Resolve<IResourceProvider>().LoadResource<TextAsset>(resourceID);
            var output = new List<string[]>();
            var reader = new StringReader(csvFile.text);
            while (reader.Peek() > -1) {
                var line = reader.ReadLine();
                output.Add(line.Split(','));
            }
            
            m_dictionary.Add(resourceID, output);

            return output;
        }
    }
}
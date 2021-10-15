using System;
using System.Collections.Generic;
using UnityEngine;

namespace RitsGameSeminar.SceneManagement {
    public class StageListProvider : SingletonMonoBehaviour<StageListProvider> {
        /// <summary>
        /// StageIDに対応するシーン名を管理するクラス.
        /// </summary>
        [Serializable]
        private class StageData {
            public EStageID StageID;
            public List<string> SceneList = new List<string>();
        }

        [SerializeField] private List<StageData> m_stageDatas = new List<StageData>();

        /// <summary>
        /// StageIDに対応するシーン名のリストを取得するメソッド.
        /// </summary>
        /// <param name="stageID"></param>
        /// <returns></returns>
        public IReadOnlyList<string> GetSceneNames(EStageID stageID) {
            var data = m_stageDatas.Find(value => value.StageID == stageID);
            if (data == null) {
                Debug.LogError(stageID + "に対応するシーンが登録されていません.");
                return null;
            }
            return data.SceneList;
        }
    }
}

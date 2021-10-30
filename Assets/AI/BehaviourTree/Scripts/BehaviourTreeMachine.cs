using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RitsGameSeminar.AI.BehaviourTree {
    /// <summary>
    /// Behaviour Treeのコア機能を提供するクラス.
    /// </summary>
    public class BehaviourTreeMachine {
        private Node m_rootNode;
        public readonly Dictionary<int, ENodeStatus> NodeStatusMap = new Dictionary<int, ENodeStatus>();
        
        private int m_currentID = 0;
        
        /// <summary>
        /// 処理を実行するメソッド.
        /// </summary>
        public void Execute() {
            //結果が返ってきたらリセットする.
            if (NodeStatusMap[m_rootNode.nodeID] == ENodeStatus.Success ||
                NodeStatusMap[m_rootNode.nodeID] == ENodeStatus.Failure) {
                Reset();
            }
            m_rootNode.Execute();
        }

        public int GetNextNodeID() {
            var nextID = m_currentID;
            m_currentID++;
            return nextID;
        }

        /// <summary>
        /// root nodeを設定するメソッド.
        /// </summary>
        /// <param name="rootNode"></param>
        public void SetRoot(Node rootNode) {
            m_rootNode = rootNode;
        }

        /// <summary>
        /// BT全体をリセットするメソッド.
        /// </summary>
        private void Reset() {
            foreach (var key in NodeStatusMap.Keys.ToList()) {
                NodeStatusMap[key] = ENodeStatus.Ready;
            }
            
            m_rootNode.Reset();
        }
    }
}
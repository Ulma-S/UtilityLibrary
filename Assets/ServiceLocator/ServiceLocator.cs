using System;
using System.Collections.Generic;
using UnityEngine;

namespace RitsGameSeminar {
    /// <summary>
    /// Service Locator
    /// 型に応じた実体の参照を提供するクラス.
    /// </summary>
    public static class ServiceLocator {
        /// <summary>
        /// 型に応じたインスタンスを保持する連想配列.
        /// </summary>
        private static readonly Dictionary<Type, object> m_instanceMap = new Dictionary<Type, object>();
        
        
        /// <summary>
        /// 型に応じたインスタンスを登録するメソッド.
        /// </summary>
        /// <param name="instance">インスタンス</param>
        /// <typeparam name="T">型</typeparam>
        public static void Register<T>(T instance) where T : class {
            if (m_instanceMap.ContainsKey(typeof(T))) {
                Debug.LogWarning(typeof(T) + "型のインスタンスは既に登録されています(上書き登録).");
                m_instanceMap[typeof(T)] = instance;
                return;
            }
            m_instanceMap.Add(typeof(T), instance);
        }

        
        /// <summary>
        /// 型に応じたインスタンスを取得するメソッド.
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <returns>インスタンス</returns>
        public static T Resolve<T>() where T : class {
            if (!m_instanceMap.ContainsKey(typeof(T))) {
                Debug.LogError(typeof(T) + "型のインスタンスは登録されていません.");
                return null;
            }

            T instance = m_instanceMap[typeof(T)] as T;
            return instance;
        }
    }
}
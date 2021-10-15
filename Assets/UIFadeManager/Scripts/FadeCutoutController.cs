using UnityEngine;
using UnityEngine.UI;

namespace RitsGameSeminar.UIFade {
    /// <summary>
    /// 画面のフェードを管理するクラス.
    /// </summary>
    public class FadeCutoutController : MonoBehaviour {
        [SerializeField] private RectTransform m_rectTransform;
        [SerializeField] private Image m_fadePanel;
        [SerializeField] public float Scale = 1f;
        private Material m_material;
        private static readonly int pScale = Shader.PropertyToID("_Scale");

        private void Start(){
            m_material = m_fadePanel.material;
            Scale = 3f;

            //16:9用に設定しています.
            //画面比率を変える際は適宜パラメータを変更してください.
            var aspect = Screen.height / (float) Screen.width;
            var rect = m_rectTransform.rect;
            if (aspect < 9f / 16f) {
                rect.width = Screen.width;
                rect.height = Screen.width * 9f / 16f;
            }
            else {
                rect.height = Screen.height;
                rect.width = Screen.height * 16f / 9f;
            }

            m_rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
        }

        private void Update(){
            //Shaderのプロパティを更新.
            m_material.SetFloat(pScale, Scale);
        }

        private void Reset(){
            m_rectTransform = GetComponent<RectTransform>();
            m_fadePanel = GetComponent<Image>();
        }
    }
}
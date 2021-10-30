using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace RitsGameSeminar.Sample {
    public class MainCameraController : MonoBehaviour {
        private void Start() {
            var camera = Camera.main.GetUniversalAdditionalCameraData();
            var uiCamera = FindObjectOfType<UICameraController>().GetComponent<Camera>();
            camera.cameraStack.Add(uiCamera);
        }
    }
}
using System.Collections;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class PlayerBulletMover : MonoBehaviour {
        [SerializeField] private Rigidbody m_rb;
        [SerializeField] private MeshRenderer m_renderer;
        [SerializeField] private Collider m_collider;
        private float m_bulletSpeed;
        private float m_bulletLife;

        private void Start() {
            m_bulletSpeed = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<PlayerStatus>(EResourceID.PlayerStatus).BulletSpeed;

            m_bulletLife = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<PlayerStatus>(EResourceID.PlayerStatus).BulletLife;
            
            Inactivate();
        }

        private IEnumerator SetLifeCoroutine() {
            yield return new WaitForSeconds(m_bulletLife);
            Inactivate();
        }

        public void Activate(Vector3 startPos, Vector3 dir) {
            m_renderer.enabled = true;
            m_collider.enabled = true;
            
            transform.position = startPos;
            m_rb.velocity = dir.normalized * m_bulletSpeed;

            StartCoroutine(SetLifeCoroutine());
        }

        private void Inactivate() {
            m_renderer.enabled = false;
            m_collider.enabled = false;
            
            m_rb.velocity = Vector3.zero;
        }

        private void Reset() {
            m_rb = GetComponent<Rigidbody>();
            m_renderer = GetComponent<MeshRenderer>();
            m_collider = GetComponent<Collider>();
        }
    }
}

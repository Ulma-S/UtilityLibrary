using System.Collections;
using UnityEngine;

namespace RitsGameSeminar.Sample {
    public class EnemyShellMover : MonoBehaviour {
        [SerializeField] private Rigidbody m_rb;
        [SerializeField] private MeshRenderer m_renderer;
        [SerializeField] private Collider m_collider;
        private float m_shellSpeed;
        private float m_shellLife;
        
        private void Start() {
            m_shellSpeed = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<EnemyStatus>(EResourceID.EnemyStatus).ShellSpeed;

            m_shellLife = ServiceLocator.Resolve<IResourceProvider>()
                .LoadResource<EnemyStatus>(EResourceID.EnemyStatus).ShellLife;
            
            Inactivate();
        }
        
        private IEnumerator SetLifeCoroutine() {
            yield return new WaitForSeconds(m_shellLife);
            Inactivate();
        }

        public void Activate(Vector3 startPos, Vector3 dir) {
            m_renderer.enabled = true;
            m_collider.enabled = true;
            
            transform.position = startPos;
            m_rb.velocity = dir.normalized * m_shellSpeed;

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
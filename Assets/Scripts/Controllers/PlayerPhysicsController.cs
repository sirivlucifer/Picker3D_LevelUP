using DG.Tweening;
using Enums;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private new Collider collider;
        [SerializeField] private new Rigidbody rigidbody;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("StageArea"))
            {
                CoreGameSignals.Instance.onStageAreaReached?.Invoke();
                InputSignals.Instance.onDisableInput?.Invoke();
                DOVirtual.DelayedCall(3, other.transform.parent.GetComponent<PoolController>().ControlSuccessCondition);
            }

            if (other.CompareTag("MiniGameArea"))
            {
                UISignals.Instance.onOpenPanel?.Invoke(UIPanels.MiniGamePanel);
            }

            if (other.CompareTag("WinZone"))
            {
                CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
            }

            if (other.CompareTag("Booster"))
            {
                
            }
        }

        public void SetPhysicsData()
        {
        }
    }
}
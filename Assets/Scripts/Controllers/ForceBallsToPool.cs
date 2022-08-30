using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Managers;
using Signals;
using UnityEngine;

namespace Controllers
{
    public class ForceBallsToPool : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        [Header("Data")] public PlayerThrowForceData ForceData;

        #endregion

        #region Serialized Variables

        [Space] [SerializeField] private PlayerManager manager;

        #endregion

        #endregion

        private void Awake()
        {
            GetReferences();
            ForceData = GetForceData();
        }

        private void GetReferences()
        {
            manager = FindObjectOfType<PlayerManager>();
        }

        private PlayerThrowForceData GetForceData() => Resources.Load<CD_Player>("Data/CD_Player").Data.ThrowForceData;

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onStageAreaReached += OnCastSphereAroundMagnet;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onStageAreaReached -= OnCastSphereAroundMagnet;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void OnCastSphereAroundMagnet()
        {
            var transform1 = manager.transform;
            var position = transform1.position;
            var forcePos = new Vector3(position.x, position.y, position.z);

            var collider = Physics.OverlapSphere(forcePos, 1.05f);

            var ballColliderList = new List<Collider>();

            foreach (var col in collider)
            {
                if (col.CompareTag("Ball")) ballColliderList.Add(col);
            }

            foreach (var ball in ballColliderList)
            {
                if (ball.GetComponent<Rigidbody>() == null) continue;
                Rigidbody rb;
                rb = ball.GetComponent<Rigidbody>();
                // rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
                // rb.AddForce(transform.up * 1.45f, ForceMode.Impulse);
                rb.AddForce(new Vector3(0, ForceData.ThrowForce.X, ForceData.ThrowForce.Y), ForceMode.Impulse);
            }

            ballColliderList.Clear();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var transform1 = manager.transform;
            var position = transform1.position;
            Gizmos.DrawSphere(new Vector3(position.x, position.y, position.z), 1.05f);
        }
    }
}
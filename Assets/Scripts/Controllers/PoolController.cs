using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using DG.Tweening;
using Managers;
using Signals;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class PoolController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        [Header("Data")] public StageData PoolData;

        #endregion

        #region Serialized Variables

        [SerializeField] private int stageID;
        [SerializeField] private TextMeshPro text;
        [SerializeField] private List<DOTweenAnimation> animationList;

        #endregion

        #region Private Variables

        private int _collectedCount;

        #endregion

        #endregion


        private void Awake()
        {
            PoolData = GetPoolData();
        }


        private void OnEnable()
        {
            SetRequiredAmountToText();
        }

        private StageData GetPoolData() => Resources.Load<CD_Level>("Data/CD_Level")
            .Levels[LevelManager.Instance.LevelID % Resources.Load<CD_Level>("Data/CD_Level")
                .Levels.Count].StageList[stageID];

        private void SetRequiredAmountToText()
        {
            text.text = $"0/{PoolData.RequiredObjectCount}";
        }

        private void UpdateRequiredTextCount()
        {
            text.text = $"{_collectedCount}/{PoolData.RequiredObjectCount}";
        }

        private void ActivateAllAnimation()
        {
            foreach (var animation in animationList)
            {
                animation.DOPlay();
            }
        }

        public void ControlSuccessCondition()
        {
            if (_collectedCount >= PoolData.RequiredObjectCount)
            {
                ActivateAllAnimation();
                DOVirtual.DelayedCall(2, () =>
                {
                    InputSignals.Instance.onEnableInput?.Invoke();
                    CoreGameSignals.Instance.onStageSuccessful?.Invoke();
                });
            }
            else
            {
                CoreGameSignals.Instance.onLevelFailed?.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Ball")) return;
            _collectedCount++;
            UpdateRequiredTextCount();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Ball")) return;
            _collectedCount--;
            UpdateRequiredTextCount();
        }
    }
}
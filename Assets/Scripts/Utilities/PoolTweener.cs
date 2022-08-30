using System;
using DG.Tweening;
using UnityEngine;

namespace Utilities
{
    public class PoolTweener : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private MeshRenderer renderer;

        #endregion

        #endregion

        public void TweenScale()
        {
            transform.DOScaleX(.35f, 1).SetRelative(true).SetEase(Ease.OutBounce);
        }

        public void TweenColor()
        {
            renderer.material.DOColor(Color.red, 1). SetEase(Ease.OutBounce);
        }
    }
}
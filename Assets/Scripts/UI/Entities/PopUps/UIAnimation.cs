using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Entities.PopUps
{
    public class UIAnimation
    {
        private readonly Transform _transform;
        private Tween _activeTween;

        public UIAnimation(Transform transform)
        {
            _transform = transform;
        }

        public void BouncyScaleAppear(float startScaleModifier, float appearDuration, float appearAmplitude,
            Action onComplete = null)
        {
            KillActiveTween();

            _transform.localScale = Vector3.one * startScaleModifier;
            
            _activeTween = _transform
                .DOScale(Vector3.one, appearDuration)
                .SetEase(Ease.OutElastic, appearAmplitude, 0)
                .OnComplete(() => onComplete?.Invoke());
        }

        public void ScaleDisappear(float disappearDuration, Action onComplete = null)
        {
            KillActiveTween();
            
            _activeTween = _transform
                .DOScale(Vector3.zero, disappearDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => onComplete?.Invoke());
        }

        private void KillActiveTween()
        {
            if (_activeTween is not null && _activeTween.active) 
                _activeTween.Kill();
        }
    }
}
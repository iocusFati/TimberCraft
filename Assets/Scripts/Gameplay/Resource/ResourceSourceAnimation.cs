using DG.Tweening;
using UnityEngine;

namespace Gameplay.Resource
{
    public class ResourceSourceAnimation
    {
        private readonly Animator _animator;
        
        private readonly int _appearId = Animator.StringToHash("Appear");
        
        private Transform _objectToAnimateTransform;
        private float _appearDuration;

        public ResourceSourceAnimation(Animator animator)
        {
            _animator = animator;
        }

        public void Appear()
        {
            _objectToAnimateTransform.localScale = Vector3.zero;
            
            _objectToAnimateTransform
                .DOScale(Vector3.one, _appearDuration)
        }
    }
}
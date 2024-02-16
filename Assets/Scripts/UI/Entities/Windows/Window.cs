using System;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.uiData;
using UI.Animation;
using UI.Entities.PopUps;
using UI.Factory;
using UnityEngine;
using Zenject;

namespace UI.Entities.Windows
{
    public class Window : MonoBehaviour, IUIEntity
    {
        [Header("Animation")]
        [SerializeField] private UIAppearAnimations _uiAppearAnimation = UIAppearAnimations.None;
        [SerializeField] private UIDisappearAnimation _uiDisappearAnimation = UIDisappearAnimation.None;

        private float _appearDuration;
        private float _disappearDuration;
        private float _startScaleModifier;
        private float _appearAmplitude;

        private UIAnimation _uiAnimation;

        [Inject]
        public void Construct(IStaticDataService staticData)
        {
            UIConfig uiConfig = staticData.UIConfig;
            
            _appearDuration = uiConfig.PopUpsAppearDuration;
            _disappearDuration = uiConfig.PopUpsDisappearDuration;
            _startScaleModifier = uiConfig.PopUpsStartScaleModifier;
            _appearAmplitude = uiConfig.PopUpsAppearAmplitude;

            _uiAnimation = new UIAnimation(transform);
        }
        
        public virtual void Show()
        {
            gameObject.SetActive(true);

            switch (_uiAppearAnimation)
            {
                case UIAppearAnimations.None:
                    break;
                case UIAppearAnimations.Bouncy:
                    _uiAnimation.BouncyScaleAppear(_startScaleModifier, _appearDuration, _appearAmplitude);                    
                    break;
            }
        }

        public virtual void Hide()
        {
            switch (_uiDisappearAnimation)
            {
                case UIDisappearAnimation.None:
                    gameObject.SetActive(false);
                    break;
                case UIDisappearAnimation.Scaling:
                    _uiAnimation.ScaleDisappear(_disappearDuration, () => gameObject.SetActive(false));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
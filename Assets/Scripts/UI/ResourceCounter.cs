using DG.Tweening;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.uiData;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay.Bots.StateMachine.States
{
    public class ResourceCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        private float _duration;
        private Vector3 _punchScale;

        public bool CanScale { get; private set; } = true;
        private RectTransform _rectTransform;

        [Inject]
        public void Construct(IStaticDataService staticDataService)
        {
            UIConfig uiConfig = staticDataService.UIConfig;
            _duration = uiConfig.ResourceCounterScaleDuration;
            _punchScale = uiConfig.ResourceCounterScaleModifier;
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        
        public void SetTextWithScaling(string text)
        {
            CanScale = false;
            
            SetText(text);
            ScaleCounter();
        }

        public void SetText(string text) => 
            _text.text = text;

        public void Deactivate() => 
            transform.parent.gameObject.SetActive(false);

        private void ScaleCounter()
        {
            _rectTransform
                .DOPunchScale(_punchScale, _duration)
                .OnComplete(() => CanScale = true);
        }
    }
}
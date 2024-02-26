using DG.Tweening;
using Infrastructure.Services.StaticDataService;
using Infrastructure.StaticData.uiData;
using TMPro;
using UnityEngine;
using Utils;
using Zenject;

namespace UI
{
    public class ResourceCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        private float _duration;
        private Vector3 _punchScale;

        private RectTransform _rectTransform;
        public bool CanScale { get; private set; } = true;

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
        
        public void SetCountWith(int count, bool doScaleAnimation = true, bool transformThousands = false)
        {
            string text = transformThousands 
                ? count.TransformThousands() 
                : count.ToString();
            
            SetText(text);

            if (doScaleAnimation && CanScale)
            {
                ScaleCounter();
                CanScale = false;
            }
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
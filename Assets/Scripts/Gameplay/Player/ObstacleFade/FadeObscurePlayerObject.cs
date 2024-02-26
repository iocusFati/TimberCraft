using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace Gameplay.Player.ObstacleFade
{
    public class FadeObscurePlayerObject : MonoBehaviour
    { 
        [SerializeField]
        private LayerMask LayerMask;
        [SerializeField]
        private float FadedAlpha = 0.33f;
        [SerializeField]
        private FadeMode FadingMode;
    
        [SerializeField]
        private float ChecksPerSecond = 10;
        [SerializeField]
        private int FadeFPS = 30;
        [SerializeField]
        private float FadeSpeed = 1;
    
        [Header("Read Only Data")]
        [SerializeField]
        private List<ObscurePlayerObject> ObjectsBlockingView = new List<ObscurePlayerObject>();
        // private List<int> IndexesToClear = new List<int>();
        
        private Camera _camera;
        private Player _player;
        private Transform _playerTransform;

        private Dictionary<ObscurePlayerObject, Coroutine> RunningCoroutines = new Dictionary<ObscurePlayerObject, Coroutine>();
    
        private readonly RaycastHit[] Hits = new RaycastHit[5];
        private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");

        [Inject]
        public void Construct(FactoriesHolderService factoriesHolder)
        {
            factoriesHolder.PlayerFactory.OnPlayerCreated += player =>
            {
                _player = player;
                _playerTransform = player.transform;
                
                StartCoroutine(CheckForObjects());
            };
        }
        
        private void Start()
        {
            _camera = Camera.main;
        }
    
        private IEnumerator CheckForObjects()
        {
            WaitForSeconds Wait = new WaitForSeconds(1f / ChecksPerSecond);
    
            while (true)
            {
                int hits = Physics.RaycastNonAlloc(_camera.transform.position,
                    (_playerTransform.transform.position - _camera.transform.position).normalized,
                    Hits,
                    Vector3.Distance(_camera.transform.position, _playerTransform.transform.position),
                    LayerMask);
                
                if (hits > 0)
                {
                    for (int i = 0; i < hits; i++)
                    {
                        ObscurePlayerObject fadingObject = GetFadingObjectFromHit(Hits[i]);
                        if (fadingObject != null && !ObjectsBlockingView.Contains(fadingObject))
                        {
                            if (RunningCoroutines.ContainsKey(fadingObject))
                            {
                                if (RunningCoroutines[fadingObject] != null) // may be null if it's already ended
                                {
                                    StopCoroutine(RunningCoroutines[fadingObject]);
                                }
    
                                RunningCoroutines.Remove(fadingObject);
                            }
    
                            RunningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectOut(fadingObject)));
                            ObjectsBlockingView.Add(fadingObject);
                        }
                    }
                }
    
                FadeObjectsNoLongerBeingHit();
    
                ClearHits();
    
                yield return Wait;
            }
        }
    
        private void FadeObjectsNoLongerBeingHit()
        {
            for (int i = 0; i < ObjectsBlockingView.Count; i++)
            {
                bool objectIsBeingHit = false;
                for (int j = 0; j < Hits.Length; j++)
                {
                    ObscurePlayerObject fadingObject = GetFadingObjectFromHit(Hits[j]);
                    if (fadingObject != null && fadingObject == ObjectsBlockingView[i])
                    {
                        objectIsBeingHit = true;
                        break;
                    }
                }
    
                if (!objectIsBeingHit)
                {
                    if (RunningCoroutines.ContainsKey(ObjectsBlockingView[i]))
                    {
                        if (RunningCoroutines[ObjectsBlockingView[i]] != null)
                        {
                            StopCoroutine(RunningCoroutines[ObjectsBlockingView[i]]);
                        }
                        RunningCoroutines.Remove(ObjectsBlockingView[i]);
                    }
    
                    RunningCoroutines.Add(ObjectsBlockingView[i], StartCoroutine(FadeObjectIn(ObjectsBlockingView[i])));
                    ObjectsBlockingView.RemoveAt(i);
                }
            }
        }
    
        private IEnumerator FadeObjectOut(ObscurePlayerObject FadingObject)
        {
            float waitTime = 1f / FadeFPS;
            WaitForSeconds Wait = new WaitForSeconds(waitTime);
            int ticks = 1;
    
            for (int i = 0; i < FadingObject.Materials.Count; i++)
            {
                FadingObject.Materials[i].SetInt(SrcBlend, (int)UnityEngine.Rendering.BlendMode.SrcAlpha); // affects both "Transparent" and "Fade" options
                FadingObject.Materials[i].SetInt(DstBlend, (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha); // affects both "Transparent" and "Fade" options
                FadingObject.Materials[i].SetInt(ZWrite, 0); // disable Z writing
                if (FadingMode == FadeMode.Fade)
                {
                    FadingObject.Materials[i].EnableKeyword("_ALPHABLEND_ON");
                }
                else
                {
                    FadingObject.Materials[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
                }
    
                FadingObject.Materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }
    
            if (FadingObject.Materials[0].HasProperty("_Color"))
            {
                while (FadingObject.Materials[0].color.a > FadedAlpha)
                {
                    for (int i = 0; i < FadingObject.Materials.Count; i++)
                    {
                        if (FadingObject.Materials[i].HasProperty("_Color"))
                        {
                            FadingObject.Materials[i].color = new Color(
                                FadingObject.Materials[i].color.r,
                                FadingObject.Materials[i].color.g,
                                FadingObject.Materials[i].color.b,
                                Mathf.Lerp(FadingObject.InitialAlpha, FadedAlpha, waitTime * ticks * FadeSpeed)
                            );
                        }
                    }
    
                    ticks++;
                    yield return Wait;
                }
            }
    
            if (RunningCoroutines.ContainsKey(FadingObject))
            {
                StopCoroutine(RunningCoroutines[FadingObject]);
                RunningCoroutines.Remove(FadingObject);
            }
        }
    
        private IEnumerator FadeObjectIn(ObscurePlayerObject FadingObject)
        {
            float waitTime = 1f / FadeFPS;
            WaitForSeconds Wait = new WaitForSeconds(waitTime);
            int ticks = 1;
    
            if (FadingObject.Materials[0].HasProperty("_Color"))
            {
                while (FadingObject.Materials[0].color.a < FadingObject.InitialAlpha)
                {
                    for (int i = 0; i < FadingObject.Materials.Count; i++)
                    {
                        if (FadingObject.Materials[i].HasProperty("_Color"))
                        {
                            FadingObject.Materials[i].color = new Color(
                                FadingObject.Materials[i].color.r,
                                FadingObject.Materials[i].color.g,
                                FadingObject.Materials[i].color.b,
                                Mathf.Lerp(FadedAlpha, FadingObject.InitialAlpha, waitTime * ticks * FadeSpeed)
                            );
                        }
                    }
    
                    ticks++;
                    yield return Wait;
                }
            }
    
            for (int i = 0; i < FadingObject.Materials.Count; i++)
            {
                if (FadingMode == FadeMode.Fade)
                {
                    FadingObject.Materials[i].DisableKeyword("_ALPHABLEND_ON");
                }
                else
                {
                    FadingObject.Materials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                }
                FadingObject.Materials[i].SetInt(SrcBlend, (int)UnityEngine.Rendering.BlendMode.One);
                FadingObject.Materials[i].SetInt(DstBlend, (int)UnityEngine.Rendering.BlendMode.Zero);
                FadingObject.Materials[i].SetInt(ZWrite, 1); // re-enable Z Writing
                FadingObject.Materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
            }
    
            if (RunningCoroutines.ContainsKey(FadingObject))
            {
                StopCoroutine(RunningCoroutines[FadingObject]);
                RunningCoroutines.Remove(FadingObject);
            }
        }
    
        private ObscurePlayerObject GetFadingObjectFromHit(RaycastHit Hit)
        {
            return Hit.collider != null ? Hit.collider.GetComponent<ObscurePlayerObject>() : null;
        }
    
        private void ClearHits()
        {
            RaycastHit hit = new RaycastHit();
            for (int i = 0; i < Hits.Length; i++)
            {
                Hits[i] = hit;
            }
        }
    
        public enum FadeMode
        {
            Transparent,
            Fade
        }
    }
}
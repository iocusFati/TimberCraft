using System;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.SaveLoad
{
    public class OnExitSave : MonoBehaviour
    {
        private ISaveLoadService _saveLoadService;

        [Inject]
        public void Construct(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        private void Awake() => 
            DontDestroyOnLoad(gameObject);

        private void OnApplicationQuit() => 
            _saveLoadService.SaveProgress();

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) 
                _saveLoadService.SaveProgress();
        }
    }
}
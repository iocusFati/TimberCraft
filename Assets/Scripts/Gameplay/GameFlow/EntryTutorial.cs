﻿using System.Collections.Generic;
using Gameplay.Environment.Buildings;
using Infrastructure.Data;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace Gameplay.GameFlow
{
    public class EntryTutorial : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private BuildingDiscover _buildingDiscover;
        [SerializeField] private MainHouse _mainHouse;
        
        [SerializeField] private List<GameObject> _showInTheBeginningObjects;
        [SerializeField] private List<GameObject> _showOnTutorialComplete;

        private bool _tutorialCompleted;

        [Inject]
        public void Construct(ISaveLoadService saveLoad)
        {
            saveLoad.Register(this);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _tutorialCompleted = progress.EntryTutorialWasCompleted;
            
            if (!_tutorialCompleted) 
                StartTutorial();
        }

        public void OnProgressCouldNotBeLoaded()
        {
            StartTutorial();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.EntryTutorialWasCompleted = _tutorialCompleted;
        }

        private void StartTutorial()
        {
            _buildingDiscover.UnlockWithKey(_mainHouse);
            _mainHouse.OnBuiltEvent += FinishTutorial;

            ShowTutorialCompleteObjects(false);
            ShowTutorialObjects();
        }

        private void FinishTutorial()
        {
            ShowTutorialCompleteObjects(true);

            _tutorialCompleted = true;
        }

        private void ShowTutorialCompleteObjects(bool show)
        {
            foreach (var objectToShow in _showOnTutorialComplete) 
                objectToShow.SetActive(show);
        }

        private void ShowTutorialObjects()
        {
            foreach (GameObject showInTheBeginningObject in _showInTheBeginningObjects) 
                showInTheBeginningObject.SetActive(true);
        }
    }
}
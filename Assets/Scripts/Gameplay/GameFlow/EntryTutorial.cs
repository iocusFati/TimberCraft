using System.Collections.Generic;
using Gameplay.Buildings;
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
        
        [SerializeField] private Transform _treesParent;
        [SerializeField] private GameObject _fence;

        [SerializeField] private List<GameObject> _showInTheBeginningObjects;

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
            
            _fence.SetActive(false);

            ShowTrees(false);
            ShowTutorialObjects();
        }

        private void FinishTutorial()
        {
            ShowTrees(true);
            
            _fence.SetActive(true);

            _tutorialCompleted = true;
        }

        private void ShowTutorialObjects()
        {
            foreach (GameObject showInTheBeginningObject in _showInTheBeginningObjects) 
                showInTheBeginningObject.SetActive(true);
        }

        private void ShowTrees(bool hide)
        {
            foreach (Transform tree in _treesParent) 
                tree.gameObject.SetActive(hide);
        }
    }
}
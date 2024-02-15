using System;
using System.Collections.Generic;
using Infrastructure.Data;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace Gameplay.Bots.StateMachine.States
{
    public class BuildingConstruction : ISavedProgress
    {
        private readonly string _id;

        private readonly int _maxNeededResourceQuantity;
        private readonly ResourceCounter _buildingResourceCounter;
        
        private int _currentlyNeededResourceQuantity;
        private int _currentStageIndex;
        
        private readonly List<(GameObject Stage, int ResourceQuantity)> _stagesAndResourcesNeeded;
        
        public bool IsBuilt { get; private set; }


        public BuildingConstruction(
            string id, 
            List<(GameObject Stage, int ResourceQuantity)> stagesAndResourcesNeeded, 
            ResourceCounter buildingResourceCounter)
        {
            _id = id;
            _stagesAndResourcesNeeded = stagesAndResourcesNeeded;
            _buildingResourceCounter = buildingResourceCounter;
            
            _currentlyNeededResourceQuantity = _maxNeededResourceQuantity 
                = stagesAndResourcesNeeded[^1].ResourceQuantity;

        }

        public void BuildWith(int resourceQuantity, Action onBuilt)
        {
            if (!_buildingResourceCounter.CanScale)
                return;
            
            _currentlyNeededResourceQuantity -= resourceQuantity;
            _buildingResourceCounter.SetTextWithScaling(_currentlyNeededResourceQuantity.ToString());

            if (IndexIsCorrect(_currentStageIndex + 1))
            {
                LevelUp();
            }

            if (_currentlyNeededResourceQuantity <= 0)
            {
                FinishConstruction();
                onBuilt.Invoke();
            }
        }

        public void OnProgressCouldNotBeLoaded()
        {
            _currentStageIndex = 0;
            
            DeactivateAllStages();
            SetStageActive(0);
            SetCurrentResourcesQuantityText();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _currentlyNeededResourceQuantity = 
                progress.BuildingsSaveData[_id].CurrentlyConstructionNeededResourceNumber;

            _currentStageIndex = GetCurrentStageIndex();
            
            DeactivateAllStages();
            SetStageActive(_currentStageIndex);

            if (IsConstructed()) 
                FinishConstruction();
            
            SetCurrentResourcesQuantityText();
        }
        
        private bool IsConstructed() => 
            _currentStageIndex == _stagesAndResourcesNeeded.Count - 1;

        public void UpdateProgress(PlayerProgress progress)
        {
            Dictionary<string, BuildingSaveData> buildingsSaveData = progress.BuildingsSaveData;

            progress.TryAddBuildingSaveDataPair(_id, new BuildingSaveData());
            
            buildingsSaveData[_id].CurrentlyConstructionNeededResourceNumber = _currentlyNeededResourceQuantity;
        }

        private void SetStageActive(int currentStageIndex)
        {
            _stagesAndResourcesNeeded[currentStageIndex].Stage.SetActive(true);
        }

        private void LevelUp()
        {
            _stagesAndResourcesNeeded[_currentStageIndex].Stage.SetActive(false);
            
            _currentStageIndex++;
            
            _stagesAndResourcesNeeded[_currentStageIndex].Stage.SetActive(true);
        }

        private void FinishConstruction()
        {
            _buildingResourceCounter.Deactivate();

            IsBuilt = true;
        }

        private int GetCurrentStageIndex()
        {
            for (int index = _stagesAndResourcesNeeded.Count - 1 ; index > 0; index--)
            {
                if (IndexIsCorrect(index)) 
                    return index;
            }

            return 0;
        }

        private bool IndexIsCorrect(int index)
        {
            int alreadySpentResources = _maxNeededResourceQuantity - _currentlyNeededResourceQuantity;
            
            return alreadySpentResources > _stagesAndResourcesNeeded[index].ResourceQuantity;
        }

        private void SetCurrentResourcesQuantityText() => 
            _buildingResourceCounter.SetText(_currentlyNeededResourceQuantity.ToString());

        private void DeactivateAllStages()
        {
            foreach (var stageAndResource in _stagesAndResourcesNeeded) 
                stageAndResource.Stage.SetActive(false);
        }
    }
}
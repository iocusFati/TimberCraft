using System;
using System.Collections.Generic;
using Infrastructure.Data;
using Infrastructure.Services.PersistentProgress;
using UI;
using UnityEngine;

namespace Gameplay.Buildings
{
    public class BuildingConstruction : ISavedProgress
    {
        private readonly string _id;

        private readonly int _maxNeededResourceQuantity;
        private readonly ResourceCounter _buildingResourceCounter;

        private int _currentStageIndex;
        private readonly bool _shouldSetCounter;

        private readonly List<(GameObject Stage, int ResourceQuantity)> _stagesAndResourcesNeeded;

        public bool IsBuilt { get; private set; }
        public int CurrentlyNeededResourceQuantity { get; private set; }
        public bool ResourcesEnough => CurrentlyNeededResourceQuantity <= 0;


        public BuildingConstruction(
            string id, 
            List<(GameObject Stage, int ResourceQuantity)> stagesAndResourcesNeeded, 
            ResourceCounter buildingResourceCounter)
        {
            _id = id;
            _stagesAndResourcesNeeded = stagesAndResourcesNeeded;
            _buildingResourceCounter = buildingResourceCounter;
            _shouldSetCounter = _buildingResourceCounter is not null;
            
            CurrentlyNeededResourceQuantity = _maxNeededResourceQuantity 
                = stagesAndResourcesNeeded[^1].ResourceQuantity;
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
            CurrentlyNeededResourceQuantity = 
                progress.BuildingsSaveData[_id].CurrentlyConstructionNeededResourceNumber;

            _currentStageIndex = GetCurrentStageIndex();
            
            DeactivateAllStages();
            SetStageActive(_currentStageIndex);

            if (IsConstructed()) 
                FinishConstruction();

            SetCurrentResourcesQuantityText();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            Dictionary<string, BuildingSaveData> buildingsSaveData = progress.BuildingsSaveData;

            progress.TryAddBuildingSaveDataPair(_id, new BuildingSaveData());
            
            buildingsSaveData[_id].CurrentlyConstructionNeededResourceNumber = CurrentlyNeededResourceQuantity;
        }

        public void Unlock()
        {
            OnProgressCouldNotBeLoaded();
        }

        public void PromiseToDeliverResources(int resourceQuantity)
        {
            CurrentlyNeededResourceQuantity -= resourceQuantity;
        }

        public void BuildWith(Action onBuilt)
        {
            if (_shouldSetCounter && !_buildingResourceCounter.CanScale)
                return;
            
            if (_shouldSetCounter) 
                _buildingResourceCounter.SetCountWith(CurrentlyNeededResourceQuantity);

            if (IndexIsCorrect(_currentStageIndex + 1))
            {
                LevelUp();
            }

            if (CurrentlyNeededResourceQuantity <= 0 && _currentStageIndex == _stagesAndResourcesNeeded.Count - 1)
            {
                FinishConstruction();
                onBuilt.Invoke();
            }
        }

        private bool IsConstructed() => 
            _currentStageIndex == _stagesAndResourcesNeeded.Count - 1;

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
            if (_shouldSetCounter) 
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
            int alreadySpentResources = _maxNeededResourceQuantity - CurrentlyNeededResourceQuantity;
            
            return alreadySpentResources >= _stagesAndResourcesNeeded[index].ResourceQuantity;
        }

        private void SetCurrentResourcesQuantityText()
        {
            if (_shouldSetCounter) 
                _buildingResourceCounter.SetText(CurrentlyNeededResourceQuantity.ToString());
        }

        private void DeactivateAllStages()
        {
            foreach (var stageAndResource in _stagesAndResourcesNeeded) 
                stageAndResource.Stage.SetActive(false);
        }
    }
}
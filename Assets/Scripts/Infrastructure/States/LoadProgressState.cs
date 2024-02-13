using Infrastructure.Data;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using Infrastructure.States.Interfaces;
using Zenject;

namespace Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private const string GameSceneName = "Game";
        
        private readonly IStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        
        public LoadProgressState(
            IStateMachine gameStateMachine, 
            IPersistentProgressService progressService,
            ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            
            _gameStateMachine.Enter<LoadLevelState, string>(GameSceneName);
        }

        public void Exit()
        {
            
        }

        private void LoadProgressOrInitNew()
        {
            if (_saveLoadService.LoadProgress() != null)
            {
                _progressService.Progress = _saveLoadService.LoadProgress();
                _progressService.Progress.WasLoaded = true;
            }
            else
            {
                _progressService.Progress = NewProgress();
            }
        }

        private PlayerProgress NewProgress() => new();
    }
}
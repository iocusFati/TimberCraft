using Infrastructure.States;
using Infrastructure.States.Interfaces;
using UI.Factory;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private float _resolutionFactor;
        
        private IStateMachine _gameStateMachine;
        private StatesFactory _statesFactory;
        private IUIFactory _uiFactory;

        [Inject]
        public void Construct(IStateMachine gameStateMachine, StatesFactory statesFactory, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _statesFactory = statesFactory;
            _uiFactory = uiFactory;
        }

        private void Start()
        {
            _uiFactory.CreateCurtain();
            RegisterStatesForStateMachine();
            SetupResolution();
        }

        private void RegisterStatesForStateMachine()
        {
            _gameStateMachine.RegisterState(_statesFactory.Create<LoadProgressState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<LoadLevelState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<GameLoopState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<GameLostState>());

            _gameStateMachine.Enter<LoadProgressState>();
        }

        public void SetupResolution()
        {
            int width = (int)(Screen.width * _resolutionFactor);
            int height = (int)(Screen.height * _resolutionFactor);
            
            Screen.SetResolution(width, height, true);
        }
    }
}
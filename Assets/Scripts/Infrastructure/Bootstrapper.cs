using Infrastructure.States;
using Infrastructure.States.Interfaces;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private float _resolutionFactor;
        
        private IStateMachine _gameStateMachine;
        private StatesFactory _statesFactory;

        [Inject]
        public void Construct(IStateMachine gameStateMachine, StatesFactory statesFactory)
        {
            _gameStateMachine = gameStateMachine;
            _statesFactory = statesFactory;
        }

        private void Start()
        {
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
using Infrastructure.States;
using Infrastructure.States.Interfaces;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
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
            _gameStateMachine.RegisterState(_statesFactory.Create<LoadProgressState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<LoadLevelState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<GameLoopState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<GameLostState>());

            _gameStateMachine.Enter<LoadProgressState>();
        }
    }
}
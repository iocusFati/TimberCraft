using Infrastructure.States.Interfaces;
using UI.Entities.Windows;
using UI.Factory;

namespace Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly IUIFactory _uiFactory;
        private HUD _hud;

        public GameLoopState(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            _hud = _uiFactory.CreateHUD();
        }
        
        public void Exit()
        {
            
        }
    }
}
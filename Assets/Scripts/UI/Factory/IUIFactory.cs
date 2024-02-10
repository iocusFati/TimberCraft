using Infrastructure.Services;
using UI.Entities.Windows;

namespace UI.Factory
{
    public interface IUIFactory : IService
    {
        void CreateGameUIRoot();
        HUD CreateHUD();
    }
}
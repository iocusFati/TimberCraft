using Infrastructure.Services;
using UI.Entities.HUD_Folder;

namespace UI.Factory
{
    public interface IUIFactory : IService
    {
        void CreateGameUIRoot();
        HUD CreateHUD();
    }
}
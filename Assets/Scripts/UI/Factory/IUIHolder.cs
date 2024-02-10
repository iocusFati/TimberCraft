using Infrastructure.Services;

namespace UI.Factory
{
    public interface IUIHolder : IService
    {
        TUIEntity Single<TUIEntity>() where TUIEntity : IUIEntity;
    }
}
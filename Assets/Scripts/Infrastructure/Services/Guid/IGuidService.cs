using UnityEngine;

namespace Infrastructure.Services.Guid
{
    public interface IGuidService : IService
    {
        string GetGuidFor(GameObject obj);
    }
}
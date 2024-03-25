using Cysharp.Threading.Tasks;

namespace Gameplay.Environment.BirdAI
{
    public interface ICustomTrigger
    {
        UniTask WaitForTriggerAsync();
    }
}
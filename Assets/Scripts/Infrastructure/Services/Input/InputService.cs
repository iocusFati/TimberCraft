using UnityEngine;

namespace Infrastructure.Services.Input
{
    public abstract class InputService : IInputService
    {
        public abstract bool Tap();

        public virtual Vector2 Axis =>
            new(SimpleInput.GetAxis("Horizontal"),
                SimpleInput.GetAxis("Vertical"));
    }
}
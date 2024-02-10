namespace Infrastructure.States.Interfaces
{
    public interface IPayloadedState<TPayload> : IExitState
    {
        public void Enter(TPayload payload);
    }
}
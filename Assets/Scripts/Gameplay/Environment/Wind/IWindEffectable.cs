namespace Gameplay.Environment.Wind
{
    public interface IWindEffectable
    {
        void GetBlownWith(Force force);
        void OnWindStopped();
    }
}
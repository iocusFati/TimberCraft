namespace Gameplay.Wind
{
    public interface IWindEffectable
    {
        void GetBlownWith(Force force);
        void OnWindStopped();
    }
}
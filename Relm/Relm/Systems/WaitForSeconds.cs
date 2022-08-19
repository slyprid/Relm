namespace Relm.Systems
{
    internal class WaitForSeconds
    {
        internal static WaitForSeconds Waiter = new WaitForSeconds();
        internal float WaitTime;

        internal WaitForSeconds Wait(float seconds)
        {
            Waiter.WaitTime = seconds;
            return Waiter;
        }
    }
}
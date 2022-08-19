namespace Relm.Systems
{
    public static class Coroutine
    {
        public static object WaitForSeconds(float seconds)
        {
            return Systems.WaitForSeconds.Waiter.Wait(seconds);
        }
    }
}
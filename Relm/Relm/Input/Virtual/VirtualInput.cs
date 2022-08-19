namespace Relm.Input
{
    public abstract class VirtualInput
    {
        protected VirtualInput()
        {
            RelmInput._virtualInputs.Add(this);
        }


        public void Deregister()
        {
            RelmInput._virtualInputs.Remove(this);
        }

        public abstract void Update();
    }
}
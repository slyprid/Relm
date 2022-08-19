namespace Relm.Components
{
    public interface IUpdateable
    {
        bool Enabled { get; }
        int UpdateOrder { get; }

        void Update();
    }
}
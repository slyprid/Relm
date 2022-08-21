namespace Relm.Persistence.Binary.DataStore.Interfaces
{
    public interface IPersistable
    {
        void Recover(IPersistableReader reader);
        void Persist(IPersistableWriter writer);
    }
}
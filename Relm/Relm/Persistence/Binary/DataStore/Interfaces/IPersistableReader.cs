namespace Relm.Persistence.Binary.DataStore.Interfaces
{
    public interface IPersistableReader : System.IDisposable
    {
        string ReadString();
        uint ReadUInt();
        int ReadInt();
        float ReadFloat();
        double ReadDouble();
        bool ReadBool();
    }
}
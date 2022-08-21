using System.IO;
using Relm.Persistence.Binary.DataStore.Interfaces;

namespace Relm.Persistence.Binary.DataStore.Implementations
{
    public class BinaryPersistableReader : BinaryReader, IPersistableReader
    {
        public BinaryPersistableReader(string filename) : base(File.OpenRead(filename))
        {
        }

        public BinaryPersistableReader(Stream input) : base(input)
        {
        }

        public uint ReadUInt() => ReadUInt32();

        public int ReadInt() => ReadInt32();

        public float ReadFloat() => ReadSingle();

        public bool ReadBool() => ReadBoolean();
    }
}
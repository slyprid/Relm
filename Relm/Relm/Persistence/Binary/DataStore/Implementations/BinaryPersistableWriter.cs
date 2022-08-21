using System.IO;
using Relm.Persistence.Binary.DataStore.Interfaces;

namespace Relm.Persistence.Binary.DataStore.Implementations
{
    public class BinaryPersistableWriter : BinaryWriter, IPersistableWriter
    {
        public BinaryPersistableWriter(string filename) : base(File.OpenWrite(filename))
        {
        }

        public BinaryPersistableWriter(Stream stream) : base(stream)
        {
        }

        public void Write(IPersistable value) => value.Persist(this);
    }
}
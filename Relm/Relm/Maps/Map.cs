using System;

namespace Relm.Maps
{
    public class Map
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Map()
        {
            Id = Guid.NewGuid();
        }
    }
}
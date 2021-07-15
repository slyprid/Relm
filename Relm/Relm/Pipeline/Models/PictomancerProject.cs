using System.Collections.Generic;
using Relm.Maps;

namespace Relm.Pipeline.Models
{
    public class PictomancerProject
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public Dictionary<string, Map> Maps { get; set; }


        public PictomancerProject()
        {
            Name = "Empty Project";
            Maps = new Dictionary<string, Map>();
        }

        public Map CreateNewMap()
        {
            var index = Maps.Count + 1;
            var name = $"Map {index}";
            var map = new Map
            {
                Name = name
            };
            Maps.Add(name, map);
            return map;
        }
    }
}
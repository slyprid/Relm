using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Relm.Content;
using Relm.Helpers;

namespace Relm.TimeElements
{
    public class TimeElementContentManager
    {
        private readonly ContentLibrary _contentLibrary;
        private readonly Dictionary<string, int> _partCounts = new Dictionary<string, int>();

        public Texture2D this[string partName] => _contentLibrary.Textures[partName];

        public TimeElementContentManager(ContentLibrary contentLibrary)
        {
            _contentLibrary = contentLibrary;
        }

        public void LoadContent()
        {
            TimeElementParts.ForEach(partName =>
            {
                var basePath = Path.Combine(Environment.CurrentDirectory, _contentLibrary.Content.RootDirectory, "gfx", "te");
                var partPath = Path.Combine(basePath, partName);

                if (Directory.Exists(partPath))
                {
                    var files = Directory.GetFiles(partPath);
                    var idx = 0;
                    foreach (var file in files)
                    {
                        var key = $"{partName}_{idx++}";
                        var texture = ContentHelper.LoadTextureFromFile(Path.Combine(partPath, file));
                        _contentLibrary.Textures.Add(key, texture);
                    }

                    _partCounts.Add(partName, idx);
                }
            });
        }

        public bool ContainsKey(string value)
        {
            return _contentLibrary.Textures.ContainsKey(value);
        }

        public int MaxIndex(string partName)
        {
            return _partCounts[partName];
        }
    }
}
using System;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Collections;

namespace Relm
{
    public class ContentLibrarySection<TKey, TValue>
        : KeyedCollection<TKey, TValue>
    {
        private readonly ContentManager _content;

        public ContentLibrarySection(Func<TValue, TKey> getKey, ContentManager content) : base(getKey)
        {
            _content = content;
        }

        public void Add(string assetName)
        {
            var asset = _content.Load<TValue>(assetName);
            Add(asset);
        }

        public void Add(string assetName, Func<TValue, TKey> setKey)
        {
            var asset = _content.Load<TValue>(assetName);
            setKey.Invoke(asset);
            Add(asset);
        }

        public TValue Get(TKey key)
        {
            TryGetValue(key, out var asset);
            return asset;
        }
    }
}

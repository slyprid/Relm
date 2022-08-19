using System;
using Microsoft.Xna.Framework.Content;
using Relm.Collections;

namespace Relm.Content
{
    public class ContentLibrarySection<TKey, TValue>
        : KeyedCollection<TKey, TValue>
    {
        private readonly ContentManager _content;
        private readonly Func<TValue, TKey, TValue> _setKey;

        public ContentLibrarySection(Func<TValue, TKey> getKey, Func<TValue, TKey, TValue> setKey, ContentManager content) : base(getKey)
        {
            _content = content;
            _setKey = setKey;
        }

        public ContentLibrarySection(Func<TValue, TKey> getKey, ContentManager content) : base(getKey)
        {
            _content = content;
        }

        public TValue Add(string assetName)
        {
            var asset = _content.Load<TValue>(assetName);
            Add(asset);
            return asset;
        }

        public TValue Add(TKey assetName, TValue asset)
        {
            _setKey?.Invoke(asset, assetName);
            Add(asset);
            return asset;
        }

        public TValue Add(string assetName, Func<TValue, TKey> setKey)
        {
            var asset = _content.Load<TValue>(assetName);
            setKey.Invoke(asset);
            Add(asset);
            return asset;
        }

        public TValue NoLoadAdd(string assetName, Func<TValue, TKey> setKey)
        {
            var asset = Activator.CreateInstance<TValue>();
            setKey.Invoke(asset);
            Add(asset);
            return asset;
        }

        public TValue Get(TKey key)
        {
            TryGetValue(key, out var asset);
            return asset;
        }
    }
}
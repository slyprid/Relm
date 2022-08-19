using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Relm.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Relm.Assets.BitmapFonts;
using Relm.Assets.Loaders;
using Relm.Assets.SpriteAtlases;
using Relm.Assets.SpriteAtlases.Loader;
using Relm.Assets.Tiled;
using Relm.Components.Renderables.Particles;
using Relm.Graphics.Effects;
using Relm.Graphics.Textures;

namespace Relm.Content
{
    public class RelmContentManager 
        : ContentManager
	{
		Dictionary<string, Effect> _loadedEffects = new Dictionary<string, Effect>();

		List<IDisposable> _disposableAssets;

		List<IDisposable> DisposableAssets
		{
			get
			{
				if (_disposableAssets == null)
				{
					var fieldInfo = Utils.GetFieldInfo(typeof(ContentManager), "disposableAssets");
					_disposableAssets = fieldInfo.GetValue(this) as List<IDisposable>;
				}

				return _disposableAssets;
			}
		}

#if FNA
		Dictionary<string, object> _loadedAssets;
		Dictionary<string, object> LoadedAssets
		{
			get
			{
				if (_loadedAssets == null)
				{
					var fieldInfo = ReflectionUtils.GetFieldInfo(typeof(ContentManager), "loadedAssets");
					_loadedAssets = fieldInfo.GetValue(this) as Dictionary<string, object>;
				}
				return _loadedAssets;
			}
		}
#endif


		public RelmContentManager(IServiceProvider serviceProvider, string rootDirectory) : base(serviceProvider, rootDirectory) { }

		public RelmContentManager(IServiceProvider serviceProvider) : base(serviceProvider) { }

		public RelmContentManager() : base(((Game)RelmGame._instance).Services, ((Game)RelmGame._instance).Content.RootDirectory) { }

		#region Strongly Typed Loaders

		public Texture2D LoadTexture(string name, bool premultiplyAlpha = false)
		{
			if (string.IsNullOrEmpty(Path.GetExtension(name)))
				return Load<Texture2D>(name);

			if (LoadedAssets.TryGetValue(name, out var asset))
			{
				if (asset is Texture2D tex)
					return tex;
			}

			using (var stream = Path.IsPathRooted(name) ? File.OpenRead(name) : TitleContainer.OpenStream(name))
			{
				var texture = premultiplyAlpha ? TextureUtils.TextureFromStreamPreMultiplied(stream) : Texture2D.FromStream(RelmGame.GraphicsDevice, stream);
				texture.Name = name;
				LoadedAssets[name] = texture;
				DisposableAssets.Add(texture);

				return texture;
			}
		}

		public SoundEffect LoadSoundEffect(string name)
		{
			if (string.IsNullOrEmpty(Path.GetExtension(name)))
				return Load<SoundEffect>(name);

			if (LoadedAssets.TryGetValue(name, out var asset))
			{
				if (asset is SoundEffect sfx)
				{
					return sfx;
				}
			}

			using (var stream = Path.IsPathRooted(name) ? File.OpenRead(name) : TitleContainer.OpenStream(name))
			{
				var sfx = SoundEffect.FromStream(stream);
				LoadedAssets[name] = sfx;
				DisposableAssets.Add(sfx);
				return sfx;
			}
		}

		public TiledMap LoadTiledMap(string name)
		{
			if (LoadedAssets.TryGetValue(name, out var asset))
			{
				if (asset is TiledMap map)
					return map;
			}

			var tiledMap = new TiledMap().LoadTiledMap(name);

			LoadedAssets[name] = tiledMap;
			DisposableAssets.Add(tiledMap);

			return tiledMap;
		}

		public ParticleEmitterConfig LoadParticleEmitterConfig(string name)
		{
			if (LoadedAssets.TryGetValue(name, out var asset))
			{
				if (asset is ParticleEmitterConfig config)
					return config;
			}

			var emitterConfig = ParticleEmitterConfigLoader.Load(name);

			LoadedAssets[name] = emitterConfig;
			DisposableAssets.Add(emitterConfig);

			return emitterConfig;
		}

		public SpriteAtlas LoadSpriteAtlas(string name, bool premultiplyAlpha = false)
		{
			if (LoadedAssets.TryGetValue(name, out var asset))
			{
				if (asset is SpriteAtlas spriteAtlas)
					return spriteAtlas;
			}

			var atlas = SpriteAtlasLoader.ParseSpriteAtlas(name, premultiplyAlpha);

			LoadedAssets.Add(name, atlas);
			DisposableAssets.Add(atlas);

			return atlas;
		}

		public BitmapFont LoadBitmapFont(string name, bool premultiplyAlpha = false)
		{
			if (LoadedAssets.TryGetValue(name, out var asset))
			{
				if (asset is BitmapFont bmFont)
					return bmFont;
			}

			var font = BitmapFontLoader.LoadFontFromFile(name, premultiplyAlpha);

			LoadedAssets.Add(name, font);
			DisposableAssets.Add(font);

			return font;
		}

		public Effect LoadEffect(string name) => LoadEffect<Effect>(name);

		public T LoadRelmEffect<T>() where T : Effect, new()
		{
			var cacheKey = typeof(T).Name + "-" + Utils.RandomString(5);
			var effect = new T();
			effect.Name = cacheKey;
			_loadedEffects[cacheKey] = effect;

			return effect;
		}

		public T LoadEffect<T>(string name) where T : Effect
		{
			// make sure the effect has the proper root directory
			if (!name.StartsWith(RootDirectory))
				name = RootDirectory + "/" + name;

			var bytes = EffectResource.GetFileResourceBytes(name);

			return LoadEffect<T>(name, bytes);
		}

		public T LoadEffect<T>(string name, byte[] effectCode) where T : Effect
		{
			var effect = Activator.CreateInstance(typeof(T), RelmGame.GraphicsDevice, effectCode) as T;
			effect.Name = name + "-" + Utils.RandomString(5);
			_loadedEffects[effect.Name] = effect;

			return effect;
		}

		public T LoadMonoGameEffect<T>() where T : Effect
		{
			var effect = Activator.CreateInstance(typeof(T), RelmGame.GraphicsDevice) as T;
			effect.Name = typeof(T).Name + "-" + Utils.RandomString(5);
			_loadedEffects[effect.Name] = effect;

			return effect;
		}

		#endregion

		public void LoadAsync<T>(string assetName, Action<T> onLoaded = null)
		{
			var syncContext = SynchronizationContext.Current;
			Task.Run(() =>
			{
				var asset = Load<T>(assetName);

				if (onLoaded != null)
				{
					syncContext.Post(d => { onLoaded(asset); }, null);
				}
			});
		}

		public void LoadAsync<T>(string assetName, Action<object, T> onLoaded = null, object context = null)
		{
			var syncContext = SynchronizationContext.Current;
			Task.Run(() =>
			{
				var asset = Load<T>(assetName);

				if (onLoaded != null)
				{
					syncContext.Post(d => { onLoaded(context, asset); }, null);
				}
			});
		}

		public void LoadAsync<T>(string[] assetNames, Action onLoaded = null)
		{
			var syncContext = SynchronizationContext.Current;
			Task.Run(() =>
			{
				for (var i = 0; i < assetNames.Length; i++)
					Load<T>(assetNames[i]);

				if (onLoaded != null)
				{
					syncContext.Post(d => { onLoaded(); }, null);
				}
			});
		}

		public void UnloadAsset<T>(string assetName) where T : class, IDisposable
		{
			if (IsAssetLoaded(assetName))
			{
				try
				{
					var assetToRemove = LoadedAssets[assetName];
					for (var i = 0; i < DisposableAssets.Count; i++)
					{
						var typedAsset = DisposableAssets[i] as T;
						if (typedAsset != null && typedAsset == assetToRemove)
						{
							typedAsset.Dispose();
							DisposableAssets.RemoveAt(i);
							break;
						}
					}

					LoadedAssets.Remove(assetName);
				}
				catch (Exception e)
				{
					Debug.Error("Could not unload asset {0}. {1}", assetName, e);
				}
			}
		}

		public bool UnloadEffect(string effectName)
		{
			if (_loadedEffects.ContainsKey(effectName))
			{
				_loadedEffects[effectName].Dispose();
				_loadedEffects.Remove(effectName);
				return true;
			}

			return false;
		}

		public bool UnloadEffect(Effect effect) => UnloadEffect(effect.Name);

		public bool IsAssetLoaded(string assetName) => LoadedAssets.ContainsKey(assetName);

		internal string LogLoadedAssets()
		{
			var builder = new StringBuilder();
			foreach (var asset in LoadedAssets.Keys)
				builder.AppendFormat("{0}: ({1})\n", asset, LoadedAssets[asset].GetType().Name);

			foreach (var asset in _loadedEffects.Keys)
				builder.AppendFormat("{0}: ({1})\n", asset, _loadedEffects[asset].GetType().Name);

			return builder.ToString();
		}

		public string GetPathForLoadedAsset(object asset)
		{
			if (LoadedAssets.ContainsValue(asset))
			{
				foreach (var kv in LoadedAssets)
				{
					if (kv.Value == asset)
						return kv.Key;
				}
			}

			return null;
		}

		public override void Unload()
		{
			base.Unload();

			foreach (var key in _loadedEffects.Keys)
				_loadedEffects[key].Dispose();

			_loadedEffects.Clear();
		}
	}

	sealed class RelmGlobalContentManager 
        : RelmContentManager
    {
        public RelmGlobalContentManager(IServiceProvider serviceProvider, string rootDirectory) : base(serviceProvider, rootDirectory) { }

        protected override Stream OpenStream(string assetName)
        {
            if (assetName.StartsWith("relm://"))
            {
                var assembly = GetType().Assembly;
                var ret = assembly.GetManifestResourceStream(assetName.Substring(7));
                return ret;
            }

            return base.OpenStream(assetName);
        }
    }
}
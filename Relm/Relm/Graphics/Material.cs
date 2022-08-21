using System;
using Microsoft.Xna.Framework.Graphics;
using Relm.Components;

namespace Relm.Graphics
{
	public class Material<T> 
        : Material, IDisposable 
        where T : Effect
    {
        public new T Effect
        {
            get => (T)base.Effect;
            set => base.Effect = value;
        }

        public Material()
        {
        }

        public Material(T effect) 
            : base(effect)
        {
        }
    }

	public class Material : IComparable<Material>, IDisposable
	{
		public static Material DefaultMaterial = new (BlendState.NonPremultiplied);

		public static Material DefaultOpaqueMaterial = new (BlendState.Opaque);

		public BlendState BlendState = BlendState.AlphaBlend;

		public DepthStencilState DepthStencilState = DepthStencilState.None;

		public SamplerState SamplerState = RelmGame.DefaultSamplerState;

		public Effect Effect;

		public Material() { }

        public Material(Effect effect)
        {
            Effect = effect;
        }

        public Material(BlendState blendState, Effect effect = null)
        {
            BlendState = blendState;
            Effect = effect;
        }

        public Material(DepthStencilState depthStencilState, Effect effect = null)
        {
            DepthStencilState = depthStencilState;
            Effect = effect;
        }

        ~Material()
        {
            Dispose();
        }

		public static Material StencilWrite(int stencilRef = 1)
		{
			return new Material
			{
				DepthStencilState = new DepthStencilState
				{
					StencilEnable = true,
					StencilFunction = CompareFunction.Always,
					StencilPass = StencilOperation.Replace,
					ReferenceStencil = stencilRef,
					DepthBufferEnable = false,
				}
			};
		}

		public static Material StencilRead(int stencilRef = 1)
		{
			return new Material
			{
				DepthStencilState = new DepthStencilState
				{
					StencilEnable = true,
					StencilFunction = CompareFunction.Equal,
					StencilPass = StencilOperation.Keep,
					ReferenceStencil = stencilRef,
					DepthBufferEnable = false
				}
			};
		}

		public static Material BlendDarken()
		{
			return new Material
			{
				BlendState = new BlendState
				{
					ColorSourceBlend = Blend.One,
					ColorDestinationBlend = Blend.One,
					ColorBlendFunction = BlendFunction.Min,
					AlphaSourceBlend = Blend.One,
					AlphaDestinationBlend = Blend.One,
					AlphaBlendFunction = BlendFunction.Min
				}
			};
		}

		public static Material BlendLighten()
		{
			return new Material
			{
				BlendState = new BlendState
				{
					ColorSourceBlend = Blend.One,
					ColorDestinationBlend = Blend.One,
					ColorBlendFunction = BlendFunction.Max,
					AlphaSourceBlend = Blend.One,
					AlphaDestinationBlend = Blend.One,
					AlphaBlendFunction = BlendFunction.Max
				}
			};
		}

		public static Material BlendScreen()
		{
			return new Material
			{
				BlendState = new BlendState
				{
					ColorSourceBlend = Blend.InverseDestinationColor,
					ColorDestinationBlend = Blend.One,
					ColorBlendFunction = BlendFunction.Add
				}
			};
		}

		public static Material BlendMultiply()
		{
			return new Material
			{
				BlendState = new BlendState
				{
					ColorSourceBlend = Blend.DestinationColor,
					ColorDestinationBlend = Blend.Zero,
					ColorBlendFunction = BlendFunction.Add,
					AlphaSourceBlend = Blend.DestinationAlpha,
					AlphaDestinationBlend = Blend.Zero,
					AlphaBlendFunction = BlendFunction.Add
				}
			};
		}
        
        public static Material BlendMultiply2x()
		{
			return new Material
			{
				BlendState = new BlendState
				{
					ColorSourceBlend = Blend.DestinationColor,
					ColorDestinationBlend = Blend.SourceColor,
					ColorBlendFunction = BlendFunction.Add
				}
			};
		}

		public static Material BlendLinearDodge()
		{
			return new Material
			{
				BlendState = new BlendState
				{
					ColorSourceBlend = Blend.One,
					ColorDestinationBlend = Blend.One,
					ColorBlendFunction = BlendFunction.Add
				}
			};
		}

		public static Material BlendLinearBurn()
		{
			return new Material
			{
				BlendState = new BlendState
				{
					ColorSourceBlend = Blend.One,
					ColorDestinationBlend = Blend.One,
					ColorBlendFunction = BlendFunction.ReverseSubtract
				}
			};
		}

		public static Material BlendDifference()
		{
			return new Material
			{
				BlendState = new BlendState
				{
					ColorSourceBlend = Blend.InverseDestinationColor,
					ColorDestinationBlend = Blend.InverseSourceColor,
					ColorBlendFunction = BlendFunction.Add
				}
			};
		}

		public static Material BlendSubtractive()
		{
			return new Material
			{
				BlendState = new BlendState
				{
					ColorSourceBlend = Blend.SourceAlpha,
					ColorDestinationBlend = Blend.One,
					ColorBlendFunction = BlendFunction.ReverseSubtract,
					AlphaSourceBlend = Blend.SourceAlpha,
					AlphaDestinationBlend = Blend.One,
					AlphaBlendFunction = BlendFunction.ReverseSubtract
				}
			};
		}

		public static Material BlendAdditive()
		{
			return new Material
			{
				BlendState = new BlendState
				{
					ColorSourceBlend = Blend.SourceAlpha,
					ColorDestinationBlend = Blend.One,
					AlphaSourceBlend = Blend.SourceAlpha,
					AlphaDestinationBlend = Blend.One
				}
			};
		}
		public virtual void Dispose()
		{
			if (BlendState != null && BlendState != BlendState.AlphaBlend)
			{
				BlendState.Dispose();
				BlendState = null;
			}

			if (DepthStencilState != null && DepthStencilState != DepthStencilState.None)
			{
				DepthStencilState.Dispose();
				DepthStencilState = null;
			}

			if (SamplerState != null && SamplerState != RelmGame.DefaultSamplerState)
			{
				SamplerState.Dispose();
				SamplerState = null;
			}

			if (Effect != null)
			{
				Effect.Dispose();
				Effect = null;
			}
		}

		public virtual void OnPreRender(Camera camera) { }

		public int CompareTo(Material other)
		{
			if (ReferenceEquals(other, null))
				return 1;

			if (ReferenceEquals(this, other))
				return 0;

			return -1;
		}

		public Material Clone()
		{
			return new Material
			{
				BlendState = BlendState,
				DepthStencilState = DepthStencilState,
				SamplerState = SamplerState,
				Effect = Effect
			};
		}
	}
}
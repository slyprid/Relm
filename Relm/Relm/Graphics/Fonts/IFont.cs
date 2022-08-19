using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Graphics.Fonts
{
	public interface IFont
    {
        float LineSpacing { get; }

        Vector2 MeasureString(string text);

        Vector2 MeasureString(StringBuilder text);

        bool HasCharacter(char c);

        void DrawInto(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth);

        void DrawInto(SpriteBatch spriteBatch, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth);
    }
}
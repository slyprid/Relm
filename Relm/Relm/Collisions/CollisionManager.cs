using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Relm.Extensions;

namespace Relm.Collisions
{
    public class CollisionManager
        : SimpleDrawableGameComponent
    {
        public bool ShowCollisionBounds { get; set; }
        public RelmGame Game { get; set; }

        public List<ICollider> Colliders { get; }
        public List<ICollisionActor> Actors { get; }

        public CollisionManager()
        {
            ShowCollisionBounds = false;
            Colliders = new List<ICollider>();
            Actors = new List<ICollisionActor>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var actor in Actors)
            {
                actor.IsColliding = false;
            }

            foreach (var collider in Colliders)
            {
                foreach (var actor in Actors)
                {
                    if (actor.CollisionBounds.Intersects(collider.Bounds))
                    {
                        actor?.OnCollision(collider);
                        actor.IsColliding = true;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (!ShowCollisionBounds) return;

            var spriteBatch = Game.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: RelmGame.Camera.GetViewMatrix());

            var texture = spriteBatch.GetWhitePixel();

            foreach (var collider in Colliders)
            {
                spriteBatch.Draw(texture, collider.Bounds, Color.Red.WithOpacity(0.5f));
            }

            foreach (var actor in Actors)
            {
                var color = actor.IsColliding ? Color.DarkRed : Color.Green;
                spriteBatch.Draw(texture, actor.CollisionBounds, color.WithOpacity(0.5f));
            }

            spriteBatch.End();
        }

        public void RegisterCollider(ICollider collider)
        {
            Colliders.Add(collider);
        }

        public void RegisterActor(ICollisionActor actor)
        {
            Actors.Add(actor);
        }

        public void Clear()
        {
            Colliders.Clear();
            Actors.Clear();
        }

        public bool IsCollidingWith(Rectangle bounds)
        {
            return Colliders.Any(collider => bounds.Intersects(collider.Bounds));
        }
    }
}
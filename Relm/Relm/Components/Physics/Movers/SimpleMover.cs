using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Relm.Components.Renderables.Sprites;
using Relm.Core;
using Relm.Extensions;
using Relm.Physics.Shapes;

namespace Relm.Components.Physics.Movers
{
    public class SimpleMover
        : Component, IUpdateable
    {
        float _speed = 600f;
        Mover _mover;
        SpriteRenderer _sprite;


        public override void OnAddedToEntity()
        {
            _sprite = this.GetComponent<SpriteRenderer>();
            _mover = new Mover();
            Entity.AddComponent(_mover);
        }


        void IUpdateable.Update()
        {
            var moveDir = Vector2.Zero;
            
            if (RelmInput.IsKeyDown(Keys.Left) || RelmInput.IsKeyDown(Keys.A) || RelmInput.Player1Controller.IsLeftStickLeft())
            {
                moveDir.X = -1f;
                if (_sprite != null) _sprite.FlipX = true;
            }
            else if (RelmInput.IsKeyDown(Keys.Right) || RelmInput.IsKeyDown(Keys.D) || RelmInput.Player1Controller.IsLeftStickRight())
            {
                moveDir.X = 1f;
                if (_sprite != null) _sprite.FlipX = false;
            }

            if (RelmInput.IsKeyDown(Keys.Up) || RelmInput.IsKeyDown(Keys.W) || RelmInput.Player1Controller.IsLeftStickUp())
            {
                moveDir.Y = -1f;
            }
            else if (RelmInput.IsKeyDown(Keys.Down) || RelmInput.IsKeyDown(Keys.S) || RelmInput.Player1Controller.IsLeftStickDown())
            {
                moveDir.Y = 1f;
            }


            if (moveDir != Vector2.Zero)
            {
                var movement = moveDir * _speed * Time.DeltaTime;

                if (_mover.Move(movement, out CollisionResult res))
                    Debug.DrawLine(Entity.Position, Entity.Position + res.Normal * 100, Color.Black, 0.3f);
            }
        }
    }
}
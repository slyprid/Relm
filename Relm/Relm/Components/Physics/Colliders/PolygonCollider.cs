using System;
using Microsoft.Xna.Framework;
using Relm.Graphics;
using Relm.Physics.Shapes;

namespace Relm.Components.Physics.Colliders
{
    public class PolygonCollider : Collider
    {
        public PolygonCollider(Vector2[] points)
        {
            var isPolygonClosed = points[0] == points[points.Length - 1];

            if (isPolygonClosed)
                Array.Resize(ref points, points.Length - 1);

            var center = Polygon.FindPolygonCenter(points);
            SetLocalOffset(center);
            Polygon.RecenterPolygonVerts(points);
            Shape = new Polygon(points);
        }

        public PolygonCollider(int vertCount, float radius)
        {
            Shape = new Polygon(vertCount, radius);
        }

        public PolygonCollider() : this(6, 40) { }

        public override void DebugRender(SpriteBatch spriteBatch)
        {
            var poly = Shape as Polygon;
            spriteBatch.DrawHollowRect(Bounds, Debug.Colors.ColliderBounds, Debug.Size.LineSizeMultiplier);
            spriteBatch.DrawPolygon(Shape.position, poly.Points, Debug.Colors.ColliderEdge, true, Debug.Size.LineSizeMultiplier);
            spriteBatch.DrawPixel(Entity.Transform.Position, Debug.Colors.ColliderPosition, 4 * Debug.Size.LineSizeMultiplier);
            spriteBatch.DrawPixel(Shape.position, Debug.Colors.ColliderCenter, 2 * Debug.Size.LineSizeMultiplier);

            // Normal debug code
            //for( var i = 0; i < poly.points.Length; i++ )
            //{
            //	Vector2 p2;
            //	var p1 = poly.points[i];
            //	if( i + 1 >= poly.points.Length )
            //		p2 = poly.points[0];
            //	else
            //		p2 = poly.points[i + 1];
            //	var perp = Vector2Ext.perpendicular( ref p1, ref p2 );
            //	Vector2Ext.normalize( ref perp );
            //	var mp = Vector2.Lerp( p1, p2, 0.5f ) + poly.position;
            //	batcher.drawLine( mp, mp + perp * 10, Color.White );
            //}
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game2
{
    public static class SpriteBatchExtensions
    {
        private static Texture2D _texture;
        
        //    create 1x1 texture for line drawing
        private static Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (_texture != null) 
                return _texture;
            
            _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _texture.SetData(new[] { Color.White });

            return _texture;
        }
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }
        public static void DrawLine(this SpriteBatch spriteBatch, Vector3 point1, Vector3 point2, Color color, float thickness)
        {
            Vector2 vector = new Vector2();
            Vector2 vector2 = new Vector2();
            vector.X = point1.X;
            vector.Y = point1.Y;
            vector2.X = point2.X;
            vector2.Y = point2.Y;
            DrawLine(spriteBatch, vector, vector2, color, thickness);
        }
    }
}

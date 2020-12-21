using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2.Figures
{
    public class Line
    {
        private Vector3 _first;
        private Vector3 _second;
        public Color Color { get; set; } = Color.Black;
        public float Thickness { get; set; } = 1;

        public Line(Vector3 first, Vector3 second)
        {
            _first = first;
            _second = second;
        }

        public void ApplyTransform(Transform transform)
        {
            _first = transform.Multiply(_first);
            _second = transform.Multiply(_second);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawLine(_first, _second, Color, Thickness);
        }
    }
}
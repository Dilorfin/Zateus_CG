using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game2.Figures
{
    public class Circle
    {
        private readonly Vector3[] _vectors;

        private float _radius;
        public float Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                Build();
            }
        }

        public Vector3 Center { get; private set; }

        public Circle(float radius, Vector3 center, int numberOfPoints = 100)
        {
            _vectors = new Vector3[numberOfPoints];

            _radius = radius;
            Center = center;

            Build();
        }

        public void ApplyTransform(Transform transform)
        {
            for (int i = 0; i < _vectors.Length; i++)
            {
                _vectors[i] = transform.Multiply(_vectors[i]);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _vectors.Length - 1; i++)
            {
                spriteBatch.DrawLine(_vectors[i], _vectors[i + 1], Color.Black, 2f);
            }

            spriteBatch.DrawLine(_vectors[^1], _vectors[0], Color.Black, 2f);
        }

        private void Build()
        {
            for (int i = 0; i < _vectors.Length; i++)
            {
                _vectors[i].X = (float)(Math.Cos(Math.PI * (2f * i / _vectors.Length - 0.5f)) * Radius + Center.X);
                _vectors[i].Y = (float)(Math.Sin(Math.PI * (2f * i / _vectors.Length - 0.5f)) * Radius + Center.Y);
            }
        }
    }
}

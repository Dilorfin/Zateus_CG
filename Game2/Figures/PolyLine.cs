using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2.Figures
{
    public class PolyLine
    {
        private readonly Vector3[] _vectors;
        
        public bool IsEnclosed { get; set; }
        
        public PolyLine(int numberOfPoints)
        {
            _vectors = new Vector3[numberOfPoints];
        }
        public PolyLine(Vector3[] vectors)
        {
            this._vectors = vectors;
        }
        
        public Vector3 this[int number]
        {
            get => _vectors[number];
            set => _vectors[number] = value;
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
            for (int i = 0; i < _vectors.Length-1; i++)
            {
                spriteBatch.DrawLine(_vectors[i], _vectors[i + 1], Color.Black, 2f);
            }
            if (IsEnclosed)
            {
                spriteBatch.DrawLine(_vectors[^1], _vectors[0], Color.Black, 2f);
            }
        }
    }
}

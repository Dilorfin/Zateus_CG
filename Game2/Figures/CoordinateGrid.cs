using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2.Figures
{
    public class CoordinateGrid
    {
        public Vector3 Center { get; }
        public float Unit { get; }

        public int Width { get; }
        public int Height { get; }

        private readonly Line[] _axises = new Line[6];
        private readonly Line[] _letters = new Line[5];

        private Line[] _gridVertical;
        private Line[] _gridHorizontal;

        private readonly Color _gridColor = Color.Gray;
        private readonly Color _arrowsColor = Color.Blue;

        public CoordinateGrid(int width, int height, float unit)
        {
            Center = new Vector3(unit * (int)((width / unit) / 2), unit * (int)((height / unit) / 2), 0);
            Unit = unit;

            Width = width;
            Height = height;
            
            Reset();
        }

        public void ApplyTransform(Transform transform)
        {
            foreach (var line in _gridVertical)
            {
                line.ApplyTransform(transform);
            }
            foreach (var line in _gridHorizontal)
            {
                line.ApplyTransform(transform);
            }
            foreach (var line in _axises)
            {
                line.ApplyTransform(transform);
            }
            foreach (var line in _letters)
            {
                line.ApplyTransform(transform);
            }

        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var line in _gridVertical)
            {
                line.Draw(batch);
            }
            foreach (var line in _gridHorizontal)
            {
                line.Draw(batch);
            }
            foreach (var line in _axises)
            {
                line.Draw(batch);
            }
            foreach (var line in _letters)
            {
                line.Draw(batch);
            }
        }

        public void Reset()
        {
            CreateGrid();
            CreateArrows();
            CreateLetters();
        }
        
        private void CreateArrows()
        {
            Vector3[] xAxis = new Vector3[4];
            Vector3[] yAxis = new Vector3[4];
            
            float offset = Unit/2;
            
            xAxis[0].Y = Center.Y;
            yAxis[0].X = Center.X;

            xAxis[1].X = Width;
            xAxis[1].Y = Center.Y;

            yAxis[1].Y = Height;
            yAxis[1].X = Center.X;

            xAxis[2].X = Width - offset;
            xAxis[2].Y = Center.Y - offset / 2;
            xAxis[3].X = Width - offset;
            xAxis[3].Y = Center.Y + offset / 2;

            yAxis[2].X = Center.X - offset / 2;
            yAxis[2].Y = Height - offset;
            yAxis[3].X = Center.X + offset / 2;
            yAxis[3].Y = Height - offset;
            
            _axises[0] = new Line(xAxis[0], xAxis[1]);
            _axises[1] = new Line(xAxis[1], xAxis[2]);
            _axises[2] = new Line(xAxis[1], xAxis[3]);

            _axises[3] = new Line(yAxis[0], yAxis[1]);
            _axises[4] = new Line(yAxis[1], yAxis[2]);
            _axises[5] = new Line(yAxis[1], yAxis[3]);

            foreach (var line in _axises)
            {
                line.Thickness = 2;
                line.Color = _arrowsColor;
            }
        }
        private void CreateLetters()
        {
            Vector3[] xLetter = new Vector3[4];
            Vector3[] yLetter = new Vector3[4];

            float offset = Unit / 2;

            xLetter[0].X = Width - Unit * 2;
            xLetter[0].Y = Center.Y - offset * 3;
            xLetter[1].X = Width - Unit;
            xLetter[1].Y = Center.Y - offset;
            xLetter[2].X = Width - Unit;
            xLetter[2].Y = Center.Y - offset * 3;
            xLetter[3].X = Width - Unit * 2;
            xLetter[3].Y = Center.Y - offset;

            yLetter[0].X = Center.X + Unit;
            yLetter[0].Y = Height;
            yLetter[1].X = Center.X + Unit;
            yLetter[1].Y = Height - offset;
            yLetter[2].X = Center.X + offset;
            yLetter[2].Y = Height - Unit;
            yLetter[3].X = Center.X + Unit + offset;
            yLetter[3].Y = Height - Unit;

            _letters[0] = new Line(xLetter[0], xLetter[1]);
            _letters[1] = new Line(xLetter[2], xLetter[3]);

            _letters[2] = new Line(yLetter[0], yLetter[1]);
            _letters[3] = new Line(yLetter[1], yLetter[2]);
            _letters[4] = new Line(yLetter[1], yLetter[3]);
            foreach (var line in _letters)
            {
                line.Thickness = 2;
                line.Color = _arrowsColor;
            }
        }
        private void CreateGrid()
        {
            _gridVertical = new Line[(int)(Width / Unit) + 1];
            for (int i = 0; i < _gridVertical.Length; i++)
            {
                var X = i * Unit;
                _gridVertical[i] = new Line(new Vector3(X, 0, 0), new Vector3(X, Height, 0))
                {
                    Color = _gridColor
                };
            }
            _gridHorizontal = new Line[(int)(Height / Unit) + 1];
            for (int i = 0; i < _gridHorizontal.Length; i++)
            {
                var Y = i * Unit;
                _gridHorizontal[i] = new Line(new Vector3(0, Y, 0), new Vector3(Width, Y, 0))
                {
                    Color = _gridColor
                };
            }
        }
    }
}
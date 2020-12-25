using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game2.Figures
{
    public class Figure
    {
        private bool _transformed;

        private PolyLine _line;
        private Circle[] _circles;
        private PolyLine _rhombus;

        private readonly CoordinateGrid _grid;

        private Circle _tempCircle;

        private float _l1;
        private float _l2;
        private float _l3;
        private float _l4;
        private float _r1;
        private float _r2;

        public float L1
        {
            get => _l1;
            set
            {
                if (_transformed)
                    return;
                if (value < 0 || value > L3 - L2)
                    return;
                _l1 = value;
                BuildLine();
            }
        }

        public float L2
        {
            get => _l2;
            set
            {
                if (_transformed)
                    return;
                if (value < 0 || value > L1)
                    return;
                _l2 = value;
                BuildLine();
            }
        }

        public float L3
        {
            get => _l3;
            set
            {
                if (_transformed)
                    return;
                if (value < R2 + R1 + L1)
                    return;
                _l3 = value;
                BuildLine();
            }
        }

        public float L4
        {
            get => _l4;
            set
            {
                if (_transformed || value < 1)
                    return;

                if (value > R2 - R1)
                    return;
                _l4 = value;
                BuildRhombus();
            }
        }

        public float R1
        {
            get => _r1;
            set
            {
                if (_transformed)
                    return;

                if (value > L3 - L2 - R2 || value > R2 - L4)
                    return;

                _r1 = value;
                BuildCircles();
            }
        }

        public float R2
        {
            get => _r2;
            set
            {
                if (_transformed)
                    return;

                if (value > L3 - L2 - R1 || value < L4 + R1)
                    return;

                _r2 = value;
                BuildCircles();
            }
        }

        public Figure(CoordinateGrid grid)
        {
            _grid = grid;

            Reset();
        }

        public void ApplyTransform(Transform transform)
        {
            _transformed = true;
            foreach (var circle in _circles)
            {
                circle.ApplyTransform(transform);
            }
            _line.ApplyTransform(transform);
            _rhombus.ApplyTransform(transform);
        }

        public void Reset()
        {
            _l1 = _grid.Unit * 6;
            _l2 = _grid.Unit * 4;
            _l3 = _grid.Unit * 15;
            _l4 = _grid.Unit * 2;
            _r1 = _grid.Unit * 2;
            _r2 = _grid.Unit * 6;
            _transformed = false;

            this.BuildCircles();
            this.BuildRhombus();
            this.BuildLine();
        }

        public void BuildCircles()
        {
            _circles = new Circle[3];
            var center = new Vector3
            {
                X = (float)(_grid.Center.X - R2 * Math.Cos(Math.PI / 6f)),
                Y = (float)(_grid.Center.Y - R2 * Math.Sin(Math.PI / 6f))
            };
            _circles[0] = new Circle(R1, center);
            _circles[1] = new Circle(R1, new Vector3((float)(_grid.Center.X - R2 * Math.Cos(5 * Math.PI / 6)), (float)(_grid.Center.Y - R2 * Math.Sin(5 * Math.PI / 6)), 0));
            _circles[2] = new Circle(R1, new Vector3(_grid.Center.X, _grid.Center.Y + R2, 0));
            _tempCircle = new Circle(R2, _grid.Center) {
                Color = Color.CadetBlue
            };
        }

        public void BuildLine()
        {
            const int n = 6;
            _line = new PolyLine(0)
            {
                IsEnclosed = true
            };
            for (int i = 0; i < n; i++)
            {
                BuildTooth(n, i);
            }
        }

        public void BuildTooth(int n, int i)
        {
            Vector3[] points = new Vector3[4];

            Transform transform = new Transform();
            transform.Rotate(new Vector3(0, 0, i * (360f / n)), _grid.Center);

            points[3] = transform.Multiply(new Vector3(_grid.Center.X + L3 - L2, _grid.Center.Y + L1 / 2, 0));
            points[2] = transform.Multiply(new Vector3(_grid.Center.X + L3, _grid.Center.Y + L1 / 2, 0));
            points[1] = transform.Multiply(new Vector3(_grid.Center.X + L3, _grid.Center.Y - L1 / 2, 0));
            points[0] = transform.Multiply(new Vector3(_grid.Center.X + L3 - L2, _grid.Center.Y - L1 / 2, 0));

            _line.Append(points);
        }

        public void BuildRhombus()
        {
            _rhombus = new PolyLine(4)
            {
                [0] = new Vector3(_grid.Center.X, _grid.Center.Y - L4 / (float)Math.Sqrt(2), 0),
                [1] = new Vector3(_grid.Center.X + L4 / (float)Math.Sqrt(2), _grid.Center.Y, 0),
                [2] = new Vector3(_grid.Center.X, _grid.Center.Y + L4 / (float)Math.Sqrt(2), 0),
                [3] = new Vector3(_grid.Center.X - L4 / (float)Math.Sqrt(2), _grid.Center.Y, 0),
                IsEnclosed = true
            };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _tempCircle.Draw(spriteBatch);
            foreach (var c in _circles)
            {
                c.Draw(spriteBatch);
            }
            _line.Draw(spriteBatch);
            _rhombus.Draw(spriteBatch);
        }
    }
}

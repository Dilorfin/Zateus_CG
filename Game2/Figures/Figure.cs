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
        private readonly Vector3 _center;

        private Circle _tempCircle;
        
        private float _l4;
        private float _l1;
        private float _l2;
        private float _l3;
        private float _r1;
        private float _r2;

        public float L1
        {
            get => _l1;
            set
            {
                if (_transformed) 
                    return;
                
                _l1 = value;
            }
        }

        public float L2
        {
            get => _l2;
            set
            {
                if (_transformed) 
                    return;
                
                _l2 = value;
            }
        }

        public float L3
        {
            get => _l3;
            set
            {
                if (_transformed) 
                    return;
                
                _l3 = value;
            }
        }

        public float L4
        {
            get => _l4;
            set
            {
                if (_transformed || value < 1) 
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
                
                _r2 = value;
                BuildCircles();
            }
        }

        public Figure(Vector3 center, float unit)
        {
            _center = center;

            _line = new PolyLine(2);
            _circles = new Circle[3];
            _rhombus = new PolyLine(4);
            
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
            _l1 = 75;
            _l2 = 50;
            _l3 = 95;
            _l4 = 20;
            _r1 = 50;
            _r2 = 150;
            _transformed = false;
            
            this.BuildCircles();
            this.BuildRhombus();
            this.BuildLine();
        }
        
        public void BuildCircles()
        {
            var center = new Vector3
            {
                X = (float)(_center.X - R2 * Math.Cos(Math.PI / 6f)),
                Y = (float)(_center.Y - R2 * Math.Sin(Math.PI / 6f))
            };
            _circles[0] = new Circle(R1, center);
            _circles[1] = new Circle(R1, new Vector3((float)(_center.X - R2 * Math.Cos(5 * Math.PI / 6)), (float)(_center.Y - R2 * Math.Sin(5 * Math.PI / 6)), 0));
            _circles[2] = new Circle(R1, new Vector3(_center.X, (float)(_center.Y + R2), 0));
            _tempCircle = new Circle(R2, _center);
        }

        public void BuildLine()
        {
            BuildTooth(1);
        }

        public void BuildTooth(int i)
        {
            Vector3 M = new Vector3((float)Math.Sin(Math.PI / 6d) * L3, (float)Math.Cos(Math.PI / 6d) * L3, 0);
            double R = L1 / 2d;
            double k = 1d / Math.Tan(Math.PI / 3d);
            double b = -M.X / Math.Tan(Math.PI / 3d) + M.Y;

            double A = k * k + 1;
            double B = 2 * M.X + 2 * k * b + 2 * k * M.Y;
            double C = M.X * M.X + b * b + 2 * b * M.Y + M.Y * M.Y - R * R;
            double D = B * B - 4 * A * C;
            double x = (B - Math.Sqrt(D)) / (2d * A);

            Vector3 p1 = new Vector3((float)x, (float)(k * x + b), 0);
            x = (B + Math.Sqrt(D)) / (2d * A);
            Vector3 p2 = new Vector3((float)x, (float)(k * x + b), 0);

            _line[0] = p1;
            _line[1] = p2;
        }

        public void build_line()
        {
            // Line[0] = new Vector3(Center.X - L3 - L2, Center.Y - L1 / 2, 0);
            // Line[1] = new Vector3(Center.X - L3, Center.Y - L1 / 2, 0);
            // Line[2]=new Vector3()
            // Line[0] = new Vector3(Center.X, Center.Y, 0);
            _line[0] = new Vector3(_center.X - L3, _center.Y - L3 * (float)Math.Tan(Math.PI / 3), 0);
            _line[1] = new Vector3(_center.X - (float)Math.Cos(Math.PI / 6) * L1 / 2, _center.Y - L3 - L1 / 2 * (float)Math.Sin(Math.PI / 6), 0);
            _line[2] = new Vector3(_center.X - (float)Math.Sin(Math.PI / 6) * L1 / 2, _center.Y - L2 - ((L2) / (float)Math.Tan(Math.PI / 6)), 0);
            //Line[2] = new Vector3(Center.X - (float)Math.Sin(11 * Math.PI / 6) * L3, Center.Y - L1 + ((L1 - Center.X - (float)Math.Sin(11 * Math.PI / 6) * L3) / (float)Math.Tan(11 * Math.PI / 6)), 0);
            _line[3] = new Vector3(_center.X + (float)Math.Sin(Math.PI / 6) * L1 / 2, _center.Y - L2 - ((L2) / (float)Math.Tan(Math.PI / 6)), 0);
            _line[4] = new Vector3(_center.X + (float)Math.Sin(Math.PI / 6) * L3, _center.Y - L1 * (float)Math.Tan(1.2), 0);
            _line[5] = new Vector3(_center.X + (float)Math.Sin(Math.PI / 6) * L1 / 2, _center.Y - L2 - ((L2) / (float)Math.Tan(Math.PI / 6)), 0);
        }

        public void BuildRhombus()
        {
            _rhombus[0] = new Vector3(_center.X, _center.Y - L4 / (float)Math.Sqrt(2), 0);
            _rhombus[1] = new Vector3(_center.X + L4 / (float)Math.Sqrt(2), _center.Y, 0);
            _rhombus[2] = new Vector3(_center.X, _center.Y + L4 / (float)Math.Sqrt(2), 0);
            _rhombus[3] = new Vector3(_center.X - L4 / (float)Math.Sqrt(2), _center.Y, 0);
            _rhombus.IsEnclosed = true;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _circles.Length; i++)
            {
                _circles[i].Draw(spriteBatch);
            }
            _tempCircle.Draw(spriteBatch);
            _line.Draw(spriteBatch);
            _rhombus.Draw(spriteBatch);
        }

    }//2.0944
}

using System;
using Microsoft.Xna.Framework;

namespace Game2
{
    public class Transform
    {
        private float[,] _matrix = {
            { 1 , 0 , 0 , 0 },
            { 0 , 1 , 0 , 0 },
            { 0 , 0 , 1 , 0 },
            { 0 , 0 , 0 , 1 }
        };

        private float this[int index]
        {
            get => _matrix[index % 4, index / 4];
            set => _matrix[index % 4, index / 4] = value;
        }

        public void Combine(Transform transform)
        {
            this._matrix = Multiply(transform)._matrix;
        }

        public Transform Multiply(Transform transform)
        {
            Transform multiply = new Transform();
            for (int i = 0; i < _matrix.GetLength(0); i++)
            {
                for (int p = 0; p < _matrix.GetLength(1); p++)
                {
                    float sum = 0;
                    for (int m = 0; m < _matrix.GetLength(0); m++)
                    {
                        sum += _matrix[i, m] * transform._matrix[m, p];
                    }
                    multiply._matrix[i, p] = sum;
                }
            }
            return multiply;
        }
        public Vector3 Multiply(Vector3 point)
        {
            return new Vector3
            {
                X = (_matrix[3, 0] + _matrix[0, 0] * point.X + _matrix[1, 0] * point.Y + _matrix[2, 0] * point.Z) /
                    (_matrix[3, 3] + _matrix[0, 3] * point.X + _matrix[1, 3] * point.Y + _matrix[2, 3] * point.Z),
                Y = (_matrix[3, 1] + _matrix[0, 1] * point.X + _matrix[1, 1] * point.Y + _matrix[2, 1] * point.Z) /
                    (_matrix[3, 3] + _matrix[0, 3] * point.X + _matrix[1, 3] * point.Y + _matrix[2, 3] * point.Z),
                Z = (_matrix[3, 2] + _matrix[0, 2] * point.X + _matrix[1, 2] * point.Y + _matrix[2, 2] * point.Z) /
                    (_matrix[3, 3] + _matrix[0, 3] * point.X + _matrix[1, 3] * point.Y + _matrix[2, 3] * point.Z)
            };
        }
        public void Move(Vector3 offset)
        {
            var temp = new Transform
            {
                _matrix = {
                    [3, 0] = (int) offset.X,
                    [3, 1] = (int) offset.Y,
                    [3, 2] = (int) offset.Z
                }
            };

            this.Combine(temp);
        }
        public void Scale(Vector3 scale)
        {
            var temp = new Transform
            {
                _matrix = {
                    [0, 0] = (int) scale.X,
                    [1, 1] = (int) scale.Y,
                    [2, 2] = (int) scale.Z
                }
            };

            this.Combine(temp);
        }

        public void Rotate(Vector3 rotate)
        {
            rotate.X *= (float)(Math.PI / 180);
            rotate.Y *= (float)(Math.PI / 180);
            rotate.Z *= (float)(Math.PI / 180);


            var rotateX = new Transform
            {
                [5] = (float) Math.Cos(rotate.X),
                [10] = (float) Math.Cos(rotate.X),
                [6] = (float) Math.Sin(rotate.X),
                [9] = (float) -Math.Sin(rotate.X)
            };
            
            this.Combine(rotateX);

            var rotateY = new Transform
            {
                [0] = (float) Math.Cos(rotate.Y),
                [10] = (float) Math.Cos(rotate.Y),
                [8] = (float) Math.Sin(rotate.Y),
                [2] = (float) -Math.Sin(rotate.Y)
            };

            this.Combine(rotateY);

            var rotateZ = new Transform
            {
                [0] = (float) Math.Cos(rotate.Z),
                [5] = (float) Math.Cos(rotate.Z),
                [4] = (float) -Math.Sin(rotate.Z),
                [1] = (float) Math.Sin(rotate.Z)
            };

            this.Combine(rotateZ);
        }
        public void Rotate(Vector3 rotate, Vector3 center)
        {
            this.Move(-center);
            this.Rotate(rotate);
            this.Move(center);
        }
        public void Affine(Vector3 r0, Vector3 rX, Vector3 rY)
        {
            Transform affine = new Transform();
            affine[0] = rX.X;
            affine[1] = rX.Y;
		
            affine[4] = rY.X;
            affine[5] = rY.Y;

            affine[12] = r0.X;
            affine[13] = r0.Y;
            this.Combine(affine);
        }
        public void Homography(Vector3 r0, Vector3 rX, Vector3 rY, Vector3 w)
        {
            Transform homography = new Transform();
            homography[0] = rX.X * w.X;
            homography[1] = rX.Y * w.X;
            homography[3] = w.X;
		
            homography[4] = rY.X * w.Y;
            homography[5] = rY.Y * w.Y;
            homography[7] = w.Y;

            homography[10] = 1;
            homography[11] = 1;
		
            homography[12] = r0.X;
            homography[13] = r0.Y;
            homography[15] = w.Z;

            this.Combine(homography);
        }
    }
}

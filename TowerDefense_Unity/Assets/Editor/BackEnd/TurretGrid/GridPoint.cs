using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BackEnd.Turrets.Editor
{
    /// <summary>
    /// Contains a point in the matrix that can be positioned using position/rotation.
    /// Reference:
    /// https://catlikecoding.com/unity/tutorials/rendering/part-1/
    /// </summary>
    class GridPoint
    {
        private readonly Matrix4x4 _PositionMat;
        private readonly Matrix4x4 _RotationMat;

        public Matrix4x4 Matrix { get; private set; }

        public GridPoint(Vector3 position, Vector3 eulerRotation)
        {
            _PositionMat = CalculatePositionMatrix(position);
            _RotationMat = CalculateRotationMatrix(eulerRotation);

            Matrix = _RotationMat * _PositionMat;
        }

        public Vector3 GetPointPosition()
        {
            Vector4 column = Matrix.GetColumn(3);
            return new Vector3(column.x, column.y, column.z);
        }

        private Matrix4x4 CalculatePositionMatrix(Vector3 position)
        {
            Matrix4x4 mat = new Matrix4x4();
            mat.SetRow(0, new Vector4(1f, 0f, 0f, position.x));
            mat.SetRow(1, new Vector4(0f, 1f, 0f, position.y));
            mat.SetRow(2, new Vector4(0f, 0f, 1f, position.z));
            mat.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
            return mat;
        }

        private Matrix4x4 CalculateRotationMatrix(Vector3 rotation)
        {
            Vector3 radians = rotation * Mathf.Deg2Rad;
            Vector3 sin = new Vector3(Mathf.Sin(radians.x), Mathf.Sin(radians.y), Mathf.Sin(radians.z));
            Vector3 cos = new Vector3(Mathf.Cos(radians.x), Mathf.Cos(radians.y), Mathf.Cos(radians.z));

            Matrix4x4 mat = new Matrix4x4();
            mat.SetColumn(0, new Vector4(
                x: cos.y * cos.z,
                y: cos.x * sin.z + sin.x * sin.y * cos.z,
                z: sin.x * sin.z - cos.x * sin.y * cos.z,
                w: 0f
            ));
            mat.SetColumn(1, new Vector4(
                x: -cos.y * sin.z,
                y: cos.x * cos.z - sin.x * sin.y * sin.z,
                z: sin.x * cos.z + cos.x * sin.y * sin.z,
                w: 0f
            ));
            mat.SetColumn(2, new Vector4(
                x: sin.y,
                y: -sin.x * cos.y,
                z: cos.x * cos.y,
                w: 0f
            ));
            mat.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
            return mat;
        }
    }
}

using UnityEngine;

namespace Game.Turrets
{
    /// <summary>
    /// Contains a point in the matrix that can be positioned using position/rotation.
    /// Reference: https://catlikecoding.com/unity/tutorials/rendering/part-1/
    /// </summary>
    public class GridPoint
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

            Matrix4x4 rotX = new Matrix4x4();
            rotX.SetRow(0, new Vector4(1.0f,   0.0f,  0.0f,   0.0f));
            rotX.SetRow(1, new Vector4(0.0f,   cos.x, -sin.x, 0.0f));
            rotX.SetRow(2, new Vector4(0.0f,   sin.x, cos.x,  0.0f));
            rotX.SetRow(3, new Vector4(0.0f,   0.0f,  0.0f,   1.0f));

            Matrix4x4 rotY = new Matrix4x4();
            rotY.SetRow(0, new Vector4(cos.y,  0.0f,  sin.y,  0.0f));
            rotY.SetRow(1, new Vector4(0.0f,   1.0f,  0.0f,   0.0f));
            rotY.SetRow(2, new Vector4(-sin.y, 0.0f,  cos.y,  0.0f));
            rotY.SetRow(3, new Vector4(0.0f,   0.0f,  0.0f,   1.0f));

            Matrix4x4 rotZ = new Matrix4x4();
            rotZ.SetRow(0, new Vector4(cos.z,  -sin.z, 0.0f,  0.0f));
            rotZ.SetRow(1, new Vector4(sin.z,  cos.z,  0.0f,  0.0f));
            rotZ.SetRow(2, new Vector4(0.0f,   0.0f,   1.0f,  0.0f));
            rotZ.SetRow(3, new Vector4(0.0f,   0.0f,   0.0f,  1.0f));

            return rotZ * rotY * rotX;
        }
    }
}

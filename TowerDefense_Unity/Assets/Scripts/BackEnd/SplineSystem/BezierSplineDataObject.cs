﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SplineSystem
{
    /// <summary>
    /// Holds all the data regarding a bezierSpline.
    /// </summary>
    [CreateAssetMenu(fileName ="new BezierSpline", menuName = "Spline", order = 0)]
    public class BezierSplineDataObject : ScriptableObject
    {
        [SerializeField, HideInInspector] private bool _IsClosed;
        [SerializeField] private List<Vector3> _Points = new List<Vector3>();

        public Vector3 this[int i] => _Points[i];
        public Vector3[] GetSegmentPoints(int index) => new[] { _Points[index * 3], _Points[index * 3 + 1], _Points[index * 3 + 2], _Points[LoopIndex(index * 3 + 3)] };
        public int SegmentCount => _Points.Count / 3;
        public int PointCount => _Points.Count;
        private int LoopIndex(int i) => (i + _Points.Count) % _Points.Count;
        public bool IsClosed
        {
            get => _IsClosed;
            set => _IsClosed = value;
        }

        public void Reset(Vector3 center)
        {
            _Points = new List<Vector3>()
            {
                center + Vector3.left,
                center + (Vector3.left + Vector3.back) * 0.5f,
                center + (Vector3.right + Vector3.forward) * 0.5f,
                center + Vector3.right
            };
        }

        public void AddSegment(Vector3 segmentEndPos)
        {
            _Points.Add(_Points[_Points.Count - 1] * 2 - _Points[_Points.Count - 2]);
            _Points.Add((_Points[_Points.Count - 1] + segmentEndPos) * 0.5f);
            _Points.Add(segmentEndPos);
        }

        public void MovePoint(int index, Vector3 point, SplineCreator.SplineMode mode)
        {
            if (index % 3 == 0)
            {
                Vector3 delta = point - _Points[index];
                _Points[index] = point;
                if (index - 1 >= 0 || _IsClosed) _Points[LoopIndex(index - 1)] += delta;
                if (index + 1 < _Points.Count || _IsClosed) _Points[LoopIndex(index + 1)] += delta;
                return;
            }

            switch (mode)
            {
                case SplineCreator.SplineMode.FREE:
                    _Points[index] = point;
                    break;
                case SplineCreator.SplineMode.MIRRORED:
                    _Points[index] = point;

                    bool isNextPointAnchor = (index + 1) % 3 == 0;
                    int correspondingControlIndex = isNextPointAnchor ? index + 2 : index - 2;
                    int anchorIndex = isNextPointAnchor ? index + 1 : index - 1;

                    if (correspondingControlIndex >= 0 && correspondingControlIndex < _Points.Count - 1 || _IsClosed)
                    {
                        float length = (_Points[LoopIndex(anchorIndex)] - _Points[LoopIndex(correspondingControlIndex)]).magnitude;
                        Vector3 direction = (_Points[LoopIndex(anchorIndex)] - point).normalized;
                        _Points[LoopIndex(correspondingControlIndex)] = _Points[LoopIndex(anchorIndex)] + direction * length;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RemoveSegment(int controlPointIndex)
        {
            if (controlPointIndex % 3 != 0 || SegmentCount <= 1) return;

            if (controlPointIndex == 0)
            {
                if (IsClosed)
                    _Points[_Points.Count - 1] = _Points[2];
                _Points.RemoveRange(controlPointIndex, 3);
            }
            else if (controlPointIndex == _Points.Count - 1)
                _Points.RemoveRange(controlPointIndex - 2, 3);
            else
                _Points.RemoveRange(controlPointIndex - 1, 3);
        }

        public void ToggleClosed(bool closed)
        {
            _IsClosed = closed;
            if (closed)
            {
                _Points.Add(_Points[_Points.Count - 1] * 2 - _Points[_Points.Count - 2]);
                _Points.Add(_Points[0] * 2 - _Points[1]);
                return;
            }
            _Points.RemoveRange(_Points.Count - 2, 2);
        }

        public void InsertSegment(int pointIndex, Vector3 point)
        {
            Vector3[] newAnchorPoint = { point + Vector3.left, point, point + Vector3.right };
            _Points.InsertRange(pointIndex + 2, newAnchorPoint);
        }

        public Vector3[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1)
        {
            float splineLength = 0;
            List<Vector3> points = GetEvenlySpacedPoints(ref splineLength, spacing, resolution);
            float remainingPointLength = IsClosed
                ? Mathf.Sqrt(
                    (points[0].x - points[points.Count - 1].x) * (points[0].x - points[points.Count - 1].x) +
                    (points[0].y - points[points.Count - 1].y) * (points[0].y - points[points.Count - 1].y) +
                    (points[0].z - points[points.Count - 1].z) * (points[0].z - points[points.Count - 1].z))
                : Mathf.Sqrt(
                    (_Points[PointCount - 1].x - points[points.Count - 1].x) * (_Points[PointCount - 1].x - points[points.Count - 1].x) +
                    (_Points[PointCount - 1].y - points[points.Count - 1].y) * (_Points[PointCount - 1].y - points[points.Count - 1].y) +
                    (_Points[PointCount - 1].z - points[points.Count - 1].z) * (_Points[PointCount - 1].z - points[points.Count - 1].z));

            spacing += remainingPointLength / points.Count;
            return GetEvenlySpacedPoints(ref splineLength, spacing, resolution).ToArray();
        }

        private List<Vector3> GetEvenlySpacedPoints(ref float splineLength, float spacing, float resolution = 1)
        {
            List<Vector3> points = new List<Vector3> { _Points[0] };
            Vector3 prevPoint = _Points[0];
            float distanceSinceLastPoint = 0;

            for (int segmentIndex = 0; segmentIndex < SegmentCount; segmentIndex++)
            {
                Vector3[] segment = GetSegmentPoints(segmentIndex);
                float estCurveLength = Utils.Bezier3DUtility.Get3DCurveLength(segment[0], segment[1], segment[2], segment[3]);
                int divisions = Mathf.CeilToInt(estCurveLength * resolution * 10);
                splineLength += estCurveLength;

                float pointPos = 0;
                while (pointPos <= 1)
                {
                    pointPos += 1f / divisions;
                    Vector3 pointOnCurve = Utils.Bezier3DUtility.CubicCurveVector3(segment[0], segment[1], segment[2], segment[3], pointPos);
                    distanceSinceLastPoint += Vector3.Distance(prevPoint, pointOnCurve);

                    while (distanceSinceLastPoint >= spacing)
                    {
                        float overShootDistance = distanceSinceLastPoint - spacing;
                        Vector3 newCorrectedPoint = pointOnCurve + (prevPoint - pointOnCurve).normalized * overShootDistance;
                        points.Add(newCorrectedPoint);
                        distanceSinceLastPoint = overShootDistance;
                        prevPoint = newCorrectedPoint;
                    }
                    prevPoint = pointOnCurve;
                }
            }
            return points;
        }
    }
}
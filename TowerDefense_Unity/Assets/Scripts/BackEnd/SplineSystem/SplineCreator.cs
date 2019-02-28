using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.SplineSystem
{
    /// <summary>A spline branch manager which, keeps track of the spline branches, which are represented as child objects.</summary>
    public class SplineCreator : MonoBehaviour
    {
        [SerializeField, HideInInspector] private List<SplineBranchCreator> _Branches;
    }
}

using UnityEngine;

namespace Game.SplineSystem
{
    /// <summary>Contains the data a selected point in one of the spline branches.</summary>
    public struct HandlePointIndex
    {
        public int HandleIndex { get; set; }
        public int BranchIndex { get; set; }
    }

    /// <summary>A spline branch manager which, keeps track of the spline branches, which are represented as child objects.</summary>
    public class SplineCreator : SplineCreatorBase
    {
        [SerializeField, HideInInspector] private SplineBranchCreator[] _Branches = new SplineBranchCreator[0];
        [SerializeField, HideInInspector] private Vector3 _Position;
        [SerializeField, HideInInspector] private HandlePointIndex _SelectedPointIndex, _ModifierPointIndex;

        public SplineBranchCreator this[int i] => _Branches[i];
        
        public SplineBranchCreator[] Branches
        {
            get => _Branches;
            set => _Branches = value;
        }

        public int BranchCount => Branches.Length;

        public Vector3 Position
        {
            get => _Position;
            set => _Position = value;
        }

        public HandlePointIndex SelectedPointIndex
        {
            get => _SelectedPointIndex;
            set => _SelectedPointIndex = value;
        }

        public HandlePointIndex ModifierPointIndex
        {
            get => _ModifierPointIndex;
            set => _ModifierPointIndex = value;
        }

        public override void Reset()
        {
            
        }
    }
}

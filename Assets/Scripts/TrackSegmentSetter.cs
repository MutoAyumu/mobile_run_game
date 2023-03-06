using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSegmentSetter : MonoBehaviour
{
    [SerializeField] Transform[] _childSegments;
    [SerializeField] Transform _origin;

    [SerializeField] float _zPositionMoveValue = 10;

    [ContextMenu("Setup")]
    void Setup()
    {
        GetChildren();

        if (_childSegments.Length == 0) return;

        var pos = _origin.position;
        var index = 0;

        foreach(Transform c in _childSegments)
        {
            c.position = new Vector3(pos.x, pos.y, index * _zPositionMoveValue);
            index++;
        }
    }

    void GetChildren()
    {
        var index = 0;
        _childSegments = new Transform[_origin.childCount];

        foreach (Transform c in _origin)
        {
            _childSegments[index] = c;
            index++;
        }
    }

    private void Reset()
    {
        _origin = this.transform;
    }
}

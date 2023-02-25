using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrackData", menuName = "Data/TrackData", order = 1)]
public class TrackData : ScriptableObject
{
    [SerializeField] TrackSegment[] _segments;
    [SerializeField] int _mapWidth = 14;
    [SerializeReference, SubclassSelector] IDamageObject[] _obstacleDataArray;

    public TrackSegment[] Senments => _segments;
    public int MapWidth => _mapWidth;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    [SerializeField] TrackData _data;

    TrackSegment[] _segments;
    int _width;
    int _generatedCount;
    TrackSegment[] _trackArray;

    private void Awake()
    {
        _segments = _data.Senments;
        _width = _data.MapWidth;
        _trackArray = new TrackSegment[_data.Senments.Length];
    }

    private void Start()
    {
        AddTrackToBeginning();
    }
    void AddTrackToBeginning()
    {
        for (int i = 0; i < _trackArray.Length; i++)
        {
            var width = _width * _generatedCount;
            _trackArray[i] = Instantiate(_segments[i], new Vector3(0, 0, width), Quaternion.identity);
            _generatedCount++;

            var track = _trackArray[i];
            track.Init(GenerateTrack);
        }
    }
    void GenerateTrack()
    {
        var index = _generatedCount % _trackArray.Length;
        var width = _width * _generatedCount;
        var track = _trackArray[index];
        track.transform.position = new Vector3(0, 0, width);
        _generatedCount++;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrackSegment : MonoBehaviour
{
    event Action _createEvent;

    const string PLAYER_TAG = "Player";

    public void Init(Action action)
    {
        _createEvent += action;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(PLAYER_TAG))
        {
            _createEvent?.Invoke();
        }
    }
}

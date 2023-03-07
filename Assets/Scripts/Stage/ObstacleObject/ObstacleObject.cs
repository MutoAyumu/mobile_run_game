using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleObject : MonoBehaviour
{
    const string PLAYER_TAG = "Player";

    protected virtual void Enter(GameObject go) { }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(PLAYER_TAG))
        {
            Enter(other.gameObject);
        }
    }
}

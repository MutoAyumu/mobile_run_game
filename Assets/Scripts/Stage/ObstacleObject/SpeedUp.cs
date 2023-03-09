using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    [SerializeField] float _addSpeed = 10f;
    [SerializeField] float _inputInvalidationTime = 4f;

    Transform _transform;

    const string PLAYER_TAG = "Player";

    private void Awake()
    {
        TryGetComponent(out _transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(PLAYER_TAG))
        {
            PlayerMove move;

            if(other.TryGetComponent(out move))
            {
                var vel = _transform.forward * _addSpeed;
                var rot = _transform.rotation;
                rot.x = 0;
                rot.z = 0;
                move.Accelerator(vel, rot, _inputInvalidationTime);
            }
        }
    }
}

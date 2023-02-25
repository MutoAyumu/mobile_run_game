using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleData
{
    [SerializeReference, SubclassSelector] IDamageObject _obstacleTypes;
    [SerializeField] int _createCount = 1;
    [SerializeField] float _spacing = 5f;

    public ObstacleData() { }
}

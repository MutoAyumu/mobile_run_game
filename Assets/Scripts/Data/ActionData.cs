using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionData", menuName = "Data/ActionData", order = 0)]
public class ActionData : ScriptableObject
{
    [SerializeReference, SubclassSelector] IAction _state;

    public IAction State => _state;
}

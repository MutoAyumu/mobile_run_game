using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionSystemAttachment : MonoBehaviour
{
    private void Awake()
    {
        ActionSystem.Instance.Init(this);
    }
}

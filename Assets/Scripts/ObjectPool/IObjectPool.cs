using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool
{
    bool IsActive { get; }
    void DisactiveForInstantiate();
    void Create();
    void Destroy();
}

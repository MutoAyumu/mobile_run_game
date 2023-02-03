using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
    public static CameraManager Instance = new CameraManager();

    CinemachineVirtualCamera[] _virtualCameras;

    public CameraManager() { }

    public void Init(CameraManagerAttachment attachment)
    {
        _virtualCameras = attachment.VirtualCameras;
    }

    /// <summary>
    /// カメラの優先順位を切り替える
    /// </summary>
    /// <param name="type"></param>
    public void ChangePreferredOrder(VCameraType type)
    {
        _virtualCameras[(int)type].MoveToTopOfPrioritySubqueue();
    }
}
public enum VCameraType
{
    PlayerFollow = 0,
    Action = 1,
    EnemyFollow = 2
}

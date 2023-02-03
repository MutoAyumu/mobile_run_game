using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerAttachment : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera[] _virtualCameras;

    public CinemachineVirtualCamera[] VirtualCameras => _virtualCameras;

    private void Awake()
    {
        CameraManager.Instance.Init(this);
    }
    private void Start()
    {
        CameraManager.Instance.ChangePreferredOrder(VCameraType.PlayerFollow);
    }
}

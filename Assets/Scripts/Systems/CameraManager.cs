using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
    public static CameraManager Instance = new CameraManager();

    CinemachineVirtualCamera[] _virtualCameras;
    float _duration;

    const float MAX_TIME_SCALE = 1f;
    const float MIN_TIME_SCALE = 0.2f;

    public CameraManager() { }

    public void Init(CameraManagerAttachment attachment)
    {
        _virtualCameras = attachment.VirtualCameras;
        _duration = Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.BlendTime;
    }

    /// <summary>
    /// �J�����̗D�揇�ʂ�؂�ւ���
    /// </summary>
    /// <param name="type"></param>
    public void ChangePreferredOrder(VCameraType type)
    {
        _virtualCameras[(int)type].MoveToTopOfPrioritySubqueue();
    }
    /// <summary>
    /// �^�C���X�P�[����ύX����
    /// </summary>
    /// <param name="type"></param>
    public void ChangeTimeScale(TimeScaleType type)
    {
        var to = MAX_TIME_SCALE;

        if(type == TimeScaleType.SlowTime)
        {
            to = MIN_TIME_SCALE;
        }

        DOVirtual.Float(Time.timeScale, to, _duration, value => Time.timeScale = value);
    }
}
public enum VCameraType
{
    PlayerFollow = 0,
    Action = 1,
    EnemyFollow = 2
}
public enum TimeScaleType
{
    NormalTime = 0,
    SlowTime = 1
}
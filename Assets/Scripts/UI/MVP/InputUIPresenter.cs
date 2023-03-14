using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InputUIPresenter : MonoBehaviour
{
    [SerializeField] MVPSkillIcon _skillIcon;

    private void Start() //���Awake�ł����Ȃ��悤�ɂ���
    {
        if(_skillIcon)
        {
            var value = 1f / ActionSystem.Instance.SkillIntervalData;

            ActionSystem.Instance.SkillInterval.Subscribe(x => _skillIcon.SetFillAmount(1 - (x * value)));
        }
    }
}

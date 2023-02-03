using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIController : MonoBehaviour
{
    [SerializeField] RectTransform _inputsUI;
    [SerializeField] RectTransform _actionUI;

    const string PLAYER_TAG = "Player";

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        var action = player.GetComponent<PlayerAttack>();

        action.IsActed.Subscribe(x => TogglePanels(x)).AddTo(this);
    }
    void TogglePanels(bool isActed)
    {
        if(isActed)
        {
            _inputsUI.gameObject.SetActive(false);
            _actionUI.gameObject.SetActive(true);
        }
        else
        {
            _inputsUI.gameObject.SetActive(true);
            _actionUI.gameObject.SetActive(false);
        }
    }
}

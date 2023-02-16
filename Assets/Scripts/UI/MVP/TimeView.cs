using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimeView : MonoBehaviour
{
    [SerializeField] Text _text;

    private void Reset()
    {
        TryGetComponent(out _text);
    }

    public void Init()
    {
        _text.text = "";
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
}

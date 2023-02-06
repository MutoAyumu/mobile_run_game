using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MVPText : MonoBehaviour
{
    [SerializeField] Text _text;

    private void Reset()
    {
        TryGetComponent(out _text);
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
}

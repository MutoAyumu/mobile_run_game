using UnityEngine;

/// <summary>�C���X�y�N�^�[�ŕ\������z��⃊�X�g�̗v�f����ێ�����N���X </summary>
public class ElementNames : PropertyAttribute
{
    /// <summary>�\�����������O�̔z�� </summary>
    public readonly string[] _names;
    public ElementNames(string[] names) { _names = names; }
}
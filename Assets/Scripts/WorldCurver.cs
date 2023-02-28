using UnityEngine;

[ExecuteAlways]
public class WorldCurver : MonoBehaviour
{
	[Range(-0.1f, 0.1f)]
	[SerializeField] float _curveStrengthY = 0.01f;
	[Range(-0.1f, 0.1f)]
	[SerializeField] float _curveStrengthX = 0.00f;

	int _CurveStrengthY;
	int _CurveStrengthX;

	private void OnEnable()
	{
		_CurveStrengthY = Shader.PropertyToID("_CurveStrengthY");
		_CurveStrengthX = Shader.PropertyToID("_CurveStrengthX");
	}

	void Update()
	{
		Shader.SetGlobalFloat(_CurveStrengthY, _curveStrengthY);
		Shader.SetGlobalFloat(_CurveStrengthX, _curveStrengthX);
	}

	public void SetCurve(float curve)
    {
		_curveStrengthX = curve;
    }
}
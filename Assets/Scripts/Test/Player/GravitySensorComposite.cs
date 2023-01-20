using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Layouts;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
[DisplayStringFormat("{gravity}")]
public class GravitySensorComposite : InputBindingComposite<Vector2>
{
#if UNITY_EDITOR
    static GravitySensorComposite()
    {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        InputSystem.RegisterBindingComposite<GravitySensorComposite>();
    }

    [InputControl(layout = "Vector3")] public Vector3 gravity = Vector3.zero;

    public override Vector2 ReadValue(ref InputBindingCompositeContext context)
    {
        return gravity;
    }

    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        return ReadValue(ref context).magnitude;
    }
}
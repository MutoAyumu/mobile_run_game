using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
[DisplayStringFormat("{up}/{left}/{down}/{right}/{front}/{back}")]
public class MyComposite : InputBindingComposite<Vector3>
{
#if UNITY_EDITOR
    static MyComposite()
    {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterBindingComposite<MyComposite>();
    }

    [InputControl(layout = "Button")] public int up = 0;
    [InputControl(layout = "Button")] public int down = 0;
    [InputControl(layout = "Button")] public int left = 0;
    [InputControl(layout = "Button")] public int right = 0;
    [InputControl(layout = "Button")] public int front = 0;
    [InputControl(layout = "Button")] public int back = 0;
    public float xSpeed = 1f;
    public float ySpeed = 1f;
    public float zSpeed = 1f;

    public override Vector3 ReadValue(ref InputBindingCompositeContext context)
    {
        return new Vector3(xSpeed * (right - left), ySpeed * (up - down), zSpeed * (back - front));
    }

    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        return ReadValue(ref context).magnitude;
    }
}
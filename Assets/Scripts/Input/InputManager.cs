public class InputManager : MonoSingleton<InputManager>
{
    public InputSystem_Actions Inputs { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Inputs = new InputSystem_Actions();
        Inputs.Enable();
    }
}

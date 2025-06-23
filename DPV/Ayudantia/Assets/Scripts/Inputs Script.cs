using UnityEngine;
using UnityEngine.InputSystem;

public class InputsScript : MonoBehaviour
{
    public void Mover(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("se mueve");
        }
        if (context.performed)
        {
            Debug.Log("se mueve");
        }
        if (context.canceled)
        {
            Debug.Log("se mueve");
        }
    }

    public void Mover2()
    {
        Debug.Log("se mueve 2");
    }
}

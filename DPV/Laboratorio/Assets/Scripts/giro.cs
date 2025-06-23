using UnityEngine;

public class giro : MonoBehaviour
{
    // Velocidad de giro
    [SerializeField] private float velocidad = 0.5f;

    // Update is called once per frame
    void Update()
    {
        // Rotar el objeto
        transform.Rotate(velocidad, 0.0f, 0.0f);
    }
}

using UnityEngine;

public class lateUpdate : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.Translate(0,0,0.01f * Time.deltaTime * 5);
    }

    private void OnEnable()
    {
        Debug.Log("cadena de texto enable");
    }
}

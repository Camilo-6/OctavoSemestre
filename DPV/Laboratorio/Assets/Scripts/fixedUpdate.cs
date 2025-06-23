using UnityEngine;

public class fixedUpdate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Translate(0,0,0.01f * Time.deltaTime * 5);
    }
}

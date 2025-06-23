using UnityEngine;
using System.Collections;

public class movimientoBueno : MonoBehaviour
{
    // Character controller
    [SerializeField] private CharacterController cc;
    // Direccion
    [SerializeField] private Vector3 direccion;
    // Gravedad
    [SerializeField] private float gravedad = 10.0f;
    // Rotacion
    [SerializeField] private float rotacion;
    // Velocidad de rotacion
    [SerializeField] private float velocidadRotacion = 5.0f;
    // Salto
    [SerializeField] private float salto = 200.0f;
    // Velocidad
    [SerializeField] private float velocidad = 5.0f;
    // Velocidad inicial
    [SerializeField] private float velocidadInicial;
    // Animator
    [SerializeField] private Animator anim;
    // Vertical
    [SerializeField] private float vertical;
    // Holder
    [SerializeField] private Transform holder;
    // Gun
    [SerializeField] private GameObject gun;
    // Bala
    [SerializeField] private GameObject bala;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.cc = this.gameObject.GetComponent<CharacterController>();
        this.anim = this.gameObject.GetComponent<Animator>();
        this.velocidadInicial = velocidad;
    }

    // Update is called once per frame
    void Update()
    {
        if (cc.isGrounded)
        {
            anim.SetTrigger("suelo");
            vertical = Input.GetAxisRaw("Vertical");
            direccion = gameObject.transform.TransformDirection(new Vector3(0, 0, vertical) * velocidad);
            rotacion = Input.GetAxis("Horizontal") * velocidadRotacion;
            anim.SetFloat("move", vertical);
            if (vertical == 0)
            {
                anim.SetBool("idle?", true);
            }
            else
            {
                anim.SetBool("idle?", false);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                direccion.y += salto * Time.deltaTime;
                cc.transform.Rotate(Vector3.zero); // evita que el personaje rote en el aire
                anim.SetTrigger("salto");
            }
        }
        else
        {
            anim.ResetTrigger("suelo");
        }
        direccion -= new Vector3(0, gravedad * Time.deltaTime, 0);
        cc.transform.Rotate(new Vector3(0, rotacion, 0));
        cc.Move(direccion * Time.deltaTime);

        // Disparar
        if (Input.GetKeyDown(KeyCode.C) && gun.activeSelf)
        {
            Instantiate(this.bala, holder.position, holder.rotation);
        }
    }

    // Establecer la velocidad
    public void setVelocidad(float velocidad)
    {
        this.velocidad = velocidad;
    }

    // Restrablecer la velocidad
    public void resetVelocidad()
    {
        this.velocidad = velocidadInicial;
    }

    // Colision Trigger entrada
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "gun")
        {
            other.gameObject.SetActive(false);
            gun.SetActive(true);
        }
    }
}

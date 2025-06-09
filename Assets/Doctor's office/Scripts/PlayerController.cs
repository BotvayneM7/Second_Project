using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadMove = 5f;
    public float fuerzaSalto = 1.5f;
    public float gravedad = -9.81f;

    [Header("Ratón / Cámara")]
    public float sensibilidadMouse = 2.0f;
    public Transform camara;                
    private float xRotación = 0f;           
    public float rotMin = -80f;             
    public float rotMax = 80f;              

    private CharacterController controller;
    private Vector3 velocidad = Vector3.zero;
    private float yVelocidad = 0f;          

    void Start()
    {
        controller = GetComponent<CharacterController>();

        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MoverJugador();
        RotarCamaraConMouse();
    }

    void MoverJugador()
    {
        
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

       
        Vector3 dirMovimiento = transform.right * inputX + transform.forward * inputZ;
        dirMovimiento.Normalize();

      
        Vector3 movimiento = dirMovimiento * velocidadMove;

      
        if (controller.isGrounded)
        {
            yVelocidad = -1f;  

            if (Input.GetButtonDown("Jump"))
            {
                yVelocidad = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
            }
        }
        else
        {
            
            yVelocidad += gravedad * Time.deltaTime;
        }

        movimiento.y = yVelocidad;

        
        controller.Move(movimiento * Time.deltaTime);
    }

    void RotarCamaraConMouse()
    {
       
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

      
        transform.Rotate(Vector3.up * mouseX);

      
        xRotación -= mouseY;
        xRotación = Mathf.Clamp(xRotación, rotMin, rotMax);

        camara.localEulerAngles = new Vector3(xRotación, 0f, 0f);
    }
}

using System.Collections;
using UnityEngine;

public class GelTrigger : MonoBehaviour
{
    [Header("Assign these in the Inspector")]
    public GameObject player;            // Tu objeto Player
    public Camera mainCamera;            // Tu cámara principal
    public Camera washCamera;            // La misma washCamera que usas para el lavabo
    public Animator gelHandsAnimator;    // El Animator para HandsContainer (Gel)

    [Header("Settings")]
    public string gelAnimName = "Gel Washing"; // Debe coincidir con el “Name” del State en tu Gel Animator Controller
    public float gelDuration = 1.0f;           // Duración de la animación de gel
    public KeyCode gelKey = KeyCode.F;         // Tecla para activar

    private bool playerInZone = false;
    private PlayerController playerController;

    void Start()
    {
        // Al inicio, la washCamera apagada y las manos de gel ocultas
        washCamera.enabled = false;
        if (gelHandsAnimator != null)
            gelHandsAnimator.gameObject.SetActive(false);
        else
            Debug.LogWarning("GelTrigger: gelHandsAnimator no está asignado.");

        // Cachea el PlayerController
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            if (playerController == null)
                Debug.LogWarning("GelTrigger: PlayerController not found on the Player GameObject.");
        }
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(gelKey))
            StartCoroutine(PlayGelSequence());
    }

    private IEnumerator PlayGelSequence()
    {
        // Deshabilita el control del jugador
        if (playerController != null)
            playerController.enabled = false;

        // Cambia a la cámara de lavado
        mainCamera.enabled = false;
        washCamera.enabled = true;

        // Muestra las manos de gel y lanza la animación
        gelHandsAnimator.gameObject.SetActive(true);
        gelHandsAnimator.Play(gelAnimName, 0, 0f);

        // Espera a que termine
        yield return new WaitForSeconds(gelDuration);

        // Oculta las manos de gel y vuelve a la cámara principal
        gelHandsAnimator.gameObject.SetActive(false);
        washCamera.enabled = false;
        mainCamera.enabled = true;

        // Reactiva el control del jugador
        if (playerController != null)
            playerController.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
            playerInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
            playerInZone = false;
    }
}

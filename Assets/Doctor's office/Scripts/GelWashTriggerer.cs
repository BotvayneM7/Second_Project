using System.Collections;
using UnityEngine;

public class GelTrigger : MonoBehaviour
{
    [Header("Assign these in the Inspector")]
    public GameObject player;           
    public Camera mainCamera;            
    public Camera washCamera;           
    public Animator gelHandsAnimator;   

    [Header("Settings")]
    public string gelAnimName = "Gel Washing";
    public float gelDuration = 1.0f;           
    public KeyCode gelKey = KeyCode.F;         

    private bool playerInZone = false;
    private PlayerController playerController;

    void Start()
    {
        
        washCamera.enabled = false;
        if (gelHandsAnimator != null)
            gelHandsAnimator.gameObject.SetActive(false);
        else
            Debug.LogWarning("GelTrigger: gelHandsAnimator no está asignado.");

       
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
        
        if (playerController != null)
            playerController.enabled = false;

       
        mainCamera.enabled = false;
        washCamera.enabled = true;

        
        gelHandsAnimator.gameObject.SetActive(true);
        gelHandsAnimator.Play(gelAnimName, 0, 0f);

        
        yield return new WaitForSeconds(gelDuration);

        
        gelHandsAnimator.gameObject.SetActive(false);
        washCamera.enabled = false;
        mainCamera.enabled = true;

       
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

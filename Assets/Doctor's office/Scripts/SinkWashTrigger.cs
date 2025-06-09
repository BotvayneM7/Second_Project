using System.Collections;
using UnityEngine;

public class SinkWashTrigger : MonoBehaviour
{
    [Header("Assign these in the Inspector")]
    public GameObject player;
    public Camera mainCamera;
    public Camera washCamera;
    public Animator handsAnimator;   
    public Animator waterAnimator;   

    [Header("Settings")]
    public string washAnimName = "Hand Washing";
    public string waterAnimName = "Water";        
    public float washDuration = 1.0f;
    public KeyCode washKey = KeyCode.E;

    private bool playerInZone = false;
    private PlayerController playerController;

    void Start()
    {
       
        washCamera.enabled = false;
        if (handsAnimator != null)
            handsAnimator.gameObject.SetActive(false);
        if (waterAnimator != null)
            waterAnimator.gameObject.SetActive(false);

       
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            if (playerController == null)
                Debug.LogWarning("SinkWashTrigger: PlayerController not found on the Player.");
        }
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(washKey))
            StartCoroutine(PlayWashSequence());
    }

    private IEnumerator PlayWashSequence()
    {
       
        if (playerController != null)
            playerController.enabled = false;

       
        mainCamera.enabled = false;
        washCamera.enabled = true;

       
        if (waterAnimator != null)
        {
            waterAnimator.gameObject.SetActive(true);
            waterAnimator.Play(waterAnimName, 0, 0f);
        }

       
        if (handsAnimator != null)
        {
            handsAnimator.gameObject.SetActive(true);
            handsAnimator.Play(washAnimName, 0, 0f);
        }

       
        yield return new WaitForSeconds(washDuration);

       
        if (waterAnimator != null)
            waterAnimator.gameObject.SetActive(false);
        if (handsAnimator != null)
            handsAnimator.gameObject.SetActive(false);

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

using System.Collections;
using UnityEngine;

public class SinkWashTrigger : MonoBehaviour
{
    [Header("Assign these in the Inspector")]
    public GameObject player;
    public Camera mainCamera;
    public Camera washCamera;
    public Animator handsAnimator;   // Animator for the hands
    public Animator waterAnimator;   // Animator for the water stream

    [Header("Settings")]
    public string washAnimName = "Hand Washing";
    public string waterAnimName = "Water";        // Name of your water‐flow state
    public float washDuration = 1.0f;
    public KeyCode washKey = KeyCode.E;

    private bool playerInZone = false;
    private PlayerController playerController;

    void Start()
    {
        // Hide the wash camera and both animators' GameObjects at start
        washCamera.enabled = false;
        if (handsAnimator != null)
            handsAnimator.gameObject.SetActive(false);
        if (waterAnimator != null)
            waterAnimator.gameObject.SetActive(false);

        // Cache PlayerController
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
        // disable player input
        if (playerController != null)
            playerController.enabled = false;

        // switch cameras
        mainCamera.enabled = false;
        washCamera.enabled = true;

        // show and trigger water animation
        if (waterAnimator != null)
        {
            waterAnimator.gameObject.SetActive(true);
            waterAnimator.Play(waterAnimName, 0, 0f);
        }

        // show and trigger hand animation
        if (handsAnimator != null)
        {
            handsAnimator.gameObject.SetActive(true);
            handsAnimator.Play(washAnimName, 0, 0f);
        }

        // wait for the washDuration
        yield return new WaitForSeconds(washDuration);

        // hide water and hands, switch back cameras
        if (waterAnimator != null)
            waterAnimator.gameObject.SetActive(false);
        if (handsAnimator != null)
            handsAnimator.gameObject.SetActive(false);

        washCamera.enabled = false;
        mainCamera.enabled = true;

        // re-enable player input
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

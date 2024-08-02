using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    [Tooltip("List of all rigidbody parts that make up this breakable object.")]
    public List<Rigidbody> allParts = new List<Rigidbody>();

    [Tooltip("Audio clip to play when the object breaks.")]
    public AudioClip breakSound;

    private AudioSource audioSource;
    private bool isBroken = false;  // Flag to check if the object is already broken

    private void Start()
    {
        // Ensure all parts are initially set to kinematic
        foreach (Rigidbody part in allParts)
        {
            part.isKinematic = true;
        }

        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Break()
    {
        // Check if the object is already broken
        if (isBroken)
        {
            return;
        }

        // Set the object as broken
        isBroken = true;

        // Set all parts to non-kinematic
        foreach (Rigidbody part in allParts)
        {
            part.isKinematic = false;
        }

        // Play the breaking sound
        if (breakSound != null)
        {
            audioSource.PlayOneShot(breakSound);
        }
    }
}

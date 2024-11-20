using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Required for TextMesh Pro

public class Move : MonoBehaviour
{
    [SerializeField] float rotationThrust = 1f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] float maxFuel = 100f; // Maximum fuel
    [SerializeField] float fuelConsumptionRate = 10f; // Fuel used per second of thrust
    [SerializeField] TMP_Text fuelText; // Reference to the TextMesh Pro text element
    [SerializeField] ParticleSystem rocketFlames; // Reference to the Rocket Flames Particle System

    AudioSource audioSource;
    Rigidbody rb;
    float currentFuel; // Current fuel level
    bool isThrusting = false; // Flag to track if the rocket is thrusting

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        currentFuel = maxFuel; // Initialize fuel
        UpdateFuelText(); // Set the initial fuel text
        rocketFlames.Stop(); // Make sure the flames are off initially
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space) && currentFuel > 0)  // Check if thrusting and fuel is available
        {
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            currentFuel -= fuelConsumptionRate * Time.deltaTime; // Decrease fuel based on consumption rate
            UpdateFuelText(); // Update fuel text on screen

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }

            // Activate the particle system if it's not already active
            if (!isThrusting)
            {
                rocketFlames.Play();  // Start the flames when thrusting
                isThrusting = true;
            }
        }
        else
        {
            audioSource.Stop();

            // Stop the particle system if no thrusting
            if (isThrusting)
            {
                rocketFlames.Stop();  // Stop the flames when not thrusting
                isThrusting = false;
            }
        }

        // Ensure fuel does not go below zero
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(-rotationThrust);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(rotationThrust);
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }

    void UpdateFuelText()
    {
        // Display fuel rounded to one decimal place
        fuelText.text = "Fuel: " + currentFuel.ToString("F1");
    }
}

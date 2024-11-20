using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    
    [SerializeField] float delayInReloadLevel = 1f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    AudioSource audioSource;

    bool isTransitioning = false;
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision other) {
        if (isTransitioning){ return; }
        
        
        switch (other.gameObject.tag) {
            case "Friendly":
                Debug.Log("This thing is freindly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.PlayOneShot(success);
        GetComponent<Move>().enabled = false;
        Invoke("LoadNextLevel", delayInReloadLevel);
    }

    void StartCrashSequence() {
        isTransitioning = true;
        audioSource.PlayOneShot(crash);
        GetComponent<Move>().enabled = false;
        Invoke("ReloadLevel", delayInReloadLevel);
    }
    void ReloadLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void LoadNextLevel() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
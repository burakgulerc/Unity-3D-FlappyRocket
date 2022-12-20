using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delayTime = 1f;

    [SerializeField] AudioClip crashClip;
    [SerializeField] AudioClip successClip;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    bool isTransitioning = false;

    public bool collisionDisableCheat = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if(isTransitioning || collisionDisableCheat) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finish":
                StartNextLevelSequence();
                break;
            default:
                StartCrashSequence(); 
                break;
        }
        
    }
    
    void StartCrashSequence()
    {
        isTransitioning = true;
        // crash SFX 
        audioSource.Stop();
        audioSource.PlayOneShot(crashClip);

        //particle effect on crash
        crashParticles.Play();

        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", delayTime); 
    }

    void ReloadLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void StartNextLevelSequence()
    {
        isTransitioning = true;
        // success SFX 
        audioSource.Stop();
        audioSource.PlayOneShot(successClip);


        // todo add  particle effect on scucces

        successParticles.Play();

        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", delayTime);
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentSceneIndex + 1;
        
        if(nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextLevelIndex = 0;
        }

        SceneManager.LoadScene(nextLevelIndex);
    }
}

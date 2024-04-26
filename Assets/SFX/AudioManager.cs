using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    [SerializeField]
    private AudioSource soundFXObject;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Something is very bad");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void PlaySound(AudioClip clip, Vector3 sourceLocation)
    {
        AudioSource audioSource = Instantiate(soundFXObject, sourceLocation, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.Play();

        float clipLength = clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
}

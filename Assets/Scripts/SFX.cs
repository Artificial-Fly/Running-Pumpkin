using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    AudioSource audioSource;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = gameManager.GetSFXVolume();
        StartCoroutine(UnMuteSounds());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator UnMuteSounds(){
        yield return new WaitForSeconds(0.5f);
        audioSource.mute = !audioSource.mute;
    }
}

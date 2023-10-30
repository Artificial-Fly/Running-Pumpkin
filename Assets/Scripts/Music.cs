using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    AudioSource audioSource;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = gameManager.GetMusicVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

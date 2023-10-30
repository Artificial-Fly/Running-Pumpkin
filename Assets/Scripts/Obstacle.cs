using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public AudioSource OnHitSound;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        if(tag=="Obstacle_C"){
            OnHitSound = GameObject.FindWithTag("CandySoundV1").GetComponent<AudioSource>();
        }else{
            OnHitSound = GameObject.FindWithTag("HitSound").GetComponent<AudioSource>();
        }
        //OnHitSound = GameObject.FindWithTag("HitSound").GetComponent<AudioSource>();//CandySoundV1//CandySoundV2
        //OnHitSound.volume = gameManager.GetSFXVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        //Debug.Log("Collision");
        if(other.tag=="Player"&&!(GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPlayerinvisible)){
            //Debug.Log("..with Player");
            if(gameObject.CompareTag("Obstacle_J")){
                //Debug.Log("..Obstacle checks Jump");
                if(!gameManager.CheckJump()){
                   // Debug.Log(".. player has no jump");
                    OnHitSound.Play();
                    gameManager.UpdateLives(-1);
                    Destroy(gameObject);
                }else{
                    //Debug.Log(".. player has jump");
                    //gameManager.UpdateScore(1);
                }
            }else if(gameObject.CompareTag("Obstacle_D")){
                //Debug.Log("..Obstacle checks Dash");
                if(!gameManager.CheckDash()){
                    //Debug.Log(".. player has no dash");
                    OnHitSound.Play();
                    gameManager.UpdateLives(-1);
                    Destroy(gameObject);
                }else{
                    //Debug.Log(".. player has dash");
                    //gameManager.UpdateScore(1);
                }
            }else if(gameObject.CompareTag("Obstacle_C")){
                //Debug.Log("..Obstacle is Candy");
                if(!gameManager.CheckJump()){
                    OnHitSound.Play();
                    //Debug.Log("Player picked up candy");
                    gameManager.UpdateScore(1);
                    Destroy(gameObject);
                }
            }
        }
    }
}

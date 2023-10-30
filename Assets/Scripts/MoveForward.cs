using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    float yBorder = 14.5f;
    bool isMoving = true;
    float currentDifficulty = 1;
    float expectedDifficulty = 0;
    public float speed=2.5f;//3.5-4.5 for background
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        InvokeRepeating("chechDifficulty", 0.0f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.isGameRunning){
            
            if(transform.position.y<yBorder&&isMoving){
                transform.Translate(Vector3.up*speed*Time.deltaTime);
            }else{
                if(!(gameObject.tag=="Background")){
                    isMoving=false;
                    Destroy(gameObject);
                }
            }
        } 
    }
    /*IEnumerator chechDifficulty(){
        speed *= gameManager.UpdateDifficulty(0);
    }*/
    
    void chechDifficulty(){
        currentDifficulty = gameManager.UpdateDifficulty(0);
        if(expectedDifficulty!=currentDifficulty){
            speed *= currentDifficulty;
            expectedDifficulty = currentDifficulty;
        }
    }
}

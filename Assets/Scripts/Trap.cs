using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    GameManager gameManager;
    public GameObject movingObstacle;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        //transform.position = new Vector3(transform.position.x, transform.position.y+2.5f*gameManager.UpdateDifficulty(0),transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        if(other.tag=="Player"&&!gameManager.CheckJump()&&gameManager.isGameRunning){
            //Debug.Log("Trap has been triggered!");
            if(movingObstacle.name=="Dog Left"){
                Instantiate(movingObstacle, new Vector3(-5.0f, transform.position.y-(2.5f/2)*gameManager.UpdateDifficulty(0), 0.0f), movingObstacle.transform.rotation);
            }else if(movingObstacle.name=="Dog Right"){
                Instantiate(movingObstacle, new Vector3(5.0f, transform.position.y-(2.5f/2)*gameManager.UpdateDifficulty(0), 0.0f), movingObstacle.transform.rotation);
            }else if(movingObstacle.name=="Ghost"){
                Instantiate(movingObstacle, new Vector3(transform.position.x/*Random.Range(-4.0f,4.0f)*/, -5.5f, 0), movingObstacle.transform.rotation);
            }          
        }
    }
}

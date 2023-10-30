using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    float yBorder = 14.5f;
    bool isMoving = true;
    float speed=2.5f;
    float dogSpeed;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        speed *= gameManager.UpdateDifficulty(0);
        dogSpeed = Mathf.Abs(gameManager.GetPLayerPosX()-transform.position.x);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.isGameRunning){
            dogSpeed = Mathf.Abs(gameManager.GetPLayerPosX()-transform.position.x);
            if(transform.position.y<yBorder&&isMoving){
            //Debug.Log("Run Dog");
                //transform.Translate(Vector3.up*speed*Time.deltaTime);
                if(name=="Dog Right(Clone)"){
                   // Debug.Log("run to Left");
                    transform.Translate(new Vector3(-0.5f*dogSpeed-0.5f,1.0f,0)*speed*Time.deltaTime);
                }else if(name=="Dog Left(Clone)"){
                   // Debug.Log("run to Right");
                    transform.Translate(new Vector3(0.5f*dogSpeed+0.5f,1.0f,0)*speed*Time.deltaTime);
                }
            }else{
                if(!(gameObject.tag=="Background")){
                    isMoving=false;
                    Destroy(gameObject);
                }
            }
        }
    }
}

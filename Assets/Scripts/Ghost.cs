using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    float yBorder = 14.5f;
    bool isMoving = true;
    float speed=2.5f;
    GameManager gameManager;
    float GhostSpeed;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        //GhostSpeed = speed * 2 * gameManager.UpdateDifficulty(0);
        GhostSpeed = speed * ((Mathf.Abs(gameManager.GetPLayerPosY()-transform.position.y))/3)* gameManager.UpdateDifficulty(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.isGameRunning){
            if(transform.position.y<yBorder&&isMoving){
                transform.Translate(Vector3.up*GhostSpeed*Time.deltaTime);
            }else{
                if(!(gameObject.tag=="Background")){
                    isMoving=false;
                    Destroy(gameObject);
                }
            }
        }
    }
}

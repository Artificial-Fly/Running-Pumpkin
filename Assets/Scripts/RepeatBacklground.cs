using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBacklground : MonoBehaviour
{
    Vector3 startPos;
    float repeatHeight;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        repeatHeight = GetComponent<BoxCollider>().size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y > startPos.y + repeatHeight){
            transform.position = startPos;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNPC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        Vector2 vector = new Vector2(0,0);
        if(changeDirection()){
            vector = getNextStage();
        }
        
        GetComponent<Rigidbody2D>().velocity = vector;
    }

    Vector2 getNextStage(){
        if(Random.Range(0,9)>5){
            return new Vector2(2,0);
        }
        return new Vector2(-2,0);
    }

    bool changeDirection(){
        if(Random.Range(0,9)>5){
            return true;
        }
        return false;
    }
    
}

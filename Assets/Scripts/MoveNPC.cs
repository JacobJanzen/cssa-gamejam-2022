using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNPC : MonoBehaviour
{
    private const int FRAME_LIMIT = 400;//frames
    private const double RADIUS = 1.5;
    private const float MAX_SPEED = 2.2f;
    private SpriteRenderer sprite;
    private int frames = 0;
    private double startPosition;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        startPosition = getXCoord();
    }
    
    // Update is called once per frame
    void Update()
    {
        if(frames >= FRAME_LIMIT){
            Vector2 vector = new Vector2(0,0);
            vector = getNextStage();
            GetComponent<Rigidbody2D>().velocity = vector;
            frames = 0;
        }
        frames++;
    }

    Vector2 getNextStage(){
        int num = Random.Range(0,21);
        if(num>17 || goRight()){
            sprite.flipX = true;
            return new Vector2(getSpeed(),0);
        }else if(num<3 || goLeft()){
            sprite.flipX = false;
            return new Vector2(-getSpeed(),0);
        }else{
            return new Vector2(0,0);
        }
    }   

    float getSpeed(){
        return Random.Range(0.1f,MAX_SPEED+1.0f);
    }

    bool goLeft(){
        if(getXCoord() > startPosition + RADIUS){
            Debug.Log("left");
            return true;
        }
        return false;
    }

    bool goRight(){
        if(getXCoord() < startPosition - RADIUS){
            Debug.Log("right");
            return true;
        }
        return false;
    }

    double getXCoord(){
        return this.transform.position.x;
    } 
}

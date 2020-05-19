using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehavior : MonoBehaviour
{   
    [SerializeField]
    private Vector3 rotationAnchorPoint;
    private static float moveTime = 1f;
    public static AudioSource blockAudio;
    private float lastMove;
    void Start(){
        blockAudio = gameObject.GetComponent<AudioSource>();
        blockAudio.volume = VolumeControl.SFX * VolumeControl.master;
    }
    void Fall(){
        this.transform.position += new Vector3 (0,-1,0);
        lastMove = Time.time;
    }

    public static void DecreaseMoveTime(float speedFactor){
        moveTime = moveTime/(moveTime+speedFactor);
    }

    public static void ResetMoveTime(){
        moveTime = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameLoop.gamePaused) return;
        //Horizontal Movement
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            if (CanMove(Direction.Left)) this.transform.position += new Vector3(-1,0,0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)){
            if (CanMove(Direction.Right)) this.transform.position += new Vector3(1,0,0);
        }
        //Speed Up
        if(Time.time - lastMove >= ((Input.GetKey(KeyCode.DownArrow))  ? moveTime/10f : moveTime)){
            if (CanMove(Direction.Down)) Fall();
            else{
                FillGrid();
                this.enabled = false;
                if(!GridHandler.GridCleanUp()) gameObject.GetComponent<AudioSource>().Play();
                if (SpawnHandler.singleton != null) Invoke("CallNewBlock", 0.5f);
           }
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            Rotate();
            if(InvalidRotation()) RotateReverse();
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            SpawnHandler.singleton.StoreBlock();
        }

    }

    void CallNewBlock(){
        SpawnHandler.singleton.NewBlock();
    }

    bool InvalidRotation(){ //Check if rotation moved block out of bounds
        foreach (Transform T in transform){
            if(T.position.x < 0 || T.position.x > 10 || T.position.y < 0){
                return true;
            }
        }
        return false;
    }

    void Rotate(){
        transform.RotateAround(transform.TransformPoint(rotationAnchorPoint), new Vector3(0,0,1), -90);
    }
    void RotateReverse(){
        transform.RotateAround(transform.TransformPoint(rotationAnchorPoint), new Vector3(0,0,1), 90);
    }
    enum Direction{
        Left,
        Right,
        Down
    }

    bool CanMove(Direction D){ //Restricts piece movement
        foreach(Transform T in transform){
            if(D == Direction.Right){
                if(T.position.x + 1 > 10 || GridHandler.tetrisGrid[T.GridPositionX()+1,T.GridPositionY()] != null){
                    return false;
                } 
            }
            if(D == Direction.Left){
                if(T.position.x - 1 < 0 || GridHandler.tetrisGrid[T.GridPositionX()-1,T.GridPositionY()] != null){
                    return false;
                }
            }
            if(D == Direction.Down){
                if(T.position.y - 1 < 0 || GridHandler.tetrisGrid[T.GridPositionX(),T.GridPositionY()-1] != null){
                    return false;
                }
            }
        }
        return true; 
    }

    void FillGrid(){
        foreach(Transform T in transform){
            GridHandler.tetrisGrid[T.GridPositionX(),T.GridPositionY()] = T;
            //Debug.Log($"Grid filled at {T.GridPositionX()},{T.GridPositionY()}");
        }
    }
}

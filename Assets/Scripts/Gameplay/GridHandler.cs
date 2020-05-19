using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    private static int gridWidth = 10;
    private static int gridHeight = 24;

    public static Transform[,] tetrisGrid;

    public static AudioSource lineSound;
    void Awake()
    {
        tetrisGrid = new Transform[gridWidth,gridHeight];
        lineSound = gameObject.GetComponent<AudioSource>();
    }

    void Start(){
        lineSound.volume = VolumeControl.SFX * VolumeControl.master;
    }

    private static void LineClear(int line){
        for(int i = 0;i < gridWidth; i++){
            Destroy(tetrisGrid[i,line].gameObject);
            tetrisGrid[i,line] = null;
        }
    }

    private static void DropLines(int line){
        for(int j = line;j<gridHeight;j++){
            for(int i = 0;i < gridWidth; i++){
                if(tetrisGrid[i,j] != null){ //Checks for blocks to drop
                    tetrisGrid[i,j-1] = tetrisGrid[i,j];
                    tetrisGrid[i,j] = null;
                    tetrisGrid[i,j-1].position += new Vector3(0,-1,0);
                }
            }
        }
        
    }

    private static bool CheckLine(int line){
        for(int i = 0;i < gridWidth; i++){
            if(tetrisGrid[i,line] == null) return false;
        }
        //Debug.Log($"Line formed at line {line}");
        return true;
    }

    public static bool GridCleanUp(){
        int clearedLines = 0;
        for (int i = 0; i < gridHeight; i++){
            if(CheckLine(i)){
                LineClear(i);
                DropLines(i);
                i--;
                clearedLines++;
            };
        }
        if(clearedLines > 0) {
            lineSound.Play();
            ScoreHandler.singleton.UpdateScore(Mathf.RoundToInt(Mathf.Pow(100,clearedLines/4f)));
        }
        for(int i = 0;i <gridWidth;i++){
            if (tetrisGrid[i,gridHeight-2] != null) GameLoop.GameOver();           
        }
        return(clearedLines>0);
    }
}

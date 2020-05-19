using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    private static int currentScore;
    private static int currentSpeed = 1;
    public static ScoreHandler singleton;

    void Awake(){
        if(singleton == null){
            singleton = this;
        }else{
            Destroy(this.gameObject);
        }
    }

    void Start(){
        currentScore = 0;
        UpdateScore();
    }

    public void UpdateScore(int score = 0){
        currentScore += score;
        this.gameObject.GetComponent<UnityEngine.UI.Text>().text = currentScore.ToString();
        Debug.Log($"Current score is {currentScore} \n Current speed is {currentSpeed}");
        if((float) currentScore / 50f > currentSpeed){
            currentSpeed += currentScore / 50;
            IncreaseGameSpeed(currentSpeed*0.1f);
            Debug.Log($"Score increase detected. Incresing speed to {currentSpeed}");
        }
    }

    void IncreaseGameSpeed(float speedFactor){
        BlockBehavior.DecreaseMoveTime(speedFactor);
        GameLoop.singleton.AdjustMusic(speedFactor/5f);
    }

    public int GetScore(){
        return currentScore;
    }
    
}

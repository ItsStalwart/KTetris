using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
{
    public static bool gamePaused = false;
    public static GameLoop singleton;

    private static GameObject pauseOverlay;
    private static GameObject quitOverlay;
    private static GameObject HUD;
    public static AudioSource musicAudio;

    public static int gameOverScore;

    void Awake(){
        if (singleton == null){
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }else{
            Destroy(this.gameObject);
        }
    }

    void Start(){
        Debug.Log("Ran GameLoop.Start");
        GetScreenObjects();
        musicAudio = gameObject.GetComponent<AudioSource>();
        musicAudio.volume = VolumeControl.music * VolumeControl.master;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        Debug.Log("Ran GameLoop.OnSceneLoaded");
        GetScreenObjects();
    }

    void GetScreenObjects(){
        if(SceneManager.GetActiveScene().name == "Gameplay"){
            pauseOverlay = GameObject.FindGameObjectWithTag("PauseScreen");
            Debug.Log(pauseOverlay);
            quitOverlay = GameObject.FindGameObjectWithTag("QuitConfirmScreen");
            Debug.Log(quitOverlay);
            HUD = GameObject.FindGameObjectWithTag("HUD");
            Debug.Log(HUD);
            HUD.SetActive(true);
            SetupButtons();
            pauseOverlay.SetActive(false);
            quitOverlay.SetActive(false);
        }else if(SceneManager.GetActiveScene().name == "Main Menu"){
            GameObject.Find("Start Game Button").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => StartGame());
            GameObject.FindGameObjectWithTag("PauseScreen").GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => Quit());
            GameObject.FindGameObjectWithTag("PauseScreen").SetActive(false);
        }else if(SceneManager.GetActiveScene().name == "GameOver"){
            GameObject[] gameOverButtons = GameObject.FindGameObjectsWithTag("GameOverButtons");
            gameOverButtons[0].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => Quit());
            gameOverButtons[1].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => Retry());
            gameOverButtons[2].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ReturnToMenu());
            SetScoreTexts();
        }
    }

    void SetupButtons(){
        HUD.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => PauseGame());
        pauseOverlay.GetComponentsInChildren<UnityEngine.UI.Button>()[0].onClick.AddListener(() => PromptQuit());
        pauseOverlay.GetComponentsInChildren<UnityEngine.UI.Button>()[1].onClick.AddListener(() => PauseGame());
        UnityEngine.UI.Button[] quitButtons = quitOverlay.GetComponentsInChildren<UnityEngine.UI.Button>();
        quitButtons[0].onClick.AddListener(() => CancelQuit());
        quitButtons[1].onClick.AddListener(() => ReturnToMenu());
        quitButtons[2].onClick.AddListener(() => Quit());
    }

    public static void PauseGame(){
        gamePaused ^= true;
        if(gamePaused){
            singleton.gameObject.GetComponent<AudioSource>().Pause();
        }else {
            singleton.gameObject.GetComponent<AudioSource>().Play();
            }
        pauseOverlay.SetActive(gamePaused);
    }

    void SetScoreTexts(){
        GameObject[] scoreTexts = GameObject.FindGameObjectsWithTag("PointsText");
        scoreTexts[0].GetComponent<UnityEngine.UI.Text>().text = gameOverScore.ToString();
        if(PlayerPrefs.HasKey("High Score")){ //there is a high score
            if(gameOverScore > PlayerPrefs.GetInt("High Score")){ //but smaller than last score
                scoreTexts[1].GetComponent<UnityEngine.UI.Text>().text = gameOverScore.ToString();
                PlayerPrefs.SetInt("High Score", gameOverScore);
            }else{ //but larger than last score
                scoreTexts[1].GetComponent<UnityEngine.UI.Text>().text = PlayerPrefs.GetInt("High Score").ToString();
            }
        }else{ //there is no high score: first time played
            scoreTexts[1].GetComponent<UnityEngine.UI.Text>().text = gameOverScore.ToString();
            PlayerPrefs.SetInt("High Score", gameOverScore);
        }
    }

    public static void PromptQuit(){
        quitOverlay.SetActive(true);
    }

    public static void CancelQuit(){
        quitOverlay.SetActive(false);
    }
    public static void GameOver(){
        singleton.gameObject.GetComponent<AudioSource>().Stop();
        Destroy(SpawnHandler.singleton.gameObject);
        Debug.Log("Game Over!");
        HUD.SetActive(false);
        gameOverScore = ScoreHandler.singleton.GetScore();
        FaderControl.FadeOut();
        SceneManager.LoadScene("GameOver");
    }

    public void Quit(){
        Debug.Log("GOODBYE");
        Application.Quit();
    }

    public static void Retry(){
        BlockBehavior.ResetMoveTime();
        singleton.gameObject.GetComponent<AudioSource>().pitch = 0.5f;
        singleton.gameObject.GetComponent<AudioSource>().Play();
        FaderControl.FadeOut();
        SceneManager.LoadScene("Gameplay");
    }

    public void StartGame(){
        FaderControl.FadeOut();
        SceneManager.LoadScene("Gameplay");
    }

    public static void ReturnToMenu(){
        if (gamePaused) PauseGame();
        singleton.gameObject.GetComponent<AudioSource>().pitch = 0.5f;
        singleton.gameObject.GetComponent<AudioSource>().Play();
        FaderControl.FadeOut();
        SceneManager.LoadScene("Main Menu");
    }

    public void AdjustMusic(float f){
        gameObject.GetComponent<AudioSource>().pitch += f;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(SceneManager.GetActiveScene().name == "Gameplay"){
                PauseGame();
            }   
        }
    }
}

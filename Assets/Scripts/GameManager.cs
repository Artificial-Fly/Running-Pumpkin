using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayerController playerController;
    public List<GameObject> obstacles;
    public TextMeshProUGUI scoreText, livesText, gameOverText, gameOverScore;
    public List<GameObject> livesTextChildren;
    public GameObject pausePanel, gameoverPanel;
    public List<GameObject> pauseButtons;
    public List<GameObject> gameoverButtons;
    int lives = 3;
    int score = 0;
    float difficulty = 1;
    float spawnRate=1.5f;
    public bool isGameRunning = true;
    float musicVolume=0.5f, effectsVolume=0.75f;
    public ParticleSystem HitParticle;
    bool isGameOver=false;
    public AudioSource MusicStage1, MusicStage2, MusicStage3;
    bool stage2=false, stage3=false;
    public AudioSource ButtonSound;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        UpdateScore(0);
        UpdateLives(0);
        UpdateDifficulty(0);
        StartCoroutine(SpawnObstacles());
    }
    public float GetMusicVolume(){
        return musicVolume;
    }
    public float GetSFXVolume(){
        return effectsVolume;
    }
    void OnEnable()
    {
        if (PlayerPrefs.HasKey ("musicVol")) {
            musicVolume  =  PlayerPrefs.GetFloat("musicVol");
            Debug.Log("Music Volume is");
            Debug.Log(musicVolume);
        } else {
            musicVolume  = 0.9f;
        }
        if (PlayerPrefs.HasKey ("effectsVol")) {
            effectsVolume  =  PlayerPrefs.GetFloat("effectsVol");
            Debug.Log("Effects Volume is");
            Debug.Log(effectsVolume);
        } else {
            effectsVolume  = 0.9f;
        }    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToMenu(){
        //SceneManager.LoadScene("Menu");
        StartCoroutine(GoToM());
    }
    public void RestartGame(){
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StartCoroutine(RestartG());
    }
    IEnumerator GoToM(){
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene("Menu");
    }
    IEnumerator RestartG(){
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator MakeGameOverPanelVisible(){
        /*difficulty = 0;
        UpdateDifficulty(1);*/
        yield return new WaitForSeconds(1.61f/difficulty+0.1f);
        isGameRunning = false;
        isGameOver=true;
        gameoverPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.0f);
        gameoverPanel.gameObject.SetActive(true);
        float tr = 0.0f;
        while(tr<=0.99f){
            gameoverPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, tr+0.1f);
            tr+=0.01f/difficulty+0.001f;
            yield return new WaitForSeconds(0.01f/difficulty+0.001f);
        }
        SetGamePause();
        pausePanel.gameObject.SetActive(false);
        pauseButtons[1].gameObject.SetActive(false);
        gameoverButtons[0].gameObject.SetActive(true);//retry
        gameOverText.gameObject.SetActive(true);
        gameOverScore.gameObject.SetActive(true);
    }
    void GameOver(){
        playerController.PlayDead();
        //gameoverPanel.gameObject.SetActive(true);
        StartCoroutine(ChangeMusicStage(0));
        StartCoroutine(MakeGameOverPanelVisible());
    }
    public void SetGamePause(){
        if(isGameRunning == true||isGameOver==true){
            isGameRunning = false;
            pausePanel.gameObject.SetActive(true);
            pauseButtons[0].gameObject.SetActive(false);//pause
            pauseButtons[1].gameObject.SetActive(true);//resume
            gameoverButtons[0].gameObject.SetActive(true);//retry
            pauseButtons[2].gameObject.SetActive(true);//exit to menu
            if(playerController.runParticle.isPlaying){playerController.runParticle.Pause();}
            if(playerController.fallParticle.isPlaying){playerController.fallParticle.Pause();}
            if(HitParticle.isPlaying){HitParticle.Pause();}
            if(stage3){MusicStage3.Pause();}else if(stage2){MusicStage2.Pause();}else{MusicStage1.Pause();}
        }else{
            isGameRunning = true;
            pausePanel.gameObject.SetActive(false);
            pauseButtons[0].gameObject.SetActive(true);
            pauseButtons[1].gameObject.SetActive(false);
            gameoverButtons[0].gameObject.SetActive(false);
            pauseButtons[2].gameObject.SetActive(false);
            if(playerController.runParticle.isPaused){playerController.runParticle.Play();}
            if(playerController.fallParticle.isPaused){playerController.fallParticle.Play();}
            if(HitParticle.isPaused){HitParticle.Play();}
            if(stage3){MusicStage3.UnPause();}else if(stage2){MusicStage2.UnPause();}else{MusicStage1.UnPause();}
        }
        //ParticleSystem.Pause();
    }
    public void UpdateScore(int  scoreToAdd=0){
        score += scoreToAdd;
        scoreText.text = ""+score;
        gameOverScore.text = "Score: "+score;
        if(score>25){UpdateDifficulty(3.1f);Debug.Log(difficulty);}else if(score>10){UpdateDifficulty(2.1f);Debug.Log(difficulty);}
    }
    public void UpdateLives(int livesToAdd=0){
        lives += livesToAdd;
        if(lives>2){
            livesTextChildren[0].gameObject.SetActive(true);
            livesTextChildren[1].gameObject.SetActive(true);
            livesTextChildren[2].gameObject.SetActive(true);
        }else if(lives>1){
            livesTextChildren[0].gameObject.SetActive(true);
            livesTextChildren[1].gameObject.SetActive(true);
            livesTextChildren[2].gameObject.SetActive(false);
        }else if(lives>0){
            livesTextChildren[0].gameObject.SetActive(true);
            livesTextChildren[1].gameObject.SetActive(false);
            livesTextChildren[2].gameObject.SetActive(false);
        }else{
            livesTextChildren[0].gameObject.SetActive(false);
            livesTextChildren[1].gameObject.SetActive(false);
            livesTextChildren[2].gameObject.SetActive(false);
            GameOver();
        }
        if(livesToAdd!=0){
            HitParticle.Play();
            StartCoroutine(ShortInvisibility());
        }
        //livesText.text = "Lives: "+lives;
    }
    public float UpdateDifficulty(float difficultyToSet=0){
        if(difficultyToSet>0){
            difficulty=difficultyToSet;
            playerController.UpdateAnimationTime(difficulty); 
            if(difficulty>=3.0f&&!stage3){
                StartCoroutine(ChangeMusicStage(3));
                stage3 = true;
                Debug.Log("Stage 3");
            }else if(difficulty>=2.0f&&!stage2){
                StartCoroutine(ChangeMusicStage(2));
                stage2 = true;
                Debug.Log("Stage 2");
            }
        }
        return difficulty;
    }
    public bool CheckJump(){
        return playerController.GetIsJumping();
    }
    public bool CheckDash(){
        return playerController.GetIsDashing();
    }
    public float GetPLayerPosX(){
        return GameObject.FindWithTag("Player").transform.position.x;
    }
    public float GetPLayerPosY(){
        return GameObject.FindWithTag("Player").transform.position.y;
    }
    IEnumerator SpawnObstacles(){
        while(true){
            if(isGameRunning){
                spawnRate = (playerController.GetDefaultCooldownTime())/difficulty+0.25f;
                //Debug.Log("Spawn Rate is "+spawnRate);
                int index = Random.Range(0,obstacles.Count);
                int obstacleNumberForWave = Random.Range(1,3);
                if(obstacleNumberForWave==3){
                    int randPos = Random.Range(-3,4);
                    for(int i=0; i<3; i++){
                        //Debug.Log("Spawn Obstacle");
                        Instantiate(obstacles[index], new Vector3(randPos, -9.5f, 0), obstacles[index].transform.rotation);
                        int newRandPos = Random.Range(-3,4);
                        while(newRandPos==randPos){
                            newRandPos = Random.Range(-3,4);
                        }
                        randPos = newRandPos;
                        yield return new WaitForSeconds(spawnRate+Random.Range(-0.25f,0.25f));
                    }
                }else if(obstacleNumberForWave==2){
                    int randPos = Random.Range(-3,4);
                    for(int i=0; i<2; i++){
                        //Debug.Log("Spawn Obstacle");
                        Instantiate(obstacles[index], new Vector3(randPos, -9.5f, 0), obstacles[index].transform.rotation);
                        int newRandPos = Random.Range(-3,4);
                        while(newRandPos==randPos){
                            newRandPos = Random.Range(-3,4);
                        }
                        randPos = newRandPos;
                        yield return new WaitForSeconds(spawnRate+Random.Range(-0.25f,0.25f));
                    }
                }else{
                    //Debug.Log("Spawn Obstacle");
                    int randPos = Random.Range(-3,4);
                    Instantiate(obstacles[index], new Vector3(randPos, -9.5f, 0), obstacles[index].transform.rotation);
                    yield return new WaitForSeconds(spawnRate);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator ShortInvisibility(){
        float invisibilityDuration = playerController.GetDefaultCooldownTime()*1.5f;
        playerController.isPlayerinvisible=true;
        Transform t = GameObject.FindWithTag("Player").transform.GetChild(0);
        SpriteRenderer characterSprite = t.GetComponent<SpriteRenderer>();
        characterSprite.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(invisibilityDuration);
        playerController.isPlayerinvisible=false;
        characterSprite.color = new Color(1f, 1f, 1f, 1.0f);
    }
    IEnumerator ChangeMusicStage(int nextStage){
        if(nextStage==2){
            float percent = 0.00f;
            while(MusicStage1.volume>0){
                MusicStage1.volume = Mathf.Lerp(musicVolume,0,percent);
                percent += Time.deltaTime / 0.05f;
                yield return null;
            }
            MusicStage1.Pause();
            if(!MusicStage2.isPlaying){
                MusicStage2.Play();
            }
            MusicStage2.UnPause();
            percent = 0.00f;
            while(MusicStage2.volume<musicVolume){
                MusicStage2.volume = Mathf.Lerp(0,musicVolume,percent);
                percent += Time.deltaTime / 0.05f;
                yield return null;
            }
            yield return new WaitForSeconds(0.0f);
        }else if(nextStage==3){
            float percent = 0.00f;
            while(MusicStage2.volume>0){
                MusicStage2.volume = Mathf.Lerp(musicVolume,0,percent);
                percent += Time.deltaTime / 0.05f;
                yield return null;
            }
            MusicStage2.Pause();
            if(!MusicStage3.isPlaying){
                MusicStage3.Play();
            }
            MusicStage3.UnPause();
            percent = 0.00f;
            while(MusicStage3.volume<musicVolume){
                MusicStage3.volume = Mathf.Lerp(0,musicVolume,percent);
                percent += Time.deltaTime / 0.05f;
                yield return null;
            }
            yield return new WaitForSeconds(0.0f);
        }else{
            float percent = 0.00f;
            while(MusicStage1.volume>0||MusicStage2.volume>0||MusicStage3.volume>0){
                MusicStage1.volume = Mathf.Lerp(musicVolume,0,percent);
                MusicStage2.volume = Mathf.Lerp(musicVolume,0,percent);
                MusicStage3.volume = Mathf.Lerp(musicVolume,0,percent);
                percent += Time.deltaTime / 0.05f;
                yield return null;
            }
            MusicStage1.Pause();
            MusicStage2.Pause();
            MusicStage3.Pause();
        }
        
    }
    public void ButtonSoundPlay(){
        ButtonSound.Play();
    }    
}

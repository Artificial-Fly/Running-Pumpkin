using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioSource JumpSource, DashSource, MoveSource, DeathSource;
    bool isJumping = false;
    bool isDashing = false;
    public GameObject charSprite;
    float horizontalLimit = 3.0f;
    float defaultCooldownTime=1.0f;
    GameManager gameManager;
    public bool isPlayerinvisible=false;
    private Animator playerAnim;
    public ParticleSystem fallParticle;
    public ParticleSystem runParticle;
    bool AnimIncl = false;
    bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        playerAnim = charSprite.GetComponent<Animator>();
        AnimIncl = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.isGameRunning){
            if((Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W))&&!(isDashing||isJumping)&&(!isDead)){
                StartCoroutine(JumpAction());
            }else if((Input.GetKeyDown(KeyCode.DownArrow)||Input.GetKeyDown(KeyCode.S))&&!(isDashing||isJumping)&&(!isDead)){
                StartCoroutine(DashAction());
            }else if((Input.GetKeyDown(KeyCode.RightArrow)||Input.GetKeyDown(KeyCode.D))&&transform.position.x<(horizontalLimit)&&(!isDead)){
                MoveSource.Play();
                transform.Translate(Vector3.right*1.5f);
            }else if((Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.A))&&transform.position.x>(-horizontalLimit)&&(!isDead)){
                MoveSource.Play();
                transform.Translate(Vector3.right*(-1.5f));
            }
        }
        if(!gameManager.isGameRunning){playerAnim.speed = 0;}else{playerAnim.speed = 1;}
    }
    public bool checkJump(){
        return isJumping;
    }
    public bool checkDash(){
        return isDashing;
    }

    IEnumerator JumpAction(){
        isJumping = true;
        runParticle.Stop();
        JumpSource.Play();
        playerAnim.SetBool("Jump", true);
        charSprite.transform.Translate(Vector3.up*0.35f);
        yield return new WaitForSeconds(defaultCooldownTime/(gameManager.UpdateDifficulty(0)));
        fallParticle.Play();
        playerAnim.SetBool("Jump", false);
        charSprite.transform.Translate(Vector3.up*(-0.35f));
        isJumping = false;
        runParticle.Play();
    }
    IEnumerator DashAction(){
        isDashing = true;
        runParticle.Stop();
        fallParticle.Play();
        DashSource.Play();
        playerAnim.SetBool("Dash", true);
        transform.Translate((Vector3.up*(-0.5f)));
        yield return new WaitForSeconds((defaultCooldownTime+0.5f)/(gameManager.UpdateDifficulty(0)));
        playerAnim.SetBool("Dash", false);
        transform.Translate((Vector3.up*(+0.5f)));
        isDashing = false;
        runParticle.Play();
    }
    public bool GetIsJumping(){
        return isJumping;
    }
    public bool GetIsDashing(){
        return isDashing;
    }
    public float GetDefaultCooldownTime(){
        return defaultCooldownTime;
    }
    public void UpdateAnimationTime(float inputVal){
        if(AnimIncl){
            float animSpeed = 1.5f/((defaultCooldownTime+0.5f)/(inputVal));
            playerAnim.SetFloat("speed", animSpeed);
        }
        
    }
    public void PlayDead(){
        isDead=true;
        playerAnim.SetBool("Dash", false);
        playerAnim.SetBool("Jump", false);
        playerAnim.SetBool("Dead", true);
        DeathSource.Play();
        runParticle.Stop();
        fallParticle.Play();
    }
}

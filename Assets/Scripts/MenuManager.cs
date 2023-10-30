using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //public AudioClip loopAudio;
    public AudioSource startAudio, loopAudio, effectAudio;
    //public GameObject camera;
    bool menuStatus=true;
    public List<GameObject> InterfaceElementsAsGO;
    float musicVol, effectsVol;
    public AudioSource ButtonSound;
    // Start is called before the first frame update
    void Start()
    {
        SetMusicVolume();
        SetEffectsVolume();
        //StartCoroutine(changeClipToLoopVersion());
        swapMusicTracks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame(){
        //SceneManager.LoadScene("GameRun");
        StartCoroutine(StartG());
    }

    public void ExitGame(){
        //Debug.Log("Exiting the Game, goodbye!");
        //Application.Quit();
        StartCoroutine(ExitG());
    }
    IEnumerator StartG(){
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene("GameRun");
    }
    IEnumerator ExitG(){
        yield return new WaitForSeconds(1.1f);
        Debug.Log("Exiting the Game, goodbye!");
        Application.Quit();
    }
    public void GoToFromContributors(){
        for(int i=0; i<6; i++){
            InterfaceElementsAsGO[i].gameObject.SetActive(!menuStatus);
        }
        for(int i=6; i<InterfaceElementsAsGO.Count;i++){
            InterfaceElementsAsGO[i].gameObject.SetActive(menuStatus);
        }
        menuStatus = (!menuStatus);
    }
    public void SetMusicVolume(){
        Slider musicSlider = GameObject.FindWithTag("MusicSlider").GetComponent<Slider>();//doesn't work with innactive objects
        musicVol = musicSlider.value/1.5f;
        //AudioSource musicSource = camera.GetComponent<AudioSource>();
        startAudio.volume = musicVol;
        loopAudio.volume = musicVol;
    }
    public void SetEffectsVolume(){
        Slider effectsSlider = GameObject.FindWithTag("EffectsSlider").GetComponent<Slider>();
        effectsVol = effectsSlider.value;
        effectAudio.volume = effectsVol;
        ButtonSound.volume = effectsVol;
    }
    public void PlayEffectSound(){
        effectAudio.Play();
    }
    void OnDisable()
    {
        PlayerPrefs.SetFloat("musicVol", musicVol);
        PlayerPrefs.SetFloat("effectsVol", effectsVol);
    }
    public void OpenLink(string link="https://www.google.com/"){
        Application.OpenURL(link);
    }
    /*IEnumerator changeClipToLoopVersion(){
        AudioSource musicSource = camera.GetComponent<AudioSource>();
        musicSource.Play();
        yield return new WaitForSeconds(musicSource.clip.length-1.4f);//musicSource.clip.length-0.01f
        musicSource.clip = loopAudio;
        musicSource.loop = true;
        musicSource.Play();
    }*/
    void swapMusicTracks(){
        //AudioSource currentTrack = startAudio;
        //AudioSource targetTrack = loopAudio;
        if(!startAudio.isPlaying){
            if(loopAudio.isPlaying){
                return;
            }else{
                loopAudio.Play();
            }
        }else{
            StartCoroutine(MixAudioSources());
        }

    }
    IEnumerator MixAudioSources(){
        yield return new WaitForSeconds(startAudio.clip.length-0.5f);//startAudio.clip.length-0.05f
        float percent = 0.00f;
        while(startAudio.volume>0){
            startAudio.volume = Mathf.Lerp(musicVol,0,percent);
            percent += Time.deltaTime / 0.05f;
            yield return null;
        }
        startAudio.Pause();
        if(!loopAudio.isPlaying){
            loopAudio.Play();
        }
        loopAudio.UnPause();
        percent = 0.00f;
        while(loopAudio.volume<musicVol){
            loopAudio.volume = Mathf.Lerp(0,musicVol,percent);
            percent += Time.deltaTime / 0.05f;
            yield return null;
        }
        yield return new WaitForSeconds(0.0f);
    }
    public void ButtonSoundPlay(){
        ButtonSound.Play();
    } 
}

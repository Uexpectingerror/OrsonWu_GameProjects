using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{


    [HideInInspector] public bool canPlaySwim = true;
    [HideInInspector] public bool canPlayWalk = true;
    [HideInInspector] public AudioSource audioSource = null;
    [HideInInspector] public IEnumerator SwimLoop;
    [HideInInspector] public IEnumerator WalkLoop;


    [SerializeField] private AudioClip[] audioClips = null;




    void Start()
    {
        audioSource = GetComponent<AudioSource>();


    }


    public void PlayOneShot(int i)
    {
        audioSource.PlayOneShot(audioClips[i]);
    }


    public void StartLooping(int i)
    {
        audioSource.clip = audioClips[i];
        //audioSource.loop = true;
        audioSource.Play();
    }


    public void SwimLoopStart()
    {
        canPlaySwim = false;

        SwimLoop = SwimLoopCheck();
        StartCoroutine(SwimLoop);

    }


    IEnumerator SwimLoopCheck()
    {
        PlayOneShot(4);

        yield return new WaitForSeconds(audioClips[4].length);

        canPlaySwim = true;

    }


    public void WalkLoopStart()
    {
        print("WalkLoopStarted");
        canPlayWalk = false;

        WalkLoop = WalkLoopCheck();
        StartCoroutine(WalkLoop);
        print("CoRoutStarted");
    }


    IEnumerator WalkLoopCheck()
    {
        int randomStep = Random.Range(0, 3);

        PlayOneShot(randomStep);
        print("playing footstep " + randomStep);

        yield return new WaitForSeconds(audioClips[randomStep].length * 4);

        print("NEWFOOTSTEPREADY");
        WalkLoop = null;
        canPlayWalk = true;
    }

}

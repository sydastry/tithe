using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class Utility_Functions : MonoBehaviour
{
    public Flowchart flowchart;
    public GameObject Backgrounds;

    public float resetTimerMax;
    public float resetTimer;

    public float needleSpeed;

    public EventInstance broadcastAdInst;
    public EventInstance broadcastEvacInst;
    public EventInstance broadcastIntroInst;
    public EventInstance broadcastDanceInst;
    public EventInstance masterDialInst;
    public EventInstance puppetMusicInst;
    public EventInstance arrivalMusicInst;

    public EventReference broadcastAdRef;
    public EventReference broadcastEvacRef;
    public EventReference broadcastIntroRef;
    public EventReference broadcastDanceRef;
    public EventReference masterDialRef;
    public EventReference puppetMusicRef;
    public EventReference arrivalMusicRef;

    private void Start()
    {

        broadcastAdInst = FMODUnity.RuntimeManager.CreateInstance(broadcastAdRef);
        broadcastEvacInst = FMODUnity.RuntimeManager.CreateInstance(broadcastEvacRef);
        broadcastIntroInst = FMODUnity.RuntimeManager.CreateInstance(broadcastIntroRef);
        broadcastDanceInst = FMODUnity.RuntimeManager.CreateInstance(broadcastDanceRef);
        masterDialInst = FMODUnity.RuntimeManager.CreateInstance(masterDialRef);
        puppetMusicInst = FMODUnity.RuntimeManager.CreateInstance(puppetMusicRef);
        arrivalMusicInst = FMODUnity.RuntimeManager.CreateInstance(arrivalMusicRef);
    }

    private void Update()
    {
        // Get parameters
        float dial = flowchart.GetFloatVariable("Haunted_UI_Dial");
        float section = flowchart.GetFloatVariable("Section_Switch");

        // Set parameters
        broadcastAdInst.setParameterByName("Haunted_UI_Dial", dial);
        broadcastEvacInst.setParameterByName("Haunted_UI_Dial", dial);
        broadcastIntroInst.setParameterByName("Haunted_UI_Dial", dial);
        broadcastDanceInst.setParameterByName("Haunted_UI_Dial", dial);
        masterDialInst.setParameterByName("Haunted_UI_Dial", dial);
        puppetMusicInst.setParameterByName("Haunted_UI_Dial", dial);
        arrivalMusicInst.setParameterByName("Section_Switch", section);

        //timer stuff
        if (Input.anyKeyDown)
        {
            resetTimer = 0;
        }
        resetTimer += Time.deltaTime;
        if (resetTimer >= resetTimerMax) 
        {
            resetTimer = 0;
            flowchart.SetBooleanVariable("is_restarting", true);
            flowchart.ExecuteBlock("Manual_Restart");
        }
        
        //manual restart: SHIFT + TAB
        if (Input.GetKey(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            resetTimer = 0;
            flowchart.SetBooleanVariable("is_restarting", true);
            flowchart.ExecuteBlock("Manual_Restart");
        }
        
        
        //dial mover:
        float hauntedDial = flowchart.GetFloatVariable("Haunted_UI_Dial");
        float hauntedNeedle = flowchart.GetFloatVariable("Haunted_Needle");
        if (hauntedNeedle < hauntedDial)
        {
            hauntedNeedle += needleSpeed;
            Debug.Log("haunted needle is: " + hauntedNeedle);
        }
        else if (hauntedNeedle > hauntedDial)
        {
            hauntedNeedle += -needleSpeed;
            Debug.Log("haunted needle is: " + hauntedNeedle);
        }

        flowchart.SetFloatVariable("Haunted_Needle", hauntedNeedle);
    }

    public void StopAllAudio()
    {
        stopAmb("dial");
        stopAmb("evac");
        stopAmb("dance");
        stopAmb("arrival");
        stopAmb("intro");
        stopAmb("ad");
        /*
        FMOD.Studio.Bus playerBus = FMODUnity.RuntimeManager.GetBus("bus:/player");
        //playerBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        */
    }

    public void SetBackground()
    {
        string target = flowchart.GetStringVariable("background");
        foreach (Transform child in Backgrounds.transform)
        {
            if (child.gameObject.name == target)
            {
                child.gameObject.SetActive(true);
            }
            else { child.gameObject.SetActive(false); }
        }
    }

    // Public functions to play the sounds
    // I use these in conjunction with the Fungus flowchart
    // to call FMOD-controlled audio from specific places in the narrative.

    public void playSound(string soundToPlay)
    {
        string eventFolder = "event:/";
        string result = eventFolder + soundToPlay;
        FMODUnity.RuntimeManager.PlayOneShot(result);
    }

    public void startAmb(string ambToPlay)
    {
        switch (ambToPlay)
        {
            case "ad":
                broadcastAdInst.start();
                break;
            case "evac":
                broadcastEvacInst.start();
                break;
            case "intro":
                broadcastIntroInst.start();
                break;
            case "dance":
                broadcastDanceInst.start();
                break;
            case "dial":
                masterDialInst.start();
                break;
            case "arrival":
                arrivalMusicInst.start();
                break;
            default:
                Debug.Log("Ambience string " + ambToPlay + " not found.");
                break;
        }
    }

    public void stopAmb(string ambToStop)
    {
        switch (ambToStop)
        {
            case "ad":
                broadcastAdInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case "evac":
                broadcastEvacInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case "intro":
                broadcastIntroInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case "dance":
                broadcastDanceInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case "dial":
                masterDialInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            case "arrival":
                arrivalMusicInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;
            default:
                Debug.Log("Ambience string " + ambToStop + " not found.");
                break;
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class FMODAudioManager : MonoBehaviour
{
    //================ SINGLETON LOGIC ================
        public static FMODAudioManager instance = null;
        public void SingletonCheck()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

    //================ VARIABLE DECLARATIONS ================
    
        // FMOD
        [SerializeField] private String pathToEventReference = "event:/music";
        private FMODUnity.EventReference musicEventReference;
        private FMOD.Studio.EventInstance musicEventInstance;
        public float fadeVolume = 1f;

        [Header("Debug")]
        [SerializeField] [Range(0f, 1f)] private float overrideInterpolation = 0f;
        [SerializeField] [Range(0, 8)] private float aldeiaBlend;

        public void SetFadeVolume(float _fadeVolume){fadeVolume = _fadeVolume;}

        public void Reset()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/bubble_pop");
            //FMODAudioManager.MusicEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }

        /*  public void Play()
        {
            fadeVolume = 1f;
            musicEventInstance.start();
        } */

        private void GetAldeiaBlend(float _aldeiaBlend){aldeiaBlend = _aldeiaBlend;}

        //================ FMOD Oneshots ================

        public void BubblePop()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/bubble_pop");
        }

    //================ MONOBEHAVIOUR FUNCTIONS ================

        public void Awake()
        {
            SingletonCheck();
        }

        public void Start()
        {
            // FMOD Setup
            musicEventReference = FMODUnity.RuntimeManager.PathToEventReference(pathToEventReference);
            musicEventInstance = FMODUnity.RuntimeManager.CreateInstance(musicEventReference);
            musicEventInstance.start();
        }

        public void Update()
        {
            musicEventInstance.setVolume(fadeVolume);
            musicEventInstance.setParameterByName("aldeiaBlend", aldeiaBlend);

            //Gestures
            /* MusicEventInstance.setParameterByName("crouch", Mathf.Lerp(
                this.GetComponent<Crouch>().value,
                crouch,
                overrideInterpolation
            )); */
        }
}
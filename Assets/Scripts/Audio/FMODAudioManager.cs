using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Events;
using UnityEngine.InputSystem;

/*----------------------------------------------------------------{
    Position of controllers is currently handled using C# Events.
    The functions subscribed to Main.Events receive broadcast 
    references to the controller Transforms

    Button Input is handled using Action Input System. The Player
    Input component inside the FMODAudioManager GameObject inkoves
    C# Events for each action defined in the FMODInput Asset.

    TO-DO:
    {
        Receive direct position of controllers using Input System and
        XR Interaction Toolkit, instead of getting the Transforms of
        the character's hands.
    }

}----------------------------------------------------------------*/

public class FMODAudioManager : MonoBehaviour
{   
    //================ VARIABLE DECLARATIONS ================

    // FMOD
    private static FMODUnity.EventReference MusicEventReference;
    private static FMOD.Studio.EventInstance MusicEventInstance;

    // Controller Transforms (Temporary implementation using Events)
    [SerializeField] private Transform headsetTransform;
    [SerializeField] private Transform leftControllerTransform;
    [SerializeField] private Transform rightControllerTransform;
    [SerializeField] private Transform rightFootTransform;

    //MEDIDAS
    private float standingHeight;
    private float wingspan;
    
    // Positional
    [SerializeField] private bool overrideSettings;
    [SerializeField] private bool calibratingWingspan;
    [SerializeField] [Range(0f, 3)] private int stage = 1;
    [SerializeField] [Range(0f, 1f)] private float volume = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float oneiric;
    [SerializeField] [Range(0f, 1.2f)] private float height;
    [SerializeField] [Range(0f, 1f)] private float flute;
    [SerializeField] [Range(0f, 5f)] private float intensity;
    [SerializeField] [Range(0f, 1f)] private float marimba;

    // Intesity Detection
    [SerializeField] private int intensitySampleSize = 120;
    [SerializeField] private double intensityScale = 20;
    [SerializeField] private double intensityMinimum = 1;

    private Queue<double> displacementQueue = new Queue<double>();
    private Vector3 currentHeadPosition;
    private Vector3 currentRightPosition;
    private Vector3 currentLeftPosition;
    
    private Vector3 lastHeadPosition;
    private Vector3 lastRightPosition;
    private Vector3 lastLeftPosition;

    private double currentDisplacement;
    private double oldestDisplacement;
    private double averageDisplacement;

    //================ MONOBEHAVIOUR FUNCTIONS ================

    void Start()
    {
        // FMOD Setup
        MusicEventReference = FMODUnity.RuntimeManager.PathToEventReference("event:/music");
        MusicEventInstance = FMODUnity.RuntimeManager.CreateInstance(MusicEventReference);
        MusicEventInstance.start();

        currentHeadPosition = rightControllerTransform.position;
        currentLeftPosition = leftControllerTransform.position;
        currentRightPosition = rightControllerTransform.position;

        standingHeight = Vector3.Distance(headsetTransform.position, rightFootTransform.position);
    }

    void Update()
    {
        if (calibratingWingspan)
        {
            wingspan = Vector3.Distance(leftControllerTransform.position, rightControllerTransform.position);
            //Debug.Log($"Wingspan = {wingspan}");
        }
        switch (stage)
        {
            case 1: { intensityMinimum = 0; intensityScale = 10; Stage1(); break; }
            case 2: { intensityMinimum = 1; intensityScale = 20; Stage2(); break; }
        }

        SetVolume(volume);
        SetOneiric(oneiric);
        SetHeight(height);
        SetFlute(flute);
        SetIntensity(intensity);
        //SetMarimba(marimba);       
    }
    
    //================ FMOD Parameter Setters ================

    public static void SetVolume(double volume)
    {
        MusicEventInstance.setVolume((float)volume);
    }
    
    public static void SetOneiric(double value)
    {
        MusicEventInstance.setParameterByName("oneiric", (float)value);
    }

    public static void SetHeight(double value)
    {
        MusicEventInstance.setParameterByName("height", (float)value);
    }
    
    public static void SetFlute(double value)
    {
        MusicEventInstance.setParameterByName("flute", (float)value);
    }

    public static void SetIntensity(double value)
    {
        MusicEventInstance.setParameterByName("intensity", (float)value);
    }
 
    public static void SetMarimba(double value)
    {
        MusicEventInstance.setParameterByName("marimba", (float)value);
    }

    public static void SetAcceleration(double value)
    {
        MusicEventInstance.setParameterByName("acceleration", (float)value);
    }

    //================ FMOD Oneshots ================

    public static void Thunder(InputAction.CallbackContext callbackContext)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder");
    }

    private void Stage1()
    {
        if (!overrideSettings)
        {
            //================ HEIGHT ================
            height = Vector3.Distance(headsetTransform.position, rightFootTransform.position) / standingHeight;

            //================ INTENSITY ================
            lastHeadPosition = currentHeadPosition;
            lastLeftPosition = currentLeftPosition;
            lastRightPosition = currentRightPosition;

            currentHeadPosition = rightControllerTransform.position;
            currentLeftPosition = leftControllerTransform.position;
            currentRightPosition = rightControllerTransform.position;

            currentDisplacement =   Vector3.Magnitude(currentHeadPosition - lastHeadPosition) +
                                    Vector3.Magnitude(currentLeftPosition - lastLeftPosition) +
                                    Vector3.Magnitude(currentRightPosition - lastRightPosition);

            displacementQueue.Enqueue(currentDisplacement);
            averageDisplacement += currentDisplacement / intensitySampleSize;

            while (displacementQueue.Count > intensitySampleSize)
            {
                oldestDisplacement = displacementQueue.Dequeue();
                averageDisplacement -= oldestDisplacement / intensitySampleSize;
            }
            marimba = (float) Math.Round((averageDisplacement * intensityScale) - intensityMinimum, 3);
        }
    }
    
    private void Stage2()
    {
        if (!overrideSettings)
        {
            //================ MARIMBA ================
            marimba = 1 - (Vector3.Distance(leftControllerTransform.position, rightControllerTransform.position) / wingspan);

            //================ HEIGHT ================
            height = Vector3.Distance(headsetTransform.position, rightFootTransform.position) / standingHeight;

            //================ FLUTE ================
            flute = (leftControllerTransform.position.y + rightControllerTransform.position.y)/2 - (headsetTransform.position.y - standingHeight/3);

            //================ INTENSITY ================
            lastHeadPosition = currentHeadPosition;
            lastLeftPosition = currentLeftPosition;
            lastRightPosition = currentRightPosition;

            currentHeadPosition = rightControllerTransform.position;
            currentLeftPosition = leftControllerTransform.position;
            currentRightPosition = rightControllerTransform.position;

            currentDisplacement =   Vector3.Magnitude(currentHeadPosition - lastHeadPosition) +
                                    Vector3.Magnitude(currentLeftPosition - lastLeftPosition) +
                                    Vector3.Magnitude(currentRightPosition - lastRightPosition);

            displacementQueue.Enqueue(currentDisplacement);
            averageDisplacement += currentDisplacement / intensitySampleSize;

            while (displacementQueue.Count > intensitySampleSize)
            {
                oldestDisplacement = displacementQueue.Dequeue();
                averageDisplacement -= oldestDisplacement / intensitySampleSize;
            }
            intensity = (float) Math.Round((averageDisplacement * intensityScale) - intensityMinimum, 3);
            //Debug.Log(intensity);
        }
    }
}
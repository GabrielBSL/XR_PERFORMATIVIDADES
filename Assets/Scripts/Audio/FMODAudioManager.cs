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
    private Transform headsetTransform;
    private Transform leftControllerTransform;
    private Transform rightControllerTransform;
    
    // Positional
    [SerializeField] private bool manualSettings;
    [SerializeField] [Range(0f, 1f)] private float distance;
    [SerializeField] [Range(0f, 1f)] private float height;
    [SerializeField] [Range(0f, 5f)] private double intensity;

    // Actions (using Input System)
    private FMODInput FMODInputInstance;

    // Intesity Detection
    [SerializeField] private int sampleSize = 60;
    [SerializeField] private double scale = 10;

    //MEDIDAS]
    [SerializeField] private float maxWingspan; //= 1.752f
    [SerializeField] private float maxHeight;
    [SerializeField] private float groundHeight;

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

    private void Awake()
    {
        FMODInputInstance = new FMODInput();
    }

    private void OnEnable()
    {
        MainEventsManager.headTransformUpdate += ReceiveHeadsetTransform;
        MainEventsManager.leftHandTargetTransformUpdate += ReceiveLeftControllerTransform;
        MainEventsManager.rightHandTargetTransformUpdate += ReceiveRightControllerTransform;

        FMODInputInstance.FMODGestures.Thunder.Enable();
        FMODInputInstance.FMODGestures.Thunder.performed += Thunder;
    }

    private void OnDisable()
    {
        MainEventsManager.headTransformUpdate -= ReceiveHeadsetTransform;
        MainEventsManager.leftHandTargetTransformUpdate -= ReceiveLeftControllerTransform;
        MainEventsManager.rightHandTargetTransformUpdate -= ReceiveRightControllerTransform;

        FMODInputInstance.FMODGestures.Thunder.Disable();
        FMODInputInstance.FMODGestures.Thunder.performed -= Thunder;
    }

    void Start()
    {
        // FMOD Setup
        MusicEventReference = FMODUnity.RuntimeManager.PathToEventReference("event:/music_magnet");
        MusicEventInstance = FMODUnity.RuntimeManager.CreateInstance(MusicEventReference);
        MusicEventInstance.start();

        currentHeadPosition = rightControllerTransform.position;
        currentLeftPosition = leftControllerTransform.position;
        currentRightPosition = rightControllerTransform.position;
    }

    void Update()
    {
        // remover isso
        
        //maxHeight = 2.43f;
        //groundHeight = 2.0f;

        //================ INTENSITY ================
        if (!manualSettings)
        {
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
            averageDisplacement += currentDisplacement / sampleSize;

            while (displacementQueue.Count > sampleSize)
            {
                oldestDisplacement = displacementQueue.Dequeue();
                averageDisplacement -= oldestDisplacement / sampleSize;
            }
            
            Debug.Log($"averageDisplacement = {averageDisplacement}");
            intensity = Math.Round(averageDisplacement * scale, 3);
        }
        Debug.Log($"intensity = {intensity}");
        SetIntensity(intensity);

        //================ DISTANCE BETWEEN CONTROLLERS ================
        if (!manualSettings)
        {
            distance = Vector3.Distance(leftControllerTransform.position, rightControllerTransform.position) / maxWingspan;
            Debug.Log($"distance = {distance}");
        }
        SetOneiric(distance);

        //================ HEADSET HEIGHT ================
        Debug.Log($"headset_height = {headsetTransform.position.y}");
        if (!manualSettings)
        {
            /*
            Debug.Log($"(maxHeight) = {maxHeight}");    
            Debug.Log($"(groundHeight) = {groundHeight}");
            Debug.Log($"(maxHeight - groundHeight) = {maxHeight - groundHeight}");
            */

            height = (headsetTransform.position.y - groundHeight) / (maxHeight - groundHeight);
            Debug.Log($"height = {height}");
        }
        SetHeight(height);
    }    
    
    //================ Unity Transforms Getters ================

    
    private void ReceiveHeadsetTransform(Transform _headsetTransform)
    {
        headsetTransform = _headsetTransform;
    }

    private void ReceiveLeftControllerTransform(Transform _leftControllerTransform)
    {
        leftControllerTransform = _leftControllerTransform;
    }
    
    private void ReceiveRightControllerTransform(Transform _rightControllerTransform)
    {
        rightControllerTransform = _rightControllerTransform;
    }

    //================ FMOD Parameter Setters ================

    public static void SetVolume(double volume)
    {
        MusicEventInstance.setVolume((float)volume);
    }

    public static void SetIntensity(double value)
    {
        MusicEventInstance.setParameterByName("intensity", (float)value);
    }

    public static void SetOneiric(double value)
    {
        MusicEventInstance.setParameterByName("oneiric", (float)value);
    }

    public static void SetHeight(double value)
    {
        MusicEventInstance.setParameterByName("height", (float)value);
    }

    //================ FMOD Oneshots ================

    public static void Thunder(InputAction.CallbackContext callbackContext)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder");
    }
}
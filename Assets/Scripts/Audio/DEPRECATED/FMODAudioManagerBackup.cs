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

public class FMODAudioManagerBackup : MonoBehaviour
{   
    //================ VARIABLE DECLARATIONS ================

    // FMOD
    private static FMODUnity.EventReference MusicEventReference;
    private static FMOD.Studio.EventInstance MusicEventInstance;

    // Controller Transforms (Temporary implementation using Events)
    private Transform leftControllerTransform;
    private Transform rightControllerTransform;
    
    // Positional
    [SerializeField] private bool manualSettings;
    [SerializeField] [Range(0f, 1f)] private float distance;
    [SerializeField] [Range(0f, 1f)] private float height;

    // Actions (using Input System)
    private FMODInput FMODInputInstance;

    //================ MONOBEHAVIOUR FUNCTIONS ================

    private void Awake()
    {
        FMODInputInstance = new FMODInput();
    }

    private void OnEnable()
    {
        MainEventsManager.leftHandTransformUpdate += ReceiveLeftControllerTransform;
        MainEventsManager.rightHandTransformUpdate += ReceiveRightControllerTransform;

        FMODInputInstance.FMODGestures.Thunder.Enable();
        FMODInputInstance.FMODGestures.Thunder.performed += Thunder;
    }

    private void OnDisable()
    {
        MainEventsManager.leftHandTransformUpdate -= ReceiveLeftControllerTransform;
        MainEventsManager.rightHandTransformUpdate -= ReceiveRightControllerTransform;

        FMODInputInstance.FMODGestures.Thunder.Disable();
        FMODInputInstance.FMODGestures.Thunder.performed -= Thunder;
    }

    void Start()
    {
        // FMOD Setup
        MusicEventReference = FMODUnity.RuntimeManager.PathToEventReference("event:/music");
        MusicEventInstance = FMODUnity.RuntimeManager.CreateInstance(MusicEventReference);
        MusicEventInstance.start();
    }

    void Update()
    {
        //================ DISTANCE BETWEEN CONTROLLERS ================
        if (!manualSettings)
        {
            distance = Vector3.Distance(leftControllerTransform.position, rightControllerTransform.position);
            //Debug.Log($"distance = {distance}");
        }
        SetOnirico(distance);

        //================ CONTROLLER HEIGHT ================
        if (!manualSettings)
        {
            height = ((leftControllerTransform.position.y + rightControllerTransform.position.y) / 2) - 1.25f;
        }
        SetWind(height);
    }    
    
    //================ Unity Transforms Getters ================

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

    public static void SetOnirico(double value)
    {
        MusicEventInstance.setParameterByName("onirico", (float)value);
    }

    public static void SetWind(double value)
    {
        MusicEventInstance.setParameterByName("wind", (float)value);
    }

    //================ FMOD Oneshots ================

    public static void Thunder(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("Thunder");
        FMODUnity.RuntimeManager.PlayOneShot("event:/thunder");
    }
}
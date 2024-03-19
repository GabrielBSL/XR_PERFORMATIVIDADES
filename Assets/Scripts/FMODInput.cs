//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/FMODInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @FMODInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @FMODInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""FMODInput"",
    ""maps"": [
        {
            ""name"": ""FMOD Gestures"",
            ""id"": ""d4441176-b3c7-4045-a263-e93a36a15d45"",
            ""actions"": [
                {
                    ""name"": ""Thunder"",
                    ""type"": ""Button"",
                    ""id"": ""7caac967-098c-44fd-8338-0780c028cb73"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4293af24-02c0-47df-aa4c-8f349745ad89"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thunder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a14d6918-186e-404b-990b-aee17a205839"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thunder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9f219c57-80fa-4052-8926-db2eb6d91f05"",
                    ""path"": ""<XRController>{RightHand}/{PrimaryButton}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic XR Controller"",
                    ""action"": ""Thunder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Generic XR Controller"",
            ""bindingGroup"": ""Generic XR Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>{LeftHand}"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<XRController>{RightHand}"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<WMRHMD>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Continuous Move"",
            ""bindingGroup"": ""Continuous Move"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>{LeftHand}"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<XRController>{RightHand}"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Noncontinuous Move"",
            ""bindingGroup"": ""Noncontinuous Move"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>{LeftHand}"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<XRController>{RightHand}"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // FMOD Gestures
        m_FMODGestures = asset.FindActionMap("FMOD Gestures", throwIfNotFound: true);
        m_FMODGestures_Thunder = m_FMODGestures.FindAction("Thunder", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // FMOD Gestures
    private readonly InputActionMap m_FMODGestures;
    private List<IFMODGesturesActions> m_FMODGesturesActionsCallbackInterfaces = new List<IFMODGesturesActions>();
    private readonly InputAction m_FMODGestures_Thunder;
    public struct FMODGesturesActions
    {
        private @FMODInput m_Wrapper;
        public FMODGesturesActions(@FMODInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Thunder => m_Wrapper.m_FMODGestures_Thunder;
        public InputActionMap Get() { return m_Wrapper.m_FMODGestures; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FMODGesturesActions set) { return set.Get(); }
        public void AddCallbacks(IFMODGesturesActions instance)
        {
            if (instance == null || m_Wrapper.m_FMODGesturesActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_FMODGesturesActionsCallbackInterfaces.Add(instance);
            @Thunder.started += instance.OnThunder;
            @Thunder.performed += instance.OnThunder;
            @Thunder.canceled += instance.OnThunder;
        }

        private void UnregisterCallbacks(IFMODGesturesActions instance)
        {
            @Thunder.started -= instance.OnThunder;
            @Thunder.performed -= instance.OnThunder;
            @Thunder.canceled -= instance.OnThunder;
        }

        public void RemoveCallbacks(IFMODGesturesActions instance)
        {
            if (m_Wrapper.m_FMODGesturesActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IFMODGesturesActions instance)
        {
            foreach (var item in m_Wrapper.m_FMODGesturesActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_FMODGesturesActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public FMODGesturesActions @FMODGestures => new FMODGesturesActions(this);
    private int m_GenericXRControllerSchemeIndex = -1;
    public InputControlScheme GenericXRControllerScheme
    {
        get
        {
            if (m_GenericXRControllerSchemeIndex == -1) m_GenericXRControllerSchemeIndex = asset.FindControlSchemeIndex("Generic XR Controller");
            return asset.controlSchemes[m_GenericXRControllerSchemeIndex];
        }
    }
    private int m_ContinuousMoveSchemeIndex = -1;
    public InputControlScheme ContinuousMoveScheme
    {
        get
        {
            if (m_ContinuousMoveSchemeIndex == -1) m_ContinuousMoveSchemeIndex = asset.FindControlSchemeIndex("Continuous Move");
            return asset.controlSchemes[m_ContinuousMoveSchemeIndex];
        }
    }
    private int m_NoncontinuousMoveSchemeIndex = -1;
    public InputControlScheme NoncontinuousMoveScheme
    {
        get
        {
            if (m_NoncontinuousMoveSchemeIndex == -1) m_NoncontinuousMoveSchemeIndex = asset.FindControlSchemeIndex("Noncontinuous Move");
            return asset.controlSchemes[m_NoncontinuousMoveSchemeIndex];
        }
    }
    public interface IFMODGesturesActions
    {
        void OnThunder(InputAction.CallbackContext context);
    }
}

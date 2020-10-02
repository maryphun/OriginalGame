// GENERATED AUTOMATICALLY FROM 'Assets/Script/Chew/debuginput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Debuginput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Debuginput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""debuginput"",
    ""maps"": [
        {
            ""name"": ""debugging"",
            ""id"": ""16d5fcc1-d9c5-4807-8126-60f96a240fec"",
            ""actions"": [
                {
                    ""name"": ""Option"",
                    ""type"": ""Button"",
                    ""id"": ""82652603-e746-4303-9b20-3e50f72b12ec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""361837a2-1ecb-40f1-be75-a12051b67dd7"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Option"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // debugging
        m_debugging = asset.FindActionMap("debugging", throwIfNotFound: true);
        m_debugging_Option = m_debugging.FindAction("Option", throwIfNotFound: true);
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

    // debugging
    private readonly InputActionMap m_debugging;
    private IDebuggingActions m_DebuggingActionsCallbackInterface;
    private readonly InputAction m_debugging_Option;
    public struct DebuggingActions
    {
        private @Debuginput m_Wrapper;
        public DebuggingActions(@Debuginput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Option => m_Wrapper.m_debugging_Option;
        public InputActionMap Get() { return m_Wrapper.m_debugging; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebuggingActions set) { return set.Get(); }
        public void SetCallbacks(IDebuggingActions instance)
        {
            if (m_Wrapper.m_DebuggingActionsCallbackInterface != null)
            {
                @Option.started -= m_Wrapper.m_DebuggingActionsCallbackInterface.OnOption;
                @Option.performed -= m_Wrapper.m_DebuggingActionsCallbackInterface.OnOption;
                @Option.canceled -= m_Wrapper.m_DebuggingActionsCallbackInterface.OnOption;
            }
            m_Wrapper.m_DebuggingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Option.started += instance.OnOption;
                @Option.performed += instance.OnOption;
                @Option.canceled += instance.OnOption;
            }
        }
    }
    public DebuggingActions @debugging => new DebuggingActions(this);
    public interface IDebuggingActions
    {
        void OnOption(InputAction.CallbackContext context);
    }
}

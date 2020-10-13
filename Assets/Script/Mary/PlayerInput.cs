// GENERATED AUTOMATICALLY FROM 'Assets/Script/Mary/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""7333bd5d-5640-4f9a-9fe4-9d09f65e6dd2"",
            ""actions"": [
                {
                    ""name"": ""HorizontalMove"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7bd492ba-3c6c-4e37-8629-8aae006b2024"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""VerticalMove"",
                    ""type"": ""PassThrough"",
                    ""id"": ""40ef36df-6b4f-4aff-b78d-f52c77fe879a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""2eafb3f7-e173-4a98-98f8-b9d05ee48ecc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraFollow"",
                    ""type"": ""Value"",
                    ""id"": ""8f7eeb24-d4c3-4334-a36f-458199a0a894"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""cd43ffc0-d6f3-4c2e-9c1d-791eb5c3a4f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Key"",
                    ""id"": ""1779a704-19eb-48d5-ad58-dcea73acb54d"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""533aa4ee-8c95-495f-b97f-b73dc07e9b1c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""564af35c-0ede-488f-a7e3-ff2deefd0342"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Key"",
                    ""id"": ""dd30ca06-fbb0-498b-8d4c-ea9ae8f1f55a"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""18141d4c-45dd-472c-9600-6d9010fe5ffb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""9c00e290-7a5d-45bb-b4d8-5e31860192fe"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VerticalMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""25f457ef-d2a0-40ee-afc3-95f65905ef45"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c26b0869-2cd4-4bc1-a90a-08d6339a0cc4"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraFollow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""393bfba3-121f-4483-8d52-6561814e6d73"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Debug"",
            ""id"": ""a995fb53-bdb5-498e-ae4d-bf21def76c6f"",
            ""actions"": [
                {
                    ""name"": ""Z"",
                    ""type"": ""Button"",
                    ""id"": ""1b61035a-8387-4b14-9eaf-367101e0b6d6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""X"",
                    ""type"": ""Button"",
                    ""id"": ""3be65d77-9dbc-4d78-8559-6a2a7464fb51"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""C"",
                    ""type"": ""Button"",
                    ""id"": ""4ac79346-0731-4a92-877f-67eb903266e5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a4ac6c76-dc69-4737-8e12-2d207b60721e"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Z"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9751b96c-89fe-44d9-96ba-5aca5ade208b"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96168151-4143-4235-99d1-917880d76138"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""C"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Cinematic"",
            ""id"": ""191e7d39-4a6b-4858-9bc8-38c72920ef55"",
            ""actions"": [
                {
                    ""name"": ""Clicked"",
                    ""type"": ""Button"",
                    ""id"": ""e16b6846-3530-4faa-84ed-03947b3a76e1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""57ccf074-3342-4b49-8b9b-1af0958814aa"",
                    ""path"": ""<Keyboard>/anyKey"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Clicked"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a5c5e30-5504-4c26-b7c3-6a84c9235407"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Clicked"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_HorizontalMove = m_Player.FindAction("HorizontalMove", throwIfNotFound: true);
        m_Player_VerticalMove = m_Player.FindAction("VerticalMove", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_CameraFollow = m_Player.FindAction("CameraFollow", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
        // Debug
        m_Debug = asset.FindActionMap("Debug", throwIfNotFound: true);
        m_Debug_Z = m_Debug.FindAction("Z", throwIfNotFound: true);
        m_Debug_X = m_Debug.FindAction("X", throwIfNotFound: true);
        m_Debug_C = m_Debug.FindAction("C", throwIfNotFound: true);
        // Cinematic
        m_Cinematic = asset.FindActionMap("Cinematic", throwIfNotFound: true);
        m_Cinematic_Clicked = m_Cinematic.FindAction("Clicked", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_HorizontalMove;
    private readonly InputAction m_Player_VerticalMove;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_CameraFollow;
    private readonly InputAction m_Player_Dash;
    public struct PlayerActions
    {
        private @PlayerInput m_Wrapper;
        public PlayerActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @HorizontalMove => m_Wrapper.m_Player_HorizontalMove;
        public InputAction @VerticalMove => m_Wrapper.m_Player_VerticalMove;
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @CameraFollow => m_Wrapper.m_Player_CameraFollow;
        public InputAction @Dash => m_Wrapper.m_Player_Dash;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @HorizontalMove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHorizontalMove;
                @HorizontalMove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHorizontalMove;
                @HorizontalMove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHorizontalMove;
                @VerticalMove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnVerticalMove;
                @VerticalMove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnVerticalMove;
                @VerticalMove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnVerticalMove;
                @Attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @CameraFollow.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraFollow;
                @CameraFollow.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraFollow;
                @CameraFollow.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraFollow;
                @Dash.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HorizontalMove.started += instance.OnHorizontalMove;
                @HorizontalMove.performed += instance.OnHorizontalMove;
                @HorizontalMove.canceled += instance.OnHorizontalMove;
                @VerticalMove.started += instance.OnVerticalMove;
                @VerticalMove.performed += instance.OnVerticalMove;
                @VerticalMove.canceled += instance.OnVerticalMove;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @CameraFollow.started += instance.OnCameraFollow;
                @CameraFollow.performed += instance.OnCameraFollow;
                @CameraFollow.canceled += instance.OnCameraFollow;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Debug
    private readonly InputActionMap m_Debug;
    private IDebugActions m_DebugActionsCallbackInterface;
    private readonly InputAction m_Debug_Z;
    private readonly InputAction m_Debug_X;
    private readonly InputAction m_Debug_C;
    public struct DebugActions
    {
        private @PlayerInput m_Wrapper;
        public DebugActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Z => m_Wrapper.m_Debug_Z;
        public InputAction @X => m_Wrapper.m_Debug_X;
        public InputAction @C => m_Wrapper.m_Debug_C;
        public InputActionMap Get() { return m_Wrapper.m_Debug; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebugActions set) { return set.Get(); }
        public void SetCallbacks(IDebugActions instance)
        {
            if (m_Wrapper.m_DebugActionsCallbackInterface != null)
            {
                @Z.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnZ;
                @Z.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnZ;
                @Z.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnZ;
                @X.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnX;
                @X.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnX;
                @X.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnX;
                @C.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnC;
                @C.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnC;
                @C.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnC;
            }
            m_Wrapper.m_DebugActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Z.started += instance.OnZ;
                @Z.performed += instance.OnZ;
                @Z.canceled += instance.OnZ;
                @X.started += instance.OnX;
                @X.performed += instance.OnX;
                @X.canceled += instance.OnX;
                @C.started += instance.OnC;
                @C.performed += instance.OnC;
                @C.canceled += instance.OnC;
            }
        }
    }
    public DebugActions @Debug => new DebugActions(this);

    // Cinematic
    private readonly InputActionMap m_Cinematic;
    private ICinematicActions m_CinematicActionsCallbackInterface;
    private readonly InputAction m_Cinematic_Clicked;
    public struct CinematicActions
    {
        private @PlayerInput m_Wrapper;
        public CinematicActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Clicked => m_Wrapper.m_Cinematic_Clicked;
        public InputActionMap Get() { return m_Wrapper.m_Cinematic; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CinematicActions set) { return set.Get(); }
        public void SetCallbacks(ICinematicActions instance)
        {
            if (m_Wrapper.m_CinematicActionsCallbackInterface != null)
            {
                @Clicked.started -= m_Wrapper.m_CinematicActionsCallbackInterface.OnClicked;
                @Clicked.performed -= m_Wrapper.m_CinematicActionsCallbackInterface.OnClicked;
                @Clicked.canceled -= m_Wrapper.m_CinematicActionsCallbackInterface.OnClicked;
            }
            m_Wrapper.m_CinematicActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Clicked.started += instance.OnClicked;
                @Clicked.performed += instance.OnClicked;
                @Clicked.canceled += instance.OnClicked;
            }
        }
    }
    public CinematicActions @Cinematic => new CinematicActions(this);
    public interface IPlayerActions
    {
        void OnHorizontalMove(InputAction.CallbackContext context);
        void OnVerticalMove(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnCameraFollow(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
    }
    public interface IDebugActions
    {
        void OnZ(InputAction.CallbackContext context);
        void OnX(InputAction.CallbackContext context);
        void OnC(InputAction.CallbackContext context);
    }
    public interface ICinematicActions
    {
        void OnClicked(InputAction.CallbackContext context);
    }
}

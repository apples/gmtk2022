//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/PlayerControls.inputactions
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

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""PlayerDice"",
            ""id"": ""1a9cae50-1dcc-44d5-b33b-e2875b14c841"",
            ""actions"": [
                {
                    ""name"": ""MoveLeft"",
                    ""type"": ""Button"",
                    ""id"": ""64f778ed-a36c-4df0-84de-dfd2056f803b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveRight"",
                    ""type"": ""Button"",
                    ""id"": ""54934ad4-8cb8-4a5f-84f6-b984e1aa52b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveUp"",
                    ""type"": ""Button"",
                    ""id"": ""d3a9c1f1-2518-43af-829a-04e7e3872206"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveDown"",
                    ""type"": ""Button"",
                    ""id"": ""73872e1e-f9fc-4082-91dd-480d83175d82"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SpinCW"",
                    ""type"": ""Button"",
                    ""id"": ""e7203de1-4a7f-4ea2-9f8d-bacd6c68e0ad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SpinModifier"",
                    ""type"": ""Button"",
                    ""id"": ""0c2bcb2f-8098-49f1-8558-a22d6d008142"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SpinCCW"",
                    ""type"": ""Button"",
                    ""id"": ""0a298f77-2a5a-4a67-a581-f58d27c0d472"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Accept"",
                    ""type"": ""Button"",
                    ""id"": ""ba514682-b537-41ee-b831-88ae3a7b268c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3de7bb3e-e3fb-4e5d-a2b2-8847e2f8c9e0"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c45f275d-6022-401e-a836-fe8486a40c6a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ab8d3d9-0d93-4bcc-a579-eb6be945661a"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""84fbd3b7-7e06-4bc2-8bbe-8c8930ef8657"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ec318f7-0f58-4c67-87af-ee3b0ce94cdf"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""152c699d-713d-443a-88aa-112f489308da"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09062f66-6299-489c-8f29-efb81ea73a32"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04285f19-393b-4dc5-92c7-b4a4a52ca636"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53bc9bb7-a822-47c9-aa44-c53712e5c5f7"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpinCW"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e9862c72-ed20-4179-be38-32987063c3a5"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpinModifier"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""29732678-4b0e-425a-8b38-9baca99eb4a3"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpinCCW"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7315015f-1ccb-4ff8-97de-8938788e3919"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accept"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43b44cac-cbf3-4f7f-996a-ec0ee8250926"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accept"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerDice
        m_PlayerDice = asset.FindActionMap("PlayerDice", throwIfNotFound: true);
        m_PlayerDice_MoveLeft = m_PlayerDice.FindAction("MoveLeft", throwIfNotFound: true);
        m_PlayerDice_MoveRight = m_PlayerDice.FindAction("MoveRight", throwIfNotFound: true);
        m_PlayerDice_MoveUp = m_PlayerDice.FindAction("MoveUp", throwIfNotFound: true);
        m_PlayerDice_MoveDown = m_PlayerDice.FindAction("MoveDown", throwIfNotFound: true);
        m_PlayerDice_SpinCW = m_PlayerDice.FindAction("SpinCW", throwIfNotFound: true);
        m_PlayerDice_SpinModifier = m_PlayerDice.FindAction("SpinModifier", throwIfNotFound: true);
        m_PlayerDice_SpinCCW = m_PlayerDice.FindAction("SpinCCW", throwIfNotFound: true);
        m_PlayerDice_Accept = m_PlayerDice.FindAction("Accept", throwIfNotFound: true);
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

    // PlayerDice
    private readonly InputActionMap m_PlayerDice;
    private IPlayerDiceActions m_PlayerDiceActionsCallbackInterface;
    private readonly InputAction m_PlayerDice_MoveLeft;
    private readonly InputAction m_PlayerDice_MoveRight;
    private readonly InputAction m_PlayerDice_MoveUp;
    private readonly InputAction m_PlayerDice_MoveDown;
    private readonly InputAction m_PlayerDice_SpinCW;
    private readonly InputAction m_PlayerDice_SpinModifier;
    private readonly InputAction m_PlayerDice_SpinCCW;
    private readonly InputAction m_PlayerDice_Accept;
    public struct PlayerDiceActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerDiceActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveLeft => m_Wrapper.m_PlayerDice_MoveLeft;
        public InputAction @MoveRight => m_Wrapper.m_PlayerDice_MoveRight;
        public InputAction @MoveUp => m_Wrapper.m_PlayerDice_MoveUp;
        public InputAction @MoveDown => m_Wrapper.m_PlayerDice_MoveDown;
        public InputAction @SpinCW => m_Wrapper.m_PlayerDice_SpinCW;
        public InputAction @SpinModifier => m_Wrapper.m_PlayerDice_SpinModifier;
        public InputAction @SpinCCW => m_Wrapper.m_PlayerDice_SpinCCW;
        public InputAction @Accept => m_Wrapper.m_PlayerDice_Accept;
        public InputActionMap Get() { return m_Wrapper.m_PlayerDice; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerDiceActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerDiceActions instance)
        {
            if (m_Wrapper.m_PlayerDiceActionsCallbackInterface != null)
            {
                @MoveLeft.started -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnMoveLeft;
                @MoveLeft.performed -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnMoveLeft;
                @MoveLeft.canceled -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnMoveLeft;
                @MoveRight.started -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnMoveRight;
                @MoveRight.performed -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnMoveRight;
                @MoveRight.canceled -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnMoveRight;
                @MoveUp.started -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnMoveUp;
                @MoveUp.performed -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnMoveUp;
                @MoveUp.canceled -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnMoveUp;
                @MoveDown.started -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnMoveDown;
                @MoveDown.performed -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnMoveDown;
                @MoveDown.canceled -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnMoveDown;
                @SpinCW.started -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnSpinCW;
                @SpinCW.performed -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnSpinCW;
                @SpinCW.canceled -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnSpinCW;
                @SpinModifier.started -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnSpinModifier;
                @SpinModifier.performed -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnSpinModifier;
                @SpinModifier.canceled -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnSpinModifier;
                @SpinCCW.started -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnSpinCCW;
                @SpinCCW.performed -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnSpinCCW;
                @SpinCCW.canceled -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnSpinCCW;
                @Accept.started -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnAccept;
                @Accept.performed -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnAccept;
                @Accept.canceled -= m_Wrapper.m_PlayerDiceActionsCallbackInterface.OnAccept;
            }
            m_Wrapper.m_PlayerDiceActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveLeft.started += instance.OnMoveLeft;
                @MoveLeft.performed += instance.OnMoveLeft;
                @MoveLeft.canceled += instance.OnMoveLeft;
                @MoveRight.started += instance.OnMoveRight;
                @MoveRight.performed += instance.OnMoveRight;
                @MoveRight.canceled += instance.OnMoveRight;
                @MoveUp.started += instance.OnMoveUp;
                @MoveUp.performed += instance.OnMoveUp;
                @MoveUp.canceled += instance.OnMoveUp;
                @MoveDown.started += instance.OnMoveDown;
                @MoveDown.performed += instance.OnMoveDown;
                @MoveDown.canceled += instance.OnMoveDown;
                @SpinCW.started += instance.OnSpinCW;
                @SpinCW.performed += instance.OnSpinCW;
                @SpinCW.canceled += instance.OnSpinCW;
                @SpinModifier.started += instance.OnSpinModifier;
                @SpinModifier.performed += instance.OnSpinModifier;
                @SpinModifier.canceled += instance.OnSpinModifier;
                @SpinCCW.started += instance.OnSpinCCW;
                @SpinCCW.performed += instance.OnSpinCCW;
                @SpinCCW.canceled += instance.OnSpinCCW;
                @Accept.started += instance.OnAccept;
                @Accept.performed += instance.OnAccept;
                @Accept.canceled += instance.OnAccept;
            }
        }
    }
    public PlayerDiceActions @PlayerDice => new PlayerDiceActions(this);
    public interface IPlayerDiceActions
    {
        void OnMoveLeft(InputAction.CallbackContext context);
        void OnMoveRight(InputAction.CallbackContext context);
        void OnMoveUp(InputAction.CallbackContext context);
        void OnMoveDown(InputAction.CallbackContext context);
        void OnSpinCW(InputAction.CallbackContext context);
        void OnSpinModifier(InputAction.CallbackContext context);
        void OnSpinCCW(InputAction.CallbackContext context);
        void OnAccept(InputAction.CallbackContext context);
    }
}

// GENERATED AUTOMATICALLY FROM 'Assets/Input System/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Main"",
            ""id"": ""e2fc3e8c-7bff-4164-b61f-a246d92f1e78"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""496f4704-4cf4-4d8c-899d-3f682e54f670"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""335b6802-ca4d-4419-9c32-ebebc5db8194"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""Button"",
                    ""id"": ""7db8bdcf-492a-46f0-bb43-4c56a4e6f68d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseMovement"",
                    ""type"": ""Value"",
                    ""id"": ""d78e05f9-657b-4e7e-b8e0-0c5d28ae766d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StartReading"",
                    ""type"": ""Button"",
                    ""id"": ""569769f0-459e-41a1-a159-8c9bfadf23bd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StopReading"",
                    ""type"": ""Button"",
                    ""id"": ""bb16a92d-03a5-4f0d-8d35-38e44eede703"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RefreshEnvironment"",
                    ""type"": ""Button"",
                    ""id"": ""2d07efd4-3ef7-47f7-86e5-0b9319df12a3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""e026db14-f3ad-459a-bc84-246de752c2d4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d1dcac69-2455-4982-bee8-52092d08261a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4ac14d84-1a0d-4c8a-99dd-662d480874f9"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""69b6aa44-c348-43ae-b76c-f97fa028d538"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b10a36b9-7597-4aa1-abc0-dee9c87922f3"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""ea4cd6ac-a4ef-4e06-bc98-c150f36f7903"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7ae04135-a020-4e8d-9310-d0a36055904e"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f5b8f82a-535f-41df-a617-1ccdd0a7fdac"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1696bb62-efaf-4a4e-83ec-7c9c2613c553"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fd3d229f-f5fc-4e6b-984b-95a6597abc98"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b16577a5-706c-4e1a-871b-579423ce2469"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""219b52db-3fcb-4d3d-aebd-7dd6be83c2f3"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""MouseMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ca1ea07-bffa-4df2-9ae1-f7fde5276988"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c2d99545-3255-40a6-b3cd-631e5dc18f5a"",
                    ""path"": ""<Keyboard>/f1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""StartReading"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e5a3b48f-95be-4f0c-8c8f-6afce3b3f48b"",
                    ""path"": ""<Keyboard>/f2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""StopReading"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ed487ee3-3279-4475-96c8-6bb8cef5455e"",
                    ""path"": ""<Keyboard>/f5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""RefreshEnvironment"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""MouseKeyboard"",
            ""bindingGroup"": ""MouseKeyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Main
        m_Main = asset.FindActionMap("Main", throwIfNotFound: true);
        m_Main_Move = m_Main.FindAction("Move", throwIfNotFound: true);
        m_Main_Click = m_Main.FindAction("Click", throwIfNotFound: true);
        m_Main_RightClick = m_Main.FindAction("RightClick", throwIfNotFound: true);
        m_Main_MouseMovement = m_Main.FindAction("MouseMovement", throwIfNotFound: true);
        m_Main_StartReading = m_Main.FindAction("StartReading", throwIfNotFound: true);
        m_Main_StopReading = m_Main.FindAction("StopReading", throwIfNotFound: true);
        m_Main_RefreshEnvironment = m_Main.FindAction("RefreshEnvironment", throwIfNotFound: true);
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

    // Main
    private readonly InputActionMap m_Main;
    private IMainActions m_MainActionsCallbackInterface;
    private readonly InputAction m_Main_Move;
    private readonly InputAction m_Main_Click;
    private readonly InputAction m_Main_RightClick;
    private readonly InputAction m_Main_MouseMovement;
    private readonly InputAction m_Main_StartReading;
    private readonly InputAction m_Main_StopReading;
    private readonly InputAction m_Main_RefreshEnvironment;
    public struct MainActions
    {
        private @InputActions m_Wrapper;
        public MainActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Main_Move;
        public InputAction @Click => m_Wrapper.m_Main_Click;
        public InputAction @RightClick => m_Wrapper.m_Main_RightClick;
        public InputAction @MouseMovement => m_Wrapper.m_Main_MouseMovement;
        public InputAction @StartReading => m_Wrapper.m_Main_StartReading;
        public InputAction @StopReading => m_Wrapper.m_Main_StopReading;
        public InputAction @RefreshEnvironment => m_Wrapper.m_Main_RefreshEnvironment;
        public InputActionMap Get() { return m_Wrapper.m_Main; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainActions set) { return set.Get(); }
        public void SetCallbacks(IMainActions instance)
        {
            if (m_Wrapper.m_MainActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_MainActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnMove;
                @Click.started -= m_Wrapper.m_MainActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnClick;
                @RightClick.started -= m_Wrapper.m_MainActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnRightClick;
                @MouseMovement.started -= m_Wrapper.m_MainActionsCallbackInterface.OnMouseMovement;
                @MouseMovement.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnMouseMovement;
                @MouseMovement.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnMouseMovement;
                @StartReading.started -= m_Wrapper.m_MainActionsCallbackInterface.OnStartReading;
                @StartReading.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnStartReading;
                @StartReading.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnStartReading;
                @StopReading.started -= m_Wrapper.m_MainActionsCallbackInterface.OnStopReading;
                @StopReading.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnStopReading;
                @StopReading.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnStopReading;
                @RefreshEnvironment.started -= m_Wrapper.m_MainActionsCallbackInterface.OnRefreshEnvironment;
                @RefreshEnvironment.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnRefreshEnvironment;
                @RefreshEnvironment.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnRefreshEnvironment;
            }
            m_Wrapper.m_MainActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @MouseMovement.started += instance.OnMouseMovement;
                @MouseMovement.performed += instance.OnMouseMovement;
                @MouseMovement.canceled += instance.OnMouseMovement;
                @StartReading.started += instance.OnStartReading;
                @StartReading.performed += instance.OnStartReading;
                @StartReading.canceled += instance.OnStartReading;
                @StopReading.started += instance.OnStopReading;
                @StopReading.performed += instance.OnStopReading;
                @StopReading.canceled += instance.OnStopReading;
                @RefreshEnvironment.started += instance.OnRefreshEnvironment;
                @RefreshEnvironment.performed += instance.OnRefreshEnvironment;
                @RefreshEnvironment.canceled += instance.OnRefreshEnvironment;
            }
        }
    }
    public MainActions @Main => new MainActions(this);
    private int m_MouseKeyboardSchemeIndex = -1;
    public InputControlScheme MouseKeyboardScheme
    {
        get
        {
            if (m_MouseKeyboardSchemeIndex == -1) m_MouseKeyboardSchemeIndex = asset.FindControlSchemeIndex("MouseKeyboard");
            return asset.controlSchemes[m_MouseKeyboardSchemeIndex];
        }
    }
    public interface IMainActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnMouseMovement(InputAction.CallbackContext context);
        void OnStartReading(InputAction.CallbackContext context);
        void OnStopReading(InputAction.CallbackContext context);
        void OnRefreshEnvironment(InputAction.CallbackContext context);
    }
}

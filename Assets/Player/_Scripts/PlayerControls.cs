// GENERATED AUTOMATICALLY FROM 'Assets/Player/_Scripts/PlayerController.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerController"",
    ""maps"": [
        {
            ""name"": ""Move"",
            ""id"": ""92d8b74b-fdea-43fb-9b71-4e53ee6ee704"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""d0e0e595-7581-4307-ad2e-4674c2c8209e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f9c86598-48d1-4675-bf24-2a6191ccf5cc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""99ed26f3-3399-42c3-bcd0-dc01872eda2c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Arrows"",
                    ""id"": ""8d9bc5dc-3c45-47eb-84d2-ea35ca596feb"",
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
                    ""id"": ""3388f32f-5996-491d-b978-91d2919c051c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""05649a06-aae0-4b4e-a3b1-58137905417b"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""93afbc42-e8e9-423e-8f43-33687987d342"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""92ef3dd6-66cd-4a4e-9586-76dcb7496228"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""0bfdb08d-119e-470e-8b80-73a2d544817b"",
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
                    ""id"": ""99c2af8d-b994-4508-b8a6-404b9e00fe48"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6caa7dc3-c2ac-485d-a87f-cfb4b6a4fa2d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""331b59db-0e50-460a-8c57-494f73dd4d50"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""57cea9dd-7ecd-486e-b73f-f7a22dc95640"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a38049eb-d162-41cb-8877-a6fa783545cb"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eb5e5061-ddce-4b58-93a6-201bd56b6073"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""ThrowDice"",
            ""id"": ""2260d739-a582-4f71-9903-7506858b6a79"",
            ""actions"": [
                {
                    ""name"": ""Action"",
                    ""type"": ""Button"",
                    ""id"": ""65d5b56c-195b-4cfc-80eb-c0ffdd3fd269"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""19d8f3da-6433-405b-b847-b9aad15244e6"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""EnemyEncounter"",
            ""id"": ""e88f049d-8bc9-422a-815c-5045f0e64d2e"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""81a52657-59c9-4909-8bef-9170141e0a9c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""27016589-bbc8-4205-b5d1-c4b44ab02440"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Title"",
            ""id"": ""b5e7931a-077f-40c1-a8ce-66ab9142d98f"",
            ""actions"": [
                {
                    ""name"": ""StartGame"",
                    ""type"": ""Button"",
                    ""id"": ""6760e68a-267c-41ab-978b-bd4ce539d53a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4afd8c51-b8b9-49c7-956c-2884956cec58"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4a72086-39d7-4aff-b5b4-4441c80a1b33"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Move
        m_Move = asset.FindActionMap("Move", throwIfNotFound: true);
        m_Move_Move = m_Move.FindAction("Move", throwIfNotFound: true);
        m_Move_Action = m_Move.FindAction("Action", throwIfNotFound: true);
        m_Move_Reload = m_Move.FindAction("Reload", throwIfNotFound: true);
        // ThrowDice
        m_ThrowDice = asset.FindActionMap("ThrowDice", throwIfNotFound: true);
        m_ThrowDice_Action = m_ThrowDice.FindAction("Action", throwIfNotFound: true);
        // EnemyEncounter
        m_EnemyEncounter = asset.FindActionMap("EnemyEncounter", throwIfNotFound: true);
        m_EnemyEncounter_Newaction = m_EnemyEncounter.FindAction("New action", throwIfNotFound: true);
        // Title
        m_Title = asset.FindActionMap("Title", throwIfNotFound: true);
        m_Title_StartGame = m_Title.FindAction("StartGame", throwIfNotFound: true);
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

    // Move
    private readonly InputActionMap m_Move;
    private IMoveActions m_MoveActionsCallbackInterface;
    private readonly InputAction m_Move_Move;
    private readonly InputAction m_Move_Action;
    private readonly InputAction m_Move_Reload;
    public struct MoveActions
    {
        private @PlayerControls m_Wrapper;
        public MoveActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Move_Move;
        public InputAction @Action => m_Wrapper.m_Move_Action;
        public InputAction @Reload => m_Wrapper.m_Move_Reload;
        public InputActionMap Get() { return m_Wrapper.m_Move; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MoveActions set) { return set.Get(); }
        public void SetCallbacks(IMoveActions instance)
        {
            if (m_Wrapper.m_MoveActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnMove;
                @Action.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnAction;
                @Action.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnAction;
                @Action.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnAction;
                @Reload.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnReload;
            }
            m_Wrapper.m_MoveActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Action.started += instance.OnAction;
                @Action.performed += instance.OnAction;
                @Action.canceled += instance.OnAction;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
            }
        }
    }
    public MoveActions @Move => new MoveActions(this);

    // ThrowDice
    private readonly InputActionMap m_ThrowDice;
    private IThrowDiceActions m_ThrowDiceActionsCallbackInterface;
    private readonly InputAction m_ThrowDice_Action;
    public struct ThrowDiceActions
    {
        private @PlayerControls m_Wrapper;
        public ThrowDiceActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Action => m_Wrapper.m_ThrowDice_Action;
        public InputActionMap Get() { return m_Wrapper.m_ThrowDice; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ThrowDiceActions set) { return set.Get(); }
        public void SetCallbacks(IThrowDiceActions instance)
        {
            if (m_Wrapper.m_ThrowDiceActionsCallbackInterface != null)
            {
                @Action.started -= m_Wrapper.m_ThrowDiceActionsCallbackInterface.OnAction;
                @Action.performed -= m_Wrapper.m_ThrowDiceActionsCallbackInterface.OnAction;
                @Action.canceled -= m_Wrapper.m_ThrowDiceActionsCallbackInterface.OnAction;
            }
            m_Wrapper.m_ThrowDiceActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Action.started += instance.OnAction;
                @Action.performed += instance.OnAction;
                @Action.canceled += instance.OnAction;
            }
        }
    }
    public ThrowDiceActions @ThrowDice => new ThrowDiceActions(this);

    // EnemyEncounter
    private readonly InputActionMap m_EnemyEncounter;
    private IEnemyEncounterActions m_EnemyEncounterActionsCallbackInterface;
    private readonly InputAction m_EnemyEncounter_Newaction;
    public struct EnemyEncounterActions
    {
        private @PlayerControls m_Wrapper;
        public EnemyEncounterActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_EnemyEncounter_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_EnemyEncounter; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(EnemyEncounterActions set) { return set.Get(); }
        public void SetCallbacks(IEnemyEncounterActions instance)
        {
            if (m_Wrapper.m_EnemyEncounterActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_EnemyEncounterActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_EnemyEncounterActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_EnemyEncounterActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_EnemyEncounterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public EnemyEncounterActions @EnemyEncounter => new EnemyEncounterActions(this);

    // Title
    private readonly InputActionMap m_Title;
    private ITitleActions m_TitleActionsCallbackInterface;
    private readonly InputAction m_Title_StartGame;
    public struct TitleActions
    {
        private @PlayerControls m_Wrapper;
        public TitleActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @StartGame => m_Wrapper.m_Title_StartGame;
        public InputActionMap Get() { return m_Wrapper.m_Title; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TitleActions set) { return set.Get(); }
        public void SetCallbacks(ITitleActions instance)
        {
            if (m_Wrapper.m_TitleActionsCallbackInterface != null)
            {
                @StartGame.started -= m_Wrapper.m_TitleActionsCallbackInterface.OnStartGame;
                @StartGame.performed -= m_Wrapper.m_TitleActionsCallbackInterface.OnStartGame;
                @StartGame.canceled -= m_Wrapper.m_TitleActionsCallbackInterface.OnStartGame;
            }
            m_Wrapper.m_TitleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @StartGame.started += instance.OnStartGame;
                @StartGame.performed += instance.OnStartGame;
                @StartGame.canceled += instance.OnStartGame;
            }
        }
    }
    public TitleActions @Title => new TitleActions(this);
    public interface IMoveActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnAction(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
    }
    public interface IThrowDiceActions
    {
        void OnAction(InputAction.CallbackContext context);
    }
    public interface IEnemyEncounterActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
    public interface ITitleActions
    {
        void OnStartGame(InputAction.CallbackContext context);
    }
}

// GENERATED AUTOMATICALLY FROM 'Assets/Objects/Input/MainInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MainInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MainInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MainInput"",
    ""maps"": [
        {
            ""name"": ""Inventory"",
            ""id"": ""20581c89-0359-46a9-9b88-bf0ce78a3733"",
            ""actions"": [
                {
                    ""name"": ""Rotate Item"",
                    ""type"": ""Button"",
                    ""id"": ""86d36a6b-47ad-4f62-9734-d2f01330771d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""06dacac9-66e3-4ab1-8339-5fe8c7e6f404"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate Item"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Inventory
        m_Inventory = asset.FindActionMap("Inventory", throwIfNotFound: true);
        m_Inventory_RotateItem = m_Inventory.FindAction("Rotate Item", throwIfNotFound: true);
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

    // Inventory
    private readonly InputActionMap m_Inventory;
    private IInventoryActions m_InventoryActionsCallbackInterface;
    private readonly InputAction m_Inventory_RotateItem;
    public struct InventoryActions
    {
        private @MainInput m_Wrapper;
        public InventoryActions(@MainInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @RotateItem => m_Wrapper.m_Inventory_RotateItem;
        public InputActionMap Get() { return m_Wrapper.m_Inventory; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InventoryActions set) { return set.Get(); }
        public void SetCallbacks(IInventoryActions instance)
        {
            if (m_Wrapper.m_InventoryActionsCallbackInterface != null)
            {
                @RotateItem.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnRotateItem;
                @RotateItem.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnRotateItem;
                @RotateItem.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnRotateItem;
            }
            m_Wrapper.m_InventoryActionsCallbackInterface = instance;
            if (instance != null)
            {
                @RotateItem.started += instance.OnRotateItem;
                @RotateItem.performed += instance.OnRotateItem;
                @RotateItem.canceled += instance.OnRotateItem;
            }
        }
    }
    public InventoryActions @Inventory => new InventoryActions(this);
    public interface IInventoryActions
    {
        void OnRotateItem(InputAction.CallbackContext context);
    }
}

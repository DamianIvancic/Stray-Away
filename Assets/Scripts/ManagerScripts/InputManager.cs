using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using System.IO;


public class InputManager : MonoBehaviour {

    public List<Text> Keys;
    public List<Action> KeyBindings;

    public static InputManager Instance;
    
    [System.Serializable]
    public class Action
    {
        public Action(string name, KeyCode key)
        {
            Name = name;
            KeyCode = key;
        }

        public void ClearCallBacks()
        {
            ActionCallback = null;
            StopCallback = null;
        }

        public string Name;
        public KeyCode KeyCode;
       
        public delegate void OnActionRegistered();
        public OnActionRegistered ActionCallback;
        public OnActionRegistered StopCallback;
    }

    private GameObject CurrentKey;

    void Start() //either sets default keybindings or loads existing ones from a file using the SLSManager if such a file exists
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            string path = Application.persistentDataPath + "/Settings.dat";

            if (File.Exists(path))
                KeyBindings = SLSManager.Instance.Settings.KeyBindings;
            else
            {
                SetDefaultBindings();
                SLSManager.Instance.SaveSettings();
            }

            RefreshDisplayedKeyBindings();
        }
        else
            Destroy(gameObject);
    }

    void Update()
    {
      
        if (GameManager.GM.gameState == GameManager.GameState.Playing)
        {    
            if (EventSystem.current.IsPointerOverGameObject())
                return;
        
            foreach (Action Binding in KeyBindings)
            {
                if (Input.GetKeyDown(Binding.KeyCode) && Binding.ActionCallback != null)
                {
                    Binding.ActionCallback.Invoke();             
                }

                if (Input.GetKeyUp(Binding.KeyCode) && Binding.StopCallback != null)
                    Binding.StopCallback.Invoke();
            }
        }
    }

    void OnGUI() // for this to be able to work the GameObject CurrentKey(a GUI Button representing the key) needs to have a name that coresponds to a string Name
    {            //on one of the Action objects in the list
        if(CurrentKey != null) 
        {
  
            foreach (Action action in KeyBindings) //all callbacks need to be cleared whenever a new binding is set because the bindings can't be serialized if the callbacks are registered
            {
                action.ClearCallBacks();
            }

            Event e = Event.current;
            KeyCode Code = KeyCode.None;

            if (e.isMouse)
                Code = (KeyCode)((int)KeyCode.Mouse0 + e.button);  
            else if (e.isKey)
                Code = e.keyCode;

            if (Code != KeyCode.None && Code != KeyCode.E)
            {
                foreach (Action Binding in KeyBindings) //loops through all bindings 
                {
                    if (Binding.KeyCode == Code) //if there's an action already bound to the pressed key unpairs the binding
                    {
                           Binding.KeyCode = KeyCode.None;

                           foreach (Text TextKey in Keys)
                           {
                                if (TextKey.text == Code.ToString())
                                     TextKey.text = "None";
                           }
                    }

                    if(Binding.Name == CurrentKey.name)  //binds the action to the new keycode and updates the text display
                    {
                        Binding.KeyCode = Code;
                        CurrentKey.transform.GetChild(0).gameObject.GetComponent<Text>().text = Code.ToString();                       
                    }
                }

                CurrentKey = null;

                SLSManager.Instance.SaveSettings();

                if(GameManager.GM.Player != null)
                    RegisterCallbacks();            
            }
        }
    }

    public void SetDefaultBindings()
    {
        KeyBindings = new List<Action>();

        Action MoveUp = new Action("Up", KeyCode.UpArrow);  
        KeyBindings.Add(MoveUp);
      
        Action MoveLeft = new Action("Left", KeyCode.LeftArrow);
        KeyBindings.Add(MoveLeft);
     
        Action MoveDown = new Action("Down", KeyCode.DownArrow);
        KeyBindings.Add(MoveDown);
      
        Action MoveRight = new Action("Right", KeyCode.RightArrow);
        KeyBindings.Add(MoveRight);
     
        Action Attack = new Action("Attack", KeyCode.Mouse0);      
        KeyBindings.Add(Attack);
     
        Action Interact = new Action("Interact", KeyCode.E);
        KeyBindings.Add(Interact);
      
        Action Inventory = new Action("Inventory", KeyCode.I);
        KeyBindings.Add(Inventory);
    }

    public void RefreshDisplayedKeyBindings()
    {
        for (int i = 0; i < KeyBindings.Count; i++)
        {
            Keys[i].text = KeyBindings[i].KeyCode.ToString();
        }
    }

    public void ChangeKey(GameObject clicked) //this is the callback to the OnClickEvent of the Buttons. The gameObject clicked is the Button itself
    {                                                //this is just used to store the button clicked into a variable inside the script
        CurrentKey = clicked;
    }

    public void RegisterCallbacks()
    {
        List<Action> keyBindings = KeyBindings;

        foreach (Action action in keyBindings)
        {
            switch (action.Name)
            {
                case ("Up"):
                    action.ActionCallback += GameManager.GM.Player.MoveUp;
                    action.StopCallback += GameManager.GM.Player.StopMovingVertical;
                    break;
                case ("Left"):
                    action.ActionCallback += GameManager.GM.Player.MoveLeft;
                    action.StopCallback += GameManager.GM.Player.StopMovingHorizontal;
                    break;
                case ("Down"):
                    action.ActionCallback += GameManager.GM.Player.MoveDown;
                    action.StopCallback += GameManager.GM.Player.StopMovingVertical;
                    break;
                case ("Right"):
                    action.ActionCallback += GameManager.GM.Player.MoveRight;
                    action.StopCallback += GameManager.GM.Player.StopMovingHorizontal;
                    break;
                case ("Attack"):
                    action.ActionCallback += GameManager.GM.Player.Swing;
                    break;
                case ("Interact"):
                    action.ActionCallback += GameManager.GM.Player.Interact;
                    break;
                case ("Inventory"):
                    action.ActionCallback += InventoryManager.Instance.ToggleDisplay;
                    break;
            }
        }
    }
}

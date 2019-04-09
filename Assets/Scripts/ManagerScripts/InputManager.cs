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
            ActionCallBack = null;
            StopActionCallBack = null;
        }

        public string Name;
        public KeyCode KeyCode;
       
        public delegate void OnActionRegistered();
        public OnActionRegistered ActionCallBack;
        public OnActionRegistered StopActionCallBack;
    }

    private GameObject CurrentKey;

	void Start ()
    {
    
        string path = Application.persistentDataPath + "/Settings.dat";
   

        if (File.Exists(path))
            KeyBindings = GameManager.GM.SaveLoadSystem.Settings.KeyBindings;
        else
        {
            SetDefaultBindings();
            GameManager.GM.InitializeSettings();
        }

        RefreshDisplayedKeyBindings();     
    }

    void Update()
    {
      
        if (GameManager.GM.gameState == GameManager.GameState.Playing)
        {
            
            if (EventSystem.current.IsPointerOverGameObject())
                return;
    
            foreach (Action Binding in KeyBindings)
            {
                if (Input.GetKeyDown(Binding.KeyCode) && Binding.ActionCallBack != null)
                {
                    Binding.ActionCallBack.Invoke();             
                }

                if (Input.GetKeyUp(Binding.KeyCode) && Binding.StopActionCallBack != null)
                    Binding.StopActionCallBack.Invoke();
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

                GameManager.GM.InitializeSettings();

                if(GameManager.GM.Player != null)
                    GameManager.GM.Player.RegisterCallbacks();            
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
      
      /*  Action Inventory = new Action("Inventory", KeyCode.I);
        KeyBindings.Add(Inventory);   */
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
}

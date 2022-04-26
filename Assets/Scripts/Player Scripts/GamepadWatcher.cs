using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadWatcher : MonoBehaviour
{
    //Eventually, this class should be used to pause the game when a player's controller is disconnected.
    //For now, it'll be used to select the gamepad control scheme by default and make playtesting smoother.

    //NOTE: at the moment, it won't detect new controllers at runtime. trying to find documentation on it

    PlayerInput inputs;

    void Awake()
    {
        inputs = GetComponent<PlayerInput>();

        if (Gamepad.current != null){
            inputs.SwitchCurrentControlScheme(Gamepad.current);
        }
    }

    public void OnGamepadLost(){
        inputs.SwitchCurrentControlScheme(Keyboard.current);
    }

    public void OnGamepadConnected(){
        inputs.SwitchCurrentControlScheme(Gamepad.current);
    }

    private void ListAllDevices(){
        Debug.Log("number of devices: " + inputs.devices.Count);
        for(int i = 0; i < inputs.devices.Count; i++){
            Debug.Log("device: " + inputs.devices[i].name);
        }
    }
}

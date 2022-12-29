using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;


public class PlayerInputManagerScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        AssignControls();
    }
    
    public void AssignControls() 
    {
        // Holt sich PlayerInput-Komponente von den beiden Schiff-Game Objects
        var player1 = PlayerInput.all[0];
        var player2 = PlayerInput.all[1];

        // Bestehende Zuweisungen verwerfen
        player1.user.UnpairDevices();
        player2.user.UnpairDevices();

        // Geräte und Control Scheme zuweisen
        var gamepadCount = Gamepad.all.Count;
        Debug.Log("Anzahl erfasster Controller: " + gamepadCount);
        
        if (gamepadCount >= 2)
        {
            InputUser.PerformPairingWithDevice(Gamepad.all[0], user: player1.user);
            InputUser.PerformPairingWithDevice(Gamepad.all[1], user: player2.user);
            player1.user.ActivateControlScheme("Gamepad");
            player2.user.ActivateControlScheme("Gamepad");
            
        } else if (gamepadCount == 1) {
            InputUser.PerformPairingWithDevice(Gamepad.all[0], user: player1.user);
            InputUser.PerformPairingWithDevice(Keyboard.current, user: player2.user);
            
            player1.user.ActivateControlScheme("Gamepad");
            player2.user.ActivateControlScheme("KeyboardLR"); 
        } else {
            InputUser.PerformPairingWithDevice(Keyboard.current, user: player1.user);
            InputUser.PerformPairingWithDevice(Keyboard.current, user: player2.user);
            
            player1.user.ActivateControlScheme("KeyboardL");
            player2.user.ActivateControlScheme("KeyboardR");
        }
        
        Debug.Log("Player 1 verbunden mit: " + player1.user.pairedDevices.ToArray()[0].description.ToString());
        Debug.Log("Player 2 verbunden mit: " + player2.user.pairedDevices.ToArray()[0].description.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

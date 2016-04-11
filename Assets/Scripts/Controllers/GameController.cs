//Import Statements
using UnityEngine;
using System.Collections;

public class GameController : Singleton<GameController> {

    /// <summary>
    /// 
    /// </summary>
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
}
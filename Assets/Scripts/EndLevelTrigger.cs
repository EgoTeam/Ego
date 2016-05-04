using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndLevelTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider collider) {
        GameController.Instance.LoadLevel("999 - Credits", 5.0f);
    }
}

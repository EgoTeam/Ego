using UnityEngine;
using System.Collections;

public class HealthKit : MonoBehaviour {
    [SerializeField][Range(001, 100)] private int _health;
    private void OnCollisionEnter(Collision _collision) {
        if(_collision.gameObject.CompareTag("Player")) {
            //Heal the player.
            _collision.gameObject.SendMessage("Heal", _health);
            //Destory the health kit.
            Destroy(gameObject);
        }
    }
}
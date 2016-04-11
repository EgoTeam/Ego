using UnityEngine;
using System.Collections;
using System;

public class Enemy : Character {
    //Data Members
    private AudioSource[] _audioSources;    //A reference to the audio source components used by the enemy.

    /// <summary>
    /// Method executes once on Object activation.
    /// </summary>
	override protected void Start () {
        base.Start();
        _audioSources = GetComponentsInChildren<AudioSource>();
	}
    /// <summary>
    /// Method handles enemy behavior on receiving damage.
    /// </summary>
    /// <param name="damage">The amount of damage the enemy receives.</param>
    override protected void OnDamage(int damage) {
        //Invoke parent method.
        base.OnDamage(damage);
        //Play damage sound effect.
        _audioSources[0].Play();
    }
}
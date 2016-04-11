using UnityEngine;
using System;
using System.Collections.Generic;

public class EventManager : Singleton<EventManager> {

    private Dictionary<Enum, List<IListener>> _listeners = new Dictionary<Enum, List<IListener>>();

    /// <summary>
    /// Method adds a listener to the list of listeners.
    /// </summary>
    /// <param name="type">Event to listen for.</param>
    /// <param name="Listener">Object to listen for event.</param>
    public void AddListener(Enum type, IListener listener) {
        //The list of listeners for this event.
        List<IListener> listenList = null;

        //Check if the type key exists...
        if (_listeners.TryGetValue(type, out listenList)) {
            //...The list exists, so add a new listener.
            listenList.Add(listener);
        }
        else {
            //Create a new list as a dictionary key.
            listenList = new List<IListener>();
            listenList.Add(listener);
            _listeners.Add(type, listenList);
        }
    }
    /// <summary>
    /// Method posts event to listeners.
    /// </summary>
    /// <param name="type">Event to invoke.</param>
    /// <param name="sender">Object invoking the event.</param>
    /// <param name="param">Optional argument.</param>
    public void PostNotification(Enum type, Component sender, System.Object param = null) {
        
        //List of listeners for this event only.
        List<IListener> listenList = null;

        //If an event exists...
        if(_listeners.TryGetValue(type, out listenList)) {
            //...For each listner in the list...
            foreach(IListener listener in listenList) {
                //...If the listener is not null...
                if(!listener.Equals(null)) {
                    //...Notify the listener.
                    listener.OnEvent(type, sender, param);
                }
            }
        }
    }

    /// <summary>
    /// Method removes event and all listeners of that event from the dictionary.
    /// </summary>
    /// <param name="type">The event to remove.</param>
    public void RemoveEvent(Enum type) {
        //Remove event entry from dictionary.
        _listeners.Remove(type);
    }

    /// <summary>
    /// Method removes all redundant event entries from the dictionary.
    /// </summary>
    public void RemoveRedundancies() {
        //Create a temporary dictionary.
        Dictionary<Enum, List<IListener>> tmpListeners = new Dictionary<Enum, List<IListener>>();

        //For each key value pair in listeners
        foreach(KeyValuePair<Enum, List<IListener>> entry in _listeners) {
            //...For each listener in every key value pair...
            foreach(IListener listener in entry.Value) {
                //...If the listener is null...
                if(listener.Equals(null)) {
                    //...Remove the listener
                    entry.Value.Remove(listener);
                }
            }

            if(entry.Value.Count > 0) {
                tmpListeners.Add(entry.Key, entry.Value);
            }
        }
    }
}
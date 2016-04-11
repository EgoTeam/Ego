using UnityEngine;
using System;

public interface IListener {
    void OnEvent(Enum type, Component sender, System.Object param = null);
}
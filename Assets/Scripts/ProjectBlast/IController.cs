using Notloc.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController : IGameObject
{
    bool enabled { get; set; }

    void SetInput(InputScript input);
    void SetControlsActive(bool state);
}

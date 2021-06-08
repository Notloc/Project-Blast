using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable : IGameObject
{
    void Damage(float amount);
}

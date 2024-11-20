using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Idamageable
{ 
    void TakeDamage(float damage, Transform damageSource);
    void Dead();
}



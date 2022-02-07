using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDmg_By_Ally<T>
{
    void DamageTaken(T dmgTaken);
   
}

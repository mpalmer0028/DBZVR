using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructible
{
    /// <summary>
    /// Causes this gameobject to destory itself
    /// </summary>
    /// <returns></returns>
    GameObject Destruct();
}

using UnityEngine;

/// <summary>
/// Base class for anything in any level that should react to being hit by the fire spell.
/// PlayerFire only knows about this class, never the specific puzzle scripts -- that way
/// adding new fire-reactive objects in level 2/3 never requires touching PlayerFire.cs again.
/// </summary>
public abstract class FireReceiver : MonoBehaviour
{
    public abstract void ReceiveFire();
}

using UnityEngine;

public class WireSocket : MonoBehaviour
{
    public WireCog cog;

    public void OnPlugConnected()
    {
        if (cog != null)
            cog.StartSpinning();
    }
}
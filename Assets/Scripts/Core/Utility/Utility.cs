using UnityEngine;

public class Utility : MonoBehaviour
{
    static GameObject _player;

    public static bool debug;

    public static GameObject GetPlayerObject()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        return _player;
    }
}

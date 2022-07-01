using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToAndFromBuildingMusic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RightHand" ||
            other.tag == "LeftHand")
        {
            AudioManager.Instance.inDoors = true;
            AudioManager.Instance.SetMusic(AudioManager.clips.ChaseBuildingMusic);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "RightHand" ||
            other.tag == "LeftHand")
        {
            AudioManager.Instance.inDoors = false;
            AudioManager.Instance.SetMusic(AudioManager.clips.BeachSound);
        }
    }
}

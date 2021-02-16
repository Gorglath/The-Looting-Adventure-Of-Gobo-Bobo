using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDoorAnimationHelper : MonoBehaviour
{
    public void ResetParameter()
    {
        GetComponent<Animator>().SetBool("Spawn", false);
    }
}

using UnityEngine;
using Bhaptics.SDK2;

public class HapticTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Play haptic");
            BhapticsLibrary.Play("punch_soft_simple");
        }
    }
}
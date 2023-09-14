using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideDuringGame : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        gameObject.SetActive(false);
    }
}

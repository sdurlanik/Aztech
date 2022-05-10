using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatforms : MonoBehaviour
{
    [SerializeField] private GameObject _rotateLeft;
    [SerializeField] private GameObject _rotateRight;
    void Start()
    {
        LeanTween.rotateAround( _rotateLeft, Vector3.forward, 360f, 4f).setLoopCount(-1);
        LeanTween.rotateAround( _rotateRight, -Vector3.forward, 360f, 4f).setLoopCount(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

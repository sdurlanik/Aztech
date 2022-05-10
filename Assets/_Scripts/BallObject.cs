using UnityEngine;

public class BallObject : MonoBehaviour
{
    [SerializeField] private Transform _nextBall = null;


    public Transform NextBall
    {
        get => _nextBall;
        set => _nextBall = value;
    }
}
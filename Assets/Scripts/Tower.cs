using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public List<Disc> discs = new List<Disc>();
    private Vector3 position;
    private bool positionIsFounded;
    private GameController gameController;
    public Transform platform;
    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    public Vector3 GetPosition()
    {
        if (positionIsFounded) return position;
        position = platform.position;
        positionIsFounded = true;
        return position;
    }
    private void OnMouseDown()
    {
        gameController.OnClick(this);
    }
}
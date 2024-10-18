using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{
    private GridObject gridObject;
    private Movable movable;

    private void Start()
    {
        gridObject = GetComponent<GridObject>();
        movable = GetComponent<Movable>();
    }

    private void Update()
    {
        Vector2Int input = GetInput();

        if (input != Vector2Int.zero)
        {
            movable.Move(input);
        }
    }

    private Vector2Int GetInput()
    {
        Vector2Int input = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) //left
            input = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) //right
            input = Vector2Int.right;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) //up
            input = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) //down
            input = Vector2Int.up;
        return input;
    }
}

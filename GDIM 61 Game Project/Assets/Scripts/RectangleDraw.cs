using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DrawState
{
    notDrawing,
    finishedDrawing,
    Drawing
}

public class RectangleDraw : MonoBehaviour
{
    [SerializeField] GameObject _rectangle;


    private DrawState _currentDrawState;
    private GameObject _currentRectangle;
    private Vector3 _startingMouseCoordinates;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0) && _currentDrawState == DrawState.Drawing)
        {
            if (_currentRectangle.GetComponent<Rectangle>().placeable == true)
            {
                TransitionDrawState(DrawState.finishedDrawing);
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && _currentDrawState == DrawState.notDrawing)
        {
            TransitionDrawState(DrawState.Drawing);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && _currentDrawState == DrawState.Drawing)
        {
            TransitionDrawState(DrawState.notDrawing);
            return;
        }

        if (_currentDrawState == DrawState.Drawing) // if user is drawing and thus has a rectangle already created 
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 lineToMouse = Vector3.Normalize(mousePosition - _currentRectangle.transform.position);
            float angle = Mathf.Atan2(lineToMouse.y, lineToMouse.x) * Mathf.Rad2Deg;

            Quaternion rotationToMouse = Quaternion.AngleAxis(angle, Vector3.forward);

            _currentRectangle.transform.rotation = rotationToMouse;
            _currentRectangle.transform.localScale = new Vector3( Vector3.Distance(_startingMouseCoordinates, mousePosition), 0.5f, 0);
            _currentRectangle.transform.position = new Vector3((_startingMouseCoordinates.x + mousePosition.x) / 2, (_startingMouseCoordinates.y + mousePosition.y) / 2, 0);

        }

    }

    private void TransitionDrawState(DrawState newDrawState)
    {
        switch (newDrawState)
        {
            case DrawState.Drawing:
                _startingMouseCoordinates = GetMouseWorldPosition();
                _currentRectangle = Instantiate(_rectangle, _startingMouseCoordinates, Quaternion.identity);
                _currentDrawState = DrawState.Drawing;
                break;
            case DrawState.finishedDrawing:
                _currentRectangle.GetComponent<Rectangle>().Place();
                _currentRectangle = null;
                _startingMouseCoordinates = Vector3.zero;
                _currentDrawState = DrawState.notDrawing;
                break;
            case DrawState.notDrawing:
                if (_currentRectangle) Destroy(_currentRectangle);
                _startingMouseCoordinates = Vector3.zero;
                _currentDrawState = DrawState.notDrawing;
                break;
        }

    }


    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseCoordinates = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseCoordinates.z = 0;
        return mouseCoordinates;
    }
}

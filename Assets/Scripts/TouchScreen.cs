using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

// This class is used to manage the grid UI meaning position the tails, resize them, and create the grid seen by the user
public class TouchScreen : MonoBehaviour, IDragHandler, IEndDragHandler
{
    #region FIELDS
    private Grid grid;
    private enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left
    }
    #endregion

    #region  IDragHandler - IEndDragHandler
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Press position + " + eventData.pressPosition);
        Debug.Log("End position + " + eventData.position);
        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
        Debug.Log("norm + " + dragVectorDirection);
        GetDragDirection(dragVectorDirection);
    }

    //It must be implemented otherwise IEndDragHandler won't work 
    public void OnDrag(PointerEventData eventData)
    {

    }

    private DraggedDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
            if(draggedDir==DraggedDirection.Right){playerController.Control(6);}else{playerController.Control(4);}
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
            if(draggedDir==DraggedDirection.Up){playerController.Control(8);}else{playerController.Control(2);}
        }
        Debug.Log(draggedDir);
        return draggedDir;
    }
    #endregion
    PlayerController playerController;
    void Start(){
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
}

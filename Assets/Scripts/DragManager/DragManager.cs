using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragManager : MonoBehaviour
{
	private DragSelection dragSelection;

    private void Start()
    {
        dragSelection = GetComponent<DragSelection>();
    }

    private void Update()
    {
        dragSelection.UpdateDragSelection();
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragDropUnit : MonoBehaviour
{
    private BoxCollider activeUnitGroundBoxCollider;
    private BoxCollider saleUnitGroundBoxCollider;
    private Camera mainCamera;
    private UnitAbilites unitAbilites;
    

    private Vector3 offset;
    private float yPosition;
    private Vector3 initPosition;

    void Start()
    {
        mainCamera = Camera.main;
        yPosition = transform.position.y;
        initPosition = transform.position;

        activeUnitGroundBoxCollider = GameObject.Find("ActiveUnitGround").GetComponent<BoxCollider>();
        saleUnitGroundBoxCollider = GameObject.Find("SaleUnitGround").GetComponent<BoxCollider>();
        unitAbilites = GetComponent<UnitAbilites>();
    }
    private void OnMouseDown()
    {
        // 마우스 클릭 시, 오브젝트와 마우스 간의 거리 계산
        offset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        // 드래그하는 동안 오브젝트의 위치를 마우스 위치 + offset, 드래그 중임을 인지 할 수 있게 y + 2로 함
        Vector3 newPosition = GetMouseWorldPos() + offset;
        newPosition.y = yPosition + 2;
        transform.position = newPosition;
    }

    private void OnMouseUp()
    {
        // 드롭 시에 오브젝트 원래 y위치로 복귀
        Bounds activeUnitBounds = activeUnitGroundBoxCollider.bounds;
        Bounds saleUnitBounds = saleUnitGroundBoxCollider.bounds;
        if (activeUnitBounds.Contains(transform.position))
        {
            Vector3 newPosition = transform.position;
            newPosition.y = yPosition;
            transform.position = newPosition;
            unitAbilites.SetActivation(true);
        }
        else if (saleUnitBounds.Contains(transform.position))
        {
            Destroy(gameObject);
            // Gold 추가 로직 구현
        }
        else
        {
            transform.position = initPosition;
        }
    }

    Vector3 GetMouseWorldPos()
    {
        // 마우스 위치를 화면 좌표에서 월드 좌표로 변환
        Vector3 mousePoint = Input.mousePosition;
        // 카메라에서 오브젝트의 월드 좌표를 화면 좌표로 변환한 z 값(월드 좌표의 y값)을 설정
        mousePoint.z = mainCamera.WorldToScreenPoint(gameObject.transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
}

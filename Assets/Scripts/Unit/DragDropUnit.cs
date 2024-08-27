using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragDropUnit : MonoBehaviour
{
	private BoxCollider activeUnitGroundBoxCollider;
	private BoxCollider saleUnitGroundBoxCollider;
	private Camera mainCamera;
	private UnitAbilities unitAbilities;
	

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
		unitAbilities = GetComponent<UnitAbilities>();
	}
	private void OnMouseDown()
	{
		// ���콺 Ŭ�� ��, ������Ʈ�� ���콺 ���� �Ÿ� ���
		offset = transform.position - GetMouseWorldPos();
	}

	private void OnMouseDrag()
	{
		// �巡���ϴ� ���� ������Ʈ�� ��ġ�� ���콺 ��ġ + offset, �巡�� ������ ���� �� �� �ְ� y + 2�� ��
		Vector3 newPosition = GetMouseWorldPos() + offset;
		newPosition.y = yPosition + 2;
		transform.position = newPosition;
	}

	private void OnMouseUp()
	{
		GameManager gameManager = GameManager.Instance;
		PlayerInformation playerInformation = PlayerInformation.Instance;
		Bounds activeUnitBounds = activeUnitGroundBoxCollider.bounds;
		Bounds saleUnitBounds = saleUnitGroundBoxCollider.bounds;

		if (activeUnitBounds.Contains(transform.position) && gameManager.GetActiveUnitList().Count < playerInformation.level)
		{
			Vector3 newPosition = transform.position;
			newPosition.y = yPosition;
			transform.position = newPosition;
			unitAbilities.SetActivation(null);
			SynergyManager.Instance.AddSynergyList(gameObject);
		}
		else if (saleUnitBounds.Contains(transform.position))
		{
			unitAbilities.SetActivation(null);
			gameManager.RemoveUnitInstance(gameObject);
			// ���� �Ǹ� �� ȹ�� ��� ����
			playerInformation.gold += 3;
		}
		else
		{
			transform.position = initPosition;
		}
	}

	Vector3 GetMouseWorldPos()
	{
		// ���콺 ��ġ�� ȭ�� ��ǥ���� ���� ��ǥ�� ��ȯ
		Vector3 mousePoint = Input.mousePosition;
		// ī�޶󿡼� ������Ʈ�� ���� ��ǥ�� ȭ�� ��ǥ�� ��ȯ�� z ��(���� ��ǥ�� y��)�� ����
		mousePoint.z = mainCamera.WorldToScreenPoint(gameObject.transform.position).z;
		return mainCamera.ScreenToWorldPoint(mousePoint);
	}
}

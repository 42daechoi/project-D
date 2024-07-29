using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawnManager : MonoBehaviour
{
    public Button[] unitSpawnButton;
    public GameObject[] inactiveUnitGround;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Button btn in unitSpawnButton)
        {
            btn.onClick.AddListener(() => HandleUnitSpawnButton(btn));
        }
    }

    void HandleUnitSpawnButton(Button clickedButton)
    {
        ButtonBinder buttonBinder = clickedButton.GetComponent<ButtonBinder>();
        for(int i = 0; i < inactiveUnitGround.Length; i++)
        {
            //inactiveUnitGround�� Ȱ��ȭ �Ǿ� �ִ��� Ȯ���ϰ� ���� ���� �� inactiveUnitGround Ȱ��ȭ ����
            VerifyActivation verifyActivation = inactiveUnitGround[i].GetComponent<VerifyActivation>(); 
            if (!verifyActivation.GetActivation())
            {
                Transform inactiveUnitGroundTransfrom = inactiveUnitGround[i].transform;
                Vector3 placePosition = inactiveUnitGroundTransfrom.position + new Vector3(0, 1, 0);
                GameObject unit = buttonBinder.unitObject;
                GameObject unitInstance = Instantiate(unit, placePosition, inactiveUnitGroundTransfrom.rotation);

                // ���������� ������ �ν��Ͻ��� ���ؼ� SetActivation ����
                GameManager.Instance.AddUnitInstance(unitInstance);
                unitInstance.GetComponent<UnitAbilities>().SetActivation(inactiveUnitGround[i]);
                clickedButton.gameObject.SetActive(false);
                return ;
            } 
        }
    }
}

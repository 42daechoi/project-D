using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InactiveUnitEvent : MonoBehaviour
{
	public Button inactiveUnitButton;

	// Start is called before the first frame update
	void Start()
	{
		if (inactiveUnitButton != null)
		{
			inactiveUnitButton.onClick.AddListener(InactiveUnits);
		}
	}

	void InactiveUnits()
	{
		GameObject[] selectedUnits = GameManager.Instance.GetSelectedUnits();
		GameObject[] inactiveUnitGrounds = GameObject.FindGameObjectsWithTag("InactiveUnitGround");
        GameObject[] sortedObjects = inactiveUnitGrounds.OrderBy(go => go.name).ToArray();

		foreach (GameObject unit in selectedUnits)
		{
            UnitAbilities ua = unit.GetComponent<UnitAbilities>();

			ParticleControl(ua);
            foreach (GameObject so in sortedObjects)
			{
				VerifyActivation verifyActivation = so.GetComponent<VerifyActivation>();
				if (!verifyActivation.GetActivation())
				{
                    Transform iugTransform = so.transform;
					Vector3 placePosition = iugTransform.position + new Vector3(0, 1, 0);

					ua.SetActivation(so);
                    unit.transform.position = placePosition;
                    SynergyManager.Instance.RemoveSynergyList(unit);
                    break ;
				}
			}
		}
	}
    private void ParticleControl(UnitAbilities ua)
    {
		Transform buffParticle = ua.gameObject.transform.Find("Buff");
		buffParticle.gameObject.SetActive(false);
        if (ua.activeParticles != null && ua.activeParticles.Count > 0)
        {
            foreach (var particle in ua.activeParticles)
            {
                if (particle != null) // 각 파티클이 null이 아닌지 확인
                {
                    Destroy(particle.gameObject);
                }
            }
            ua.activeParticles.Clear(); // 리스트 비우기
        }
    }
}

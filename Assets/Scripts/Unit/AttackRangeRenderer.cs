using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeRenderer : MonoBehaviour
{
    public int segments = 50;  // 원을 이루는 선분의 개수
    private LineRenderer lineRenderer;

    void Awake()
    {
        // LineRenderer 컴포넌트를 가져옴
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;  
        lineRenderer.useWorldSpace = false;  // 로컬 좌표계를 사용
    }

    public void UpdateCircle(float attackRange)
    {
        if (lineRenderer == null)
        {
            Debug.LogError("라인렌더러 null값");
            return;
        }

        // 부모 오브젝트의 스케일을 반영하여 반지름을 조정
        float parentScaleFactor = transform.lossyScale.x;  // 부모 오브젝트의 전체 스케일을 고려
        float adjustedRange = attackRange / parentScaleFactor;

        float angle = 0f;
        float increment = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * adjustedRange;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * adjustedRange;

            lineRenderer.SetPosition(i, new Vector3(x, 0, z));

            angle += increment;
        }
    }

    public void Show()
    {
        lineRenderer.enabled = true;
    }

    public void Hide()
    {
        lineRenderer.enabled = false;
    }
}

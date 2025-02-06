using UnityEngine;

public class TestManager : MonoBehaviour
{
    private Orc selectedOrc;

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 클릭
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Orc orc = hit.collider.GetComponent<Orc>();
                if (orc != null)
                {
                    selectedOrc = orc;
                    Debug.Log($"선택된 Orc: {orc.gameObject.name}");

                    selectedOrc.Die();
                }
            }
        }
    }
}
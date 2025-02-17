using TMPro;
using UnityEngine;

namespace UntilTheEnd
{
    /// <summary>
    /// 여기를 이름으로 안쓸꺼면 반짝이는 다이아로 캐릭터 향해서 계속 보여주는식으로도 괜찮을듯?
    /// 그리고 캐릭터랑 상호작용 (스페이스바를 쳤다면 더이상 반짝이지 않게 하는것도...?)
    /// 반짝이는건 파티클시스템을 이용하도록 하자!!
    /// 챗지피티는 Renderer을 받아와서 Emission을 체크해서 색상을 변경해주는걸 하라고 하더라고...흠
    /// </summary>
    public class ItemObject : MonoBehaviour, IInteractable
    {
        public Item item;
        //public GameObject canvas_SpaceBarText;

        private void Start()
        {
            //혹시라도 켜져있으면 꺼둠
            //canvas_SpaceBarText.gameObject.SetActive(false);



            //이거는 소모성 아이템일 때 사용하면 되긴할텐데...
            //디스트로이말고 비활성화로 가야 맞는걸? 아직 잘모르겠음
            //Destroy(gameObject);

            //왜냐면 주로 이런 소모성 아이템들은 clone으로 소환해서 관리하는게 제일 좋지않나 싶거든...
            //일단 잠시 보류
        }
















        // 일단 인터페이스를 사용해서 상호작용을 만들면 온트리거쪽은 좀 수정이 필요함
        // 이거대로 제대로 동작안할테니 일단 잠시 보류해둠


        #region 플레이어가 오브젝트와 상호작용 할 때 SpaceBar 누르라고 WorldCanvas Text가 나오게 하는 역할
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(StringValues.Tag.player))
            {
                //EquipmentManager.instance.EquipItem(StringValues.MortalObjectsCSV.mainTest, item);

                EquipmentManager.instance.isInteractedObject = true;



                //canvas_SpaceBarText.gameObject.SetActive(true);
                // UIWorldCanvasController 이용해 UI 표시
                UIWorldCanvasController.instance.ShowUI(this.gameObject.transform.position);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(StringValues.Tag.player))
            {
                EquipmentManager.instance.isInteractedObject = false;




                //canvas_SpaceBarText.gameObject.SetActive(false);
                // UI 숨기기
                UIWorldCanvasController.instance.HideUI();
            }
        }
        #endregion

        public void Interact()
        {
            Debug.Log("아이템과 상호작용 시작 !!");


            EquipmentManager.instance.EquipItem(StringValues.MortalObjectsCSV.mainTest, item);


            // 상호작용하고 난 후... 일단 보류
            //canvas_SpaceBarText.gameObject.SetActive(false);
            //gameObject.SetActive(false);
            
            // UI 숨기기
            //UIItemController.instance.HideUI();
        }
        
    }
}
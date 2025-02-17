using System.Collections;
using TMPro;
using UnityEngine;

namespace UntilTheEnd
{
    public class UIDialogue : MonoBehaviour
    {
        public GameObject dialogueBackGroundImage;
        public TextMeshProUGUI dialogueText;
        public float typingSpeed = 0.05f;

        private Coroutine _typingCoroutine;
        private string _fullDialogueText; // 전체 문장을 저장할 변수
        private bool _isTyping = false;

        // 타이핑 상태 확인하는 프로퍼티
        public bool IsTyping
        {
            get { return _isTyping; }
        }

        public void ShowDialogueUI()
        {
            dialogueBackGroundImage.SetActive(true);
        }

        public void HideDialogueUI()
        {
            dialogueBackGroundImage.SetActive(false);
            dialogueText.text = "";
        }

        public void DisplayDialogueText(string text)
        {
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
            }
            _typingCoroutine = StartCoroutine(TypeTextEffect(text));
        }

        private IEnumerator TypeTextEffect(string text)
        {
            _isTyping = true;
            dialogueText.text = "";
            _fullDialogueText = text; // 전체 문장 저장 (즉시 출력용)

            foreach (char letter in text)
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            _isTyping = false;
        }


        // 글자를 즉시 출력
        
        public void FinishTyping()
        {
            if (_typingCoroutine != null)
            {
                StopCoroutine(_typingCoroutine);
                _typingCoroutine = null;
            }

            dialogueText.text = _fullDialogueText; // 🔹 전체 문장 즉시 출력
            _isTyping = false;
        }

    }
}
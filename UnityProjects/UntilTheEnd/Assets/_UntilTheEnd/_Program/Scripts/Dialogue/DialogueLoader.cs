using System.Collections.Generic;
using UnityEngine;

namespace UntilTheEnd
{
    /// <summary>
    /// 기존 DialogueManager에서 CSV 데이터를 읽어 List를 반환하는 역할만 수행하도록 함
    /// 딱 데이터 로드 부분만 떼옴
    /// </summary>
    public class DialogueLoader
    {
        private TextAsset _csvFile;

        public DialogueLoader(TextAsset csvFile)
        {
            _csvFile = csvFile;
        }

        public List<Dialogue> LoadDialogues()
        {
            List<Dialogue> dialogues = new List<Dialogue>();

            if (_csvFile == null)
            {
                Debug.LogError("CSV 파일이 없음");
                return dialogues;
            }

            string[] dataLines = _csvFile.text.Split('\n');

            for (int i = 1; i < dataLines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(dataLines[i])) continue;
                string[] rowData = dataLines[i].Split(',');
                if (rowData.Length < 5) continue;      // 컬럼 개수 부족한 경우 예외 방지
                string sceneName = rowData[0].Trim();
                string numberStr = rowData[1].Trim();
                string npc = rowData[2].Trim();
                string answerStr = rowData[3].Trim();
                string dialog = rowData[4].Trim();
                if (!int.TryParse(numberStr, out int number))
                {
                    //Debug.LogWarning($"[DialogueLoader] Number 값 파싱 실패 (줄: {i + 1}) => '{numberStr}'");
                    continue;
                }
                bool dialogueAnswer = answerStr.ToUpper() == "TRUE"; // Answer 파싱 (True/False 외 값 방지 가능)

                dialogues.Add(new Dialogue(sceneName, number, npc, dialogueAnswer, dialog));
            }

            return dialogues;
        }
    }
}
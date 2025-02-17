using System.Collections.Generic;
using UnityEngine;

namespace UntilTheEnd
{
    /// <summary>
    /// 기존 DialogueManager에서 CSV 데이터를 읽어 List를 반환하는 역할만 수행하도록 함
    /// 딱 데이ㅓ 로드 부분만 떼옴
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
                if (string.IsNullOrWhiteSpace(dataLines[i]))
                    continue;

                string[] rowData = dataLines[i].Split(',');
                string sceneName = rowData[0].Trim();
                int number = int.Parse(rowData[1]);
                string npc = rowData[2].Trim();
                bool dialogueAnswer = rowData[3].Trim().ToUpper() == "TRUE";
                string dialog = rowData[4].Trim();

                dialogues.Add(new Dialogue(sceneName, number, npc, dialogueAnswer, dialog));
            }

            return dialogues;
        }
    }
}
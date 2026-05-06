using Code.Dialogue;

namespace EventSystem
{
    public class DialogueEvents
    {
        public static DialogueStartRequestEvent DialogueStartRequestEvent = new DialogueStartRequestEvent();
        public static DialogueEndEvent DialogueEndEvent = new DialogueEndEvent();
        public static DialogueShowEvent DialogueShowEvent = new DialogueShowEvent();
        public static DialogueInputEvent DialogueInputEvent = new DialogueInputEvent();
    }

    public class DialogueStartRequestEvent : GameEvent
    {
        public string DialogueID;
        public DialoguePositionData[] Position;

        /// <param name="dialogueID">다이얼로그 시작하는 부분의 ID. 다이얼로그 시트에서 확인 가능</param>
        /// <param name="position">말하는 캐릭터의 머리 꼭대기 좌표 보내주면 됨</param>
        public DialogueStartRequestEvent Initializer(string dialogueID, DialoguePositionData position)
        {
            DialogueID = dialogueID;
            Position = new[] { position };
            return this;
        }

        /// <param name="dialogueID">다이얼로그 시작하는 부분의 ID. 다이얼로그 시트에서 확인 가능</param>
        /// <param name="position">말하는 캐릭터의 머리 꼭대기 좌표. 다이얼로그 시트의 인덱스 번호에 맞춰서 보내줘야함(플레이어는 안 보내줘도 됨) </param>
        public DialogueStartRequestEvent Initializer(string dialogueID, DialoguePositionData[] position)
        {
            DialogueID = dialogueID;
            Position = position;
            return this;
        }
    }

    public class DialogueEndEvent : GameEvent
    {
    }

    public class DialogueShowEvent : GameEvent
    {
        public string Text;
        public DialoguePositionData Position;
        public string[] ChoicesText;
        public bool Auto;

        public DialogueShowEvent Initializer(string text, DialoguePositionData position, string[] choicesText = null,
            bool auto = false)
        {
            Text = text;
            Position = position;
            ChoicesText = choicesText;
            Auto = auto;
            return this;
        }
    }

    public class DialogueInputEvent : GameEvent
    {
        public int ChoiceIndex;

        public DialogueInputEvent Initializer(int choiceIndex = 0)
        {
            ChoiceIndex = choiceIndex;
            return this;
        }
    }
}
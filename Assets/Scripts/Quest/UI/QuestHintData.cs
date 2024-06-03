using UnityEngine;

namespace CC.Quest.UI
{
    [CreateAssetMenu(fileName = "Quest Hint Data", menuName = "Game/Quest/Quest Hint Data")]
    public class QuestHintData : ScriptableObject
    {
        public string questObjective;
        public int ID;
    }
}

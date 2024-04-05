using UnityEngine;

namespace CC.Characters
{

    [CreateAssetMenu(fileName = "BlockMove", menuName = "Game/Block Move")]
    public class BlockSO : ScriptableObject
    {
        [field: SerializeField] public string AnimationName { get; private set; }
        //[field: SerializeField] public int ComboStateIndex { get; private set; } = -1;
        //[field: SerializeField] public float ComboAttackTime { get; private set; }
        //[field: SerializeField] public float ForceTime { get; private set; }
        //[field: SerializeField] public float Force { get; private set; }
        //[field: SerializeField] public int Damage { get; private set; }
    }
}

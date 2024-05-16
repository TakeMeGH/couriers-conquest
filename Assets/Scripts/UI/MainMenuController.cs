using UnityEngine;

namespace CC.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] InputReader _inputReader;

        private void Start()
        {
            _inputReader.EnableInventoryUIInput();
        }
    }
}

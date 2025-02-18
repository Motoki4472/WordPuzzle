using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Result
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ChangeBountyHuntResultText : MonoBehaviour
    {
        [SerializeField] private TMP_Text RemainingTurnText = default;
        [SerializeField] private TMP_Text PassedTurnText = default;
        [SerializeField] private TMP_Text BountyListText = default;
        private TextMeshProUGUI RemainingTurnUGUI;
        private TextMeshProUGUI PassedTurnUGUI;
        private TextMeshProUGUI BountyListUGUI;
        public int RemainingTurn = 0;
        public int PassedTurn = 0;
        public List<string> BountyList = new List<string>();

        private void Start()
        {
            if(BountyList.Count == 0)
            {
                BountyListText.text = "YOU WIN!";
            }
            else
            {
                BountyListText.text = $"BOUNTY LIST\n{string.Join("\n", BountyList).ToUpper()}";
            }
            RemainingTurnText.text = $"REMAINING TURN: {RemainingTurn}";
            PassedTurnText.text = $"PASSED TURN: {PassedTurn}";
            
            RemainingTurnUGUI = RemainingTurnText.GetComponent<TextMeshProUGUI>();
            PassedTurnUGUI = PassedTurnText.GetComponent<TextMeshProUGUI>();
            BountyListUGUI = BountyListText.GetComponent<TextMeshProUGUI>();
            RemainingTurnUGUI.maxVisibleCharacters = 0;
            PassedTurnUGUI.maxVisibleCharacters = 0;
            BountyListUGUI.maxVisibleCharacters = 0;

            RemainingTurnUGUI.DOMaxVisibleCharacters(RemainingTurnUGUI.text.Length, 2.0f).SetDelay(1.3f);
            PassedTurnUGUI.DOMaxVisibleCharacters(PassedTurnUGUI.text.Length, 1.5f).SetDelay(1.8f);
            BountyListUGUI.DOMaxVisibleCharacters(BountyListUGUI.text.Length, 5f).SetDelay(1.3f);
        }

    }

}
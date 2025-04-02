using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Yudiz.StarterKit.UI
{
    public class GameWinScreen : Screen
    {
        [SerializeField] Game game;

        [SerializeField] Button playAgainBtn;
        [SerializeField] Button homeButton;

        [SerializeField] Image winnerKingImage;

        [SerializeField] Sprite whiteKingSprite; 
        [SerializeField] Sprite blackKingSprite;

        [SerializeField] TextMeshPro winnerText;

        public override void Show()
        {
            base.Show();

            string winner = game.GetWinner();
            if (winner == "white")
            {
                winnerKingImage.sprite = whiteKingSprite;
            }
            else if (winner == "black")
            {
                winnerKingImage.sprite = blackKingSprite;
            }

            playAgainBtn.onClick.AddListener(OnPlayAgain);
            homeButton.onClick.AddListener(OnHomee);
        }

        public void OnPlayAgain()
        {
            UIManager.Instance.ShowScreen(ScreenName.PlayScreen);
        }

        public void OnHomee()
        {
            Debug.Log("Pressed home button");
            UIManager.Instance.ShowScreen(ScreenName.StartScreen);
        }

        public override void Hide()
        {
            base.Hide();
            playAgainBtn.onClick.RemoveListener(OnPlayAgain);
            homeButton.onClick.RemoveListener(OnHomee);
        }
    }
}

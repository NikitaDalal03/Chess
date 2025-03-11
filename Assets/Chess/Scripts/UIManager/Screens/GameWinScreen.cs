using UnityEngine;
using UnityEngine.UI;

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

        public override void Show()
        {
            base.Show();

            // Access the winner's color from the Game class
            string winner = game.GetWinner();

            // Display the corresponding king sprite based on the winner's color
            if (winner == "white")
            {
                winnerKingImage.sprite = whiteKingSprite;
            }
            else if (winner == "black")
            {
                winnerKingImage.sprite = blackKingSprite;
            }

            // Add listeners for the buttons
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
            // Remove listeners to prevent multiple bindings
            playAgainBtn.onClick.RemoveListener(OnPlayAgain);
            homeButton.onClick.RemoveListener(OnHomee);
        }
    }
}

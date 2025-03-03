using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yudiz.StarterKit.UI
{
    public class GameOverScreen : Screen
    {
        [SerializeField] Button restartBtn;
        [SerializeField] Button homeBtn;

        public override void Show()
        {
            base.Show();
            restartBtn.onClick.AddListener(OnRestart);
            homeBtn.onClick.AddListener(OnHome);
        }

        public void OnRestart()
        {
            UIManager.Instance.ShowScreen(ScreenName.PlayScreen);
        }

        public void OnHome()
        {
            UIManager.Instance.ShowScreen(ScreenName.StartScreen);
        }

        public override void Hide()
        {
            base.Hide();
            restartBtn.onClick.RemoveListener(OnRestart);
            homeBtn.onClick.RemoveListener(OnHome);
        }
    }
}

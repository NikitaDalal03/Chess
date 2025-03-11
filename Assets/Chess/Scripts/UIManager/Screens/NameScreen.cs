using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Yudiz.StarterKit.UI
{
    public class NameScreen : Screen
    {
        [SerializeField] Button nextBtn;
        [SerializeField] Button backBtn;

        [SerializeField] TMP_InputField player1;
        [SerializeField] TMP_InputField player2;

        public static string player1Name;
        public static string player2Name;

        public override void Show()
        {
            base.Show();
            nextBtn.onClick.AddListener(OnNext);
            backBtn.onClick.AddListener(OnBack);
        }

        public void OnNext()
        {
            player1Name = player1.text;
            player2Name = player2.text;
            UIManager.Instance.ShowScreen(ScreenName.PlayScreen);
        }

        public void OnBack()
        {
            UIManager.Instance.ShowScreen(ScreenName.MenuScreen);
        }

        public override void Hide()
        {
            base.Hide();
            nextBtn.onClick.RemoveListener(OnNext);
            backBtn.onClick.RemoveListener(OnBack);
        }
    }
}
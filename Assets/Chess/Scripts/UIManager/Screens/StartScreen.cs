using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yudiz.StarterKit.UI
{
    public class StartScreen : Screen
    {
        [SerializeField] Button startBtn;

        public override void Show()
        {
            base.Show();
            startBtn.onClick.AddListener(OnStart);
        }

        public void OnStart()
        {
            UIManager.Instance.ShowScreen(ScreenName.MenuScreen);
        }

        public override void Hide()
        {
            base.Hide();
            startBtn.onClick.RemoveListener(OnStart);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yudiz.StarterKit.UI
{
    public class OpponentSelectionScreen : Screen
    {
        [SerializeField] Button high;
        [SerializeField] Button medium;
        [SerializeField] Button low;
        [SerializeField] Button backBtn;

        public override void Show()
        {
            base.Show();
            high.onClick.AddListener(OnHigh);
            medium.onClick.AddListener(OnMedium);
            low.onClick.AddListener(OnLow);
            backBtn.onClick.AddListener(OnBack);
        }

        public void OnHigh()
        {
            UIManager.Instance.ShowScreen(ScreenName.PlayScreen);
        }

        public void OnMedium()
        {
            UIManager.Instance.ShowScreen(ScreenName.PlayScreen);
        }

        public void OnLow()
        {
            UIManager.Instance.ShowScreen(ScreenName.PlayScreen);
        }

        public void OnBack()
        {
            UIManager.Instance.ShowScreen(ScreenName.MenuScreen);
        }

        public override void Hide()
        {
            base.Hide();
            high.onClick.RemoveListener(OnHigh);
            medium.onClick.RemoveListener(OnMedium);
            low.onClick.RemoveListener(OnLow);
            backBtn.onClick.RemoveListener(OnBack);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yudiz.StarterKit.UI
{
    public class MenuScreen : Screen
    {
        [SerializeField] Button playWithAIBtn;
        [SerializeField] Button playTogetherBtn;
        [SerializeField] Button backBtn;

        public override void Show()
        {
            base.Show();
            playWithAIBtn.onClick.AddListener(OnPlayWithAI);
            playTogetherBtn.onClick.AddListener(OnPlayTogether);
            backBtn.onClick.AddListener(OnBack);
        }

        public void OnPlayWithAI()
        {
            UIManager.Instance.ShowScreen(ScreenName.OpponentSelectionScreen);
        }

        public void OnPlayTogether()
        {
            UIManager.Instance.ShowScreen(ScreenName.PlayScreen);
        }

        public void OnBack()
        {
            UIManager.Instance.ShowScreen(ScreenName.StartScreen);
        }

        public override void Hide()
        {
            base.Hide();
            playWithAIBtn.onClick.RemoveListener(OnPlayWithAI);
            playTogetherBtn.onClick.RemoveListener(OnPlayTogether);
            backBtn.onClick.RemoveListener(OnBack);
        }
    }
}

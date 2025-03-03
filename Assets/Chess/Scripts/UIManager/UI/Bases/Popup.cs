using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yudiz.StarterKit.UI
{
	public class Popup : BaseUI
    {
		[SerializeField] private Button bgButton;

		public override void Show()
		{
			base.Show();
			if (bgButton != null)
				bgButton.onClick.AddListener(OnBgButtonClicked);
		}

		protected virtual void OnBgButtonClicked()
		{

		}

		public override void Back()
		{
			base.Back();
			if (bgButton != null)
				bgButton.onClick.RemoveListener(OnBgButtonClicked);
		}
	}
}
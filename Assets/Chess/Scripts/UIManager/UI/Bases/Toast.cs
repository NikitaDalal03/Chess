using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Yudiz.StarterKit.UI
{
	public class Toast : BaseUI
	{
		[SerializeField] private TMP_Text titleText;
		[SerializeField] private TMP_Text messageText;

		public override void Show()
		{
			base.Show();
			Invoke("HideToast", 20f);
		}

		public void HideToast()
		{
			UIManager.Instance.HideToast();
		}

		public void SetData(string titleText, string messageText)
		{
			this.titleText.text = titleText;
			this.messageText.text = messageText;
		}

		public override void Back()
		{
			base.Back();
			UIManager.Instance.HideToast();
		}
	}
}

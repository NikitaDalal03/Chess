using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yudiz.StarterKit.UI
{
	public class BaseUI : MonoBehaviour
	{
		private Canvas canvas;
		[HideInInspector]
		public Transform bg;
		[HideInInspector]
		public Transform content;
		[HideInInspector]
		public bool isActive;
		private UIAnimator uiAnimator;

		public delegate void OnScreenStateChangeDelegate(bool isActive);
		public OnScreenStateChangeDelegate OnScreenStateChanged;

		private void Awake()
		{
			bg = transform.GetChild(0);
			content = transform.GetChild(1);
			canvas = GetComponent<Canvas>();
			uiAnimator = GetComponent<UIAnimator>();
		}

		public virtual void OnShowAnimationCompleted()
		{
		}

		public virtual void Show()
		{
			if (isActive) return;

			canvas.enabled = true;
			isActive = true;
			NotifyState();
			if (uiAnimator == null)
			{
				OnShowAnimationCompleted();
			}
		}

		public virtual void OnHideAnimationCompleted()
		{
			canvas.enabled = false;
		}

		public virtual void Disable()
		{
			canvas.enabled = false;
			isActive = false;
		}

		public virtual void Hide()
		{
			if (!isActive) return;

			isActive = false;
			NotifyState();
			if (uiAnimator == null)
			{
				OnHideAnimationCompleted();
			}
		}

		public virtual void Back()
		{
			
		}

		private void NotifyState()
		{
			OnScreenStateChanged?.Invoke(isActive);
		}
	}

}

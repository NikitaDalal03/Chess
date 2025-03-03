using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

namespace Yudiz.StarterKit.UI
{

    [RequireComponent(typeof(ScreenAnimatable))]
    public class UIAnimator : MonoBehaviour
    {
        public Ease InAnimationEase = Ease.OutExpo;
        public Ease OutAnimationEase = Ease.OutExpo;
        public float InTime = 0.7f;
        public float OutTime = 0.5f;



        public List<Animatable> animatables;
        BaseUI _screen;



        private void Start()
        {
            if (!_screen)
            {
                Initialize();
            }
        }

        private void OnDestroy()
        {
            CleanUp();
        }

        private void Initialize()
        {
            _screen = GetComponentInParent<BaseUI>();
            _screen.OnScreenStateChanged += OnScreenStateChanged;

            if (animatables != null)
            {
                animatables.Clear();
            }

            animatables = new List<Animatable>(GetComponentsInChildren<Animatable>());
            animatables.Sort();

            foreach (Animatable animatable in animatables)
            {
                animatable.Initialize(_screen);
            }
        }

        private void CleanUp()
        {
            if (_screen)
            {
                _screen.OnScreenStateChanged -= OnScreenStateChanged;
                animatables.Clear();
            }
        }

        private void OnScreenStateChanged(bool isActive)
        {
            if (isActive)
            {
				foreach (Animatable animatable in animatables)
				{
					if (animatable is ScreenAnimatable)
					{
						animatable.ShowAnimation(InTime, InAnimationEase, animatable.AnimaionLayer * InTime, _screen.OnShowAnimationCompleted);
					}
					else
					{
						animatable.ShowAnimation(InTime, InAnimationEase, animatable.AnimaionLayer * InTime * 0.2f, null);
					}
				}
			}
            else
            {
				animatables.Reverse();

				foreach (Animatable animatable in animatables)
				{
					if (animatable is ScreenAnimatable)
					{
						animatable.HideAnimation(OutTime, OutAnimationEase, animatable.AnimaionLayer * OutTime, animatable.AnimationTransition, _screen.OnHideAnimationCompleted);
					}
					else
					{
						animatable.HideAnimation(OutTime, OutAnimationEase, 0, animatable.AnimationTransition, null);
					}
				}
				animatables.Reverse();
			}
        }
    }
}
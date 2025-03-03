using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yudiz.StarterKit.Utilities;

namespace Yudiz.StarterKit.UI
{
	public class UIManager : IndestructibleSingleton<UIManager>
	{
		[Header("Screen Data")]
		[SerializeField] private List<ScreenData> screenList;
		[SerializeField] private ScreenName startScreen = ScreenName.None;

		[Header("Popup Data")]
		[SerializeField] private List<PopupData> popupList = new List<PopupData>();
		[SerializeField] private List<Popup> listOfCurrentPopups = new List<Popup>();

		[Header("Toast")]
		[SerializeField] private Toast toast;

		private ScreenData currentScreen = null;    

		private void Start()
		{
			Init();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (currentScreen != null)
					currentScreen.screen.Back();

				if (listOfCurrentPopups.Count > 0)
					listOfCurrentPopups[listOfCurrentPopups.Count - 1].Back();
			}
		}

		public void Init()
		{
			for (int index = 0; index < screenList.Count; index++)
			{
				screenList[index].screen.Disable();
			}

			for (int index = 0; index < popupList.Count; index++)
			{
				popupList[index].popup.Disable();
			}

			if (toast != null)
			{
				toast.Disable();
			}

			if (startScreen != ScreenName.None)
			{
				ShowScreen(startScreen,0);
			}
		}

		public void HideAllUI()
		{
			for (int index = 0; index < screenList.Count; index++)
			{
				screenList[index].screen.Hide();
			}

			for (int index = 0; index < popupList.Count; index++)
			{
				popupList[index].popup.Hide();
			}

			if (toast != null)
			{
				toast.Hide();
			}
		}

		public void ShowScreen(ScreenName screen, float ShowDelay = 0.8f)
		{
			if (currentScreen != null)
			{
				currentScreen.screen.Hide();
			}

			currentScreen = screenList.Find(x => x.screenName == screen);
			if (currentScreen != null)
			{
				currentScreen.screen.Show();
			}

			//Utilities.Utilities.AddDelay(ShowDelay, () =>
			//{
				
			//});
		}

		public void HideScreen(ScreenName screenName)
		{
			Screen screen = screenList.Find(x => x.screenName == screenName).screen;
			if (screen != null)
			{
				screen.Hide();
			}
		}

		public void HideCurrentScreen()
		{
			if (currentScreen != null)
				currentScreen.screen.Hide();
		}

		public void HideAllCurrentPopups()
		{
			if (listOfCurrentPopups.Count > 0)
			{
				foreach(Popup popup in listOfCurrentPopups)
				{
					popup.Hide();
				}
			}
			listOfCurrentPopups.Clear();
		}

		public void ShowPopup(PopupName popupName)
		{
			Popup popup = popupList.Find(x => x.popupName == popupName).popup;
			if (popup != null)
			{
				popup.Show();
				listOfCurrentPopups.Add(popup);
			}
		}

		public void HidePopup(PopupName popupName)
		{
			Popup popup = popupList.Find(x => x.popupName == popupName).popup;
			if (popup != null)
			{
				popup.Hide();
				listOfCurrentPopups.Remove(popup);
			}
		}

		public void ShowToast()
		{
			toast.Show();
		}

		public void HideToast()
		{
			toast.Hide();
		}

		public Screen GetCurrentScreen()
		{
			return currentScreen.screen;
		}

		public Popup GetLastPopup()
		{
			return listOfCurrentPopups.Last();
		}

		public T GetScreen<T>(ScreenName screenName)
		{
			return screenList.Find(x => x.screenName == screenName).screen.GetComponent<T>();
		}

		public T GetPopup<T>(PopupName popupName)
		{
			return popupList.Find(x => x.popupName == popupName).popup.GetComponent<T>();
		}

		public Toast GetToast()
		{
			return toast;
		}
	}

	[System.Serializable]
	public class ScreenData
	{
		public Screen screen;
		public ScreenName screenName;
	}

	[System.Serializable]
	public class PopupData
	{
		public Popup popup;
		public PopupName popupName;
	}

	public enum ScreenName
	{
		None,
		StartScreen,
		PlayScreen,
		GameWinScreen,
		OpponentSelectionScreen,
		MenuScreen,
		//LevelUpScreen,
		//SettingsScreen,
		//InGameSettingsScreen,
		//LevelSelectionTutorialScreen,
		//DeckTutorialScreen,
		//MainLevelTutorialScreen,
		//VideoTutorialScreen,
	}

	public enum PopupName
	{
		//AlertPopup,
		//AnnoucementSectionPopup,
		//AnnouncementDetailsPopup,
		//DailyBonusPopup,
		//DailyBonusRewardPopup,
		//BuyGemPopup,
		//ShopPopup,
		//NewGamePlusPopup,
		//NoMovesOrTimePopup,
		//ExitGamePopup,
		//VersionInCompatiblePopup,
		//NoInternetPopup,
		//MainMenuSettingsPopup,
		//LoadingPopup,
		//HelpSectionPopup,
	}
}

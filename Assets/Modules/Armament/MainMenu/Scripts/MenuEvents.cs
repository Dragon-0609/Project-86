using System;
using UnityEngine;
namespace Armament.MainMenu
{
	public class MenuEvents : MonoBehaviour
	{
		public static MenuEvents Instance;

		public event Action<TabSwitcher> OnTabChanged;
		
		private void Awake()
		{
			Instance = this;
		}

		private void OnDestroy()
		{
			Instance = null;
		}

		public void FireOnTabChanged(TabSwitcher tab)
		{
			OnTabChanged?.Invoke(tab);
		}
	}
}
using System;
using UIKit;

namespace com.interactiverobert.prototypes.movieexplorer.apple.lib
{
	public class PresentationUtility
	{
		public PresentationUtility ()
		{
		}

		public static T CreateFromStoryboard<T> (string storyboardId) where T : UIViewController {
			T result = UIStoryboard.FromName ("Main", null).InstantiateViewController (storyboardId) as T;
			return result;
		}
	}
}


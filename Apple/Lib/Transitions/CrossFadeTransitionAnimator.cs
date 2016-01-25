using System;

using UIKit;
using CoreGraphics;

namespace com.interactiverobert.prototypes.movieexplorer.apple.lib
{
	public class CrossFadeTransitionAnimator : UIViewControllerAnimatedTransitioning
	{
		public CrossFadeTransitionAnimator () {
			
		}

		public override double TransitionDuration (IUIViewControllerContextTransitioning transitionContext) {
			return 0.35;
		}

		public override void AnimateTransition (IUIViewControllerContextTransitioning transitionContext) {
			var inView = transitionContext.ContainerView;
			var toVC = transitionContext.GetViewControllerForKey (UITransitionContext.ToViewControllerKey);
			var toView = toVC.View;

			toView.Alpha = 0.0f;
			inView.AddSubview (toView);

			UIView.Animate (TransitionDuration (transitionContext), () => {
				toView.Alpha = 1.0f;
			}, () => {
				transitionContext.CompleteTransition (true);
			});
		}
	}
}
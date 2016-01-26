using System;

using UIKit;
using CoreGraphics;

namespace com.interactiverobert.prototypes.movieexplorer.apple.lib.Transitions
{
	public class CrossFadeTransitionAnimator : UIViewControllerAnimatedTransitioning
	{
		#region Constructor
		/// <summary>
		/// Initializes a new instance of <see cref="CrossFadeTransitionAnimator"/>.
		/// </summary>
		public CrossFadeTransitionAnimator () {
			
		}
		#endregion

		#region UIViewControllerAnimatedTransitioning overrides
		/// <summary>
		/// Transitions the duration of the transition.
		/// </summary>
		/// <returns>The duration.</returns>
		/// <param name="transitionContext">Transition context.</param>
		public override double TransitionDuration (IUIViewControllerContextTransitioning transitionContext) {
			return 0.35;
		}

		/// <summary>
		/// Animates the transition.
		/// </summary>
		/// <param name="transitionContext">Transition context.</param>
		public override void AnimateTransition (IUIViewControllerContextTransitioning transitionContext) {
			var inView = transitionContext.ContainerView;
			var toVC = transitionContext.GetViewControllerForKey (UITransitionContext.ToViewControllerKey);
			var toView = toVC.View;

			toView.Alpha = 0.0f;
			inView.AddSubview (toView);
			toView.TranslatesAutoresizingMaskIntoConstraints = false;
			inView.AddConstraint (NSLayoutConstraint.Create (inView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, toView, NSLayoutAttribute.Top, 1.0f, 0.0f));
			inView.AddConstraint (NSLayoutConstraint.Create (inView, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, toView, NSLayoutAttribute.Trailing, 1.0f, 0.0f));
			inView.AddConstraint (NSLayoutConstraint.Create (inView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, toView, NSLayoutAttribute.Bottom, 1.0f, 0.0f));
			inView.AddConstraint (NSLayoutConstraint.Create (inView, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, toView, NSLayoutAttribute.Leading, 1.0f, 0.0f));

			UIView.Animate (TransitionDuration (transitionContext), () => {
				toView.Alpha = 1.0f;
			}, () => {
				transitionContext.CompleteTransition (true);
			});
		}
		#endregion
	}
}
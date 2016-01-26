using System;

using UIKit;
using CoreImage;
using Foundation;
using CoreGraphics;

namespace com.interactiverobert.prototypes.movieexplorer.apple.lib.Imaging
{
	/// <summary>
	/// Image manipulation utilities
	/// </summary>
	public static class ImageManipulation
	{
		#region Public methods
		/// <summary>
		/// Scales and blurs the specified image.
		/// </summary>
		/// <returns>The resulting image.</returns>
		/// <param name="size">The desired size of the image</param>
		/// <param name="img">The image to scale and blur</param>
		public static UIImage ScaleAndBlurImage (CGSize size, UIImage img) {
			if (img == null)
				return null;
			
			var gaussianBlur = CIFilter.FromName ("CIGaussianBlur");
			gaussianBlur.SetDefaults ();
			var inputImage = CIImage.FromCGImage (img.CGImage);
			gaussianBlur.Image = inputImage;
			gaussianBlur.SetValueForKey (new NSNumber (10), CIFilterInputKey.Radius);

			var blurredImage = gaussianBlur.OutputImage;
			var result = UIImage.FromImage (blurredImage);
			result = result.Scale (size);

			return result;
		}
		#endregion
	}
}
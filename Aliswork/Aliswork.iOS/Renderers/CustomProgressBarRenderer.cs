using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Aliswork.Timekeeping;
using Aliswork.iOS.Renderers;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;

[assembly: ExportRenderer(typeof(CustomProgressBar), typeof(CustomProgressBarRenderer))]

namespace Aliswork.iOS.Renderers
{
    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        protected override void OnElementChanged(
     ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            Control.ProgressTintColor = Color.FromHex("#61bafe").ToUIColor();
            //Control.TrackTintColor = Color.FromRgb(188, 203, 219).ToUIColor();
        }


        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var X = 1.0f;
            var Y = 10.0f;

            CGAffineTransform transform = CGAffineTransform.MakeScale(X, Y);
            this.Control.Transform = transform;



            this.ClipsToBounds = true;
            this.Layer.MasksToBounds = true;
            this.Layer.CornerRadius = 5; // This is for rounded corners.
        }
    }
}
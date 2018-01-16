using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aliswork;
using Aliswork.Droid.Renderers;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderedButton), typeof(BorderButtonRenderer))]

namespace Aliswork.Droid.Renderers
{
    public class BorderButtonRenderer : Xamarin.Forms.Platform.Android.AppCompat.ButtonRenderer
    {
        private GradientDrawable _normal, _pressed;

        public BorderButtonRenderer() : base()
        {
            SetWillNotDraw(false);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                SetAlignment();

                var density = Math.Max(1, Resources.DisplayMetrics.Density);
                var button = e.NewElement as BorderedButton;
                var mode = MeasureSpec.GetMode(button.BorderRadius);
                var borderRadius = button.BorderRadius * density;
                var borderWidth = button.BorderWidth * density;

                // Create a drawable for the button's normal state
                _normal = new GradientDrawable();
                _normal.SetColor(button.NormalBackgroundColor.ToAndroid());
                _normal.SetStroke((int)borderWidth, button.BorderColor.ToAndroid());
                _normal.SetCornerRadius(borderRadius);

                // Create a drawable for the button's pressed state
                _pressed = new GradientDrawable();
                _pressed.SetColor(button.PressedBackgroundColor.ToAndroid());
                _pressed.SetStroke((int)borderWidth, button.BorderColor.ToAndroid());
                _pressed.SetCornerRadius(borderRadius);

                // Add the drawables to a state list and assign the state list to the button
                var sld = new StateListDrawable();
                sld.AddState(new int[] { Android.Resource.Attribute.StatePressed }, _pressed);
                sld.AddState(new int[] { }, _normal);
                Control.SetBackground(sld);
                Control.SetTextColor(new ColorStateList(
                   new int[][] {
                      new int [] {Android.Resource.Attribute.StatePressed},
                      new int [] {}
                   },
                   new int[] {
                      button.PressedTextColor.ToAndroid(),
                      button.NormalTextColor.ToAndroid()
                   }
                ));
            }
        }

        private void SetAlignment()
        {
            var element = Element as Xamarin.Forms.Button;

            if (element == null || Control == null)
            {
                return;
            }

            Control.Gravity = GravityFlags.CenterHorizontal | GravityFlags.CenterVertical;
        }
    }
}
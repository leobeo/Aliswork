using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Aliswork.Droid.Renderers;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;

[assembly: ExportRenderer(typeof(Label), typeof(MyLabelRenderer))]

namespace Aliswork.Droid.Renderers
{
    public class MyLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            //if (!String.IsNullOrEmpty(Element.FontFamily))
                //Control.Typeface = Typeface.CreateFromAsset(Forms.Context.Assets, "Fonts/" + Element.FontFamily + ".ttf");
        }
    }
}
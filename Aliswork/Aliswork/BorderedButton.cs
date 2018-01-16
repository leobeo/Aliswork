using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Aliswork
{
    public class BorderedButton : Button
    {
        public static readonly BindableProperty NormalTextColorProperty = BindableProperty.Create(
               $"{nameof(NormalTextColor)}", typeof(Color), typeof(BorderedButton), Color.Black);

        public Color NormalTextColor
        {
            get { return (Color)GetValue(NormalTextColorProperty); }
            set { SetValue(NormalTextColorProperty, value); }
        }

        public static readonly BindableProperty PressedTextColorProperty = BindableProperty.Create(
            $"{nameof(PressedTextColor)}", typeof(Color), typeof(BorderedButton), Color.White);

        public Color PressedTextColor
        {
            get { return (Color)GetValue(PressedTextColorProperty); }
            set { SetValue(PressedTextColorProperty, value); }
        }

        public static readonly BindableProperty NormalBackgroundColorProperty = BindableProperty.Create(
            $"{nameof(NormalBackgroundColor)}", typeof(Color), typeof(BorderedButton), Color.White);

        public Color NormalBackgroundColor
        {
            get { return (Color)GetValue(NormalBackgroundColorProperty); }
            set { SetValue(NormalBackgroundColorProperty, value); }
        }

        public static readonly BindableProperty PressedBackgroundColorProperty = BindableProperty.Create(
            $"{nameof(PressedBackgroundColor)}", typeof(Color), typeof(BorderedButton), Color.FromHex("#5C9AD5"));

        public Color PressedBackgroundColor
        {
            get { return (Color)GetValue(PressedBackgroundColorProperty); }
            set { SetValue(PressedBackgroundColorProperty, value); }
        }
    }
}
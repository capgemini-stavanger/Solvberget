using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace Solvberget.Droid.Views.Components
{
    [Register("solvberget.droid.views.components.BackgroundBindableLinearLayout")]
    public class BackgroundBindableLinearLayout : LinearLayout
    {
        protected BackgroundBindableLinearLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public BackgroundBindableLinearLayout(Context context) : base(context)
        {
        }

        public BackgroundBindableLinearLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public BackgroundBindableLinearLayout(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        private Color _backgroundColor;
        public Color BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
                SetBackgroundColor(value);
            }
        }
    }
}
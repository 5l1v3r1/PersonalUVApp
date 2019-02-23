using System;
using Android.Content;
using Android.Views;
using PersonalUVApp.Controls;
using PersonalUVApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyTableView), typeof(MyTableViewRenderer))]
namespace PersonalUVApp.Droid.Renderers
{
    public class MyTableViewRenderer : TableViewRenderer
    {
        public MyTableViewRenderer(Context context) : base(context)
        {

        }
        private int _mPosition;
        public override bool DispatchTouchEvent(MotionEvent e)
        {
            if (e.ActionMasked == MotionEventActions.Down)
            {
                _mPosition = this.Control.PointToPosition((int)e.GetX(), (int)e.GetY());
                return base.DispatchTouchEvent(e);
            }

            if (e.ActionMasked == MotionEventActions.Move)
            {
                return true;
            }

            if (e.ActionMasked == MotionEventActions.Up)
            {
                if (this.Control.PointToPosition((int)e.GetX(), (int)e.GetY()) == _mPosition)
                {
                    base.DispatchTouchEvent(e);
                }
                else
                {
                    Pressed = false;
                    Invalidate();
                    return true;
                }
            }

            return base.DispatchTouchEvent(e);
        }
    }
}

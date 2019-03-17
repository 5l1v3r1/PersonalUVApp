using System;
using PersonalUVApp.Controls;
using PersonalUVApp.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MyTableView), typeof(MyTableViewRenderer))]
namespace PersonalUVApp.iOS.Renderers
{
    public class MyTableViewRenderer : TableViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
        {
            base.OnElementChanged(e);

            Control.ScrollEnabled = false;
        }
    }
}
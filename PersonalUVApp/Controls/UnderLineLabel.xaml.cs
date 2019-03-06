using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace PersonalUVApp.Controls
{
    public partial class UnderLineLabel : StackLayout
    {
        public Command SelectMenuCommand { protected set; get; }

        public UnderLineLabel()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }



        public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text),
                                                  typeof(string),
                                                  typeof(UnderLineLabel),
                                                  defaultBindingMode: BindingMode.TwoWay,
                                                  propertyChanged: (bindable, oldVal, newVal) =>
                                                  {
                                                      var matEntry = (UnderLineLabel)bindable;
                                                      matEntry.customLabel.Text = (string)newVal;
                                                  });
       
        public static BindableProperty LineColorProperty = BindableProperty.Create(nameof(LineColor),
                                                  typeof(Color),
                                                  typeof(UnderLineLabel),
                                                   Color.Default,
                                                  defaultBindingMode: BindingMode.TwoWay,
                                                  propertyChanged: (bindable, oldVal, newVal) =>
                                                  {
                                                      var matEntry = (UnderLineLabel)bindable;
                                                      matEntry.customBox.BackgroundColor = (Color)newVal;
                                                  });

        public static readonly BindableProperty TextColorProperty =
          BindableProperty.Create(
              nameof(TextColor),
              typeof(Color),
              typeof(UnderLineLabel),
              Color.Default,
              defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                var matEntry = (UnderLineLabel)bindable;
                matEntry.customLabel.TextColor = (Color)newVal;
            });

      
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TinkerWorX.Silverlight.Input;

namespace TinkerWorX.Silverlight.InputSample
{
    public partial class MainPage : UserControl
    {
        private Int32 index = 0;

        public MainPage()
        {
            this.InitializeComponent();

            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        void CompositionTarget_Rendering(Object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(delegate()
            {
                var gamepad = GamePad.GetState(index);
                if (gamepad.IsActive)
                {
                    Dispatcher.BeginInvoke(delegate()
                    {
                        this.LeftStickPointer.SetValue(Canvas.LeftProperty, 50.00 + gamepad.LeftStickX * 50 - this.LeftStickPointer.Width / 2.00);
                        this.LeftStickPointer.SetValue(Canvas.TopProperty, 50.00 + gamepad.LeftStickY * 50 - this.LeftStickPointer.Height / 2.00);
                        this.LeftStickValue.Text = "{" + gamepad.LeftStickX.ToString("0.00") + "; " + gamepad.LeftStickY.ToString("0.00") + "}";

                        this.RightStickPointer.SetValue(Canvas.LeftProperty, 50.00 + gamepad.RightStickX * 50 - this.RightStickPointer.Width / 2.00);
                        this.RightStickPointer.SetValue(Canvas.TopProperty, 50.00 + gamepad.RightStickY * 50 - this.RightStickPointer.Height / 2.00);
                        this.RightStickValue.Text = "{" + gamepad.RightStickX.ToString("0.00") + "; " + gamepad.RightStickY.ToString("0.00") + "}";
                    });
                }
            });
        }
    }
}

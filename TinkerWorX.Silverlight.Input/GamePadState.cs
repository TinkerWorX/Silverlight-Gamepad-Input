/*
The MIT License (MIT)

Copyright (c) 2013 TinkerWorX

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Windows.Browser;

namespace TinkerWorX.Silverlight.Input
{
    public class GamePadState
    {
        internal GamePadState(Int32 index)
        {
            this.Index = index;
            this.IsActive = false;
            this.Identifier = "N/A";
            this.Name = "Unknown";
        }

        public Int32 Index { get; private set; }

        public String Name { get; private set; }

        public String Identifier { get; private set; }

        public Boolean IsActive { get; private set; }

        public Double LeftStickX { get; private set; }

        public Double LeftStickY { get; private set; }

        public Double LeftStickButton { get; private set; }

        public Double RightStickX { get; private set; }

        public Double RightStickY { get; private set; }

        public Double RightStickButton { get; private set; }

        public Double FaceButton0 { get; private set; }

        public Double FaceButton1 { get; private set; }

        public Double FaceButton2 { get; private set; }

        public Double FaceButton3 { get; private set; }

        public Double LeftShoulder0 { get; private set; }

        public Double RightShoulder0 { get; private set; }

        public Double LeftShoulder1 { get; private set; }

        public Double RightShoulder1 { get; private set; }

        public Double Select { get; private set; }

        public Double Start { get; private set; }

        public Double DPadUp { get; private set; }

        public Double DPadDown { get; private set; }

        public Double DPadLeft { get; private set; }

        public Double DPadRight { get; private set; }

        public Double DeadZoneLeftStick { get; private set; }

        public Double DeadZoneRightStick { get; private set; }

        public Double DeadZoneShoulders { get; private set; }

        internal void ReadAsWindowsFirefoxXBox360(ScriptObject gamepad)
        {
            this.IsActive = true;
            this.Identifier = (gamepad.GetProperty("id") as String);
            this.Name = "XBox 360 Controller";

            // Axes
            var axes = (gamepad.GetProperty("axes") as ScriptObject);
            this.LeftStickX = (Double)(axes.GetProperty(0));
            this.LeftStickY = (Double)(axes.GetProperty(1));
            this.LeftShoulder1 = ((Double)(axes.GetProperty(2)) > 0.00 ? (Double)(axes.GetProperty(2)) : 0.00);
            this.RightShoulder1 = ((Double)(axes.GetProperty(2)) < 0.00 ? -(Double)(axes.GetProperty(2)) : 0.00);
            this.RightStickX = (Double)(axes.GetProperty(3));
            this.RightStickY = (Double)(axes.GetProperty(4));
            this.DPadLeft = ((Double)(axes.GetProperty(5)) < -0.50 ? 1.00 : 0.00);
            this.DPadRight = ((Double)(axes.GetProperty(5)) > 0.50 ? 1.00 : 0.00);
            this.DPadUp = ((Double)(axes.GetProperty(6)) < -0.50 ? 1.00 : 0.00);
            this.DPadDown = ((Double)(axes.GetProperty(6)) > 0.50 ? 1.00 : 0.00);

            // Buttons
            var buttons = (gamepad.GetProperty("buttons") as ScriptObject);
            this.FaceButton0 = (Double)(buttons.GetProperty(0));
            this.FaceButton1 = (Double)(buttons.GetProperty(1));
            this.FaceButton2 = (Double)(buttons.GetProperty(2));
            this.FaceButton3 = (Double)(buttons.GetProperty(3));
            this.LeftShoulder0 = (Double)(buttons.GetProperty(4));
            this.RightShoulder0 = (Double)(buttons.GetProperty(5));
            this.Select = (Double)(buttons.GetProperty(6));
            this.Start = (Double)(buttons.GetProperty(7));
            this.LeftStickButton = (Double)(buttons.GetProperty(8));
            this.RightStickButton = (Double)(buttons.GetProperty(9));

            // Deadzones
            // From http://msdn.microsoft.com/en-us/library/windows/desktop/ee417001(v=vs.85).aspx
            this.DeadZoneLeftStick = (7849.00 / 32767.00);
            this.DeadZoneRightStick = (8689.00 / 32767.00);
            this.DeadZoneShoulders = (30.00 / 255.00);
        }

        internal void ReadAsWindowsFirefoxF310(ScriptObject gamepad)
        {
            // Similar to XBox 360
            this.ReadAsWindowsFirefoxXBox360(gamepad);
            this.Name = "Logitech F310 Controller";
        }

        internal void ReadAsWindowsFirefoxF510(ScriptObject gamepad)
        {
            // Similar to XBox 360
            this.ReadAsWindowsFirefoxXBox360(gamepad);
            this.Name = "Logitech F510 Controller";
        }

        internal void ReadAsWindowsChromeXInput(ScriptObject gamepad)
        {
            this.IsActive = true;
            this.Identifier = (gamepad.GetProperty("id") as String);
            this.Name = "Generic XInput Controller";

            // Axes
            var axes = (gamepad.GetProperty("axes") as ScriptObject);
            this.LeftStickX = (Double)(axes.GetProperty(0));
            this.LeftStickY = (Double)(axes.GetProperty(1));
            this.RightStickX = (Double)(axes.GetProperty(2));
            this.RightStickY = (Double)(axes.GetProperty(3));

            // Buttons
            var buttons = (gamepad.GetProperty("buttons") as ScriptObject);
            this.FaceButton0 = (Double)(buttons.GetProperty(0));
            this.FaceButton1 = (Double)(buttons.GetProperty(1));
            this.FaceButton2 = (Double)(buttons.GetProperty(2));
            this.FaceButton3 = (Double)(buttons.GetProperty(3));
            this.LeftShoulder0 = (Double)(buttons.GetProperty(4));
            this.RightShoulder0 = (Double)(buttons.GetProperty(5));
            this.LeftShoulder1 = (Double)(buttons.GetProperty(6));
            this.RightShoulder1 = (Double)(buttons.GetProperty(7));
            this.Select = (Double)(buttons.GetProperty(8));
            this.Start = (Double)(buttons.GetProperty(9));
            this.LeftStickButton = (Double)(buttons.GetProperty(10));
            this.RightStickButton = (Double)(buttons.GetProperty(11));
            this.DPadUp = (Double)(buttons.GetProperty(12));
            this.DPadDown = (Double)(buttons.GetProperty(13));
            this.DPadLeft = (Double)(buttons.GetProperty(14));
            this.DPadRight = (Double)(buttons.GetProperty(15));

            // Deadzones
            // From http://msdn.microsoft.com/en-us/library/windows/desktop/ee417001(v=vs.85).aspx
            this.DeadZoneLeftStick = (7849.00 / 32767.00);
            this.DeadZoneRightStick = (8689.00 / 32767.00);
            this.DeadZoneShoulders = (30.00 / 255.00);
        }

        internal void ReadAsMacintoshFirefoxXBox360(ScriptObject gamepad)
        {
            this.IsActive = true;
            this.Identifier = (gamepad.GetProperty("id") as String);
            this.Name = "XBox 360 Controller";

            // Axes
            var axes = (gamepad.GetProperty("axes") as ScriptObject);
            this.LeftStickX = (Double)(axes.GetProperty(0));
            this.LeftStickY = (Double)(axes.GetProperty(1));
            this.RightStickX = (Double)(axes.GetProperty(2));
            this.RightStickY = (Double)(axes.GetProperty(3));
            this.LeftShoulder1 = (((Double)(axes.GetProperty(4)) + 1.00) / 2.00);
            this.RightShoulder1 = (((Double)(axes.GetProperty(5)) + 1.00) / 2.00);

            // Buttons
            var buttons = (gamepad.GetProperty("buttons") as ScriptObject);
            this.FaceButton0 = (Double)(buttons.GetProperty(0));
            this.FaceButton1 = (Double)(buttons.GetProperty(1));
            this.FaceButton2 = (Double)(buttons.GetProperty(2));
            this.FaceButton3 = (Double)(buttons.GetProperty(3));
            this.LeftShoulder0 = (Double)(buttons.GetProperty(4));
            this.RightShoulder0 = (Double)(buttons.GetProperty(5));
            this.LeftStickButton = (Double)(buttons.GetProperty(6));
            this.RightStickButton = (Double)(buttons.GetProperty(7));
            this.Start = (Double)(buttons.GetProperty(8));
            this.Select = (Double)(buttons.GetProperty(9));
            this.DPadUp = (Double)(buttons.GetProperty(11));
            this.DPadDown = (Double)(buttons.GetProperty(12));
            this.DPadLeft = (Double)(buttons.GetProperty(13));
            this.DPadRight = (Double)(buttons.GetProperty(14));

            // Deadzones
            // From http://msdn.microsoft.com/en-us/library/windows/desktop/ee417001(v=vs.85).aspx
            this.DeadZoneLeftStick = (7849.00 / 32767.00);
            this.DeadZoneRightStick = (8689.00 / 32767.00);
            this.DeadZoneShoulders = (30.00 / 255.00);
        }

        internal void ReadAsMacintoshFirefoxPS3(ScriptObject gamepad)
        {
            this.IsActive = true;
            this.Identifier = (gamepad.GetProperty("id") as String);
            this.Name = "Playstation 3 Controller";

            // axes
            var axes = (gamepad.GetProperty("axes") as ScriptObject);
            this.LeftStickX = (Double)(axes.GetProperty(0));
            this.LeftStickY = (Double)(axes.GetProperty(1));
            this.RightStickX = (Double)(axes.GetProperty(2));
            this.RightStickY = (Double)(axes.GetProperty(3));

            // buttons
            var buttons = (gamepad.GetProperty("buttons") as ScriptObject);
            this.FaceButton0 = (Double)(buttons.GetProperty(14));
            this.FaceButton1 = (Double)(buttons.GetProperty(13));
            this.FaceButton2 = (Double)(buttons.GetProperty(15));
            this.FaceButton3 = (Double)(buttons.GetProperty(12));
            this.LeftShoulder0 = (Double)(buttons.GetProperty(10));
            this.RightShoulder0 = (Double)(buttons.GetProperty(11));
            this.LeftShoulder1 = (Double)(buttons.GetProperty(8));
            this.RightShoulder1 = (Double)(buttons.GetProperty(9));
            this.Select = (Double)(buttons.GetProperty(0));
            this.Start = (Double)(buttons.GetProperty(3));
            this.LeftStickButton = (Double)(buttons.GetProperty(1));
            this.RightStickButton = (Double)(buttons.GetProperty(2));
            this.DPadUp = (Double)(buttons.GetProperty(4));
            this.DPadDown = (Double)(buttons.GetProperty(6));
            this.DPadLeft = (Double)(buttons.GetProperty(7));
            this.DPadRight = (Double)(buttons.GetProperty(5));
        }

        internal void ReadAsMacintoshChromeXBox360(ScriptObject gamepad)
        {
            this.IsActive = true;
            this.Identifier = (gamepad.GetProperty("id") as String);
            this.Name = "XBox 360 Controller";

            // Axes
            var axes = (gamepad.GetProperty("axes") as ScriptObject);
            this.LeftStickX = (Double)(axes.GetProperty(0));
            this.LeftStickY = (Double)(axes.GetProperty(1));
            this.LeftShoulder1 = (((Double)(axes.GetProperty(2)) + 1.00) / 2.00);
            this.RightStickX = (Double)(axes.GetProperty(3));
            this.RightStickY = (Double)(axes.GetProperty(4));
            this.RightShoulder1 = (((Double)(axes.GetProperty(5)) + 1.00) / 2.00);

            // Buttons
            var buttons = (gamepad.GetProperty("buttons") as ScriptObject);
            this.FaceButton0 = (Double)(buttons.GetProperty(0));
            this.FaceButton1 = (Double)(buttons.GetProperty(1));
            this.FaceButton2 = (Double)(buttons.GetProperty(2));
            this.FaceButton3 = (Double)(buttons.GetProperty(3));
            this.LeftShoulder0 = (Double)(buttons.GetProperty(4));
            this.RightShoulder0 = (Double)(buttons.GetProperty(5));
            this.LeftStickButton = (Double)(buttons.GetProperty(6));
            this.RightStickButton = (Double)(buttons.GetProperty(7));
            this.Start = (Double)(buttons.GetProperty(8));
            this.Select = (Double)(buttons.GetProperty(9));
            this.DPadUp = (Double)(buttons.GetProperty(11));
            this.DPadDown = (Double)(buttons.GetProperty(12));
            this.DPadLeft = (Double)(buttons.GetProperty(13));
            this.DPadRight = (Double)(buttons.GetProperty(14));

            // Deadzones
            // From http://msdn.microsoft.com/en-us/library/windows/desktop/ee417001(v=vs.85).aspx
            this.DeadZoneLeftStick = (7849.00 / 32767.00);
            this.DeadZoneRightStick = (8689.00 / 32767.00);
            this.DeadZoneShoulders = (30.00 / 255.00);
        }

        internal void ReadAsMacintoshChromePS3(ScriptObject gamepad)
        {
            this.IsActive = true;
            this.Identifier = (gamepad.GetProperty("id") as String);
            this.Name = "Playstation 3 Controller";

            // axes
            var axes = (gamepad.GetProperty("axes") as ScriptObject);
            this.LeftStickX = (Double)(axes.GetProperty(0));
            this.LeftStickY = (Double)(axes.GetProperty(1));
            this.RightStickX = (Double)(axes.GetProperty(2));
            this.RightStickY = (Double)(axes.GetProperty(5));

            // buttons
            var buttons = (gamepad.GetProperty("buttons") as ScriptObject);
            this.FaceButton0 = (Double)(buttons.GetProperty(14));
            this.FaceButton1 = (Double)(buttons.GetProperty(13));
            this.FaceButton2 = (Double)(buttons.GetProperty(15));
            this.FaceButton3 = (Double)(buttons.GetProperty(12));
            this.LeftShoulder0 = (Double)(buttons.GetProperty(10));
            this.RightShoulder0 = (Double)(buttons.GetProperty(11));
            this.LeftShoulder1 = (Double)(buttons.GetProperty(8));
            this.RightShoulder1 = (Double)(buttons.GetProperty(9));
            this.Select = (Double)(buttons.GetProperty(0));
            this.Start = (Double)(buttons.GetProperty(3));
            this.LeftStickButton = (Double)(buttons.GetProperty(1));
            this.RightStickButton = (Double)(buttons.GetProperty(2));
            this.DPadUp = (Double)(buttons.GetProperty(4));
            this.DPadDown = (Double)(buttons.GetProperty(6));
            this.DPadLeft = (Double)(buttons.GetProperty(7));
            this.DPadRight = (Double)(buttons.GetProperty(5));
        }

        internal void ReadAsMacintoshChromeF310(ScriptObject gamepad)
        {
            this.IsActive = true;
            this.Identifier = (gamepad.GetProperty("id") as String);
            this.Name = "Logitech F310 Controller";

            // Axes
            var axes = (gamepad.GetProperty("axes") as ScriptObject);
            this.LeftStickX = (Double)(axes.GetProperty(0));
            this.LeftStickY = (Double)(axes.GetProperty(1));
            this.RightStickX = (Double)(axes.GetProperty(2));
            this.RightStickY = (Double)(axes.GetProperty(5));
            // There is a switch to toggle the left joystick and dpad
            // only one is enabled at a time and the output always goes
            // through the left joystick
            this.DPadLeft = Double.NaN;
            this.DPadRight = Double.NaN;
            this.DPadUp = Double.NaN;
            this.DPadDown = Double.NaN;

            // Buttons
            var buttons = (gamepad.GetProperty("buttons") as ScriptObject);
            this.FaceButton0 = (Double)(buttons.GetProperty(1));
            this.FaceButton1 = (Double)(buttons.GetProperty(2));
            this.FaceButton2 = (Double)(buttons.GetProperty(0));
            this.FaceButton3 = (Double)(buttons.GetProperty(3));
            this.LeftShoulder0 = (Double)(buttons.GetProperty(4));
            this.RightShoulder0 = (Double)(buttons.GetProperty(5));
            this.LeftShoulder1 = (Double)(buttons.GetProperty(6));
            this.RightShoulder1 = (Double)(buttons.GetProperty(7));
            this.Select = (Double)(buttons.GetProperty(8));
            this.Start = (Double)(buttons.GetProperty(9));
            this.LeftStickButton = (Double)(buttons.GetProperty(10));
            this.RightStickButton = (Double)(buttons.GetProperty(11));

            // Deadzones
            // From http://msdn.microsoft.com/en-us/library/windows/desktop/ee417001(v=vs.85).aspx
            this.DeadZoneLeftStick = (7849.00 / 32767.00);
            this.DeadZoneRightStick = (8689.00 / 32767.00);
            this.DeadZoneShoulders = (30.00 / 255.00);
        }
    }
}

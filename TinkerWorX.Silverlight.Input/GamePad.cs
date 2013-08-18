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
    public static class GamePad
    {
        private static Boolean IsWindows { get { return HtmlPage.BrowserInformation.UserAgent.Contains("Windows NT"); } }
        private static Boolean IsMacintosh { get { return HtmlPage.BrowserInformation.UserAgent.Contains("Macintosh"); } }

        private static Boolean IsChrome { get { return HtmlPage.BrowserInformation.UserAgent.Contains("Chrome/"); } }
        private static Boolean IsFirefox { get { return HtmlPage.BrowserInformation.UserAgent.Contains("Firefox/"); } }

        public static GamePadState GetState(Int32 index, HtmlWindow window)
        {
            var gamepads = (HtmlPage.Window.Eval("navigator.webkitGamepads || navigator.mozGamepads || navigator.gamepads") as ScriptObject);
            if (gamepads == null)
                throw new NotSupportedException("This browser does not appear to support gamepads. Only the latest dev version of Firefox or Chrome supports gamepads so far.");

            var gamepadState = new GamePadState(index);

            var gamepad = (gamepads.GetProperty(index) as ScriptObject);
            if (gamepad == null)
                return gamepadState;
            var identifier = (gamepad.GetProperty("id") as String);

            if (IsWindows)
            {
                if (IsChrome)
                {
                    if (identifier.Contains("XInput")) // XInput
                    {
                        if (identifier.Contains("GAMEPAD")) // GAMEPAD
                        {
                            gamepadState.ReadAsWindowsChromeXInput(gamepad);
                        }
                        else
                        {
                            throw new NotImplementedException(identifier);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException(identifier);
                    }
                }
                else if (IsFirefox)
                {
                    if (identifier.Contains("45e-")) // Microsoft
                    {
                        if (identifier.Contains("28e-") || identifier.Contains("2a1-")) // XBox 360 controller
                        {
                            gamepadState.ReadAsWindowsFirefoxXBox360(gamepad);
                        }
                        else
                        {
                            throw new NotImplementedException(identifier);
                        }
                    }
                    else if (identifier.Contains("46d-")) // Logitech
                    {
                        if (identifier.Contains("c21d-")) // F310 controller
                        {
                            gamepadState.ReadAsWindowsFirefoxF310(gamepad);
                        }
                        else if (identifier.Contains("c21e-")) // F510 controller
                        {
                            gamepadState.ReadAsWindowsFirefoxF510(gamepad);
                        }
                        else
                        {
                            throw new NotImplementedException(identifier);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException(identifier);
                    }
                }
                else
                {
                    throw new NotSupportedException("Unsupported browser.");
                }
            }
            else if (IsMacintosh)
            {
                if (IsChrome)
                {
                    if (identifier.Contains("Vendor: 045e")) // Microsoft
                    {
                        if (identifier.Contains("Product: 028e") || identifier.Contains("Product: 02a1")) // XBox 360 controller
                        {
                            gamepadState.ReadAsMacintoshFirefoxXBox360(gamepad);
                        }
                        else
                        {
                            throw new NotImplementedException(identifier);
                        }
                    }
                    else if (identifier.Contains("Vendor: 054c")) // Sony
                    {
                        if (identifier.Contains("Product: 0268")) // Playstation 3 controller
                        {
                            gamepadState.ReadAsMacintoshChromePS3(gamepad);
                        }
                        else
                        {
                            throw new NotImplementedException(identifier);
                        }
                    }
                    else if (identifier.Contains("Vendor: 046d")) // Logitech
                    {
                        if (identifier.Contains("Product: c216")) // F310 controller
                        {
                            gamepadState.ReadAsMacintoshChromeF310(gamepad);
                        }
                        else
                        {
                            throw new NotImplementedException(identifier);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException(identifier);
                    }
                }
                else if (IsFirefox)
                {
                    if (identifier.Contains("45e-")) // Microsoft
                    {
                        if (identifier.Contains("28e-") || identifier.Contains("2a1-")) // XBox 360 controller
                        {
                            gamepadState.ReadAsMacintoshChromeXBox360(gamepad);
                        }
                        else
                        {
                            throw new NotImplementedException(identifier);
                        }
                    }
                    else if (identifier.Contains("54c-")) // Sony
                    {
                        if (identifier.Contains("268-")) // Playstation 3 controller
                        {
                            gamepadState.ReadAsMacintoshFirefoxPS3(gamepad);
                        }
                        else
                        {
                            throw new NotImplementedException(identifier);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException(identifier);
                    }
                }
                else
                {
                    throw new NotSupportedException("Unsupported browser.");
                }
            }
            else
            {
                throw new NotSupportedException("Unsupported operating system.");
            }

            return gamepadState;
        }

        public static GamePadState GetState(Int32 index)
        {
            return GamePad.GetState(index, HtmlPage.Window);
        }
    }
}

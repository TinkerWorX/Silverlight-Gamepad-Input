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
using System.Collections.Generic;
using System.Windows.Browser;

namespace TinkerWorX.Silverlight.Input
{
    public static class GamePad
    {
        private static GamePadOperatingSystem OperatingSystem { get; set; }

        private static GamePadBrowser Browser { get; set; }

        public static GamePadSupport Support { get; private set; }

        static GamePad()
        {
            // Detect operating system
            if (HtmlPage.BrowserInformation.UserAgent.Contains("Windows NT"))
                OperatingSystem = GamePadOperatingSystem.Windows;
            else if (HtmlPage.BrowserInformation.UserAgent.Contains("Macintosh"))
                OperatingSystem = GamePadOperatingSystem.Macintosh;

            if (OperatingSystem == GamePadOperatingSystem.Unknown)
            {
                Support = GamePadSupport.Unsupported;
                return;
            }

            // Detect browser
            if (HtmlPage.BrowserInformation.UserAgent.Contains("Chrome/"))
                Browser = GamePadBrowser.Chrome;
            else if (HtmlPage.BrowserInformation.UserAgent.Contains("Firefox/"))
                Browser = GamePadBrowser.Firefox;

            if (Browser == GamePadBrowser.Unknown)
            {
                Support = GamePadSupport.Unsupported;
                return;
            }

            // Call the correct initializer
            switch (Browser)
            {
                case GamePadBrowser.Chrome:
                    Chrome_Initialize();
                    break;

                case GamePadBrowser.Firefox:
                    Firefox_Initialize();
                    break;
            }

            // Call the common initializer
            Common_Initialize();
        }

        #region Chrome

        private static ScriptObject Chrome_GetGamePads()
        {
            if ((HtmlPage.Window.Eval("navigator.webkitGetGamepads()") as ScriptObject) != null) // Chrome 22 and newer
                return (HtmlPage.Window.Eval("navigator.webkitGetGamepads()") as ScriptObject);
            if ((HtmlPage.Window.Eval("navigator.webkitGamepads") as ScriptObject) != null) // Chrome 21
                return (HtmlPage.Window.Eval("navigator.webkitGamepads") as ScriptObject);
            return null;
        }

        private static ScriptObject Chrome_GetGamePad(Int32 index)
        {
            return (Chrome_GetGamePads().GetProperty(index) as ScriptObject);
        }

        private static void Chrome_Initialize()
        {
            if (Chrome_GetGamePads() == null)
            {
                Support = GamePadSupport.Unsupported;
                return;
            }

            // Do we need to initialize more?
        }

        #endregion Chrome

        #region Firefox

        private static Dictionary<Int32, ScriptObject> Firefox_GamePads = new Dictionary<Int32, ScriptObject>();

        private static ScriptObject Firefox_GetGamePad(Int32 index)
        {
            var gamepad = (ScriptObject)null;
            if (Firefox_GamePads.TryGetValue(index, out gamepad))
                return gamepad;
            return null;
        }

        private static void Firefox_Initialize()
        {
            // Currently there is no way to detect gamepad support in Firefox.
            Support = GamePadSupport.Unknown;

            HtmlPage.Window.AttachEvent("MozGamepadConnected", new EventHandler<HtmlEventArgs>(delegate(Object s, HtmlEventArgs e)
            {
                var gamepad = (e.EventObject.GetProperty("gamepad") as ScriptObject);
                var index = (Int32)(Double)gamepad.GetProperty("index");
                Firefox_GamePads.Add(index, gamepad);
            }));
            HtmlPage.Window.AttachEvent("MozGamepadDisconnected", new EventHandler<HtmlEventArgs>(delegate(Object s, HtmlEventArgs e)
            {
                var gamepad = (e.EventObject.GetProperty("gamepad") as ScriptObject);
                var index = (Int32)(Double)gamepad.GetProperty("index");
                Firefox_GamePads.Remove(index);
            }));
        }

        #endregion Firefox

        #region Common

        private static ScriptObject Common_GetGamePad(Int32 index)
        {
            switch (Browser)
            {
                case GamePadBrowser.Chrome: return Chrome_GetGamePad(index);
                case GamePadBrowser.Firefox: return Firefox_GetGamePad(index);
                default: return null;
            }
        }

        private static void Common_Initialize()
        {

        }

        #endregion Common

        public static GamePadState GetState(Int32 index)
        {
            if (Support == GamePadSupport.Unsupported)
                throw new NotSupportedException("This browser does not appear to support gamepads.");


            var gamepadState = new GamePadState(index);

            var gamepad = Common_GetGamePad(index);
            if (gamepad == null)
                return gamepadState;
            var identifier = (gamepad.GetProperty("id") as String);

            switch (OperatingSystem)
            {
                case GamePadOperatingSystem.Windows:
                    switch (Browser)
                    {
                        case GamePadBrowser.Chrome:
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
                            break;

                        case GamePadBrowser.Firefox: 
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
                            break;

                        case GamePadBrowser.Unknown:
                            throw new NotSupportedException("Unsupported browser.");
                    }
                    break;

                case GamePadOperatingSystem.Macintosh:
                    switch (Browser)
                    {
                        case GamePadBrowser.Chrome:
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
                            break;

                        case GamePadBrowser.Firefox:
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
                            break;

                        case GamePadBrowser.Unknown:
                            throw new NotSupportedException("Unsupported browser.");
                    }
                    break;

                case GamePadOperatingSystem.Unknown:
                    throw new NotSupportedException("Unsupported operating system.");
            }

            return gamepadState;
        }

        private enum GamePadOperatingSystem
        {
            Unknown,
            Windows,
            Macintosh
        }

        private enum GamePadBrowser
        {
            Unknown,
            Chrome,
            Firefox
        }
    }
}

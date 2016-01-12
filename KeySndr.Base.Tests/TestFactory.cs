using System;
using System.Collections.Generic;
using KeySndr.Base.Domain;
using KeySndr.Common;

namespace KeySndr.Base.Tests
{
    public static class TestFactory
    {
        public static AppConfig CreateTestAppConfig()
        {
            return new AppConfig
            {
                BroadcastIdentifier = "broadcast",
                CheckUpdateOnStart = false,
                DataFolder = "d:\\keysndr",
                EnableKeyboardAndMouse = false,
                FirstTimeRunning = false,
                LastIp = "127.0.0.1",
                LastPath = "something",
                LastPort = 1234,
                ProcessNumber = IntPtr.Zero,
                UpdateVersionCheckUrl = "http://some.com"
            };
        }

        public static InputConfiguration CreateTestInputConfiguration()
        {
            return new InputConfiguration
            {
                Actions = new List<InputAction>(),
                FileName = "test.json",
                Id = Guid.NewGuid(),
                Name = "test",
                ProcessName = "",
                UseDesktopWindow = false,
                UseForegroundWindow = true,
                View = ""
            };
        }
    }
}

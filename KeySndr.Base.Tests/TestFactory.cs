﻿using System;
using System.Collections.Generic;
using KeySndr.Base.Domain;
using KeySndr.Base.Dto;
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

        public static InputAction CreateTestInputAction()
        {
            return new InputAction
            {
                Color = "#ffffff",
                IsEnabled = true,
                IsTarget = false,
                MouseSequences = new List<MouseSequenceItem>(),
                Name = "action",
                Sequences = new List<SequenceItem>(),
                ScriptSequences = new List<ScriptSequenceItem>(),
                TextColor = "#000000"
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

        public static InputConfiguration Copy(InputConfiguration c)
        {
            return new InputConfiguration
            {
                Actions = c.Actions,
                FileName = c.FileName,
                Id = c.Id,
                Name = c.Name,
                ProcessName = c.ProcessName,
                UseDesktopWindow = c.UseDesktopWindow,
                UseForegroundWindow = c.UseForegroundWindow,
                View = c.View
            };
        }

        public static InputScript Copy(InputScript s)
        {
            return new InputScript
            {
                Context = s.Context,
                FileName = s.FileName,
                Id = s.Id,
                Inputs = s.Inputs,
                IsValid = s.IsValid,
                Name = s.Name,
                SourceFileNames = s.SourceFileNames,
                SourceFiles = s.SourceFiles
            };
        }
        public static InputScript CreateTestInputScript()
        {
            return new InputScript
            {
                FileName = "s.script",
                Id = Guid.NewGuid(),
                Inputs = new List<ScriptInputParameter>
                {
                    new ScriptInputParameter
                    {
                        Key = "TheKey",
                        Value = "TheValue"
                    }
                },
                IsValid = true,
                Name = "s",
                SourceFiles = new List<SourceFile>
                {
                    new SourceFile
                    {
                        CanExecute = false,
                        Contents = "function execute() {}",
                        Error = string.Empty,
                        FileName = "script.js",
                        IsValid = false,
                        ParseOk = false
                    }
                },
                SourceFileNames = new List<string>
                {
                    "script.js"
                }
            };
        }
    }
}

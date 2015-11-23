﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class InputAction
    {
        private const string BackGroundColor = "#000000";
        private const string ForeGroundColor = "#FFFFFF";

        [DataMember(Name = "sequences")]
        public List<SequenceItem> Sequences { get; set; }

        [DataMember(Name = "scriptSequences")]
        public List<ScriptSequenceItem> ScriptSequences { get; set; }

        [DataMember(Name = "mouseSequences")]
        public List<MouseSequenceItem> MouseSequences { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "isEnabled")]
        public bool IsEnabled { get; set; }

        [DataMember(Name = "color")]
        public string Color { get; set; }

        [DataMember(Name = "textColor")]
        public string TextColor { get; set; }

        public bool IsTarget { get; set; }
        public bool HasKeySequences => Sequences.Count > 0;
        public bool HasScriptSequences => ScriptSequences.Count > 0;
        public bool HasMouseSequences => MouseSequences.Count > 0;

        public InputAction()
        {
            Name = string.Empty;
            IsEnabled = false;
            Color = BackGroundColor;
            TextColor = ForeGroundColor;
            Sequences = new List<SequenceItem>();
            ScriptSequences = new List<ScriptSequenceItem>();
            MouseSequences = new List<MouseSequenceItem>();
            IsTarget = false;
        }

        public InputAction(string name, List<SequenceItem> sequences)
        {
            Name = name;
            IsEnabled = true;
            Color = string.Empty;
            Sequences = sequences;
            ScriptSequences = new List<ScriptSequenceItem>();
            MouseSequences = new List<MouseSequenceItem>();
        }

        public InputAction(string name, List<SequenceItem> sequences, List<ScriptSequenceItem> scripts)
        {
            Name = name;
            IsEnabled = true;
            Color = string.Empty;
            Sequences = sequences;
            ScriptSequences = scripts;
            MouseSequences = new List<MouseSequenceItem>();
        }

        public InputAction(string name, List<SequenceItem> sequences, List<ScriptSequenceItem> scripts, List<MouseSequenceItem> mouseSequences)
        {
            Name = name;
            IsEnabled = true;
            Color = string.Empty;
            Sequences = sequences;
            ScriptSequences = scripts;
            MouseSequences = mouseSequences;
        }

        public void Clear()
        {
            IsEnabled = false;
            Name = string.Empty;
            Color = string.Empty;
            Sequences.Clear();
        }

        public static List<InputAction> GenerateFullSet(int max)
        {
            var l = new List<InputAction>();
            for (var i = 0; i < max; i++)
            {
                l.Add(new InputAction());
            }
            return l;
        }

        public override string ToString()
        {
            return "Input action: " + Name;
        }
    }
}
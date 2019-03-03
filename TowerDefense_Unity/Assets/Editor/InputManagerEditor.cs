using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Editor
{
    /// <summary>Used for simple input checking.</summary>
    [InitializeOnLoad]
    public static class InputManagerEditor
    {
        public delegate void OnInput(KeyCode keyCode);
        private static readonly List<KeyCode> s_HeldDown = new List<KeyCode>();
        public static event OnInput OnRelease, OnPressed;
        public static bool IsKeyDown(KeyCode key) => s_HeldDown.Contains(key);

        static InputManagerEditor()
        {
            SceneView.onSceneGUIDelegate += OnSceneRender;
        }

        private static void OnSceneRender(SceneView scene)
        {
            Event e = Event.current;
            HandleFilthyShiftInput(e);
            if (e.keyCode == KeyCode.None) return; //Sometimes returns none for some reason, might be unity input lag.

            switch (e.type)
            {
                case EventType.KeyDown:
                {
                    s_HeldDown.Add(e.keyCode);
                    OnPressed?.Invoke(e.keyCode);
                    break;
                }
                case EventType.KeyUp:
                {
                    s_HeldDown.Remove(e.keyCode);
                    OnRelease?.Invoke(e.keyCode);
                    break;
                }
                case EventType.MouseDown:
                {
                    s_HeldDown.Add(e.keyCode);
                    OnPressed?.Invoke(e.keyCode);
                    break;
                }
                case EventType.MouseUp:
                {
                    s_HeldDown.Remove(e.keyCode);
                    OnRelease?.Invoke(e.keyCode);
                    break;
                }
            }
        }

        private static void HandleFilthyShiftInput(Event input)
        {
            if (input.shift)
                s_HeldDown.Add(KeyCode.LeftShift);
            else if(s_HeldDown.Contains(KeyCode.LeftShift))
                s_HeldDown.Remove(KeyCode.LeftShift);
        }
    }
}

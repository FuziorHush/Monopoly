using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GameDebug
{
    public class DevConsoleBehaviour : MonoSingleton<DevConsoleBehaviour>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        //[SerializeField] private string Prefix = string.Empty;
        [SerializeField] private ConsoleCommand[] Commands = new ConsoleCommand[0];
        private float _pausedTimeScale;
        private List<string> _messages = new List<string>();
        private StringBuilder _sb = new StringBuilder();

        [Header("UI")]
        [SerializeField] private GameObject UICanvas = null;
        [SerializeField] private ContentSizeFitter _content = null;
        [SerializeField] private TMPro.TMP_Text ConsoleLog = null;
        [SerializeField] private TMPro.TMP_InputField InputField = null;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                //Toggle();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (UICanvas.activeSelf)
                {
                    ProcessCommand(InputField.text);
                    InputField.ActivateInputField();
                }
            }
        }

        /*
        public void Toggle()
        {
            if (WindowsManager.Instance != null)
            {
                if (WindowsManager.Instance.OpenedWindow != null)
                {
                    WindowsManager.Instance.CloseCurrentWindow();
                }
            }

            if (UICanvas.activeSelf)
            {
                UICanvas.SetActive(false);

                if (WindowsManager.Instance != null)
                {
                    WindowsManager.Instance.UnpauseGame();
                }
                else
                {
                    Time.timeScale = _pausedTimeScale;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            else
            {
                UICanvas.SetActive(true);
                InputField.ActivateInputField();

                if (WindowsManager.Instance != null)
                {
                    WindowsManager.Instance.PauseGame();
                }
                else
                {
                    _pausedTimeScale = Time.timeScale;
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }
        */

        public void Log(string message)
        {
            _messages.Add($"{message}\n");
            if (_messages.Count > 80)
            {
                _messages.RemoveAt(0);
            }
            for (int i = 0; i < _messages.Count; i++)
            {
                _sb.Append(_messages[i]);
            }
            ConsoleLog.text = _sb.ToString();
            _sb.Clear();

            _content.SetLayoutVertical();
        }

        public void ProcessCommand(string inputValue)
        {
            DevConsole.ProcessCommand(inputValue);
            InputField.text = string.Empty;
        }

        public void ClearLog()
        {
            _messages = new List<string>();
            ConsoleLog.text = string.Empty;
        }

        private DevConsole _devConsole;
        private DevConsole DevConsole
        {
            get
            {
                if (_devConsole != null)
                    return _devConsole;

                return _devConsole = new DevConsole(Commands);
            }
        }
    }
}

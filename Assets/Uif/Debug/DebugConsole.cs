using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace Uif {
	public class DebugConsole : MonoBehaviour {
		public int LineLimit = 32;

		public Text uiText;
		public List<string> lines;

		public void OnValidate() {
			if (uiText == null) uiText = GetComponent<Text>();
		}

		public void OnEnable() {
			Application.logMessageReceived += HandleLog;

//			Debug.Log("Console Ready");
//			Debug.LogWarning("Console Warning Ready");
//			Debug.LogError("Console Error Ready");
		}

		public void OnDisable() {
			Application.logMessageReceived -= HandleLog;
		}

		void HandleLog(string logString, string stackTrace, LogType type) {
			string stack = stackTrace.Split('\n')[1];

			switch (type) {
				case LogType.Log:
					lines.Add(string.Format("{0}\n", logString));
					break;
				case LogType.Warning:
					lines.Add(string.Format("<color=yellow>{0}</color>\n\t{1}\n", logString, stack));
					break;
				case LogType.Error:
				case LogType.Exception:
					lines.Add(string.Format("<color=red>{0}</color>\n\t{1}\n", logString, stackTrace));
					break;
				default:
					lines.Add(string.Format("<color=lime>{0}</color>\n\t{1}\n", logString, stack));
					break;
			}

			if (lines.Count > LineLimit) lines.RemoveRange(0, lines.Count - LineLimit);
			uiText.text = lines.Aggregate("", (acc, x) => acc + x).TrimEnd();
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEditor;

namespace DI.SimpleNote {
#if UNITY_EDITOR
	[CustomEditor(typeof(MonoBehaviour), true)]
	public class SimpleNoteAttributeDrawer : Editor
	{
		MonoBehaviour monoBehaviour;
		string title, note = "\n";
		int index = -1;

		void OnEnable()
		{
			monoBehaviour = (MonoBehaviour)target;
			if (SimpleNoteManager.Instance != null) {
				if (SimpleNoteManager.Instance.getIndexAttributeScriptNote(monoBehaviour.gameObject, monoBehaviour) != -1)
				{
					index = SimpleNoteManager.Instance.getIndexAttributeScriptNote(monoBehaviour.gameObject, monoBehaviour);
					title = SimpleNoteManager.Instance.attScriptNote[index].note.title;
					note = SimpleNoteManager.Instance.attScriptNote[index].note.note;
				}
			}
			else{
				title = "Title Here";
				note = "Note Here";
			}
		}

		void OnDisable()
		{
			if (monoBehaviour == null && index != -1 && monoBehaviour.GetType() != typeof(SimpleNoteManager))
			{
				//Delete note
				if(SimpleNoteManager.Instance != null)
					SimpleNoteManager.Instance.attScriptNote.RemoveAt(index);
			}
		}
	

		public override void OnInspectorGUI()
		{
			if(monoBehaviour == null)
				monoBehaviour = (MonoBehaviour)target;
			SimpleNoteAttribute attribute = (SimpleNoteAttribute)PropertyAttribute.GetCustomAttribute(monoBehaviour.GetType(), typeof(SimpleNoteAttribute));
			if (attribute != null)
			{
				//Check if note stored to manager;
				if (SimpleNoteManager.Instance == null)
					SimpleNoteManager.Init();
				if (SimpleNoteManager.Instance.getIndexAttributeScriptNote(monoBehaviour.gameObject, monoBehaviour) == -1) {
					if (string.IsNullOrEmpty(attribute.title) && string.IsNullOrEmpty(attribute.note))
						SimpleNoteManager.Instance.attScriptNote.Add(new SimpleNoteManager.AttributeScriptNote(monoBehaviour.gameObject, monoBehaviour, title, note));
					else {
						SimpleNoteManager.Instance.attScriptNote.Add(new SimpleNoteManager.AttributeScriptNote(monoBehaviour.gameObject, monoBehaviour, attribute.title, attribute.note));
						title = attribute.title;
						note = attribute.note;
					}
				}
				if (index == -1)
					index = SimpleNoteManager.Instance.getIndexAttributeScriptNote(monoBehaviour.gameObject, monoBehaviour);

				GUIStyle textField = new GUIStyle(EditorStyles.textField);
				textField.fontStyle = FontStyle.Bold;
				if (GUI.GetNameOfFocusedControl() != "Title" + monoBehaviour.gameObject.GetInstanceID())
					textField.normal = EditorStyles.label.normal;

				EditorGUILayout.BeginHorizontal();
				title = EditorGUILayout.TextField(title, textField);
				//EditorPrefs.SetString(monoBehaviour.GetInstanceID() + "-SimpleNote-Title", title);
				if(SimpleNoteManager.Instance.attScriptNote[index].note.title != title)
					SimpleNoteManager.Instance.attScriptNote[index].note.title = title;
				EditorGUILayout.EndHorizontal();

				GUIStyle textArea = new GUIStyle(EditorStyles.textArea);
				textArea.richText = true;
				if (GUI.GetNameOfFocusedControl() != "Note" + monoBehaviour.gameObject.GetInstanceID())
					textArea.normal = EditorStyles.label.normal;

				EditorGUILayout.BeginHorizontal();

				note = EditorGUILayout.TextArea(note, textArea);
				//EditorPrefs.SetString(monoBehaviour.GetInstanceID() + "-SimpleNote-Note", note);
				if(SimpleNoteManager.Instance.attScriptNote[index].note.note != note)
					SimpleNoteManager.Instance.attScriptNote[index].note.note = note;

				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
				EditorGUI.DrawRect(GUILayoutUtility.GetLastRect(), SimpleNoteData.Instance.getBgColor1);
			}

			DrawDefaultInspector();
		}
	}

#endif
}

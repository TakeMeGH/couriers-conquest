using UnityEngine;
using System.Collections;

namespace DI.SimpleNote {
	[System.Serializable]
	public class Note {
		public string title;
		public string note;
		public Note(string title, string note) {
			this.title = title;
			this.note = note;
		}
		public Note() { }
	}
}

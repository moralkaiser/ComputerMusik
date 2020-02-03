using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatrixFactory : MonoBehaviour {

	public Button buttonRessource;
	public InputField inputInstrument;

	Sprite uiImageRessource;

	// Use this for initialization
	void Start () 
	{
		uiImageRessource = buttonRessource.GetComponent<Image> ().sprite;
	}

	// Calculate Harmonic-Tones based on a baseTone, for all possible Chords
	int[] createToneArrays(int toneType, int baseTone)
	{
		switch(toneType){
		// Pentatonic Major
		case 0:
			return new int[] { baseTone+21, baseTone+19, baseTone+16, baseTone+14, baseTone+12, baseTone+9, baseTone+7, baseTone+4, baseTone+2, baseTone};
			break;
		// Pentatonic Minor
		case 1:
			return new int[] { baseTone+22, baseTone+19, baseTone+17, baseTone+15, baseTone+12, baseTone+10, baseTone+7, baseTone+5, baseTone+3, baseTone};
			break;
		// Major Chord
		case 2:
			return new int[] { baseTone+36, baseTone+31, baseTone+28, baseTone+24, baseTone+19, baseTone+16, baseTone+12, baseTone+7, baseTone+4, baseTone};
			break;
		// Minor Chord
		case 3:
			return new int[] { baseTone + 36, baseTone + 31, baseTone + 27, baseTone + 24, baseTone + 19, baseTone + 15, baseTone + 12, baseTone + 7, baseTone + 3, baseTone };
			break;
		// Drum-Panel 1
		case 4:
			return new int[] { 43,42,41,40,39,38,37,36,35,34 };
			//return new int[] { 34,35,36,37,38,39,40,41,42,43 };
			//return new int[]{35,36,37,38,39,40,41,42,43,44};
			break;
		// Drum-Panel 2
		case 5:
			return new int[] { 53,52,51,50,49,48,47,46,45,44 };
			//return new int[] { 44,45,46,47,48,49,50,51,52,53 };
			break;
		// Drum Panel 3
		case 6:
			return new int[] { 63,62,61,60,59,58,57,56,55,54 };
			//return new int[] { 54,55,56,57,58,59,60,61,62,63 };
			break;
		// Drum Panel 4
		case 7:
			return new int[] { 73,72,71,70,69,68,67,66,65,64 };
			//return new int[] { 64,65,66,67,68,69,70,71,72,73 };
			break;
		// Drum Panel 5
		case 8:
			return new int[] { 81,80,79,78,77,76,75,74,43,42 };
			//return new int[] { 74,75,76,77,78,79,80,41,42,43 };
			break;
		default:
			return new int[]{};
			break;
		}
	}

	// Return new MatrixPanel based on the Type of Chord
	public MatrixPanel createPanel(int toneType, int instrumentNumber, int baseTone)
	{
		if (toneType < 4) {
			return new MatrixPanel (toneType, instrumentNumber, baseTone, createToneArrays (toneType, baseTone), true, uiImageRessource);
		} else {
			return new MatrixPanel (toneType, instrumentNumber, baseTone, createToneArrays (toneType, baseTone), false, uiImageRessource);
		}
	}

}

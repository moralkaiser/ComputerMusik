using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTileData : MonoBehaviour {

	int index;
	int instrument;
	int note;

	static int idCounter = 0;
	int id;

	// Constructor
	public MusicTileData(int indexX, int indexY, int instrumentIn, int[] tones)
	{
        this.index = indexX;
		this.instrument = instrumentIn;

        this.note = tones[indexY];

		id = idCounter;
		idCounter++;
    }

	// Getter for Index
	public int getIndex()
	{
		return index;
	}

	// Getter for ID
	public int getId()
	{
		return id;
	}

	// Getter for Instrument
	public int getInstrument()
	{
		return instrument;
	}

	// Getter for Note
	public int getNote()
	{
		return note;
	}
}

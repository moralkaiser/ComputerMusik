using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {

	public Image[] markers;
	private IEnumerator coroutine;
	Color activatedColor;
	Color unactivatedColor;
	float secondsToWait = 0.2f;
	public Button stopButton;
	public Button playButton;
	public Slider slider;

	int tilesCount;

	List<MusicTileData>[] verticalRowsInstruments;
	List<MusicTileData>[] verticalRowsInstrumentsPlaying;

	List<MusicTileData>[] verticalRowsDrums;
	List<MusicTileData>[] verticalRowsDrumsPlaying;

	public MIDISequencer midiSequencer;

	bool playing;

	// Use this for initialization
	// Initialisation of Lists, Parsing of Colors for the Marker, Initialisation of Coroutine and Delegation of Click Events
	void Start () 
	{
		tilesCount = markers.Length;

		initLists ();

		ColorUtility.TryParseHtmlString ("#002CFF64", out activatedColor);
		ColorUtility.TryParseHtmlString ("#002CFF00", out unactivatedColor);

		coroutine = waitAndPlayBetter();

		playButton.onClick.AddListener (() => Play());
		stopButton.onClick.AddListener (() => Stop());

		playing= false;
	}

	void initLists()
	{
		verticalRowsInstruments = new List<MusicTileData>[tilesCount];
		verticalRowsInstrumentsPlaying = new List<MusicTileData>[tilesCount];

		verticalRowsDrums = new List<MusicTileData>[tilesCount];
		verticalRowsDrumsPlaying = new List<MusicTileData>[tilesCount];

		for (int initCounter = 0; initCounter < tilesCount; initCounter++) 
		{
			verticalRowsInstruments [initCounter] = new List<MusicTileData> ();
			verticalRowsInstrumentsPlaying [initCounter] = new List<MusicTileData> ();

			verticalRowsDrums [initCounter] = new List<MusicTileData> ();
			verticalRowsDrumsPlaying [initCounter] = new List<MusicTileData> ();
		}
	}

	// Regulation of Music-Speed per UI-Slider
	void Update()
	{
		secondsToWait = 0.4f * slider.value;
	}

	// Register Tile in the List of InstrumentTiles to Play
	public void addTile(MusicTileData tileIn)
	{
		verticalRowsInstruments [tileIn.getIndex()].Add (tileIn);
	}

	// Register Tile in the List of DrumTiles to Play
	public void addDrumTile(MusicTileData tileIn)
	{
		verticalRowsDrums [tileIn.getIndex()].Add (tileIn);
	}

	// Remove Tile from the List of InstrumentTiles to Play
	public void removeTile(MusicTileData tileIn)
	{
		MusicTileData[] tilesToRemove = verticalRowsInstruments [tileIn.getIndex ()].ToArray ();
		verticalRowsInstruments [tileIn.getIndex ()].Clear ();

		for (int i = 0; i < tilesToRemove.Length; i++) 
		{
			if (tileIn.getId () != tilesToRemove [i].getId ()) {
				Debug.Log (tilesToRemove [i].getId ());
				verticalRowsInstruments [tileIn.getIndex ()].Add (tilesToRemove [i]);
			}
		}
	}

	// Remove Tile from the List of DrumTiles to Play
	public void removeDrumTile(MusicTileData tileIn)
	{
		MusicTileData[] tilesToRemove = verticalRowsDrums [tileIn.getIndex ()].ToArray ();
		verticalRowsDrums [tileIn.getIndex ()].Clear ();

		for (int i = 0; i < tilesToRemove.Length; i++) 
		{
			if (tileIn.getId () != tilesToRemove [i].getId ()) {
				Debug.Log (tilesToRemove [i].getId ());
				verticalRowsDrums [tileIn.getIndex ()].Add (tilesToRemove [i]);
			}
		}
	}

	// public Method to toggle Coroutine to play Music
	public void Play()
	{
		if (!playing) {
			StartCoroutine (coroutine);
			playing = true;
		}
	}

	// public Method to toggle Coroutine to stop Music
	public void Stop()
	{
		StopAllCoroutines ();
		midiSequencer.stop ();
		playing = false;
	}

	// coroutine responsible for playing the registered tiles in the MIDISequencer
	private IEnumerator waitAndPlayBetter()
	{

		while (true) 
		{
			for (int counterVerticalRow = 0; counterVerticalRow < tilesCount; counterVerticalRow++) 
			{
				verticalRowsInstrumentsPlaying [counterVerticalRow] = new List<MusicTileData>(verticalRowsInstruments [counterVerticalRow]);
				verticalRowsDrumsPlaying [counterVerticalRow] = new List<MusicTileData>(verticalRowsDrums [counterVerticalRow]);

				markers[counterVerticalRow].color = activatedColor;

				foreach (MusicTileData tileData in verticalRowsInstrumentsPlaying[counterVerticalRow]) 
				{
					midiSequencer.play (tileData.getNote(), tileData.getInstrument());
				}

				foreach (MusicTileData tileData in verticalRowsDrumsPlaying[counterVerticalRow]) 
				{
					midiSequencer.playDrum (tileData.getNote());
				}

				yield return new WaitForSeconds (secondsToWait);

				markers[counterVerticalRow].color = unactivatedColor;

				#if UNITY_STANDALONE_WIN
				foreach (MusicTileData tileData in verticalRowsInstrumentsPlaying[counterVerticalRow])
				{
				midiSequencer.stop(tileData.getNote());
				}

				foreach (MusicTileData tileData in verticalRowsDrumsPlaying[counterVerticalRow]) 
				{
				midiSequencer.stopDrum (tileData.getNote());
				}
				#endif

				#if UNITY_STANDALONE_LINUX
				midiSequencer.stop ();
				#endif    
                
            }
		}
	}

	// public Method to toggle Coroutine to immediately stop Music
	public void stopAllCurrentSounds()
	{
		midiSequencer.stop ();
	}
}

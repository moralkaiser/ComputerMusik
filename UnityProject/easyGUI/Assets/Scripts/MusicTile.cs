using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;


[RequireComponent(typeof(AudioSource))]
public class MusicTile : MonoBehaviour {

	ColorBlock unpressedColor;
	ColorBlock pressedColor;
	Button b;
	public bool switchOn;
	bool isInstrument;
	UnityEvent clickEvent;
	AudioSource sound;
	int index;
	GameHandler handler;

	MusicTileData metaData;

	// Use this for initialization
	// Init of Colors, Delegation of Button-Click Events and Initialisation of the handler-Object by searching in the IDE for name "GameHandler"
	void Start () 
	{
		
		Color unpressedNormalColor;
		Color unpressedHighlightedColor;
		Color unpressedPressedColor;

		Color pressedNormalColor;
		Color pressedHighlightedColor;
		Color pressedPressedColor;

		if (ColorUtility.TryParseHtmlString ("#FFFFFFFF", out unpressedNormalColor))
		if (ColorUtility.TryParseHtmlString ("#F5F5F5FF", out unpressedHighlightedColor))
		if (ColorUtility.TryParseHtmlString ("#0086DBFF", out pressedNormalColor))
		if (ColorUtility.TryParseHtmlString ("#2BADFFFF", out pressedHighlightedColor)) 
		{
			unpressedColor = new ColorBlock ();
			unpressedColor.normalColor = unpressedNormalColor;
			unpressedColor.highlightedColor = unpressedHighlightedColor;
			unpressedColor.pressedColor = pressedNormalColor;
			unpressedColor.colorMultiplier = 1.0f;
			unpressedColor.fadeDuration = 0.1f;

			pressedColor = new ColorBlock ();
			pressedColor.normalColor = pressedNormalColor;
			pressedColor.highlightedColor = pressedHighlightedColor;
			pressedColor.pressedColor = unpressedNormalColor;
			pressedColor.colorMultiplier = 1.0f;
			pressedColor.fadeDuration = 0.1f;
		}

		b = this.GetComponent<Button> ();
		b.colors = unpressedColor;
		switchOn = false;
		b.onClick.AddListener (() => tileSwitch());
		//sound = this.GetComponent<AudioSource> ();

		handler = GameObject.Find ("GameHandler").GetComponent<GameHandler>();
	}

	// Setter for TileType (Instument OR Drum)
	public void setIsInstrument(bool isInstrumentIN)
	{
		this.isInstrument = isInstrumentIN;
	}

	// Init of MusicTileData of this MusicTile
	public void createMetadata(int indexX, int indexY, int instrumentNumber, int[] tones)
	{
		metaData = new MusicTileData (indexX, indexY, instrumentNumber, tones);
	}

	// Setter of Index
	public void setIndex(int indexIn)
	{
		index = indexIn;
	}

	// Getter of Index
	public int getIndex()
	{
		return index;
	}

	// Getter of ID
	public int getId()
	{
		return metaData.getId ();
	}

	// Toggle-Method for MusicTile Button
	// Register/Unregister Tile in GameHandler
	public void tileSwitch()
	{
		switchOn = !switchOn;
		if (switchOn) 
		{
			b.colors = pressedColor;
			if (isInstrument) 
			{
				handler.addTile(metaData);
			} else {
				handler.addDrumTile (metaData);
			}

		}
		else
		{
			b.colors = unpressedColor;
			if (isInstrument) 
			{
				handler.removeTile (metaData);
			} else {
				handler.removeDrumTile (metaData);
			}
		}
	}

	public void removeTile()
	{
		if (switchOn) 
		{
			if (isInstrument) 
			{
				handler.removeTile(metaData);
			} else {
				handler.removeDrumTile(metaData);
			}
		}
	}
		
}

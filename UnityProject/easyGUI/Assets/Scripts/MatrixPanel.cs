using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class MatrixPanel : MonoBehaviour {

	GameObject panel;
	GameHandler handler;

	int toneType;
	int instrumentNumber;
	int baseTone;

	int[] tones;

	bool isInstrument;

	List<MusicTile> musicTiles;
	List<string> trackNames;

	// Constructor
	// Initialisation of the handler-Object by searching in the IDE for name "GameHandler"
	public MatrixPanel(int toneType, int instrumentNumber, int baseTone, int[] tones, bool isInstrumentIN, Sprite uiImageRessource)
	{
		this.toneType = toneType;
		this.instrumentNumber = instrumentNumber;
		this.baseTone = baseTone;
		this.isInstrument = isInstrumentIN;

		this.tones = tones;

		musicTiles = new List<MusicTile> ();
		trackNames = new List<string> ();

		handler = GameObject.Find ("GameHandler").GetComponent<GameHandler>();

		initTrackNames ();

		decorateWithButtons (createPlainPanel (instrumentNumber), tones, uiImageRessource);
	}

	// Create Plain Panel, color based on type (drum/instrument) and instrument number
	GameObject createPlainPanel(int instrumentNumber)
	{
		panel = new GameObject(instrumentNumber + "-Panel");
		panel.AddComponent<CanvasRenderer>();
		Image img = panel.AddComponent<Image>();
		img.rectTransform.sizeDelta = new Vector2 (1230.0f, 605.0f);
		if(isInstrument)
			img.color = Color.HSVToRGB(instrumentNumber/128.0f,0.5f,0.5f);
		else
			img.color = Color.HSVToRGB(instrumentNumber/128.0f,0f,0.5f);

		return panel;
	}

	// Create the MusicTiles and append them to the panel
	void decorateWithButtons(GameObject panel, int[] tones, Sprite uiImageRessource)
	{
		for (int y = 0; y < 10; y++) 
		{
			for(int x=0; x<16;x++)
			{
				GameObject button = createMusicTile(uiImageRessource);
				button.GetComponent<MusicTile> ().setIndex (x);
				button.GetComponent<MusicTile> ().setIsInstrument (isInstrument);
				button.GetComponent<MusicTile> ().createMetadata(x,y,instrumentNumber,tones);

				button.name = "musicTile | ID: " + button.GetComponent<MusicTile> ().getId(); 

				RectTransform buttonPosition = button.GetComponent<RectTransform> ();
				buttonPosition.anchorMin = new Vector2(0.025f, 0.97f);
				buttonPosition.anchorMax = new Vector2(0.025f, 0.97f);
				buttonPosition.pivot = new Vector2(0.5f, 0.5f);

				button.transform.SetParent(panel.transform, false);
				button.transform.position = panel.transform.position;
				buttonPosition.anchoredPosition = new Vector2(14.8f*(x+1)+45.0f*x, -14.8f*(y+1)-45.0f*y);

				musicTiles.Add(button.GetComponent<MusicTile>());
			}

			createDescriptionText (panel, y);
		}
	}

	// Create Description Text for the line Y
	void createDescriptionText(GameObject panel, int indexY)
	{
		GameObject textDescription = new GameObject ();
		textDescription.AddComponent<Text> ();

		textDescription.GetComponent<RectTransform> ().sizeDelta = new Vector2 (190, 35);

		Text textDescriptionText = textDescription.GetComponent<Text> ();
		if (isInstrument) {
			textDescriptionText.text = trackNames[indexY];
		} else {
			textDescriptionText.text = trackNames[9 - indexY];
		}

		textDescriptionText.font = Resources.GetBuiltinResource (typeof(Font), "Arial.ttf") as Font;
		textDescriptionText.fontSize = 18;
		textDescriptionText.color = Color.black;

		RectTransform textPosition = textDescription.GetComponent<RectTransform> ();
		textPosition.anchorMin = new Vector2(0.03f, 0.97f);
		textPosition.anchorMax = new Vector2(0.03f, 0.97f);
		textPosition.pivot = new Vector2(0.5f, 0.5f);

		textDescription.transform.SetParent(panel.transform, false);
		textDescription.transform.position = panel.transform.position;
		textPosition.anchoredPosition = new Vector2(1030, -20f*(indexY+1)-40.0f*indexY);
	}

	// Create Template for MusicTile
	GameObject createMusicTile(Sprite uiImageRessource)
	{
		GameObject tile = new GameObject ();

		tile.AddComponent<RectTransform> ();
		tile.GetComponent<RectTransform> ().sizeDelta = new Vector2 (45, 45);

		tile.AddComponent<CanvasRenderer> ();

		tile.AddComponent<Image> ();
		tile.GetComponent<Image> ().sprite = uiImageRessource;
		tile.GetComponent<Image> ().type = Image.Type.Sliced;

		tile.AddComponent<Button> ();
		tile.AddComponent<AudioSource> ();
		tile.AddComponent<MusicTile> ();

		return tile;
	}

	// If the MatrixPanel is Deleted, unregister all the corresponding MusicTiles in the GameHandler
	public void removeTilesFromGameLogic()
	{
		foreach (MusicTile musicTile in musicTiles) 
		{
			musicTile.removeTile ();
		}

		handler.stopAllCurrentSounds ();
	}

	// Getter of the Panel-Component
	public GameObject getPanel()
	{
		return panel;
	}

	// Format Text for Tracks
	void initTrackNames()
	{
		if (isInstrument) 
		{
			for (int counterLines = 0; counterLines < 10; counterLines++) {
				string trackName = "Track: " + baseTone + "+" + (tones [counterLines] - baseTone);
				trackNames.Add (trackName);
			}
		} else 
		{
			List<string> drumNames = readDrumsListCSV ();

			foreach (string line in drumNames) {
				Debug.Log (line);
			}

			switch (toneType) 
			{
				case 4:
					trackNames = drumNames.GetRange (0, 10); 
				break;
				case 5:
					trackNames = drumNames.GetRange (10, 10); 
				break;
				case 6:
					trackNames = drumNames.GetRange (20, 10); 
				break;
				case 7:
					trackNames = drumNames.GetRange (30, 10); 
				break;
				case 8:
					trackNames = drumNames.GetRange (40, 10); 
				break;
			}
		}
	}

	// Parsing of DrumsList.CSV
	List<string> readDrumsListCSV()
	{
		List<string> drumNames = new List<string> ();

		try 
		{
			using (StreamReader sr = new StreamReader("Assets/Text/drumsList.csv")) 
			{
				string line;
				while ((line = sr.ReadLine()) != null) 
				{
					drumNames.Add(line);
				}
			}
		}
		catch
		{

		}



		return drumNames;
	}
}

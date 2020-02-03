using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LayoutManager : MonoBehaviour {

	public GameObject mainWindow;
	public MatrixFactory matrixFactory;
	public GameObject tabBar;
	public GameObject footerBar;
	public Button templateTab;
	public Button templateDeleteButton;

    public Dropdown toneTypeDropDown;
    public InputField instrumentNumberInput;
    public InputField baseToneInput;

	List<Button> tabs;
	List<GameObject> panels;
	List<Button> deleteButtons;
	List<string> instrumentNames;


	// Method for Add-Button in the UI
	public void addPanel()
	{
		if (toneTypeDropDown.value < 4)
			addInstrumentPanel ();
		else
			addDrumPanel ();
	}

	// Create Panel for an Instrument, set TabButton and Delete Button and Delegate the Click-Events for these
	void addInstrumentPanel()
	{
		int instrumentNumber = System.Int32.Parse(instrumentNumberInput.text)-1;
		int baseToneNumber = System.Int32.Parse(baseToneInput.text);

		if (0 >= instrumentNumber || instrumentNumber >= 128)
			instrumentNumber = 0;

		resetMainView ();

		MatrixPanel panel = matrixFactory.createPanel (toneTypeDropDown.value, instrumentNumber, baseToneNumber);

		panel.getPanel().transform.SetParent(mainWindow.transform, false);
		panels.Add (panel.getPanel());

		string typeText = "";
		switch (toneTypeDropDown.value) 
		{
		case 0:
			typeText = "P-Major";
		break;
		case 1:
			typeText = "P-Minor";
			break;
		case 2:
			typeText = "Major";
		break;
		case 3:
			typeText = "Minor";	
		break;
		}

		Button delB = (Button)Object.Instantiate (templateDeleteButton);
		delB.transform.SetParent (footerBar.transform,false);
		deleteButtons.Add (delB);

		Button b = (Button)Object.Instantiate (templateTab);
		b.GetComponentInChildren<Text> ().text = typeText + "|" + instrumentNames[instrumentNumber] + "|" + baseToneInput.text;
		b.GetComponent<RectTransform> ().sizeDelta = new Vector2 (1280, 45);
		b.onClick.AddListener (() => b.GetComponent<ToggleButtonLookalike>().toggle());
		b.onClick.AddListener (() => refreshMainView(panel.getPanel(),b,delB));

		tabs.Add (b);
		b.transform.SetParent (tabBar.transform, false);
		b.GetComponent<ToggleButtonLookalike> ().setChecked (true);
		resizeTabBar ();

		delB.onClick.AddListener (() => deletePanel(panel,b,delB));
	}

	// Create Panel for a Drum, set TabButton and Delete Button and Delegate the Click-Events for these
	void addDrumPanel()
	{
		resetMainView ();

		MatrixPanel panel = matrixFactory.createPanel (toneTypeDropDown.value, 0, 0);

		panel.getPanel().transform.SetParent(mainWindow.transform, false);
		panels.Add (panel.getPanel());

		string typeText = "";
		switch (toneTypeDropDown.value) 
		{
		case 0:
			typeText = "P-Major";
			break;
		case 1:
			typeText = "P-Minor";
			break;
		case 2:
			typeText = "Major";
			break;
		case 3:
			typeText = "Minor";	
			break;
		case 4:
			typeText = "Drums 1";	
			break;
		case 5:
			typeText = "Drums 2";	
			break;
		case 6:
			typeText = "Drums 3";	
			break;
		case 7:
			typeText = "Drums 4";	
			break;
		case 8:
			typeText = "Drums 5";	
			break;
		}

		Button delB = (Button)Object.Instantiate (templateDeleteButton);
		delB.transform.SetParent (footerBar.transform,false);
		deleteButtons.Add (delB);

		Button b = (Button)Object.Instantiate (templateTab);
		b.GetComponentInChildren<Text> ().text = typeText;
		b.GetComponent<RectTransform> ().sizeDelta = new Vector2 (1280, 45);
		b.onClick.AddListener (() => b.GetComponent<ToggleButtonLookalike>().toggle());
		b.onClick.AddListener (() => refreshMainView(panel.getPanel(),b,delB));

		tabs.Add (b);
		b.transform.SetParent (tabBar.transform, false);
		b.GetComponent<ToggleButtonLookalike> ().setChecked (true);
		resizeTabBar ();

		delB.onClick.AddListener (() => deletePanel(panel,b,delB));
	}


	// Create a Wrapper for a new Panel
	GameObject createNewView(int instrumentNumber)
	{
		GameObject newView = (GameObject)Object.Instantiate (mainWindow);
		newView.name = instrumentNumber + "-View";
		newView.transform.SetParent(mainWindow.transform, false);

		return newView;
	}

	// Use this for initialization
	// Init of Lists and Parsing of instrument List
	void Start () 
	{
		tabs = new List<Button> ();
		panels = new List<GameObject> ();
		deleteButtons = new List<Button> ();
		instrumentNames = new List<string> ();

		readInstrumentListCSV ();
	}

	// If a TabButton was Added, resize every Button in the Bar
	void resizeTabBar()
	{
		foreach (Button b in tabs) 
		{
			Vector2 newButtonSize = new Vector2 (1280/tabs.Count, 45);
			b.GetComponent<RectTransform> ().sizeDelta = newButtonSize;
			b.GetComponent<ToggleButtonLookalike> ().resizeBackground(newButtonSize);
		}
	}

	// Method to switch between MatrixPanels, if corresponding Tab button was pressed
	public void refreshMainView(GameObject panelToShow, Button tabButton, Button deleteButton)
	{
		resetMainView ();
		panelToShow.SetActive (true);
		tabButton.GetComponent<ToggleButtonLookalike> ().setChecked (true);
		deleteButton.transform.gameObject.SetActive (true);
	}

	// Method to Erase MatrixPanel, TabButton and Delete Button from the Game Logic if corresponding Delete button was pressed
	public void deletePanel(MatrixPanel panelToShow, Button tabButton, Button deleteButton)
	{
		panelToShow.removeTilesFromGameLogic ();

		panels.Remove (panelToShow.getPanel());
		tabs.Remove (tabButton);
		deleteButtons.Remove (deleteButton);

		Destroy (panelToShow.getPanel());
		Destroy (tabButton.transform.gameObject);
		Destroy (deleteButton.transform.gameObject);

		if(panels.Count > 0 && tabs.Count > 0 && deleteButtons.Count > 0)
			refreshMainView (panels [0], tabs [0], deleteButtons [0]);
		resizeTabBar ();
	}

	// Deactivate all GameObjects of panels, tabs and deleteButtons
	void resetMainView()
	{
		foreach (GameObject panel in panels) 
		{
			panel.SetActive (false);
		}

		foreach (Button tab in tabs) 
		{
			tab.GetComponent<ToggleButtonLookalike> ().setChecked (false);
		}

		foreach (Button delB in deleteButtons) 
		{
			delB.transform.gameObject.SetActive (false);
		}
	}

	// Parsing of InstrumentList.CSV
	void readInstrumentListCSV()
	{
		try 
		{
			using (StreamReader sr = new StreamReader("Assets/Text/instrumentList.csv")) 
			{
				string line;
				while ((line = sr.ReadLine()) != null) 
				{
					instrumentNames.Add(line);
				}
			}
		}
		catch
		{

		}
	}
	
	// Update is called once per frame
	// If in the Dropdown: the Drumkits were selected? Dont show the Inputs for instrumentNumber and baseTone
	void Update () 
	{
		if (toneTypeDropDown.value > 3) {
			instrumentNumberInput.transform.gameObject.SetActive (false);
			baseToneInput.transform.gameObject.SetActive (false);
		} else {
			instrumentNumberInput.transform.gameObject.SetActive (true);
			baseToneInput.transform.gameObject.SetActive (true);
		}
	}
}

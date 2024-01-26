using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;
using System.IO;
using Unity.Collections;
using System.Collections.Generic;

public class AdTemplateEditor: EditorWindow
{
    [SerializeField]
    private Image logo;
    [SerializeField]
    private TMPro.TextMeshProUGUI description;
    [SerializeField]
    private Button cta;
    [SerializeField]
    private Camera uiCamera;

    public string templateName = " Add template name here ";

    
    private Vector2 imageSize = new Vector2(100, 100); // New image size
    private Vector3 imageRotation = Vector3.zero; // New image rotation

    private Color textFontColor = Color.black; // New text font color
    private string textContent = "Your TMP Text";
    private float textFontSize = 24; // New text font size

    private Color buttonColor = Color.green; // New button color
    private Vector2 buttonSize = new Vector2(150, 50); // New button size
   
    private Texture2D newImage; // New image field

    private string jsonFolderPath = "Assets/JSONData";
    //private string jsonFilePath = "Assets/JSONData/Addata.json";
    private string selectedJsonFile = ""; // Store the selected JSON file
    

    [MenuItem("Window/AdObjectGenerator Window")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<AdTemplateEditor>("AdObjectGenerator");
    }


    private void OnGUI()
    {
        templateName = EditorGUILayout.TextField("Template Name", templateName);

        // UI Element Selection
        logo = (Image)EditorGUILayout.ObjectField("Logo", logo, typeof(Image), true) as Image;   //ObjectField uses reflection an returns an value of type UnityEngine.Object, 'as' operator is used for explicit casting which returs the specified type
        description = (TextMeshProUGUI)EditorGUILayout.ObjectField("Description", description, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
        cta = (Button)EditorGUILayout.ObjectField("CTA Button", cta, typeof(Button), true) as Button  as Button ;
        uiCamera= (Camera)EditorGUILayout.ObjectField("Camera", uiCamera, typeof(Camera), true) as Camera;

        EditorGUILayout.Space();

        // UI Element Properties
        if (GUILayout.Button("Reset Elements"))
        {
            UpdateUIElements();
        }

   

        EditorGUILayout.Space();
        GUILayout.Label("Load Properties from JSON:");

        // Dropdown menu to select a JSON file
        List<string> jsonFiles = GetJsonFilesInFolder(jsonFolderPath);
        int selectedIndex = jsonFiles.IndexOf(selectedJsonFile);
        selectedIndex = EditorGUILayout.Popup("Select JSON File", selectedIndex, jsonFiles.ToArray());
        if (selectedIndex >= 0 && selectedIndex < jsonFiles.Count)
        {
            selectedJsonFile = jsonFiles[selectedIndex];
        }

        if (GUILayout.Button("Load Properties from Selected JSON"))
        {
            LoadPropertiesFromJson(selectedJsonFile);
        }


        EditorGUILayout.Space();
        GUILayout.Label("UI Element Properties:");

        // Image properties
        EditorGUILayout.Space();
        GUILayout.Label("New Image:");
        newImage = (Texture2D)EditorGUILayout.ObjectField("Select Image", newImage, typeof(Texture2D), false);
        imageSize = EditorGUILayout.Vector2Field("Image Size", imageSize);
        imageRotation = EditorGUILayout.Vector3Field("Image Rotation", imageRotation);

        // TextMeshProUGUI properties
        textContent = EditorGUILayout.TextField("TMP Text Content", textContent);
        textFontColor = EditorGUILayout.ColorField("Text Font Color", textFontColor);
        textFontSize = EditorGUILayout.FloatField("Text Font Size", textFontSize);

        // Button properties
        buttonColor = EditorGUILayout.ColorField("Button Color", buttonColor);
        buttonSize = EditorGUILayout.Vector2Field("Button Size", buttonSize);

        

        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Ad Banner"))
        {
            if (logo != null && description != null && cta != null)
            {
                GenerateUIHierarchy();
            }
            else
            {
                Debug.LogError("One or more UI elements (logo, description, or cta) is not assigned.");
            }
        }

        EditorGUILayout.Space();
        GUILayout.Label("Save Properties to JSON:");

        if (GUILayout.Button("Save Properties to JSON"))
        {
            SavePropertiesToJson();
        }

        EditorGUILayout.Space();
        GUILayout.Label("Instantiate UI Hierarchy in Scene:");

        // Button to instantiate UI hierarchy
        if (GUILayout.Button("Instantiate UI Hierarchy"))
        {
            InstantiateUIHierarchy();
        }

    }

    private List<string> GetJsonFilesInFolder(string folderPath)
    {
        List<string> jsonFiles = new List<string>();

        if (Directory.Exists(folderPath))
        {
            string[] files = Directory.GetFiles(folderPath, "*.json");
            foreach (string file in files)
            {
                jsonFiles.Add(Path.GetFileName(file));
            }
        }

        return jsonFiles;
    }
    private void LoadPropertiesFromJson(string fileName)
    {
        string filePath = Path.Combine(jsonFolderPath, fileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            // Parse JSON into AdProperties object
            AdProperties adProperties = JsonUtility.FromJson<AdProperties>(json);

            // Set properties based on loaded data
            templateName = adProperties.templateName;
            imageSize = adProperties.imageSize;
            imageRotation = adProperties.imageRotation;
            textContent = adProperties.textContent;
            textFontColor = adProperties.textFontColor;
            textFontSize = adProperties.textFontSize;
            buttonColor = adProperties.buttonColor;
            buttonSize = adProperties.buttonSize;

            Debug.Log("Properties loaded from JSON.");
        }
        else
        {
            Debug.LogError("JSON file not found at path: " + filePath);
        }
    }

    private void SavePropertiesToJson()
    {
        // Increment a counter to make each filename unique
        int counter = 1;

        // Generate a new filename based on the counter
        string newJsonFileName = "Addata_" + counter + ".json";

        string newJsonFilePath = Path.Combine("Assets/JSONData", newJsonFileName);

        // Check if the file already exists, increment the counter until a unique filename is found
        while (File.Exists(newJsonFilePath))
        {
            counter++;
            newJsonFileName = "Addata_" + counter + ".json";
            newJsonFilePath = Path.Combine("Assets/JSONData", newJsonFileName);
        }

        // Serialize AdProperties object to JSON
        string json = JsonUtility.ToJson(new AdProperties
        {
            templateName = templateName,
            imageSize = imageSize,
            imageRotation = imageRotation,
            textContent = textContent,
            textFontColor = textFontColor,
            textFontSize = textFontSize,
            buttonColor = buttonColor,
            buttonSize = buttonSize
        }, true);

        // Write JSON to file
        File.WriteAllText(newJsonFilePath, json);

        Debug.Log("Properties saved to new JSON file: " + newJsonFileName);
    }
    private void YourButtonClickFunction()
    {
        Debug.Log("Button Clicked!");
    }


    private void UpdateUIElements()
    {
        // Update UI elements based on the custom window's data
        if (logo != null)
        {
            // Example: Update Image component properties
           // logo.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(" ");
            logo.color = Color.white;
            logo.rectTransform.sizeDelta = new Vector2(100f, 100f);
        }

        if (description != null)
        {
            // Example: Update TextMeshProUGUI component properties
            description.text = "New Description";
            description.color = Color.blue;
            description.fontSize = 24;
        }

        if (cta != null)
        {
            // Example: Update Button component properties
            cta.onClick.AddListener(YourButtonClickFunction);
            cta.interactable = true;
        }
    }

    private void GenerateUIHierarchy()
    {
        // Find the Canvas in the scene
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("Canvas not found in the scene. Make sure your Canvas is active in the scene.");
            return;
        }

        // Find the BG panel in the Canvas
        Transform bgPanel = canvas.transform.Find("BG");

        if (bgPanel == null)
        {
            Debug.LogError("BG Panel not found under the Canvas. Make sure your hierarchy is correct.");
            return;
        }
        Transform logoTransform = bgPanel.Find("Logo");

        if (logoTransform == null)
        {
            Debug.LogError("Logo not found under the BG Panel. Make sure your hierarchy is correct.");
            return;
        }

        // Find Image, Text, and Button under the BG panel
        Image imageComponent = bgPanel.Find("Logo").GetComponent<Image>();
        TextMeshProUGUI textComponent = bgPanel.GetComponentInChildren<TextMeshProUGUI>();
        Button buttonComponent = bgPanel.GetComponentInChildren<Button>();

        if (imageComponent == null || textComponent == null || buttonComponent == null)
        {
            Debug.LogError("One or more UI elements (Image, Text, or Button) not found under the BG Panel.");
            return;
        }

        RectTransform imageRectTransform = imageComponent.rectTransform;
        imageRectTransform.sizeDelta = imageSize;
        imageRectTransform.localRotation = Quaternion.Euler(imageRotation);

        textComponent.text = textContent;
        textComponent.color = textFontColor;
        textComponent.fontSize = textFontSize;

        Image buttonImage = buttonComponent.image;
        buttonImage.color = buttonColor;
        buttonImage.rectTransform.sizeDelta = buttonSize;

        // Set new image if available
        if (newImage != null)
        {
            // Create a new Sprite using the selected Texture2D
            Sprite newSprite = Sprite.Create(newImage, new Rect(0, 0, newImage.width, newImage.height), new Vector2(0.5f, 0.5f));

            // Assign the new Sprite to the Image component
            imageComponent.sprite = newSprite;
        }

        // Undo record for better editor experience
        Undo.RecordObject(bgPanel.gameObject, "Modify UI Hierarchy");

        // Mark the scene as dirty
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    private void InstantiateUIHierarchy()
    {
        // Create a Canvas
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;

        canvas.worldCamera = uiCamera;

        // Add CanvasScaler for responsive UI
        canvasObject.AddComponent<CanvasScaler>();

        // Add GraphicRaycaster to handle raycasting for UI interactions
        canvasObject.AddComponent<GraphicRaycaster>();

        // Create a Panel (BG)
        GameObject panelObject = new GameObject("BG");
        panelObject.transform.SetParent(canvasObject.transform, false);
        RectTransform panelRectTransform = panelObject.AddComponent<RectTransform>();
        panelRectTransform.anchorMin = new Vector2(0.0f - 2.5f / canvas.pixelRect.width, 0.0f - 2.5f / canvas.pixelRect.height);
        panelRectTransform.anchorMax = new Vector2(1.0f + 2.5f / canvas.pixelRect.width, 1.0f + 2.5f / canvas.pixelRect.height);
        panelRectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
        panelRectTransform.eulerAngles = new Vector3(0.1942283660173416f, -0.007704324088990688f, 0.0015254971804097295f);
        panelRectTransform.localScale = new Vector3(0.546375036239624f, 0.4675103724002838f, 1.0f);
        Image panelImage = panelObject.AddComponent<Image>();
        panelImage.color = Color.white; // Set the color as needed

        // Create an Image
        GameObject imageObject = new GameObject("Logo");
        imageObject.transform.SetParent(panelObject.transform, false);
        RectTransform imageRectTransform = imageObject.AddComponent<RectTransform>();
        Image imageComponent = imageObject.AddComponent<Image>();
        // Set the anchor presets to middle
        imageRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        imageRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        imageRectTransform.pivot = new Vector2(0.5f, 0.5f);
        imageRectTransform.localScale = new Vector2(1.5f, 1.5f);
        // Set the position, size, and rotation
        imageRectTransform.anchoredPosition = new Vector2(-170f, 0.100000001f);
        imageRectTransform.sizeDelta = new Vector2(100f, 100f);
        imageRectTransform.rotation = Quaternion.Euler(imageRotation);
        // Set the sprite based on your loaded image
        imageComponent.sprite = Sprite.Create(newImage, new Rect(0, 0, newImage.width, newImage.height), new Vector2(0.5f, 0.5f));


        // Create an Text
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(panelObject.transform, false);
        RectTransform textRectTransform = textObject.AddComponent<RectTransform>();
        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        // Set the anchor presets to middle
        textRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        textRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        textRectTransform.pivot = new Vector2(0.5f, 0.5f);
        // Set the position, size, and scale
        textRectTransform.anchoredPosition = new Vector2(90f, 50f);
        textRectTransform.sizeDelta = new Vector2(200f, 50f);
        textRectTransform.localScale = new Vector3(1f, 1f, 1f);
        // Set text properties
        textComponent.text = textContent;
        textComponent.color = textFontColor;
        textComponent.fontSize = textFontSize;


        // Create a Button
        GameObject buttonObject = new GameObject("Button");
        buttonObject.transform.SetParent(panelObject.transform, false);
        RectTransform buttonRectTransform = buttonObject.AddComponent<RectTransform>();
        Button buttonComponent = buttonObject.AddComponent<Button>();

        // Create a new Image component for the button
        Image buttonImage = buttonObject.AddComponent<Image>();

        // Set the anchor presets to middle
        buttonRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRectTransform.pivot = new Vector2(0.5f, 0.5f);
        // Set the position, size, and rotation
        buttonRectTransform.anchoredPosition = new Vector2(9.53f, -105f);
        buttonRectTransform.sizeDelta = new Vector2(150f, 50f);
        buttonRectTransform.localScale = new Vector3(1f, 1f, 1f);

        // Set the default sprite state of the button
        SpriteState spriteState = buttonComponent.spriteState;
        buttonImage.sprite = spriteState.disabledSprite; // You can use any state here depending on your requirement
        buttonImage.color = buttonColor; // Set the color for the button image

        // Create a Text component as a child of the button
        GameObject buttonTextObject = new GameObject("Text");
        buttonTextObject.transform.SetParent(buttonObject.transform, false);
        RectTransform buttonTextRectTransform = buttonTextObject.AddComponent<RectTransform>();
        Text buttonTextComponent = buttonTextObject.AddComponent<Text>();

        // Set the text properties
        buttonTextComponent.text = "Click Me"; // Set your desired text
        buttonTextComponent.color = Color.black; // Set the text color
        buttonTextComponent.fontSize = 20; // Set the font size

        // Set the anchor presets and pivot for centering the text on the button
        buttonTextRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        buttonTextRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        buttonTextRectTransform.pivot = new Vector2(0.5f, 0.5f);

        // Set the position to the center of the button
        buttonTextRectTransform.anchoredPosition = Vector2.zero;


        Debug.Log("UI Hierarchy instantiated in the scene.");
    }

}



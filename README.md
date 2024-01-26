# UI-Object-Generation-With-JSON
__A project to generate custom UI object templates with the help of JSON data using Unity custom editor__

# 1) Drag & drop all the respective elements into their respective fields in the AdObjectGenerator Custom Window

    
![1st](https://github.com/AbhiMGame/UI-Object-Template-Generation-With-JSON-Format/assets/99332106/5b7558aa-03ff-41b7-8d54-27e2d5014962)

# 2) Load, view and edit JSON template data directly in the custom editor 

    
![2nd](https://github.com/AbhiMGame/UI-Object-Template-Generation-With-JSON-Format/assets/99332106/1e95a0af-e870-470f-ba68-30005ac3fe0f)

# 3) If the user wants to save any edited data, it can be directly saved with a click of button.
The saved data will be saved in a new JSON file in the JSONData folder. 

![4th](https://github.com/AbhiMGame/UI-Object-Template-Generation-With-JSON-Format/assets/99332106/5fc94e24-9a71-464a-9092-31587b48242b)

![5th](https://github.com/AbhiMGame/UI-Object-Template-Generation-With-JSON-Format/assets/99332106/f750c5cc-3848-49d0-b5fb-a1fb8c5d5323)

# 4) The user can instantiate the UI object hierarchy in the scene directly with the loaded element data

#Note: As the UI element property fields are limited in the editor, the new instantiated object doesn't carry with it the excluded properties like the button position, text alignment, the bg image etc.
To tweak it, the user can increment the fields in the JSON template directly from IDE and then load it in custom editor. Also Adproperties.cs and AdTemplateEditor.cs needs to be modified. 


![8th](https://github.com/AbhiMGame/UI-Object-Template-Generation-With-JSON-Format/assets/99332106/3f3967d4-9504-48ba-a848-5a6e620eb307)

Unity Dialogue System
Overview
This Unity Dialogue System is a flexible and extensible framework designed for implementing interactive dialogue in Unity games. It supports dynamic dialogue loading from Excel files, typewriter-style text display with sound effects, speaker visuals with active/inactive states, and choice-based branching dialogues. The system is ideal for narrative-driven games, such as visual novels, RPGs, or adventure games.
Features

Excel Integration: Import dialogue data from Excel files (.xlsx, .xls) using the DialogueExcelImporter script, supporting multilingual text and sprite assignments.
Dynamic Dialogue Flow: Manage dialogue sequences with a DialogueData ScriptableObject, including speaker names, sprites, and branching choices.
Typewriter Effect: Display dialogue text with a customizable typewriter effect, accompanied by optional typing sound effects.
Speaker Visuals: Display left and right speaker sprites with active/inactive color states to highlight the current speaker.
Choice System: Support for dialogue choices that can branch to different dialogue sequences or trigger gameplay effects (e.g., morality, coins, health changes).
Interactable Dialogues: Start dialogues via interactable objects in the game world, with configurable interaction prompts and distances.
Log System: Optionally add log entries after specific dialogue nodes for narrative tracking.

Installation

Clone or Import: Add the provided scripts to your Unity project's Assets folder.
Dependencies:
Ensure TextMeshPro (TMPro) is installed via the Unity Package Manager for text rendering.
Install the ExcelDataReader NuGet package for Excel import functionality (used in DialogueExcelImporter.cs).


Setup:
Create a DialogueDataListSO asset via the Unity menu (Assets > Create > 对话/DialogueDataListSO).
Create DialogueData assets for individual dialogues (Assets > Create > 对话/DialogueData).
Configure the DialogueSystem component on a GameObject in your scene, assigning the UI components and DialogueDataListSO.



Usage

Excel Setup:

Prepare an Excel file with the following columns (at minimum):
ID: Unique dialogue ID (integer).
SpeakerNameLeft: Name of the left speaker.
SpeakerNameRight: Name of the right speaker.
SpriteLeftPath: Resource path to the left speaker's sprite.
SpriteRightPath: Resource path to the right speaker's sprite.
Row: Speaker position (left or right).
Speech: Dialogue text.
ChooseID: Optional ID for branching choices (integer).
ChooseText: Optional text for choice buttons.


Place the Excel file in Assets/Resources/DialogueExcel.
Use the Unity Editor menu Tools > Dialogue > Import Excel (Fixed) to import the Excel data into DialogueData assets.


Scene Setup:

Attach the DialogueSystem script to a GameObject.
Assign UI elements (e.g., TMP_Text for names and dialogue, Image for speaker sprites, Button for continue/next, and a Transform for choice button parenting) in the Inspector.
Assign a DialogueDataListSO asset containing your dialogue data.
Add DialogueInteractable components to interactable objects in the scene to trigger dialogues.


Customization:

Adjust typingSpeed and typingSound in the DialogueSystem for the typewriter effect.
Modify activeSpeakerColor and inactiveSpeakerColor to control speaker sprite visuals.
Set keepInactiveVisible to true to keep inactive speakers visible (dimmed) or false to hide them.
Implement custom effects in ApplyChoiceEffects for choice-based gameplay mechanics (e.g., morality, coins, health).


Starting a Dialogue:

Call DialogueSystem.Instance.StartDialogue(dialogueID) to begin a dialogue with the specified ID.
Interact with objects containing the DialogueInteractable component to trigger dialogues in-game.



Project Structure

DialogueData.cs: Defines the DialogueData ScriptableObject for storing dialogue information, including speaker details, dialogue lines, and choices.
DialogueExcelImporter.cs: Handles importing dialogue data from Excel files in the Unity Editor, creating DialogueData assets.
DialogueSystem.cs: Core system for managing dialogue flow, UI updates, typewriter effects, and choice handling.
DialogueInteractable.cs: Implements the IInteractable interface for triggering dialogues via in-game interactions.
DialogueDataListSO.cs: Manages a list of DialogueData assets and a dictionary for quick lookup by ID.
DialoguePanel.cs: Defines the UI components for displaying dialogue text, speaker names, sprites, and choice buttons.

Notes

The system assumes the presence of a Resources/DialogueData folder for storing generated DialogueData assets.
Ensure sprite assets are placed in the Resources folder for Resources.Load<Sprite> to work correctly.
The DialogueExcelImporter requires the ExcelDataReader library and is only active in the Unity Editor (#if UNITY_EDITOR).
The typewriter effect includes customizable delays for punctuation marks (e.g., commas, periods) to enhance readability.
The system supports dialogue branching via choices, with effects applied through the ApplyChoiceEffects method.

Limitations

The Excel importer assumes a specific column structure; deviations may cause parsing errors.
The system currently supports only left/right speaker positions; additional positions would require extending the Row enum.
Dialogue assets are overwritten during Excel import; ensure backups if manual edits are made.
The typewriter sound effect requires an AudioClip and an AudioSource component on the DialogueSystem GameObject.

Future Improvements

Add support for animated speaker sprites or transitions.
Implement a dialogue editor window for manual dialogue creation in Unity.
Support for multi-language dialogues by extending the Excel importer.
Add event triggers for specific dialogue nodes (e.g., animations, scene changes).
Optimize memory usage for large dialogue datasets.

License
This project is provided as-is for educational and non-commercial use. Feel free to modify and extend it for your own projects.


using System;
using System.IO;
using Newtonsoft.Json;

namespace cis_580_game;

public class SaveManager
{
    public static readonly string SaveFileLocation = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "james-claar-cis-580-game");

    public static readonly string SaveFileName = "save.json";

    public static readonly string AutosaveFileName = "autosave.json";

    public static readonly string SaveFilePath = Path.Join(SaveFileLocation, SaveFileName);

    public static readonly string AutosaveFilePath = Path.Join(SaveFileLocation, AutosaveFileName);

    private JsonSerializer _serializer;

    public SaveManager()
    {
        _serializer = new()
        {
            Formatting = Formatting.Indented
        };
    }

    public void CreateSaveFile(SavedState state, bool isAutosave = false)
    {
        string path = isAutosave ? AutosaveFilePath : SaveFilePath;
        Directory.CreateDirectory(SaveFileLocation);
        using StreamWriter file = new(path);
        _serializer.Serialize(file, state);
    }

    public SavedState LoadSaveFile(bool isAutosave = false)
    {
        string path = isAutosave ? AutosaveFilePath : SaveFilePath;
        if (isAutosave && !File.Exists(path)) return null;
        using StreamReader file = new(path);
        return (SavedState)_serializer.Deserialize(file, typeof(SavedState));
    }

    public static bool DoesSaveFileExist(bool isAutosave = false)
    {
        string path = isAutosave ? AutosaveFilePath : SaveFilePath;
        return File.Exists(path);
    }
}

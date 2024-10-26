using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace SequoiaEngine
{
    public class InputConfig
    {

        public static InputConfig Instance { get; private set; }

        public Dictionary<string, Keys> ActionsToKeyboardKeys = new();
        public Dictionary<string, Buttons> ActionsToControllerButtons = new();
        public Dictionary<string, MouseButton> ActionsToMouseButtons = new();

        public string ActionKeyboardSaveLocation = "KeyboardControls.json";
        public string ActionControllerSaveLocation = "ControllerControls.json";
        public string ActionMouseSaveLocation = "MouseControls.json";


        private bool isSavingKeyboard = false;
        private bool isSavingMouse = false;
        private bool isSavingController = false;

        private bool isLoadingKeyboard = false;
        private bool isLoadingMouse = false;
        private bool isLoadingController = false;

        private bool isKeyboardLoaded = false;
        private bool isMouseLoaded = false;
        private bool isControllerLoaded = false;

        /// <summary>
        /// Read only: Shorthand to determine if everything is loaded.
        /// </summary>
        public bool IsLoaded
        {
            get {  return isKeyboardLoaded && isMouseLoaded && isControllerLoaded; }
        }

        /// <summary>
        /// Read only: Shorthand to determine is anything is saving
        /// </summary>
        public bool IsSaving
        {
            get { return isSavingController || isSavingController || isLoadingKeyboard; }
        }


        public InputConfig()
        {
            if (Instance != null)
            {
                Debug.Fail("Input Config was already defined. This should not be");
                Debug.Assert(Instance != null);
            }

            Instance = this;


            // This is where you will load / save config for this. Right now, I will probably just do this manually.
        }

        public void RegisterKeyboardDefaultConfig(Dictionary<string, Keys> keyboardActions)
        {
            foreach (KeyValuePair<string, Keys> keyValuePair in keyboardActions)
            {
                if (!ActionsToKeyboardKeys.ContainsKey(keyValuePair.Key))
                {
                    ActionsToKeyboardKeys.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        public void RegisterControllerDefaultConfig(Dictionary<string, Buttons> controllerActions)
        {
            foreach (KeyValuePair<string, Buttons> keyValuePair in controllerActions)
            {
                if (!ActionsToControllerButtons.ContainsKey(keyValuePair.Key))
                {
                    ActionsToControllerButtons.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        public void RegisterMouseDefaultConfig(Dictionary<string, MouseButton> mouseActions)
        {
            foreach (KeyValuePair<string, MouseButton> keyValuePair in mouseActions)
            {
                if (!ActionsToMouseButtons.ContainsKey(keyValuePair.Key))
                {
                    ActionsToMouseButtons.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        public void LoadControls()
        {
            LoadSavedController();
            LoadSavedMouse();
            LoadSavedKeybinds();
        }

        public void SaveControls()
        {
            SaveController();
            SaveMouse();
            SaveKeybinds();
        }


        #region Load Methods
        private void LoadSavedKeybinds()
        {
            if (!isLoadingKeyboard)
            {
                isLoadingKeyboard = true;
                isKeyboardLoaded = false;
                FinalizeAsyncKeyboardLoad();
            }
        }

        private void LoadSavedController()
        {
            if (!isLoadingController)
            {
                isLoadingController = true;
                isControllerLoaded = false;
                FinalizeAsyncControllerLoad();
            }
        }

        private void LoadSavedMouse()
        {
            if (!isLoadingMouse)
            {
                isLoadingMouse = true;
                isMouseLoaded = false;
                FinalizeAsyncMouseLoad();
            }
        }
        #endregion

        #region Save Methods

        public void SaveKeybinds()
        {
            if (!isSavingKeyboard)
            {
                isSavingKeyboard = true;
                FinalizeAsyncKeyboardSave();
            }
        }

        public void SaveController()
        {
            if (!isSavingController)
            {
                isSavingController = true;
                FinalizeAsyncControllerSave();
            }
        }

        public void SaveMouse()
        {
            if (!isSavingMouse)
            {
                isSavingMouse = true;
                FinalizeAsyncMouseSave();
            }
        }

        #endregion

        #region Async Saving
        // ==========================================================================================================================================
        //
        //
        //  Async Saving
        //
        //
        // ==========================================================================================================================================

        private async void FinalizeAsyncKeyboardSave()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile(ActionKeyboardSaveLocation, FileMode.Create))
                        {
                            if (fs != null)
                            {
                                using (var isoFileWriter = new StreamWriter(fs))
                                {
                                    isoFileWriter.WriteAsync(JsonConvert.SerializeObject(ActionsToKeyboardKeys));
                                }
                            }
                        }
                    }
                    catch (IsolatedStorageException error)
                    {

                        Debug.Fail(error.ToString());
                    }
                }
                isSavingKeyboard = false;
            });
        }

        private async void FinalizeAsyncMouseSave()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile(ActionKeyboardSaveLocation, FileMode.Create))
                        {
                            if (fs != null)
                            {
                                using (var isoFileWriter = new StreamWriter(fs))
                                {
                                    isoFileWriter.WriteAsync(JsonConvert.SerializeObject(ActionsToMouseButtons));
                                }
                            }
                        }
                    }
                    catch (IsolatedStorageException error)
                    {

                        Debug.Fail(error.ToString());
                    }
                }
                isSavingKeyboard = false;
            });
        }

        private async void FinalizeAsyncControllerSave()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile(ActionKeyboardSaveLocation, FileMode.Create))
                        {
                            if (fs != null)
                            {
                                using (var isoFileWriter = new StreamWriter(fs))
                                {
                                    isoFileWriter.WriteAsync(JsonConvert.SerializeObject(ActionsToControllerButtons));
                                }
                            }
                        }
                    }
                    catch (IsolatedStorageException error)
                    {

                        Debug.Fail(error.ToString());
                    }
                }
                isSavingKeyboard = false;
            });
        }

        #endregion

        #region Async Loading
        private async void FinalizeAsyncKeyboardLoad()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile(ActionKeyboardSaveLocation, FileMode.Open))
                        {
                            if (fs != null)
                            {

                                using (var isoFileReader = new StreamReader(fs))
                                {
                                    ActionsToKeyboardKeys = JsonConvert.DeserializeObject<Dictionary<string, Keys>>(isoFileReader.ReadToEnd());
                                }
                            }
                        }
                    }
                    catch (IsolatedStorageException e)
                    {
                        Console.WriteLine($"Failed to save file because of {e}");
                    }
                }

                isLoadingKeyboard = false;
                isKeyboardLoaded = true;
            });
        }

        private async void FinalizeAsyncControllerLoad()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile(ActionControllerSaveLocation, FileMode.Open))
                        {
                            if (fs != null)
                            {

                                using (var isoFileReader = new StreamReader(fs))
                                {
                                    ActionsToControllerButtons = JsonConvert.DeserializeObject<Dictionary<string, Buttons>>(isoFileReader.ReadToEnd());
                                }
                            }
                        }
                    }
                    catch (IsolatedStorageException e)
                    {
                        Console.WriteLine($"Failed to save file because of {e}");
                    }
                }

                isLoadingController = false;
                isControllerLoaded = true;
            });
        }

        private async void FinalizeAsyncMouseLoad()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile(ActionMouseSaveLocation, FileMode.Open))
                        {
                            if (fs != null)
                            {

                                using (var isoFileReader = new StreamReader(fs))
                                {
                                    ActionsToMouseButtons = JsonConvert.DeserializeObject<Dictionary<string, MouseButton>>(isoFileReader.ReadToEnd());
                                }
                            }
                        }
                    }
                    catch (IsolatedStorageException e)
                    {
                        Console.WriteLine($"Failed to save file because of {e}");
                    }
                }

                isLoadingMouse = false;
                isMouseLoaded = true;
            });
        }
        #endregion


    }
}

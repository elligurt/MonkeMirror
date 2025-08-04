using System;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace MonkeMirror
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)] 
    public class Plugin : BaseUnityPlugin
    {
        public GameObject MonkeMirror;
        public static Plugin Instancing;


        void Awake()
        {
            if (Instancing == null)
            {
                Instancing = this;
            }
            GorillaTagger.OnPlayerSpawned(OnGameInitialized);
        }

        void OnGameInitialized()
        {
            try
            {
                if (MonkeMirror != null)
                {
                    return;
                }

                MonkeMirror = Instantiate(InitialiseMonkeMirror("MonkeMirror.Content.mirror").LoadAsset<GameObject>("GorillaMirror"));
                MonkeMirror.transform.localPosition = new Vector3 (-63.0683f, 12.1292f, - 81.7965f);
                MonkeMirror.transform.localEulerAngles = new Vector3(0f, 318.4624f, 0f);
                MonkeMirror.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error initializing MonkeMirror: {e.Message}");
            }
        }


        public AssetBundle InitialiseMonkeMirror(string path)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            return AssetBundle.LoadFromStream(stream);
        }
    }
}

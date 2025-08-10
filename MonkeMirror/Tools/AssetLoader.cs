using System;
using System.Reflection;
using BepInEx;
using UnityEngine;
using MonkeMirror.Behaviours;

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

                MonkeMirror = Instantiate(InitialiseMonkeMirror("MonkeMirror.Content.mirror").LoadAsset<GameObject>("MonkeMirror"));
                MonkeMirror.transform.localPosition = new Vector3(-64.6139f, 11.4219f, - 81.7491f);
                MonkeMirror.transform.localEulerAngles = new Vector3(0.9453f, 36.3249f, 357.2256f);
                MonkeMirror.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

                MonkeMirror.AddComponent<MirrorUI>();
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

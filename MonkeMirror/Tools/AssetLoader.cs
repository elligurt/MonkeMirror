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
                MonkeMirror.transform.localPosition = new Vector3(-63.1665f, 11.8746f, - 81.880f);
                MonkeMirror.transform.localEulerAngles = new Vector3(1.2508f, 52.3207f, 1.3364f);
                MonkeMirror.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

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

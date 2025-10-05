using System;
using System.Threading.Tasks;
using BepInEx;
using UnityEngine;
using MonkeMirror.Tools;
using MonkeMirror.Behaviours;

namespace MonkeMirror
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }

        private GameObject _mirrorPrefab;

        private void Awake()
        {
            Instance = this;
            _ = SetupMirror();
        }

        private async Task SetupMirror()
        {
            try
            {
                _mirrorPrefab = await AssetLoader.LoadAsset<GameObject>("MonkeMirror");
                if (_mirrorPrefab == null)
                {
                    Debug.LogError("[MonkeMirror] Failed to load prefab.");
                    return;
                }

                GameObject mirrorInstance = Instantiate(_mirrorPrefab);
                mirrorInstance.SetActive(true);
                mirrorInstance.transform.position = new Vector3(-65.5466f, 10.2129f, - 80.04f);
                mirrorInstance.transform.rotation = Quaternion.Euler(2.5711f, 349.3142f, 3.8056f);
                mirrorInstance.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f); //by the friends stand

                mirrorInstance.AddComponent<MirrorUI>();

                var button = mirrorInstance.transform.Find("UIButton/Button")?.gameObject;
                if (button != null)
                    button.AddComponent<PressableButton>();
            }
            catch (Exception ex)
            {
                Debug.LogError("[MonkeMirror] Error setting up mirror: " + ex);
            }
        }
    }
}
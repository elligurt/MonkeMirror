using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MonkeMirror.Behaviours
{
    public class MirrorUI : MonoBehaviour
    {
        private TMP_Text nameText;

        private void Awake()
        {
            var nameTextTransform = transform.Find("Base/Name Text");
            if (nameTextTransform != null)
                nameText = nameTextTransform.GetComponent<TMP_Text>();
        }

        private void Start()
        {
            StartCoroutine(SetNameWhenReady());
        }

        private IEnumerator SetNameWhenReady()
        {
            yield return new WaitUntil(() => PhotonNetwork.LocalPlayer != null);
            yield return new WaitForSeconds(1f);

            if (nameText != null)
            {
                string nickname = PhotonNetwork.LocalPlayer.NickName;
                nameText.text = string.IsNullOrEmpty(nickname) ? "???" : nickname;
            }
        }
    }
}
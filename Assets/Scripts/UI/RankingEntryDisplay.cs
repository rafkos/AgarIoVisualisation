using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class RankingEntryDisplay : MonoBehaviour
    {
        public Text PlayerName;
        public Image PlayerColor;
        public Text Score;

        // Use this for initialization
        void Start ()
        {
        }

        // Update is called once per frame
        void Update () {
	
        }

        public void Update(PlayerScore rankingEntry)
        {
            if (rankingEntry == null)
            {
                return;
            }

            if (PlayerName != null)
            {
                PlayerName.text = rankingEntry.PlayerName;
            }

            if (Score != null)
            {
                Score.text = rankingEntry.Score.ToString();
            }

            if (PlayerColor != null)
            {
                PlayerColor.color = rankingEntry.PlayerColor;
            }
        }
    }
}

using UnityEngine;
using TMPro;

public class BattleOverScreen : GenericOptionList<BattleFailedOption>
{
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI subtitleText;

    public Transform RewardList;
    public BattleSuccessReward rewardPrefab;

    public bool success
    {
        set
        {
            if (value)
            {
                mainText.text = "CONGRATULATIONS!";

                Interactable = false;
                List.gameObject.SetActive(false);
                RewardList.gameObject.SetActive(true);

                foreach (BattleReward reward in FindObjectOfType<BattleManager>().Rewards)
                {
                    BattleSuccessReward rewardObject = Instantiate(rewardPrefab, RewardList);
                    rewardObject.rewardIcon = null;
                    rewardObject.rewardLabel = null;
                }
            }
            else
            {
                mainText.text = "BATTLE FAILED";
                Interactable = true;
            }
        }
    }

    public string message
    {
        set
        {
            subtitleText.text = value;
        }
    }

    protected override void OnEnable()
    {
        foreach (BattleFailedOption option in List.GetComponentsInChildren<BattleFailedOption>())
        {
            Add(option);
        }

        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();

        if (!Interactable)
        {
            if (Input.GetButtonDown("Submit"))
            {
                FindObjectOfType<BattleManager>().GetComponent<Animator>().SetTrigger("Success");
            }
        }
    }
}

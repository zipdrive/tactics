using UnityEngine.SceneManagement;

public class CampaignBattleScene : CampaignScene
{
    private string m_Map;
    private string m_Battle;

    public CampaignBattleScene(string map, string battle)
    {
        m_Map = map;
        m_Battle = battle;
    }

    public void Load()
    {
        BattleManager.NextMapFile = m_Map;
        BattleManager.NextBattle = m_Battle;

        SceneManager.LoadScene("Battle");
    }
}

using UnityEngine;
using MoreMountains.Feedbacks;
using Eggtato.Utility;

public class GameSceneManager : Singleton<GameSceneManager>
{
    [SerializeField] private MMFeedbacks loadVisualNovelSceneFeedbacks;
    [SerializeField] private MMFeedbacks loadShopSceneFeedbacks;
    [SerializeField] private MMFeedbacks loadGatheringSceneFeedbacks;

    public void LoadVisualNovelScene()
    {
        loadVisualNovelSceneFeedbacks.PlayFeedbacks();
    }
    public void LoadShopScene()
    {
        loadShopSceneFeedbacks.PlayFeedbacks();
    }
    public void LoadGatheringScene()
    {
        loadGatheringSceneFeedbacks.PlayFeedbacks();
    }
}

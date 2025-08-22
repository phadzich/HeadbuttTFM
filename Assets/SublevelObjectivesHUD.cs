using UnityEngine;

public class SublevelObjectivesHUD : MonoBehaviour
{
    [SerializeField] private SublevelObjectiveHUDIndicator objectiveIndicator;
    [SerializeField] private Transform container;

    private Sublevel currentSublevel;

    public void OnSublevelEntered(Sublevel _newSublevel)
    {

        if (currentSublevel != null)
            currentSublevel.onSublevelObjectivesUpdated -= RefreshObjectives;


        foreach (Transform child in container)
            Destroy(child.gameObject);

        currentSublevel = _newSublevel;
        currentSublevel.onSublevelObjectivesUpdated += RefreshObjectives;

        RefreshObjectives();
    }

    private void RefreshObjectives()
    {
        foreach (Transform _child in container)
            Destroy(_child.gameObject);

        foreach (var objective in currentSublevel.activeObjectives)
        {
            var ui = Instantiate(objectiveIndicator, container);
            ui.Setup(objective.GetIcon(), objective.current, objective.goal, objective.progress);
        }
    }

    public void ShowCheckpoint()
    {
        foreach (Transform _child in container)
            Destroy(_child.gameObject);
    }
}

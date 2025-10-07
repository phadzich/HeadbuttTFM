using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlasterBiomePanelUI : MonoBehaviour
{
    public int levelID;
    private int maxDepth;
    public Transform floorsHolder;
    private LevelConfig levelData;

    public TextMeshProUGUI levelNameTXT;
    public Image levelIcon;
    public Image border;

    public GameObject checkpointButton;
    public GameObject floorToken;

    private Color mainColor;

    private void OnEnable()
    {
        SetupPanel(LevelManager.Instance.levelsList[levelID]);
    }
    public void SetupPanel(LevelConfig _level)
    {
        ClearFloors();
        levelData = _level;
        levelNameTXT.text = _level.levelName;
        mainColor = _level.levelColor;
        levelNameTXT.color = mainColor;
        levelIcon.sprite = _level.levelIcon;
        border.color = mainColor;

        maxDepth = _level.maxDepth;
        int _floorIndex = 0;
        foreach (SublevelConfig _floor in _level.subLevels)
        {
            if(_floor is MiningSublevelConfig)
            {
                AddFloor(_floorIndex);
            }
            else
            {
                AddCheckpoint(_floorIndex);
            }
            _floorIndex++;
        }
    }

    private void ClearFloors()
    {
        foreach(Transform _child in floorsHolder)
        {
            Destroy(_child.gameObject);
        }
    }
    private void AddCheckpoint(int _index)
    {
        var _checkpoint = Instantiate(checkpointButton,floorsHolder);
        _checkpoint.GetComponent<Image>().color = mainColor;
        _checkpoint.transform.GetComponentInChildren<TextMeshProUGUI>().text = $"CHECKPOINT {_index + 1}";
        _checkpoint.GetComponent<Button>().onClick.AddListener(() => LoadCheckpoint(levelData, _index));
        if (_index > maxDepth)
        {
            _checkpoint.GetComponent<Button>().interactable = false;
        }
        if(levelID == 5)
        {
            _checkpoint.GetComponent<Button>().interactable = false;
        }
    }

    private void LoadCheckpoint(LevelConfig _level, int _depth)
    {
        Debug.Log("LOADING CHECKPIINT DIRECT");
        int _levelIndex = LevelManager.Instance.levelsList.IndexOf(_level);
        LevelManager.Instance.ChangeLevelAndCheckpoint(_levelIndex, _depth);
        UIManager.Instance.NPCElevatorPanel.SetActive(false);
        UIManager.Instance.frontEndFrame.CloseFrame();
    }

    private void AddFloor(int _index)
    {
        var _token = Instantiate(floorToken, floorsHolder);
        if (_index <= maxDepth)
        {
            _token.GetComponent<Image>().color = mainColor;
        }
    }
}

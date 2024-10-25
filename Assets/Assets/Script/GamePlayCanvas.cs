using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections.Generic;
namespace Script
{
    public class GamePlayCanvas : UICanvas
    { 
        [SerializeField] private Text _levelText;
        [SerializeField] private Transform stepCountContainer; // Container để chứa các UI Count
        [SerializeField] private GameObject stepCountPrefab; // Prefab của UI Count
        [SerializeField] private Sprite fullSprite; // Sprite khi đầy
        [SerializeField] private Sprite emptySprite; // Sprite khi rỗng

        private GameManager _gameManager;
        private List<Image> stepCountImages = new List<Image>();

    private void Awake()
    {
        if (_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
    }

    public void PauseBtn()
    {
        UIManager.Instance.OpenUI<PauseCanvas>();
    }

    private void Update()
    {
        UpdateLevelText();
    }

    private void UpdateLevelText()
    {
        if (_levelText != null)
        {
            _levelText.text = SceneManager.GetActiveScene().name;
        }
    }

    public void spawnStepCount()
    {
        if (_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        // Xóa các UI Count hiện có
        foreach (Transform child in stepCountContainer)
        {
            Destroy(child.gameObject);
        }
        stepCountImages.Clear();

        // Tạo các UI Count mới dựa trên numStep
        for (int i = 0; i < _gameManager.numStep; i++)
        {
            GameObject stepCount = Instantiate(stepCountPrefab, stepCountContainer);
            Image stepCountImage = stepCount.GetComponent<Image>();
            stepCountImage.sprite = fullSprite;
            stepCountImages.Add(stepCountImage);
        }
    }

    public void UpdateStepCount()
    {
        if (_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        int remainingSteps = _gameManager.remainingSteps;

        for (int i = 0; i < stepCountImages.Count; i++)
        {
            if (i < remainingSteps)
            {
                stepCountImages[i].sprite = fullSprite;
            }
            else
            {
                stepCountImages[i].sprite = emptySprite;
            }
        }
    }
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public List<FillNode> nodes = new List<FillNode>();
        public int numStep;
        public PlayerMove player;
        public bool hasWon = false;
        
        public int remainingSteps;
        void Awake()
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            if (numStep >= player.StepCount)
            {
                CheckWinCondition();
            }

            if (numStep < player.StepCount && !hasWon)
            {
              
            }
            remainingSteps = Mathf.Max(0, CalculateRemainingSteps());

           
        }
        private void CheckWinCondition()
        {
            if (hasWon) return; // Nếu đã "win" thì không kiểm tra nữa

            foreach (FillNode node in nodes)
            {
                if (!node.isFilled)
                {
                    return; // Nếu có bất kỳ node nào chưa được lấp đầy, thoát khỏi hàm
                }
            }

            // Nếu tất cả các node đều được lấp đầy
            hasWon = true;
            LevelManager.Instance.SaveGame();
            
        }
        private int CalculateRemainingSteps()
        {
            int steps = numStep - player.StepCount;
            return Mathf.Max(0, steps);
        }
    }
}



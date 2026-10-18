// Sarah Hannah Langdon
// 2026-04-10
// Advanced Unity Programming
// https://www.youtube.com/watch?v=ES2jPGnWQoU

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager sManager;
    public int playerScore = 0;
    
    void Start()
    {
        sManager = this;
    }

    public void IncreaseScore(int Increase)
    {
        playerScore += Increase;
    }
}

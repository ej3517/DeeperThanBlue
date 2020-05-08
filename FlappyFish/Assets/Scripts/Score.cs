using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score
{
   public static void Start()
   {
      ResetHighScore();
   }

   public static int GetHighScore()
   {
      return PlayerPrefs.GetInt("highscore");
   }

   public static bool TrySetNewHighScore(int score)
   {
      int highScore = GetHighScore();
      if (score > highScore)
      {
         PlayerPrefs.SetInt("highscore", score);
         PlayerPrefs.Save();
         return true;
      }
      else
      {
         return false;
      }
   }

   public static void ResetHighScore()
   {
      PlayerPrefs.SetInt("highscore", 0);
      PlayerPrefs.Save();
   }
}

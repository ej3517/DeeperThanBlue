using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoundData
{
    public string module;
    public DifficultyData[] hardOrEasy;
    
    // in the case or one there are no hard questions or easy questions
    private AnswersList[] answersList;
    private QuestionsList[] questionsList;
    private DifficultyData difficultyData;

    // private DifficultyData difficultyData;

    public DifficultyData GetHardQuestion()
    {
        if (hardOrEasy.Length > 0)
        {
            return hardOrEasy[0];
        }
        else
        {
            return QuestionsMissing("easy");
        }
    }
    
    public DifficultyData GetEasyQuestion()
    {
        if (hardOrEasy.Length > 1)
        {
            return hardOrEasy[1];
        }
        else
        {
            return QuestionsMissing("hard");
        }
    }

    private DifficultyData QuestionsMissing(string difficulty)
    {
        // answer
        answersList[0] = new AnswersList();
        answersList[0].answer = "Click";
        answersList[0].isTrue = true;
        // question
        questionsList[0] = new QuestionsList();
        questionsList[0].answers = answersList;
        questionsList[0].question = "There are no" + difficulty + " questions";
        // hardoreasy
        difficultyData.questions = questionsList;
        difficultyData.isHard = false;
        return difficultyData;
    }
}



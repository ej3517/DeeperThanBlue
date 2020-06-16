using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoundData
{
    public string module;
    public DifficultyData[] hardOrEasy;
    
    // in the case or one there are no hard questions or easy questions
    private AnswersList[] answersList = new AnswersList[1];
    private QuestionsList[] questionsList = new QuestionsList[1];
    private DifficultyData difficultyData = new DifficultyData();

    // private DifficultyData difficultyData;

    public DifficultyData GetHardQuestion()
    {
        DifficultyData hardData;
        if (hardOrEasy[0].questions.Length > 0)
        {
            hardData =  hardOrEasy[0];
        }
        else
        {
            hardData = QuestionsMissing("hard");
        }
        return hardData;
    }
    
    public DifficultyData GetEasyQuestion()
    {
        DifficultyData easyData;
        if (hardOrEasy[1].questions.Length > 0)
        {
            easyData = hardOrEasy[1];
        }
        else
        {
            easyData = QuestionsMissing("easy");
        }
        return easyData;
    }

    private DifficultyData QuestionsMissing(string difficulty)
    {
        // hardoreasy
        difficultyData = new DifficultyData();
        difficultyData.questions = questionsList;
        difficultyData.isHard = true;
        // question
        questionsList[0] = new QuestionsList();
        questionsList[0].answers = answersList;
        questionsList[0].question = "There are no" + difficulty + " questions";
        // answer
        answersList[0] = new AnswersList();
        answersList[0].answer = "Click";
        answersList[0].isTrue = true;
        // return
        return difficultyData;
    }
}



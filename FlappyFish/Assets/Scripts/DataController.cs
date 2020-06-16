using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CloudantInterface; 
using System.Threading.Tasks;
using Newtonsoft.Json; 
using System.Threading;

public class DataController : MonoBehaviour
{
    public Interface interfaceLink; 


    public LeaderboardStructure[] leaderboardArray;  

    public List<Leaderboard> leaderboardList; 
    // List of question before formatting 
    public List<QuestionStructure> questionList = new List<QuestionStructure>(); 

    // List of questions for Elliot 
    public RoundData[] questionSet;
    private RoundData questionSetWanted;
    private LeaderboardStructure leaderboardWanted; 
    public bool isFinishedFetching;

    // Start is called before the first frame update
    async void Start()
    {
        Debug.Log("Start");

        DontDestroyOnLoad(gameObject);
        isFinishedFetching = false;
        Loader.Load(Loader.Scene.LoginScene);

        // Get login 
        PlayerPrefs.SetString("username", "elliott");
        string gameUser = PlayerPrefs.GetString("username"); 
        
        // Fetch class in which participates 
        // Blocked async function
        interfaceLink = new Interface();
        var classesJson = await interfaceLink.GetSignUpClasses(gameUser);
        UserSpecs specs = JsonConvert.DeserializeObject<UserSpecs>(classesJson); 
        
        Debug.Log(classesJson); 
  

        // Fetch leaderboard from each corresponding classTag 

        // Need to fetch leaderboard for each classTag user is on 
        // At every iteration we pick the score from N entry of score 
        int z = 0; 
        foreach(string _class in specs.docs[0].classTag) {
            // fetch Leaderboard
            var leaderboardJson = await interfaceLink.GetLeaderboard(_class, specs.docs[0].school); 
            Thread.Sleep(1100); 

            Leaderboard leaderboard = new Leaderboard(); 
            leaderboard = JsonConvert.DeserializeObject<Leaderboard>(leaderboardJson); 
            
            leaderboardList.Add(leaderboard); 
            leaderboardList[z].module = _class; 
            z++; 
            Debug.Log(leaderboardJson);
        }


     


        foreach (string _class in specs.docs[0].classTag) {
            // fetch Question
            var questionJson = await interfaceLink.GetQuestions(_class, specs.docs[0].school); 
            Thread.Sleep(1100); 
            QuestionStructure questionData = new QuestionStructure(); 
            questionData = JsonConvert.DeserializeObject<QuestionStructure>(questionJson); 
            questionData.module = _class; 
            questionList.Add(questionData); 
            Debug.Log(questionJson); 

        }
        
        // Size of RoundData array 
        int moduleSize = questionList.Count; 
        
        // Initialize RoundData for Elliot and Array of set 

        questionSet = new RoundData[moduleSize]; 

        for (int i = 0; i < moduleSize; i++)
        {
            questionSet[i] = new RoundData(); 
            questionSet[i].module = specs.docs[0].classTag[i]; 

            // Initialize array of easy and hard questions 
            questionSet[i].hardOrEasy = new DifficultyData[2]; 
            questionSet[i].hardOrEasy[0] = new DifficultyData(); 
            questionSet[i].hardOrEasy[1] = new DifficultyData(); 

            questionSet[i].hardOrEasy[0].isHard = true; 
            questionSet[i].hardOrEasy[1].isHard = false; 

            // Get number of "Easy" and "Hard" questions
            int[] diffCount = GetEasyNumber(questionList[i].docs); 
         
            // Define number of questions per module 
            questionSet[i].hardOrEasy[0].questions = new QuestionsList[diffCount[0]]; 
            questionSet[i].hardOrEasy[1].questions = new QuestionsList[diffCount[1]]; 
          
            int easyCount = 0; 
            int hardCount = 0; 
            
            for (int j = 0; j < questionList[i].docs.Count; j++)
            {
                // Check number of options for question 
                int numberAnswers = GetNumberAnswers(questionList[i].docs[j]); 

                // Fill easy questions with difficulty == hard
                if (questionList[i].docs[j].difficulty == "Hard") {
          
                    // Create question instance 
                    questionSet[i].hardOrEasy[0].questions[hardCount] = new QuestionsList(); 
                    // Fill question
                    questionSet[i].hardOrEasy[0].questions[hardCount].question = questionList[i].docs[j].question; 
                    // Fill answers for the question 
                    questionSet[i].hardOrEasy[0].questions[hardCount].answers = new AnswersList[numberAnswers];         
                    FillEasyAnswers(numberAnswers, questionSet[i].hardOrEasy[0].questions[hardCount].answers, i, j);

                    hardCount++; 
                }

                // Fill hard questions with difficulty == easy 
                else if (questionList[i].docs[j].difficulty == "Easy") {

                    // Create question instance 
                    questionSet[i].hardOrEasy[1].questions[easyCount] = new QuestionsList(); 
                    
                    // Fill question
                    questionSet[i].hardOrEasy[1].questions[easyCount].question = questionList[i].docs[j].question; 
                    // Fill answers for the question 
                    questionSet[i].hardOrEasy[1].questions[easyCount].answers = new AnswersList[numberAnswers];         
                    FillEasyAnswers(numberAnswers, questionSet[i].hardOrEasy[1].questions[easyCount].answers, i, j);

                    easyCount++; 
                }
            }


        }
        


        Loader.Load(Loader.Scene.LoginScene);

        questionSetWanted = questionSet[0];

        // Transform Leaderboard List into Leaderboard Array Object

        // Create Leaderboard Array Object 
        leaderboardArray = new LeaderboardStructure[specs.docs[0].classTag.Count];

        for (int i = 0; i < specs.docs[0].classTag.Count; i++)
        {
            leaderboardArray[i] = new LeaderboardStructure(); 
            leaderboardArray[i].module = leaderboardList[i].module; 
            leaderboardArray[i].score = new List<string>();
            leaderboardArray[i].user = new List<string>(); 

            // Initialize string list 
            for (int j = 0; j < leaderboardList[i].docs.Count; j++)
            {
                // Iterate through users found 
                // Assign each module its socres and users corresponding 
                for (int k = 0; k < leaderboardList[i].docs[j].classTag.Count; k++)
                {

                    if (leaderboardList[i].docs[j].classTag[k] == leaderboardList[i].module)
                    {
                        //Debug.Log(leaderboardList[i].docs[j].score[k]);
                        leaderboardArray[i].score.Add(leaderboardList[i].docs[j].score[k]);
                        leaderboardArray[i].user.Add(leaderboardList[i].docs[j]._id);   
                    }
                }
                           
            }
        }       
        leaderboardWanted = leaderboardArray[0]; 
        // The Fetching is done
        isFinishedFetching = true;
        var responseMessage = interfaceLink.UpdateHighScore(specs.docs[0]);
    }

    private int[] GetEasyNumber(List<Question> questionList)
    {
        int easyCount = 0; 
        int hardCount = 0; 
        int[] returnValue = new int[2]; 
        for (int i = 0; i < questionList.Count; i++)
        {
            if (questionList[i].difficulty == "Easy")
            {
                easyCount++; 
            }
            if (questionList[i].difficulty == "Hard")
            {
                hardCount++; 
            }
        }

        returnValue[0] = hardCount;
        returnValue[1] = easyCount;
        return returnValue;
    }

    private void FillEasyAnswers (int numberAnswers, AnswersList[] answers, int i, int j)
    {
        int index; 
        List<int> randSelect;
        
        switch (numberAnswers) 
        {
            case 1:
                // Create instance 
                answers[0] = new AnswersList(); 

                // Assign answers
                answers[0].answer = questionList[i].docs[j].answer; 
                break; 
            case 2: 
                // Create list for random select
                randSelect = new List<int>(){0,1};


                index = Random.Range(0, randSelect.Count);  
                answers[randSelect[index]] = new AnswersList(); 
                answers[randSelect[index]].answer = questionList[i].docs[j].answer;
                answers[randSelect[index]].isTrue = true; 
                randSelect.RemoveAt(index); 
            
                index = Random.Range(0, randSelect.Count);  
                answers[randSelect[index]] = new AnswersList(); 
                answers[randSelect[index]].answer = questionList[i].docs[j].wrong1; 
                answers[randSelect[index]].isTrue = false;
                randSelect.RemoveAt(index); 

                break; 
            case 3:
                // Create list for random select
                randSelect = new List<int>(){0,1,2};


                index = Random.Range(0, randSelect.Count);  
                answers[randSelect[index]] = new AnswersList(); 
                answers[randSelect[index]].answer = questionList[i].docs[j].answer;
                answers[randSelect[index]].isTrue = true; 
                randSelect.RemoveAt(index); 
            
                index = Random.Range(0, randSelect.Count);  
                answers[randSelect[index]] = new AnswersList(); 
                answers[randSelect[index]].answer = questionList[i].docs[j].wrong1; 
                answers[randSelect[index]].isTrue = false;
                randSelect.RemoveAt(index); 

                index = Random.Range(0, randSelect.Count);  
                answers[randSelect[index]] = new AnswersList(); 
                answers[randSelect[index]].answer = questionList[i].docs[j].wrong2; 
                answers[randSelect[index]].isTrue = false;
                randSelect.RemoveAt(index); 
                break;
            case 4:
                // Create list for random select
                randSelect = new List<int>(){0,1,2,3};


                index = Random.Range(0, randSelect.Count);  
                answers[randSelect[index]] = new AnswersList(); 
                answers[randSelect[index]].answer = questionList[i].docs[j].answer;
                answers[randSelect[index]].isTrue = true; 
                randSelect.RemoveAt(index); 
            
                index = Random.Range(0, randSelect.Count);  
                answers[randSelect[index]] = new AnswersList(); 
                answers[randSelect[index]].answer = questionList[i].docs[j].wrong1; 
                answers[randSelect[index]].isTrue = false;
                randSelect.RemoveAt(index); 

                index = Random.Range(0, randSelect.Count);  
                answers[randSelect[index]] = new AnswersList(); 
                answers[randSelect[index]].answer = questionList[i].docs[j].wrong2; 
                answers[randSelect[index]].isTrue = false;
                randSelect.RemoveAt(index); 

                index = Random.Range(0, randSelect.Count);  
                answers[randSelect[index]] = new AnswersList(); 
                answers[randSelect[index]].answer = questionList[i].docs[j].wrong3; 
                answers[randSelect[index]].isTrue = false;
                randSelect.RemoveAt(index); 
                break; 
            case 5:
                // Create list for random select
                randSelect = new List<int>(){0,1,2,3,4};


                index = Random.Range(0, randSelect.Count);  
                answers[randSelect[index]] = new AnswersList(); 
                answers[randSelect[index]].answer = questionList[i].docs[j].answer;
                answers[randSelect[index]].isTrue = true; 
                randSelect.RemoveAt(index); 
            
                index = Random.Range(0, randSelect.Count);  
                answers[randSelect[index]] = new AnswersList(); 
                answers[randSelect[index]].answer = questionList[i].docs[j].wrong1; 
                answers[randSelect[index]].isTrue = false;
                randSelect.RemoveAt(index); 

                index = Random.Range(0, randSelect.Count);  
                answers[randSelect[index]] = new AnswersList(); 
                answers[randSelect[index]].answer = questionList[i].docs[j].wrong2; 
                answers[randSelect[index]].isTrue = false;
                randSelect.RemoveAt(index); 

                index = Random.Range(0, randSelect.Count);  
                answers[randSelect[index]] = new AnswersList(); 
                answers[randSelect[index]].answer = questionList[i].docs[j].wrong3; 
                answers[randSelect[index]].isTrue = false;
                randSelect.RemoveAt(index); 

                index = Random.Range(0, randSelect.Count);  
                answers[randSelect[index]] = new AnswersList(); 
                answers[randSelect[index]].answer = questionList[i].docs[j].wrong4; 
                answers[randSelect[index]].isTrue = false;
                randSelect.RemoveAt(index); 
                break; 
        }
    }


    private int GetNumberAnswers(Question question) 
    {
        if (question.wrong1 == "" || question.wrong1 == null) return 1; 
        else if (question.wrong2 == "" || question.wrong2 == null) return 2; 
        else if (question.wrong3 == "" || question.wrong3 == null) return 3; 
        else if (question.wrong4 == "" || question.wrong4 == null) return 4; 
        else {
            return 5; 
        }
    }

    public RoundData GetCurrentRoundData()
    {
        return questionSetWanted;
    }

    public LeaderboardStructure GetLeaderboardModuleData()
    {
        return leaderboardWanted; 
    }

    public void NewLeaderboardWanted( LeaderboardStructure newLeaderboardWanted)
    {
        leaderboardWanted = newLeaderboardWanted;  
    }

    public void NewQuestionSetWanted( RoundData newQuestionSet)
    {
        questionSetWanted = newQuestionSet;
    }

    public RoundData[] GetCurrentAllRounds()
    {
        return questionSet;
    }

    public LeaderboardStructure[] GetCurrentAllLeaderboards()
    {
        return leaderboardArray;
    }


    // Students specs -- school in which he studies and classes 
    public class UserSpecs
    {
        public List<StudyItem> docs { get; set; }
    }


}

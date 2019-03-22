using System;
using System.Collections.Generic;

namespace Quiz_Master.Models
{
    public class QuestionManager
    {
        List<Question> _questions;

        public QuestionManager()
        {


            _questions = new List<Question> {
                new Question() {
                    question = "Select option A?",
                    answers = new string[] { "Option A", "Option B", "Option C" },
                    correctAnswer = 0
                },
                new Question() {
                    question = "Select option B?",
                    answers = new string[] { "Option A", "Option B", "Option C" },
                    correctAnswer = 1
                },
                new Question() {
                    question = "Select option C?",
                    answers = new string[] { "Option A", "Option B", "Option C" },
                    correctAnswer = 2
                }
            };
        }


        public List<Question> GetAll { get { return _questions; } }
    }
}

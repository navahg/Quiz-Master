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
                    question = "Agent 007, featured in many movies dating back to 1962, is known as _______ Bond?",
                    answers = new string[] { "John", "Benedict", "James" },
                    correctAnswer = 2
                },
                new Question() {
                    question = "What plant is traditionally the primary ingredient in wine?",
                    answers = new string[] { "Agave", "Grape", "Peach" },
                    correctAnswer = 1
                },
                new Question() {
                    question = "The Average American does what 22 times a day?",
                    answers = new string[] { "Opens Fridge", "Takes a bus", "Yawns" },
                    correctAnswer = 0
                },
                new Question() {
                    question = "In South Dakota it's illegal to fall down and sleep where?",
                    answers = new string[] { "Class", "In a Cheese Factory", "Church" },
                    correctAnswer = 1
                },
                new Question() {
                    question = "It's illegal in Georgia to do what with a fork?",
                    answers = new string[] { "Eat a cake", "Eat Spaghetti", "Eat fried chicken" },
                    correctAnswer = 2
                },
                new Question() {
                    question = "Two fathers took their sons fishing. They all caught one fish each. When they came back, they had three fish. None of the fish was stolen, eaten or thrown overboard. How come they only had three?",
                    answers = new string[] { "Because the grandfather took his son fishing, and the son took his son fishing", "Because someone ate it", "Because they gave one fish away" },
                    correctAnswer = 0
                },
                new Question() {
                    question = "What is the most common form of Chinese spoken in the south of China, in Hong Kong and in Macau?",
                    answers = new string[] { "Shanghainese", "Hakka", "Cantonese" },
                    correctAnswer = 2
                },
                new Question() {
                    question = "What is Chinese called in Chinese?",
                    answers = new string[] { "Zhong Yu", "Zhong Wen", "China Yu" },
                    correctAnswer = 1
                }

            };
        }


        public List<Question> GetAll { get { return _questions; } }
    }
}

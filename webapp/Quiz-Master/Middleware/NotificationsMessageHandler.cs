using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Quiz_Master.Models;
using WebSocketsManager;

namespace Quiz_Master.Middleware
{
    public class NotificationsMessageHandler : WebSocketHandler
    {

        private readonly QuestionManager questionManager;

        private readonly List<Question> questions;

        private int currentQuestionIndex;

        private ConcurrentDictionary<string, int> score = new ConcurrentDictionary<string, int>();

        private Timer timer;

        private bool isOpen;

        private bool hasOneClient;

        private int totalClients;

        public NotificationsMessageHandler(WebSocketConnectionManager webSocketConnectionManager, QuestionManager _questionManager) : base(webSocketConnectionManager)
        {

            questionManager = _questionManager;
            questions = questionManager.GetAll;
            currentQuestionIndex = 0;
            isOpen = true;
            hasOneClient = false;
            totalClients = 0;
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
            score[WebSocketConnectionManager.GetId(socket)] = 0;
            totalClients++;
            if (!hasOneClient)
            {
                hasOneClient = true;
                GameStartTimer(60).ContinueWith(_ => this.SendQuestionsIncrementally(60));
            }

            //Sending user information to all connected clients
            SendMessageToAllAsync($"{{\"USERS\": {totalClients}}}").Wait();
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            score[WebSocketConnectionManager.GetId(socket)] = 0;
            await base.OnDisconnected(socket);
            totalClients--;
            if (totalClients == 0)
                hasOneClient = false;
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);


            if (!message.StartsWith("ANSWER:") || currentQuestionIndex >= questions.Count)
            {
                return;
            }
            var socketId = WebSocketConnectionManager.GetId(socket);
            var chosenAnswer = message.Substring(6);
            try
            {
                var optionNumber = UInt16.Parse(chosenAnswer);
                if (optionNumber == questions[currentQuestionIndex].correctAnswer)
                {
                    score[socketId] = score[socketId] + 1;
                }
            } catch { }
        }

        public void SendQuestionsIncrementally(int interval) {
            for (int i = 0; i < questions.Count; i++)
            {
                currentQuestionIndex = i;
                var jsonifiedQuestion = JsonConvert.SerializeObject(questions[i]);
                SendMessageToAllAsync($"{{\"QUESTION\": {jsonifiedQuestion}}}").Wait();
                GameStartTimer(60).Wait();
            }
            SendScores();
        }

        public void SendScores()
        {
            var jsonifiedScores = JsonConvert.SerializeObject(score);
            SendMessageToAllAsync($"{{\"SCORES\": {jsonifiedScores}}}").Wait();
        }

        public async Task GameStartTimer(int timeout) {
            var timerState = new TimerState { counter = 0 };

            timer = new Timer(
                    callback: new TimerCallback(TimerTask),
                    state: timerState,
                    dueTime: 1000,
                    period: 1000
                );

            while (timerState.counter <= timeout)
            {
                await Task.Delay(1000);
            }

            timer.Dispose();
        }

        public void StartTimer(int timeout) {
            var timerState = new TimerState { counter = 0 };

            timer = new Timer(
                    callback: new TimerCallback(TimerTask),
                    state: timerState,
                    dueTime: 1000,
                    period: 1000
                );

            while (timerState.counter <= timeout)
            {
                Task.Delay(1000).Wait();
            }

            timer.Dispose();
        }

        private void TimerTask(object timerState)
        {
            var state = timerState as TimerState;
            Interlocked.Increment(ref state.counter);
            SendMessageToAllAsync($"{{\"TIMER\":{state.counter}}}").Wait();
        }

        class TimerState
        {
            public int counter;
        }
    }
}

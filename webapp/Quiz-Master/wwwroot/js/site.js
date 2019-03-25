// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let $initialView;
let $gameView;

let $joinButton;
let $leaveButton;

let $loaderIcon;

let $quizArea;
let $quetionArea;
let $answersArea;

let $timerArea;
let $userCountArea;

let $scoreModal;
let $scoreBody;

let $usernameInput;

let answerChosen = false;
let currentQuestion = {};

let socket = null;

const socketURL = `ws://${window.location.host}/notifications`;

function initializeVariables() {
    $initialView = $('#initialView');
    $gameView = $('#gamePage');

    $joinButton = $('#joinChannel');
    $leaveButton = $('#leaveButton');

    $loaderIcon = $('#loadingItems');

    $quizArea = $('#quizArea');
    $quetionArea = $('#questionArea');
    $answersArea = $('#answersArea');

    $timerArea = $('#timer');
    $userCountArea = $('#userCount');

    $scoreModal = $('#scoreModal');
    $scoreBody = $('#scoreBody');

    $usernameInput = $('#usernameInput');
}

function initializeListeners() {
    $joinButton.click(function () {

        if ($usernameInput.val() === '') {
            window.alert('Please enter your name to join.');
            return;
        }

        if (socket != null) {
            disconnect();
        }

        socket = new WebSocket(socketURL);

        socket.onopen = function (_) {
            socket.send(`USERNAME:${$usernameInput.val()}`);

            $initialView.hide();
            $gameView.show();

            $loaderIcon.show();
            $quizArea.hide();
        }

        socket.onclose = function (_) {
            $initialView.show();
            $gameView.hide();
        };

        socket.onmessage = function (event) {
            let message = event.data;
            let data = {};
            try {
                data = JSON.parse(message);
            } catch (e) { return; }

            let key = Object.keys(data)[0];

            switch (key) {
                case 'TIMER':
                    updateTimer(data['TIMER']);
                    break;
                case 'QUESTION':
                    updateQuestion(data['QUESTION']);
                    break;
                case 'SCORES':
                    showScores(data['SCORES']);
                    break;
                case 'USERS':
                    updateUsers(data['USERS']);
                    break;
                default:
            }
        };
    });
    
}

function leaveQuiz() {
    disconnect();
    $initialView.show();
    $gameView.hide();
    $scoreModal.hide();
    updateTimer(0);
}

function disconnect() {
    if (socket === null || socket.readyState === WebSocket.CLOSED || socket.readyState === WebSocket.CLOSING) {
        return;
    }
    typeof socket.close === 'function' && socket.close();
    socket.onopen = null;
    socket.onclose = null;
    socket.onmessage = null;
}

function updateQuestion(question = {}) {
    $loaderIcon.hide();
    $quizArea.show();

    currentQuestion = question;
    $quetionArea.html(question['question']);

    updateTimer(0);
    updateAnswers(question['answers']);
}

function updateTimer(value) {
    let timeLeft = 20 - value;
    $timerArea.html(timeLeft + 's');
    $timerArea.attr('aria-valuenow', timeLeft);
    $timerArea.css({ width: `${(timeLeft / 20) * 100}%` });
}

function updateAnswers(answers = []) {
    answerChosen = false;
    $answersArea.children().removeClass('active bg-success text-white');
    for (let i = 0; i < answers.length; i++) {
        $answersArea.children('li')[i].innerHTML = answers[i];
    }
}

function updateUsers(count = 0) {
    $userCountArea.html(count);
}

function showScores(scores) {
    $scoreBody.empty();

    Object.keys(scores).forEach(function (userId) {
        let scoreRecord = $(`<tr><td>${userId}</td><td>${scores[userId]}</td></tr>`);
        $scoreBody.append(scoreRecord);
    });

    $scoreModal.show();
}

function selectAnswer(index) {
    if (answerChosen) {
        return;
    }

    if (socket == null || socket.readyState === undefined || socket.readyState !== socket.OPEN) {
        return;
    }

    socket.send(index);
    answerChosen = true;
    $($answersArea.children('li')[index]).addClass('active');
    showCorrectAnswer();
}

function showCorrectAnswer() {
    $($answersArea.children('li')[currentQuestion.correctAnswer]).addClass('bg-success text-white');
}

$(document).ready(function () {
    initializeVariables();
    initializeListeners();
});
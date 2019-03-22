// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let $initialView;
let $gameView;

let $joinButton;
let $leaveButton;

let $loaderIcon;
let $loaderText;

let $quizArea;
let $quetionArea;
let $answersArea;

let $timerArea;

let socket = null;

const socketURL = `ws://${window.location.host}/notifications`;

function initializeVariables() {
    $initialView = $('#initialPage');
    $gameView = $('#gamePage');

    $joinButton = $('#joinChannel');
    $leaveButton = $('#leaveButton');

    $loaderIcon = $('#loadingItems');
    $loaderText = $('#loaderText');

    $quizArea = $('#quizArea');
    $quetionArea = $('#questionArea');
    $answersArea = $('#answerArea');

    $timerArea = $('#timer');
}

function initializeListeners() {
    $joinButton.click(function () {
        if (socket != null) {
            disconnect();
        }

        socket = new WebSocket(socketURL);

        socket.onopen = function (_) {
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
                default:
            }
        };
    });
}

function disconnect() {
    typeof socket.close === 'function' && socket.close();
    socket.onopen = null;
    socket.onclose = null;
    socket.onmessage = null;
}

function updateQuestion(question = {}) {
    $quetionArea.html(question['question']);
}

function updateTimer(value) {
    $timerArea.html(value);
}

function updateAnswers(answers) {
    // TODO: implement this
}

function showScores(scores) {
    // TODO: Implement this
}

$(document).ready(function () {
    initializeVariables();
    initializeListeners();
});
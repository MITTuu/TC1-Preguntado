document.addEventListener('DOMContentLoaded', function () {
    const startButton = document.getElementById('start-button');
    const restartButton = document.getElementById('restart-button');

    startButton.addEventListener('click', async function () {
        const questions = await fetchQuestions();
        startGame(questions);
    });

    restartButton.addEventListener('click', async function () {
        const questions = await fetchQuestions();
        score = 0;
        questionNumber = 0;
        updateQuestionNumber();
        updateScore();

        startGame(questions);
        restartButton.style.display = 'none'; 
    });

    let idJugador = document.getElementById('user-id').value;
    let questionNumber = 0;
    let currentQuestionIndex = 0;
    let questions = [];
    let score = 0;
    let usedQuestionIds = [];

    async function fetchQuestions() {
        try {
            const response = await fetch('/json/Preguntas.json');
            if (!response.ok) {
                throw new Error('Error al cargar las preguntas');
            }
            return await response.json();
        } catch (error) {
            console.error('Error:', error);
            alert('No se pudieron cargar las preguntas del juego.');
            return [];
        }
    }

    function startGame(loadedQuestions) {
        if (loadedQuestions.length === 0) {
            alert('No hay preguntas disponibles para el juego.');
            return;
        }

        document.getElementById('start-button').style.display = 'none';
        document.getElementById('question-container').style.display = 'block';

        questions = loadedQuestions;
        currentQuestionIndex = 0;
        usedQuestionIds = []; 

        showQuestion();
    }

    function showQuestion() {
        const questionText = document.getElementById('question-text');
        const answersContainer = document.getElementById('answers-container');

        if (currentQuestionIndex >= 2) {
            document.getElementById('restart-button').style.display = 'block'
            document.getElementById('restart-button').style.margin = '0 auto 20px auto';

            const buttons = document.querySelectorAll('.answer-button');
            buttons.forEach(btn => btn.classList.add('disabled'));

            saveGameResult(idJugador, score, score >= 6); 

            if (score < 6) {
                alert('¡Has perdido!');
            } else {
                alert('¡Has ganado!');
            }
            return;
        }

        const availableQuestions = questions.filter(question => !usedQuestionIds.includes(question.Id));

        if (availableQuestions.length === 0) {
            alert('No hay más preguntas disponibles.');
            return;
        }

        const question = availableQuestions[Math.floor(Math.random() * availableQuestions.length)];

        usedQuestionIds.push(question.Id);

        questionText.textContent = question.Pregunta;
        answersContainer.innerHTML = '';

        const answers = [question.Respuesta, ...question.Incorrectas];
        answers.sort(() => Math.random() - 0.5);

        answers.forEach(answer => {
            const button = document.createElement('button');
            button.textContent = answer;
            button.className = 'btn btn-secondary answer-button';
            button.addEventListener('click', function () {
                if (!answersContainer.querySelector('.disabled')) {
                    checkAnswer(button, answer, question.Respuesta);
                }
            });
            answersContainer.appendChild(button);
        });
    }

    function checkAnswer(button, selectedAnswer, correctAnswer) {
        const buttons = document.querySelectorAll('.answer-button');
        buttons.forEach(btn => btn.classList.add('disabled'));
        updateQuestionNumber();

        if (selectedAnswer === correctAnswer) {
            button.classList.add('correct');
            score++;
        } else {
            button.classList.add('incorrect');
        }

        updateScore();

        setTimeout(() => {
            currentQuestionIndex++;
            clearButtonStyles(button);
            showQuestion();
        }, 1000);
    }

    function updateScore() {
        document.getElementById('score').textContent = score;
    }

    function updateQuestionNumber() {
        questionNumber++;
        document.getElementById('question-number').textContent = questionNumber;
    }

    function clearButtonStyles(button) {
        const buttons = document.querySelectorAll('.answer-button');
        buttons.forEach(btn => btn.classList.remove('disabled', 'correct', 'incorrect'));
    }

    async function saveGameResult(idJugador, aciertos, resultado) {
        const gameResult = {
            IdJugador: parseInt(idJugador),
            Aciertos: aciertos,
            FechaPartida: new Date().toISOString(),
            Resultado: resultado
        };

        try {
            const response = await fetch('/HistorialPartida/Create', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(gameResult)
            });

            if (response.ok) {
                console.log('Resultado de la partida guardado exitosamente.');
            } else {
                const errorText = await response.text();
                console.error('Error al guardar el resultado de la partida:', errorText);
            }
        } catch (error) {
            console.error('Error:', error);
        }
    }
});


const Shapes = {
  Rock: "R",
  Scissors: "S",
  Paper: "P",
};

const beatenBy = {
  R: "P",
  S: "R",
  P: "S",
};

const shapesArray = ["R", "S", "P"];

const matrixP1 = {
  // prev -> current
  R: { R: 0, S: 0, P: 0 },
  S: { R: 0, S: 0, P: 0 },
  P: { R: 0, S: 0, P: 0 },
};

const matrixP2 = {
  R: { R: 0, S: 0, P: 0 },
  S: { R: 0, S: 0, P: 0 },
  P: { R: 0, S: 0, P: 0 },
};

let player1Counter = 0;
let player2Counter = 0;
let drawCounter = 0;

function player1(opponentHistory) {
  if (opponentHistory.length > 1 || Math.random() < 0.1) {
    const curr = opponentHistory[opponentHistory.length - 1];

    const probs = getProbabilitiesRow(curr, matrixP1);
    const predicted = Object.keys(probs).reduce((a, b) => (probs[a] > probs[b] ? a : b));
    return beatenBy[predicted];
  }
  return shapesArray[Math.floor(Math.random() * 3)];
}

//TODO
function player2(opponentHistory) {
  if (opponentHistory.length < 2 || Math.random() < 0.1) {
    return shapesArray[Math.floor(Math.random() * 3)];
  }
  const lastMove = opponentHistory[opponentHistory.length - 1];
  const probs = getProbabilitiesRow(lastMove, matrixP2);

  let predictedMove = "R";
  let maxProb = -1;

  for (const shape of shapesArray) {
    if (probs[shape] > maxProb) {
      maxProb = probs[shape];
      predictedMove = shape;
    }
  }

  return beatenBy[predictedMove];
}

function getProbabilitiesRow(shape, matrix) {
  const row = matrix[shape];

  const alpha = 1;
  const total = row.R + row.S + row.P + 3 * alpha;

  return {
    R: (row.R + alpha) / total,
    S: (row.S + alpha) / total,
    P: (row.P + alpha) / total,
  };
}

function defineWinner(player1, player2) {
  if (player1 === player2) {
    // console.log(`Draw. Player 1 and Player 2 both have ${player1}`);
    return 0;
  }

  const args = [player1, player2];

  if (args.includes(Shapes.Rock)) {
    let rockPosition = args.indexOf(Shapes.Rock);

    if (args[+!rockPosition] === Shapes.Scissors) {
      // console.log(`Player ${rockPosition + 1} wins. Player 1 - ${player1} | Player 2 - ${player2}`);
      return rockPosition + 1;
    }

    if (args[+!rockPosition] === Shapes.Paper) {
      // console.log(`Player ${+!rockPosition + 1} wins. Player 1 - ${player1} | Player 2 - ${player2}`);
      return +!rockPosition + 1;
    }
  } else {
    let scissorsPosition = args.indexOf(Shapes.Scissors);
    // console.log(`Player ${scissorsPosition + 1} wins. Player 1 - ${player1} | Player 2 - ${player2}`);
    return scissorsPosition + 1;
  }
}

function init(n) {
  let player1History = [];
  let player2History = [];
  for (let i = 0; i < n; i++) {
    const player1Move = player1(player2History);
    const player2Move = player2(player1History);
    // const player1Move = player1User();
    // const player2Move = player2User();

    player1History.push(player1Move);
    player2History.push(player2Move);

    if (player2History.length > 1) {
      const prev = player2History[player2History.length - 2];
      const curr = player2History[player2History.length - 1];
      matrixP1[prev][curr]++;
    }

    if (player1History.length > 1) {
      const prev = player1History[player1History.length - 2];
      const curr = player1History[player1History.length - 1];
      matrixP2[prev][curr]++;
    }

    const result = defineWinner(player1Move, player2Move);
    if (!result) {
      drawCounter++;
    } else if (result === 1) {
      player1Counter++;
    } else {
      player2Counter++;
    }
  }
  console.log(`------------------------------`);
  console.log(`Overall played games - ${n}`);
  console.log(`Draws amount - ${drawCounter}`);
  console.log(`Player 1 amount of wins - ${player1Counter}`);
  console.log(`Player 2 amount of wins - ${player2Counter}`);
  console.log(`------------------------------`);
  console.log("Player1 matrix - ", matrixP1);
  console.log("Player2 matrix - ", matrixP2);
}

init(1000);

const Shapes = {
  Rock: "Rock",
  Scissors: "Scissors",
  Paper: "Paper",
};

const shapesArray = ["Rock", "Scissors", "Paper"];

const player1Counter = 0;
const player2Counter = 0;
const drawCounter = 0;

function player1() {
  const random = Math.floor(Math.random() * 3);
  return shapesArray[random];
}

//TODO
function player2() {
  const random = Math.floor(Math.random() * 3);
  return shapesArray[random];
}

function defineWinner(player1, player2) {
  if (player1 === player2) {
    console.log(`Draw. Player 1 and Player 2 both have ${player1}`);
    return 0;
  }

  const args = [player1, player2];

  if (args.includes(Shapes.Rock)) {
    let rockPosition = args.indexOf(Shapes.Rock);

    if (args[+!rockPosition] === Shapes.Scissors) {
      console.log(`Player ${rockPosition + 1} wins. Player 1 - ${player1} | Player 2 - ${player2}`);
      return rockPosition + 1;
    }

    if (args[+!rockPosition] === Shapes.Paper) {
      console.log(`Player ${+!rockPosition + 1} wins. Player 1 - ${player1} | Player 2 - ${player2}`);
      return +!rockPosition + 1;
    }
  } else {
    let scissorsPosition = args.indexOf(Shapes.Scissors);
    console.log(`Player ${scissorsPosition + 1} wins. Player 1 - ${player1} | Player 2 - ${player2}`);
    return scissorsPosition + 1;
  }
}

console.log(defineWinner(player1(), player2()));

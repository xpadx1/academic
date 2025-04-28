const fs = require("fs");
const csv = require("csv-parser");
const csvFile = "outGame.csv";
// const csvFile = "outGameExpanded.csv";
const dataSet = [];

function naiveBayesClassifier(trainDataSet, applySmoothingAll = true) {
  const labelCount = {
    yes: 0,
    no: 0,
  };
  const featureCounts = {};
  const featurePossibleValues = {};
  const priorProbability = {};
  const totalRows = trainDataSet.length;

  trainDataSet.forEach((row) => {
    const label = row.play;
    if (row.play === "yes") labelCount.yes++;
    else labelCount.no++;

    for (const [feature, value] of Object.entries(row)) {
      if (feature === "play") continue;

      featurePossibleValues[feature] = featurePossibleValues[feature] || new Set();
      featurePossibleValues[feature].add(value);

      featureCounts[feature] = featureCounts[feature] || {};
      featureCounts[feature][value] = featureCounts[feature][value] || { yes: 0, no: 0, pYes: 0, pNo: 0 };
      if (label === "yes") {
        featureCounts[feature][value].yes++;
      } else {
        featureCounts[feature][value].no++;
      }
    }
  });

  for (const label in labelCount) {
    priorProbability[label] = labelCount[label] / totalRows;
  }

  for (const feature in featureCounts) {
    for (const value in featureCounts[feature]) {
      let countYes = featureCounts[feature][value].yes;
      let countNo = featureCounts[feature][value].no;

      if (applySmoothingAll) {
        featureCounts[feature][value].pYes = simpleSmothing(
          countYes,
          labelCount.yes,
          featurePossibleValues[feature].size
        );
        featureCounts[feature][value].pNo = simpleSmothing(countNo, labelCount.no, featurePossibleValues[feature].size);
      } else {
        if (countYes === 0) {
          featureCounts[feature][value].pYes = simpleSmothing(
            countYes,
            labelCount.yes,
            featurePossibleValues[feature].size
          );
        } else {
          featureCounts[feature][value].pYes = countYes / labelCount.yes;
        }
        if (countNo === 0) {
          featureCounts[feature][value].pNo = simpleSmothing(
            countNo,
            labelCount.no,
            featurePossibleValues[feature].size
          );
        } else {
          featureCounts[feature][value].pNo = countNo / labelCount.no;
        }
      }
    }
  }
  return { priorProbability, featureCounts };
}

function evaluate(testData, model) {
  let correct = 0;
  const labels = new Set(testData.map((r) => r.play));
  const metrics = {};

  for (const label of labels) {
    metrics[label] = { tp: 0, fp: 0, fn: 0 };
  }

  for (const row of testData) {
    const trueLabel = row.play;
    const predictedLabel = predict(row, model);
    if (trueLabel === predictedLabel) correct++;

    for (const label of labels) {
      if (predictedLabel === label) {
        if (trueLabel === label) {
          metrics[label].tp++;
        } else {
          metrics[label].fp++;
        }
      } else {
        if (trueLabel === label) {
          metrics[label].fn++;
        }
      }
    }
  }

  const accuracy = correct / testData.length;
  const detailed = {};
  for (const label of labels) {
    const { tp, fp, fn } = metrics[label];
    const precision = tp / (tp + fp || 1);
    const recall = tp / (tp + fn || 1);
    const f1 = (2 * precision * recall) / (precision + recall || 1);

    detailed[label] = { precision, recall, f1 };
  }

  return { accuracy, detailed };
}

function predict(inputRow, model) {
  const { priorProbability, featureCounts } = model;
  const probabilities = {};
  let probabilityYes = priorProbability.yes;
  let probabilityNo = priorProbability.no;

  for (const [feature, value] of Object.entries(inputRow)) {
    if (feature === "play") continue;
    if (featureCounts[feature] && featureCounts[feature][value] && featureCounts[feature][value].pYes) {
      probabilityYes *= featureCounts[feature][value].pYes;
    }
    if (featureCounts[feature] && featureCounts[feature][value] && featureCounts[feature][value].pNo) {
      probabilityNo *= featureCounts[feature][value].pNo;
    }
  }

  probabilities.yes = probabilityYes;
  probabilities.no = probabilityNo;
  const prediction = Object.entries(probabilities).reduce((a, b) => (a[1] > b[1] ? a : b))[0];
  console.log("Object to predict: ", inputRow);
  console.log("Predicted value: ", prediction);
  return prediction;
}

function simpleSmothing(numerator, denominator, classes = 0) {
  return (numerator + 1) / (denominator + classes);
}

function splitData(dataset) {
  const data = [...dataset];
  for (let i = data.length - 1; i > 0; i--) {
    const j = Math.floor(Math.random() * (i + 1));
    [data[i], data[j]] = [data[j], data[i]];
  }

  const testSet = data.slice(0, 20);
  const trainSet = data.slice(20);

  return { testSet, trainSet };
}

function main() {
  fs.createReadStream(csvFile)
    .pipe(csv())
    .on("data", (data) => dataSet.push(data))
    .on("end", () => {
      const { testSet, trainSet } = splitData(dataSet);
      const model = naiveBayesClassifier(trainSet);
      //   console.log("Test Set: ", testSet);

      const evalResults = evaluate(testSet, model);

      console.log("Accuracy:", evalResults.accuracy.toFixed(3));

      for (const [label, scores] of Object.entries(evalResults.detailed)) {
        console.log(`Class: ${label}`);
        console.log(`  Precision: ${scores.precision.toFixed(3)}`);
        console.log(`  Recall:    ${scores.recall.toFixed(3)}`);
        console.log(`  F1-Score:  ${scores.f1.toFixed(3)}`);
      }
    });
}

main();

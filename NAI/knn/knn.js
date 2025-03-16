const fs = require("fs");
const csv = require("csv-parser");

const csvFile = "IRIS.csv";
const userInput = [];
const inputKeys = ["Sepal length", "Sepal width", "Petal length", "Petal width"];
const k = 10;
let trainDataSet, testDataSet, dataSet;

const readline = require("readline").createInterface({
  input: process.stdin,
  output: process.stdout,
});

function prepareDataSet(csvFileName) {
  return new Promise((resolve, reject) => {
    let dataSet = [];
    fs.createReadStream(csvFileName)
      .pipe(csv())
      .on("data", (row) => {
        const vector = [
          parseFloat(row["sepal_length"]),
          parseFloat(row["sepal_width"]),
          parseFloat(row["petal_length"]),
          parseFloat(row["petal_width"]),
        ];

        let targetClass = dataSet.find((el) => el.class === row.species);
        if (targetClass) {
          targetClass.class = row.species;
          targetClass.vectors.push(vector);
        } else {
          dataSet.push({
            class: row.species,
            vectors: [vector],
          });
        }
      })
      .on("end", () => resolve(dataSet))
      .on("error", (error) => reject(error));
  });
}

function trainTestSplit(dataSet) {
  const trainDataSet = [];
  const testDataSet = [];

  dataSet.forEach((el, i) => {
    trainDataSet.push({
      class: el.class,
      vectors: [],
    });
    for (let j = 0; j < Math.floor((el.vectors.length * 66) / 100); j++) {
      trainDataSet[i].vectors.push(el.vectors[j]);
    }
  });

  dataSet.forEach((el, i) => {
    testDataSet.push({
      class: el.class,
      vectors: [],
    });
    testDataSet[i].vectors = el.vectors.splice(trainDataSet[i].vectors.length, el.vectors.length);
  });

  return { trainDataSet, testDataSet };
}

function measureAccuracy(realClasses, predictedClass) {
  const predictedAmount = realClasses.find((el) => el.class === predictedClass).entries;
  const generalAmount = realClasses.reduce((acc, curr) => acc + curr.entries, 0);
  const accuracy = ((predictedAmount / generalAmount) * 100).toFixed(1);
  return accuracy;
}

async function knn(k, csvFile, inputData = []) {
  if (inputData.length && trainDataSet.length) {
    const distances = trainDataSet.flatMap((entry) =>
      entry.vectors.map((vector) => ({
        class: entry.class,
        distance: euclideanDistance(vector, inputData),
      }))
    );
    const sortedDistances = sortDistances(distances);
    // console.log(sortedDistances);
    const [realClasses, predictedClass] = findPredictedClass(sortedDistances, k);
    console.log(`Based on ${k} nearest neighbours`);
    console.log("User input: ", inputData);
    console.log("Predicted class: ", predictedClass);
    console.log(`Accuracy: ${measureAccuracy(realClasses, predictedClass)}%`);
    return;
  }
  await prepareDataSet(csvFile).then((data) => {
    dataSet = data;
    ({ trainDataSet, testDataSet } = trainTestSplit(data));
  });
  let distances = [];
  testDataSet.forEach((element, i) => {
    element.vectors.forEach((testVector) => {
      trainDataSet[i].vectors.forEach((trainVector) => {
        distances.push({
          class: element.class,
          distance: euclideanDistance(testVector, trainVector),
        });
      });
    });
  });
  //   console.log(distances);
  let sortedDistances = sortDistances(distances);
  //   console.dir(distances, { depth: 100, maxArrayLength: null });
  const [realClasses, predictedClass] = findPredictedClass(sortedDistances, k);
  console.log(`Based on ${k} nearest neighbours`);
  console.log("Predicted class: ", predictedClass);
  console.log(`Accuracy: ${measureAccuracy(realClasses, predictedClass)}%`);
  askQuestion();
}

function euclideanDistance(a, b) {
  return Math.hypot(
    ...Object.keys(a).map((k) => {
      return b[k] - a[k];
      if (k == 1 || k == 3) {
        return b[k] - a[k];
      } else {
        return 0;
      }
    })
  );
}

function sortDistances(distances) {
  let isChanged = false;
  for (let i = 0; i < distances.length; i++) {
    if (distances[i].distance > distances[i + 1]?.distance) {
      [distances[i], distances[i + 1]] = [distances[i + 1], distances[i]];
      isChanged = true;
    }
  }
  if (isChanged) {
    return sortDistances(distances);
  }
  return distances;
}

function findPredictedClass(sortedDistances, k) {
  let entries = [];
  for (let i = 0; i < k; i++) {
    const similarDistances = sortedDistances.filter((el) => el.distance === sortedDistances[i].distance);
    if (similarDistances.length > k - i) {
      for (let j = 0; j < k - i; i++) {
        const randomizedElement = similarDistances[Math.floor(Math.random() * similarDistances.length)];
        sortedDistances.splice(similarDistances.indexOf(randomizedElement), 1);
        const element = entries.find((entry) => entry.class === randomizedElement.class);
        if (element) {
          element.entries++;
        } else {
          entries.push({
            class: randomizedElement.class,
            entries: 1,
          });
        }
      }
      break;
    } else {
      similarDistances.forEach((el, j) => {
        let element = entries.find((entry) => entry.class === el.class);
        if (element) {
          element.entries++;
        } else {
          entries.push({
            class: el.class,
            entries: 1,
          });
        }
        if (j) i++;
      });
    }
  }
  console.log(entries);
  return [entries, predict(entries)];
}

function predict(arr) {
  let largest = 0;
  arr.forEach((el) => {
    if (largest < el.entries) {
      largest = el.entries;
    }
  });

  const result = arr.filter((e) => e.entries === largest);
  if (result.length > 1) {
    return result[Math.floor(Math.random() * result.length)].class;
  }
  return result[0].class;
}

function askQuestion(index = 0) {
  if (index < inputKeys.length) {
    readline.question(`Enter ${inputKeys[index]} ---> `, (input) => {
      if (isNaN(parseFloat(input))) {
        console.error("Should be number");
        askQuestion(index);
      } else {
        userInput.push(parseFloat(input));
        askQuestion(index + 1);
      }
    });
  } else {
    // console.log("User Input:", userInput);
    knn(k, csvFile, userInput);
    readline.close();
  }
}

knn(k, csvFile);

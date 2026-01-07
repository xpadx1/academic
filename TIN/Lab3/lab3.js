const array = [1, 2, 3, "ok", "", false];
const object = {
  name: "Arsenii",
  email: "test@gmail.com",
  age: 22,
  status: false,
};

// 1
for (let i = 0; i < array.length; i++) {
  console.log("For: ", array);
}

// 2
let j = 0;
while (j < array.length) {
  console.log("While: ", array);
  j++;
}

// 3
for (element in object) {
  console.log(element);
}

// 4
for (element of array) {
  console.log(element);
}

// 5, 6, 7
function isNumber(...arguments) {
  let containsNan = arguments.map((el) => +el).find((el) => isNaN(el) === true);
  if (typeof containsNan === "undefined") {
    return true;
  }
  throw new Error("Not all are numbers");
}

console.log(isNumber(2, 3, true));

// 8
function acceptNumbers(x, y, callback) {
  return `${callback.name}: ${callback(++x, ++y)}`;
}

const findMin = (x, y) => {
  return x <= y ? x : y;
};

const findMax = (x, y) => {
  return x >= y ? x : y;
};

const isEqual = (x, y) => {
  return x === y;
};

console.log(acceptNumbers(5, 15, findMax));

// 9
function Car(marque, model, year) {
  if (typeof marque !== "string" || typeof model !== "string") {
    throw new Error("Invalid type for marque or model. Should be string");
  }
  if (typeof year !== "number") {
    throw new Error("Invalid type for year. Should be number");
  }
  this.marque = marque;
  this.model = model;
  this.year = year;
  this.getFullName = function () {
    return `${this.marque} ${this.model} ${this.year}`;
  };
}

const car = new Car("Toyota", "Corolla", 2022);
console.log(car);
console.log(car.getFullName());

// 10
class Human {
  constructor(name, surname, age, isMarried) {
    if (typeof name !== "string" || typeof surname !== "string") {
      throw new Error("Invalid type for name or surname. Should be string");
    }
    if (typeof age !== "number") {
      throw new Error("Invalid type for age. Should be number");
    }
    if (typeof isMarried !== "boolean") {
      throw new Error("Invalid type for isMarried. Should be boolean");
    }
    this.name = name;
    this.surname = surname;
    this.age = age;
    this.isMarried = isMarried;
  }

  getYearOfBirth() {
    const currentYear = new Date().getFullYear();
    return currentYear - this.age;
  }
}
const arsenii = new Human("Arsenii", "Shchaslyvyi", 24, false);
console.log(arsenii);
console.log(arsenii.getYearOfBirth());

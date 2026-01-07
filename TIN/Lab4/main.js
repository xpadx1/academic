const form = document.getElementById("form");

const name = document.getElementById("name");
const nameError = document.getElementById("name-error");

const email = document.getElementById("email");
const emailError = document.getElementById("email-error");

const password = document.getElementById("password");
const passwordError = document.getElementById("password-error");

const icon1 = document.getElementById("icon1");
const icon2 = document.getElementById("icon2");

icon1.addEventListener("click", (event) => {
  const password = document.getElementById("password");
  icon1.style.display = "none";
  icon2.style.display = "block";
  password.type = "text";
});

icon2.addEventListener("click", (event) => {
  const password = document.getElementById("password");
  icon2.style.display = "none";
  icon1.style.display = "block";
  password.type = "password";
});

name.addEventListener("click", (event) => {
  name.classList.remove("error");
  for (child in nameError.children) {
    nameError.children[child].style.display = "none";
  }
});

email.addEventListener("click", (event) => {
  email.classList.remove("error");
  for (child in emailError.children) {
    emailError.children[child].style.display = "none";
  }
});

password.addEventListener("click", (event) => {
  password.classList.remove("error");
  for (child in passwordError.children) {
    passwordError.children[child].style.display = "none";
  }
});

form.addEventListener("submit", (event) => {
  event.preventDefault();
  const formData = Object.fromEntries(new FormData(form));
  const isValid = validateForm(formData);
  isValid ? alert("Success") : "";
});

function validateForm(formData) {
  let isValid = false;
  for (el in formData) {
    if (el === "name") {
      const nameInput = formData[el];
      isValid = false;
      if (nameInput.length <= 2) {
        name.classList.add("error");
        nameError.children[0].style.display = "block";
      } else if (/[^a-zA-Z]/.test(nameInput)) {
        name.classList.add("error");
        nameError.children[1].style.display = "block";
      } else if (nameInput.length > 15) {
        name.classList.add("error");
        nameError.children[2].style.display = "block";
      } else {
        isValid = true;
      }
    }

    if (el === "email") {
      const emailInput = formData[el];
      isValid = false;
      if (emailInput.length <= 2) {
        email.classList.add("error");
        emailError.children[0].style.display = "block";
      } else if (emailInput.length > 30) {
        email.classList.add("error");
        emailError.children[1].style.display = "block";
      } else {
        isValid = true;
      }
    }

    if (el === "password") {
      const passwordInput = formData[el];
      const regex = /^(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>/?]).+$/;
      isValid = false;
      if (passwordInput.length <= 8) {
        password.classList.add("error");
        passwordError.children[0].style.display = "block";
      } else if (!regex.test(passwordInput)) {
        password.classList.add("error");
        passwordError.children[1].style.display = "block";
      } else {
        isValid = true;
      }
    }
  }
  return isValid;
}

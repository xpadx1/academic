const form = document.getElementById("form");

const name = document.getElementById("name");
const nameError = document.getElementById("name-error");

const email = document.getElementById("email");
const emailError = document.getElementById("email-error");

const password = document.getElementById("password");
const passwordError = document.getElementById("password-error");

const icon1 = document.getElementById("icon1");
const icon2 = document.getElementById("icon2");

const info = document.getElementById("info");

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
  isValid ? toggleInfo(formData) : "";
});

function validateForm(formData) {
  let isValid = [false, false, false];
  isValid[0] = true;
  for (el in formData) {
    // if (el === "name") {
    //   const nameInput = formData[el];
    //   isValid[0] = false;
    //   if (nameInput.length <= 2) {
    //     name.classList.add("error");
    //     nameError.children[0].style.display = "block";
    //   } else if (/[^a-zA-Z]/.test(nameInput)) {
    //     name.classList.add("error");
    //     nameError.children[1].style.display = "block";
    //   } else if (nameInput.length > 15) {
    //     name.classList.add("error");
    //     nameError.children[2].style.display = "block";
    //   } else {
    //     isValid[0] = true;
    //   }
    // }

    if (el === "email") {
      const emailInput = formData[el];
      isValid[1] = false;
      if (emailInput.length <= 2) {
        email.classList.add("error");
        emailError.children[0].style.display = "block";
      } else if (emailInput.length > 30) {
        email.classList.add("error");
        emailError.children[1].style.display = "block";
      } else {
        isValid[1] = true;
      }
    }

    if (el === "password") {
      const passwordInput = formData[el];
      const regex = /^(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>/?]).+$/;
      isValid[2] = false;
      if (passwordInput.length <= 8) {
        password.classList.add("error");
        passwordError.children[0].style.display = "block";
      } else if (!regex.test(passwordInput)) {
        password.classList.add("error");
        passwordError.children[1].style.display = "block";
      } else {
        isValid[2] = true;
      }
    }
  }
  return !isValid.includes(false);
}

async function toggleInfo(data) {
  const res = await fetch("/submit", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  const profile = document.getElementById("profile");
  const wrapper = document.getElementById("profile-wrapper");
  const resData = JSON.parse(await res.text());
  if (resData.data && resData.status === "Success") {
    info.innerHTML = resData.status;
    info.className = "success";
    profile.style.display = "success";
    profile.style.display = "flex";
    wrapper.children[0].innerHTML = resData.data.name;
    wrapper.children[1].innerHTML = resData.data.email;
  } else {
    info.innerHTML = resData.status;
    info.className = "error";
  }
  setTimeout(() => {
    info.innerHTML = "";
  }, 3000);
}

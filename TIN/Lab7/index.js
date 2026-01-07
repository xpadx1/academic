const express = require("express");
const path = require("path");
const app = express();
const port = 3000;

app.use(express.json());

app.use(express.static(path.join(__dirname, "assets")));

app.get("/", (req, res) => {
  res.sendFile(path.join(__dirname, "assets", "form.html"));
});
``;

const authToken = "Qwerty12345";

function createRandomKey(length) {
  const chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
  let result = "";
  for (let i = 0; i < length; i++) {
    result += chars.charAt(Math.floor(Math.random() * chars.length));
  }
  return result;
}

app.post("/submit", (req, res) => {
  for (el in req.body) {
    if (el === "name") {
      const nameInput = req.body[el];
      if (nameInput.length <= 2 || /[^a-zA-Z]/.test(nameInput) || nameInput.length > 15) {
        res.send({
          status: "Validation error",
          description: "Name should be longer than 2 symbols and shorter than 15",
        });
        return;
      }
    }

    if (el === "email") {
      const emailInput = req.body[el];
      if (emailInput.length <= 2 || emailInput.length > 30) {
        res.send({
          status: "Validation error",
          description: "Email should be longer than 2 symbols and shorter than 30",
        });
        return;
      }
    }

    if (el === "password") {
      const passwordInput = req.body[el];
      const regex = /^(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>/?]).+$/;
      if (passwordInput.length <= 8 || !regex.test(passwordInput)) {
        res.send({
          status: "Validation error",
          description:
            "Password should be longer than 8 symbols and include at least one number and one special symbol",
        });
        return;
      }
    }
  }

  res.send({
    status: "Success",
    description: "Frontend and backend validation is successful",
    token: authToken,
    data: {
      name: req.body.name + ` - ${createRandomKey(5)}`,
      email: req.body.email + ` - ${createRandomKey(5)}`,
      key: "Current random key ( initial ) - " + createRandomKey(20),
    },
  });
});

app.get("/polling", (req, res) => {
  if (req.query.token === authToken) {
    res.send({
      status: "Success",
      description: "Random token was updated",
      data: {
        key: "Current random key ( polling ) - " + createRandomKey(20),
      },
    });
  } else {
    res.send({
      status: "Rejected",
      description: "No authentication token provided",
    });
  }
});

app.listen(port, () => {
  console.log(`Example app listening on port ${port}`);
});

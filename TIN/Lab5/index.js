const express = require("express");
const path = require("path");
const app = express();
const port = 3000;

app.use(express.json());

app.use(express.static(path.join(__dirname, "assets")));

app.get("/", (req, res) => {
  res.sendFile(path.join(__dirname, "pages", "form.html"));
});

app.get("/assigment_A1", (req, res) => {
  res.sendFile(path.join(__dirname, "pages", "Assigment_A1.html"));
});

app.get("/assigment_B1", (req, res) => {
  res.sendFile(path.join(__dirname, "pages", "Assigment_B1.html"));
});

app.get("/assigment_A2", (req, res) => {
  res.sendFile(path.join(__dirname, "pages", "Assigment_A2.html"));
});

app.post("/submit", (req, res) => {
  for (el in req.body) {
    if (el === "name") {
      const nameInput = req.body[el];
      if (nameInput.length <= 2 || /[^a-zA-Z]/.test(nameInput) || nameInput.length > 15) {
        res.send({
          status: "Error, invalid name",
        });
        return;
      }
    }

    if (el === "email") {
      const emailInput = req.body[el];
      if (emailInput.length <= 2 || emailInput.length > 30) {
        res.send({
          status: "Error, invalid email",
        });
        return;
      }
    }

    if (el === "password") {
      const passwordInput = req.body[el];
      const regex = /^(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>/?]).+$/;
      if (passwordInput.length <= 8 || !regex.test(passwordInput)) {
        res.send({
          status: "Error, invalid password",
        });
        return;
      }
    }
  }

  res.send({
    status: "Success",
    data: {
      name: req.body.name,
      email: req.body.email,
    },
  });
});

app.listen(port, () => {
  console.log(`Example app listening on port ${port}`);
});

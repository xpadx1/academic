const bcrypt = require("bcrypt");
const jwt = require("jsonwebtoken");
const { User } = require("../models");

const JWT_SECRET = "supersecret";

const isEmail = (email) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
const isRequired = (...fields) => fields.every(Boolean);

const generateToken = (id) => jwt.sign({ id }, JWT_SECRET, { expiresIn: "7d" });

exports.register = async (req, res) => {
  const { name, email, password, department } = req.body;

  if (!isRequired(name, email, password, department)) {
    return res.status(400).json({ message: "Missing fields" });
  }

  if (!isEmail(email)) {
    return res.status(400).json({ message: "Invalid email" });
  }

  if (password.length < 6) {
    return res.status(400).json({ message: "Password too short" });
  }

  const exists = await User.findOne({ where: { email } });
  if (exists) {
    return res.status(409).json({ message: "Email already registered" });
  }

  const hash = await bcrypt.hash(password, 10);

  const user = await User.create({
    name,
    email,
    password: hash,
    department,
    isAdmin: false,
  });

  res.status(201).json({
    token: generateToken(user.id),
    user: {
      id: user.id,
      isAdmin: user.isAdmin,
      email: user.email,
      name: user.name,
    },
  });
};

exports.login = async (req, res) => {
  const { email, password } = req.body;

  if (!isRequired(email, password)) {
    return res.status(400).json({ message: "Missing credentials" });
  }

  const user = await User.findOne({ where: { email } });
  if (!user) {
    return res.status(401).json({ message: "Invalid credentials" });
  }

  const match = await bcrypt.compare(password, user.password);
  if (!match) {
    return res.status(401).json({ message: "Invalid credentials" });
  }

  res.json({
    token: generateToken(user.id),
    user: {
      id: user.id,
      isAdmin: user.isAdmin,
      email: user.email,
      name: user.name,
    },
  });
};

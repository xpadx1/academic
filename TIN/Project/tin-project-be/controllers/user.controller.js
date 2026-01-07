const { User } = require("../models");
const { register } = require("../controllers/auth.controller");
const bcrypt = require("bcrypt");

const isEmail = (email) => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
const isRequired = (...fields) => fields.every(Boolean);

exports.createUser = async (req, res) => {
  try {
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
      user,
    });
  } catch (err) {
    res.status(400).json({ error: err.message });
  }
};

exports.getUsers = async (req, res) => {
  const users = await User.findAll({
    attributes: { exclude: ["password"] },
  });
  res.json(users);
};

exports.getUserById = async (req, res) => {
  const user = await User.findByPk(req.params.id, {
    attributes: { exclude: ["password"] },
  });

  if (!user) {
    return res.status(404).json({ message: "User not found" });
  }

  res.json(user);
};

exports.updateUser = async (req, res) => {
  try {
    const { name, email, password, department, isAdmin } = req.body;

    const user = await User.findByPk(req.params.id);
    if (!user) {
      return res.status(404).json({ message: "User not found" });
    }

    if (email && !isEmail(email)) {
      return res.status(400).json({ message: "Invalid email" });
    }

    if (email && email !== user.email) {
      const exists = await User.findOne({ where: { email } });
      if (exists) {
        return res.status(409).json({ message: "Email already registered" });
      }
    }

    if (password && password.length < 6) {
      return res.status(400).json({ message: "Password too short" });
    }

    if (name !== undefined) user.name = name;
    if (email !== undefined) user.email = email;
    if (department !== undefined) user.department = department;
    if (isAdmin !== undefined) user.isAdmin = isAdmin;

    if (password) {
      user.password = await bcrypt.hash(password, 10);
    }

    await user.save();

    res.json({
      user,
    });
  } catch (err) {
    res.status(400).json({ error: err.message });
  }
};

exports.deleteUser = async (req, res) => {
  const user = await User.findByPk(req.params.id);
  if (!user) {
    return res.status(404).json({ message: "User not found" });
  }

  await user.destroy();
  res.json({ message: "User deleted" });
};

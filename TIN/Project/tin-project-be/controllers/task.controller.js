const { Task, User, Project } = require("../models");

exports.create = async (req, res) => {
  try {
    res.status(201).json(await Task.create(req.body));
  } catch (e) {
    console.error(e.errors);
    res.status(400).json({ error: e.message });
  }
};

exports.findAll = async (_, res) => {
  res.json(
    await Task.findAll({
      include: [User, Project],
    })
  );
};

exports.findById = async (req, res) => {
  const task = await Task.findByPk(req.params.id, {
    include: [User, Project],
  });
  if (!task) return res.status(404).json({ message: "Task not found" });
  res.json(task);
};

exports.update = async (req, res) => {
  const task = await Task.findByPk(req.params.id);
  if (!task) return res.status(404).json({ message: "Task not found" });
  await task.update(req.body);
  res.json(task);
};

exports.remove = async (req, res) => {
  const task = await Task.findByPk(req.params.id);
  if (!task) return res.status(404).json({ message: "Task not found" });
  await task.destroy();
  res.json({ message: "Task deleted" });
};

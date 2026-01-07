const { Collaborator, User, Project } = require("../models");

exports.create = async (req, res) => {
  try {
    res.status(201).json(await Collaborator.create(req.body));
  } catch (e) {
    res.status(400).json({ error: e.message });
  }
};

exports.findAll = async (_, res) => {
  res.json(
    await Collaborator.findAll({
      include: [User, Project],
    })
  );
};

exports.findById = async (req, res) => {
  const collab = await Collaborator.findByPk(req.params.id, {
    include: [User, Project],
  });
  if (!collab) return res.status(404).json({ message: "Collaborator not found" });
  res.json(collab);
};

exports.update = async (req, res) => {
  const collab = await Collaborator.findByPk(req.params.id);
  if (!collab) return res.status(404).json({ message: "Collaborator not found" });
  await collab.update(req.body);
  res.json(collab);
};

exports.remove = async (req, res) => {
  const collab = await Collaborator.findByPk(req.params.id);
  if (!collab) return res.status(404).json({ message: "Collaborator not found" });
  await collab.destroy();
  res.json({ message: "Collaborator removed" });
};

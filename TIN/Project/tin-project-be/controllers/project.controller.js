const { Project, Task, User } = require("../models");

exports.create = async (req, res) => {
  try {
    res.status(201).json(await Project.create(req.body));
  } catch (e) {
    res.status(400).json({ error: e.message });
  }
};

exports.findAll = async (req, res) => {
  if (req.auth.isUser && !req.auth.isAdmin) {
    const projects = await Project.findAll({
      include: [
        {
          model: Task,
          where: {
            userId: req.auth.user.id,
          },
          attributes: [],
        },
      ],
      distinct: true,
    });

    return res.json(projects);
  } else if (req.auth.isAdmin) {
    res.json(await Project.findAll());
  }
};

exports.findById = async (req, res) => {
  const project = await Project.findByPk(req.params.id, {
    include: [
      {
        model: Task,
        include: [
          {
            model: User,
            attributes: ["id", "name", "email"],
          },
        ],
      },
    ],
  });
  if (!project) return res.status(404).json({ message: "Project not found" });
  res.json(project);
};

exports.update = async (req, res) => {
  const project = await Project.findByPk(req.params.id);
  if (!project) return res.status(404).json({ message: "Project not found" });
  await project.update(req.body);
  res.json(project);
};

exports.remove = async (req, res) => {
  const project = await Project.findByPk(req.params.id);
  if (!project) return res.status(404).json({ message: "Project not found" });
  await project.destroy();
  res.json({ message: "Project deleted" });
};

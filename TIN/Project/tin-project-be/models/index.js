const sequelize = require("../config/database");

const User = require("./User");
const Project = require("./Project");
const Task = require("./Task");
const Collaborator = require("./Collaborator");

User.belongsToMany(Project, {
  through: Task,
  foreignKey: "userId",
  as: "taskProjects",
});

Project.belongsToMany(User, {
  through: Task,
  foreignKey: "projectId",
  as: "taskUsers",
});

User.belongsToMany(Project, {
  through: Collaborator,
  foreignKey: "userId",
  as: "collaborations",
});

Project.belongsToMany(User, {
  through: Collaborator,
  foreignKey: "projectId",
  as: "collaborators",
});

Task.belongsTo(User, { foreignKey: "userId" });
Task.belongsTo(Project, { foreignKey: "projectId" });

Collaborator.belongsTo(User, { foreignKey: "userId" });
Collaborator.belongsTo(Project, { foreignKey: "projectId" });

User.hasMany(Task, { foreignKey: "userId" });
Project.hasMany(Task, { foreignKey: "projectId" });

User.hasMany(Collaborator, { foreignKey: "userId" });
Project.hasMany(Collaborator, { foreignKey: "projectId" });

module.exports = {
  sequelize,
  User,
  Project,
  Task,
  Collaborator,
};

const { DataTypes } = require("sequelize");
const sequelize = require("../config/database");

const Collaborator = sequelize.define(
  "Collaborator",
  {
    id: {
      type: DataTypes.INTEGER,
      primaryKey: true,
      autoIncrement: true,
    },
    userId: {
      type: DataTypes.INTEGER,
      allowNull: false,
      field: "user_id",
    },
    projectId: {
      type: DataTypes.INTEGER,
      allowNull: false,
      field: "project_id",
    },
    role: {
      type: DataTypes.ENUM("viewer", "contributor", "manager"),
      allowNull: false,
      defaultValue: "viewer",
    },
    hours: {
      type: DataTypes.INTEGER,
      allowNull: false,
      defaultValue: 0,
    },
    isActive: {
      type: DataTypes.BOOLEAN,
      allowNull: false,
      defaultValue: true,
    },
  },
  {
    indexes: [
      {
        unique: true,
        fields: ["user_id", "project_id"],
      },
    ],
    tableName: "collaborators",
    timestamps: true,
  }
);

module.exports = Collaborator;
